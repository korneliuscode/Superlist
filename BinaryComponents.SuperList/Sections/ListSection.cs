/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using BinaryComponents.Utility.Collections;
using System.Xml;

namespace BinaryComponents.SuperList.Sections
{
	public class ListSection : ScrollableSection, HeaderSection.ILayoutController
	{
		public ListSection( ListControl listControl )
			: base( listControl )
		{
			_headerSection = Host.SectionFactory.CreateHeaderSection( listControl, listControl.Columns.VisibleItems );
			_headerSection.LayoutController = this;
			listControl.Columns.GroupedItems.DataChanged += GroupedItems_DataChanged;
			Host.FocusedSection = this; // we handle row focus manually from here.
			Children.Add( _headerSection );
			ExcludeFirstChildrenFromVScroll = 1;
			listControl.SelectedItems.DataChanged += SelectedItems_DataChanged;
		}
		public bool AllowRowDragDrop
		{
			get
			{
				return _allowRowDragDrop;
			}
			set
			{
				_allowRowDragDrop = value;
			}
		}

		public void SizeColumnsToFit( params Column[] columns )
		{
			using( Graphics grfx = Host.CreateGraphics() )
			{
				GraphicsSettings grfxSettings = new GraphicsSettings( grfx );
				int[] columnWidths = new int[Columns.Count];
				int position = 0;

				HeaderColumnSection[] hcsSections = new HeaderColumnSection[columns.Length];
				for( int iColumn = 0; iColumn < columns.Length; iColumn++ )
				{
					HeaderColumnSection hcs = Host.SectionFactory.CreateHeaderColumnSection( Host, HeaderColumnSection.DisplayMode.Customise, columns[iColumn] );
					hcs.Layout( grfxSettings, new Size( int.MaxValue, int.MaxValue ) );
					hcsSections[iColumn] = hcs;
				}

				Type[] lastColumnTypes = new Type[columns.Length];
				CellSection[] columnCellSections = new CellSection[columns.Length];

				//
				//	Pick the widest cell to be the width of the column.
				foreach( object rowItem in ItemList.ToArray() )
				{
					for( int iColumn = 0; iColumn < columns.Length; iColumn++ )
					{
						Column column = columns[iColumn];
						CellSection cs = columnCellSections[iColumn];
						object columnItem = column.ColumnItemAccessor( rowItem );
						Type lastColumnType = lastColumnTypes[iColumn] == null ? null : lastColumnTypes[iColumn].GetType();
						if( lastColumnType == null || columnItem.GetType() != lastColumnType )
						{
							cs = columnCellSections[iColumn] = Host.SectionFactory.CreateCellSection( Host, hcsSections[iColumn], rowItem );
							lastColumnTypes[iColumn] = columnItem.GetType();
						}
						cs.Item = rowItem;
						Size size = cs.GetIdealSize( grfxSettings );
						if( columnWidths[iColumn] < size.Width )
						{
							columnWidths[iColumn] = size.Width;
						}
					}
					position++;
				}

				//
				//	Set columns widths...
				for( int iColumn = 0; iColumn < columns.Length; iColumn++ )
				{
					columns[iColumn].Width = columnWidths[iColumn] == 0 ? hcsSections[iColumn].Size.Width : columnWidths[iColumn];
				}
			}
		}


		public bool AllowMultiSelect
		{
			get
			{
				return _allowMultiSelect;
			}
			set
			{
				_allowMultiSelect = value;
			}
		}

		public int GroupIndent
		{
			get
			{
				return _groupIndent;
			}
			set
			{
				_groupIndent = value;
			}
		}


		public void SerializeState( System.IO.TextWriter writer )
		{
			Helper.SerializationState ss = new BinaryComponents.SuperList.Helper.SerializationState();
			List<Helper.SerializationState.ColumnState> columnStates = new List<Helper.SerializationState.ColumnState>();

			foreach( Column c in Columns )
			{
				Helper.SerializationState.ColumnState columnState = new Helper.SerializationState.ColumnState();

				columnState.Name = c.Name;
				columnState.SortOrder = c.SortOrder;
				columnState.GroupSortOrder = c.GroupSortOrder;
				columnState.VisibleIndex = Columns.VisibleItems.IndexOf( c );
				columnState.GroupedIndex = Columns.GroupedItems.IndexOf( c );
				columnState.Width = c.Width;
				columnStates.Add( columnState );
			}
			ss.ColumnStates = columnStates.ToArray();

			List<Helper.SerializationState.GroupInstance> groupStates = new List<Helper.SerializationState.GroupInstance>();

			ss.GlobalGroupState = _globalGroupState == GroupState.Collapsed ? Helper.SerializationState.GroupState.GroupCollapsed : Helper.SerializationState.GroupState.GroupExpanded;

			foreach( GroupIdentifier gi in _groupExpansionState )
			{
				Helper.SerializationState.GroupInstance groupInstance = new Helper.SerializationState.GroupInstance();

				groupInstance.GroupPath = gi.GroupValues;
				groupInstance.GroupName = gi.LastColumn.Name;
				groupStates.Add( groupInstance );
			}

			ss.GroupStates = groupStates.ToArray();

			using( XmlTextWriter tw = new XmlTextWriter( writer ) )
			{
				tw.Formatting = Formatting.Indented;

				Helper.SerializationState.Serialize( tw, ss );
			}
		}

		public void DeSerializeState( System.IO.TextReader reader )
		{
			Helper.SerializationState ss;

			using( XmlTextReader tr = new XmlTextReader( reader ) )
			{
				ss = Helper.SerializationState.Deserialize( tr );
			}

			if( ss == null || (ss.ColumnStates.Length == 0 && ss.GroupStates.Length == 0) )
			{
				return;
			}

			Columns.GroupedItems.Clear();
			Columns.VisibleItems.Clear();

			//
			// Add groups
			Array.Sort( ss.ColumnStates,
								 delegate( Helper.SerializationState.ColumnState x, Helper.SerializationState.ColumnState y )
								 {
									 return x.GroupedIndex - y.GroupedIndex;
								 }
					);
			foreach( Helper.SerializationState.ColumnState columnState in ss.ColumnStates )
			{
				if( columnState.GroupedIndex >= 0 )
				{
					Column column = Columns.FromName( columnState.Name );
					if( column != null )
					{
						Columns.GroupedItems.Add( column );
					}
				}
			}

			//
			// Add visible items and set properties.
			Array.Sort<Helper.SerializationState.ColumnState>( ss.ColumnStates,
																												delegate( Helper.SerializationState.ColumnState x, Helper.SerializationState.ColumnState y )
																												{
																													return x.VisibleIndex - y.VisibleIndex;
																												}
					);
			foreach( Helper.SerializationState.ColumnState columnState in ss.ColumnStates )
			{
				Column column = Columns.FromName( columnState.Name );
				if( column != null )
				{
					if( columnState.VisibleIndex >= 0 )
					{
						Columns.VisibleItems.Add( column );
					}

					column.SortOrder = columnState.SortOrder;
					column.GroupSortOrder = columnState.GroupSortOrder;
					column.Width = columnState.Width;
				}
			}

			//
			// Restore group expansion states
			_groupExpansionState.Clear();
			_globalGroupState = ss.GlobalGroupState == Helper.SerializationState.GroupState.GroupCollapsed ? GroupState.Collapsed : GroupState.Expanded;
			_groupExpansionState.Clear();
			foreach( Helper.SerializationState.GroupInstance gi in ss.GroupStates )
			{
				Column column = Columns.FromName( gi.GroupName );
				if( column != null )
				{
					_groupExpansionState.Add( new GroupIdentifier( gi.GroupPath, column ) );
				}
			}
			Host.LazyLayout( null );
		}

		internal void LazyLayout()
		{
			Host.LazyLayout( null );
		}


		public virtual int ReservedSpaceFromHeaderFirstColumn
		{
			get
			{
				return ListControl.Columns.GroupedItems.Count * _groupIndent;
			}
		}


		public void SelectAll()
		{
			List<RowIdentifier> allItems = new List<RowIdentifier>( ItemList.Count );
			for( int i = 0; i < ItemList.Count; i++ )
			{
				RowIdentifier rowIdentifier = new NonGroupRow( ItemList[i] );
				allItems.Add( rowIdentifier );
			}
			SelectedItems.ClearAndAdd( allItems.ToArray() );
			Invalidate();
		}

		public override Point GetScrollCoordinates()
		{
			Point pt = base.GetScrollCoordinates();
			pt.Y = 0;
			return pt;
		}


		public override void Layout( Section.GraphicsSettings gs, System.Drawing.Size maximumSize )
		{
			Size = maximumSize;
			CalculateListItems();


			Column[] groupColumns = ListControl.Columns.GroupedItems.ToArray();

			//
			// Leave header and options toolbar sections...
			DeleteRange( ExcludeFirstChildrenFromVScroll, Children.Count - ExcludeFirstChildrenFromVScroll );

			base.Layout( gs, maximumSize );

			if( Columns.Count > 0 && _headerSection.IsVisible )
			{
				HorizontalScrollbarVisible = _headerSection.IdealWidth > maximumSize.Width;
			}

			if( ItemList.Count > 0 )
			{
				bool stop = false;
				while( !stop )
				{
					Rectangle rowsRectangle = WorkingRectangle;
					int yPos = _layoutDirection == Direction.Forward ? rowsRectangle.Top : rowsRectangle.Bottom;

					_countDisplayed = 0;

					//
					//	Create sections until either we have no more items
					//	or go over our height
					int position = LineStart;
					foreach( RowIdentifier rowInfo in GetRowEnumerator( LineStart, _layoutDirection ) )
					{
						object item = rowInfo.Items[0];

						Section newSection;

						int groupIndex = rowInfo.GroupColumns.Length-1;

						if( groupIndex != -1 && groupIndex < ListControl.Columns.GroupedItems.Count )
						{
							newSection = Host.SectionFactory.CreateGroupSection( ListControl,
																																	rowInfo,
																																	_headerSection,
																																	position,
																																	_groupIndent * groupIndex );
							Children.Add( newSection );
							newSection.Layout( gs, new Size( Size.Width, Size.Height ) );
						}
						else
						{
							newSection = Host.SectionFactory.CreateRowSection( ListControl, rowInfo, _headerSection, position );
							Children.Add( newSection );
							newSection.Layout( gs, new Size( Size.Width, Size.Height ) );
						}

						switch( _layoutDirection )
						{
							case Direction.Forward:
								newSection.Location = new Point( Location.X, yPos );
								yPos = newSection.Rectangle.Bottom;
								stop = yPos >= rowsRectangle.Bottom;
								position++;
								break;
							case Direction.Backward:
								newSection.Location = new Point( Location.X, yPos - newSection.Size.Height );
								yPos = newSection.Rectangle.Top;
								stop = yPos <= rowsRectangle.Top;
								position--;
								break;
						}
						if( stop )
						{
							break;
						}

						_countDisplayed++;
					}
					if( !stop ) // must have run out of things to fill
					{
						switch( _layoutDirection )
						{
							case Direction.Forward:
								if( LineStart > 0 )
								{
									DeleteRange( ExcludeFirstChildrenFromVScroll, Children.Count - ExcludeFirstChildrenFromVScroll );
									_layoutDirection = Direction.Backward;
									_lineStart = _rowInformation.Count - 1;
								}
								else
								{
									stop = true;
								}
								break;
							case Direction.Backward:
								DeleteRange( ExcludeFirstChildrenFromVScroll, Children.Count - ExcludeFirstChildrenFromVScroll );
								_layoutDirection = Direction.Forward;
								_lineStart = 0;
								break;
						}
					}
					else
					{
						break; // all done
					}
				}
			}

			if( VScrollbar != null )
			{
				VScrollbar.LargeChange = _countDisplayed;
			}

			VerticalScrollbarVisible = _countDisplayed < RowInformation.Count;
			if( Columns.Count == 0 && !_headerSection.IsVisible )
			{
				HorizontalScrollbarVisible = MaxWidth > maximumSize.Width;
			}

			UpdateScrollInfo();
		}

		private bool IsNearStart( Section rowSection )
		{
			int firstCount = 0;
			foreach( Section s in Children )
			{
				if( s is RowSection )
				{
					if( s == rowSection )
					{
						return true;
					}
					if( ++firstCount == 2 )
					{
						break;
					}
				}
			}
			return false;
		}
		private bool IsNearEnd( Section rowSection )
		{
			if( Children.Count > ExcludeFirstChildrenFromVScroll )
			{
				if( Children[Children.Count-1] == rowSection )
				{
					return true;
				}
				if( Children.Count-2 > 0 && Children[Children.Count-2] == rowSection )
				{
					return true;
				}
			}
			return false;
		}

		public override void DescendentDraggedOver( Section s )
		{
			base.DescendentDraggedOver( s );

			if( IsNearStart( s ) && VerticalScrollbarVisible && VScrollbar.Value > 0  )
			{
				StartScrollTimer();
			}

			if( IsNearEnd( s ) && VerticalScrollbarVisible && VScrollbar.Value < VScrollbar.Maximum )
			{
				StartScrollTimer();
			}
		}

		private void StartScrollTimer()
		{
			if( _scrollTimer == null )
			{
				_scrollTimer = new Timer();
				_scrollTimer.Interval = 200;
				_scrollTimer.Start();
				_scrollTimer.Tick += new EventHandler( _scrollTimer_Tick );
			}
		}
		private void StopScrollTimer()
		{
			if( _scrollTimer != null )
			{
				_scrollTimer.Stop();
				_scrollTimer.Dispose();
				_scrollTimer = null;
			}
		}


		public int GroupIndexFromGroup( Column groupColumn )
		{
			return ListControl.Columns.GroupedItems.IndexOf( groupColumn );
		}


		public SelectedItemsCollection SelectedItems
		{
			get
			{
				return ListControl.SelectedItems;
			}
		}


		public override void KeyDown( KeyEventArgs e )
		{
			if( RowInformation == null )
			{
				return;
			}

			bool shiftBeingPressed = (e.Modifiers & Keys.Shift) == Keys.Shift;
			bool ctrlBeingPressed = (e.Modifiers & Keys.Control) == Keys.Control;


			int? newPos = null;
			switch( e.KeyCode )
			{
				case Keys.Subtract:
				case Keys.Left:
					{
						if( FocusedItem != null )
						{
							PositionedRowIdentifier groupItem;
							int position = FocusedItem.Position;
							if( FocusedItem.RowIdentifier.GroupColumns != null )
							{
								GroupIdentifier gi = FocusedItem.RowIdentifier as GroupIdentifier;
                                if (gi != null)
                                {
                                    if (GetGroupState(gi) == GroupState.Collapsed && position > 0)
                                    {
                                        --position;
                                    }
                                }
							}
							groupItem = FindGroupFromRowIdentifier( position );
							if( groupItem != null )
							{
								SetFocusWithSelectionCheck( groupItem );
								SetGroupState( groupItem.RowIdentifier, GroupState.Collapsed, true );
							}
						}
					}
					break;
				case Keys.Add:
				case Keys.Right:
					{
						if( FocusedItem != null )
						{
							PositionedRowIdentifier groupItem = FindGroupFromRowIdentifier( FocusedItem.Position );
							if( groupItem != null )
							{
								SetGroupState( groupItem.RowIdentifier, GroupState.Expanded, false );
							}
						}
					}
					break;
				case Keys.Multiply:
					SetGlobalGroupState( GroupState.Expanded );
					break;
				case Keys.Divide:
					SetGlobalGroupState( GroupState.Collapsed );
					break;
				case Keys.Down:
					newPos = _focusedItem == null ? 0 : _focusedItem.Position + 1;
					break;
				case Keys.Up:
					if( _focusedItem != null )
					{
						newPos = _focusedItem.Position - 1;
					}
					break;
				case Keys.Space:
					if( FocusedItem != null )
					{
						if( shiftBeingPressed )
						{
							SelectedItems.AddInternal( FocusedItem.RowIdentifier );
						}
						else
						{
							SelectedItems.ClearAndAdd( FocusedItem.RowIdentifier );
						}
						return;
					}
					break;
				case Keys.PageDown:
					newPos = _focusedItem == null ? _countDisplayed : _focusedItem.Position + _countDisplayed;
					break;
				case Keys.PageUp:
					if( _focusedItem != null )
					{
						newPos = _focusedItem.Position - _countDisplayed;
					}
					break;
				case Keys.End:
					newPos = RowInformation.Count;
					break;
				case Keys.Home:
					newPos = 0;
					break;
			}
			if( newPos == null )
			{
				return; // nothing to do
			}


			//
			// Bounds check
			if( newPos.Value >= RowInformation.Count )
			{
				newPos = RowInformation.Count - 1;
			}
			if( newPos.Value < 0 )
			{
				newPos = 0;
			}

			if( newPos.Value < RowInformation.Count )
			{
				RowIdentifier ri = RowInformation[newPos.Value];
				PositionedRowIdentifier newFocusedItem = new PositionedRowIdentifier( ri, newPos.Value );
				if( ctrlBeingPressed )
				{
					FocusedItem = newFocusedItem;
				}
				else
				{
					SetFocusWithSelectionCheck( newFocusedItem );
				}
			}
		}

		public override void MouseWheel( MouseEventArgs e )
		{
			base.MouseWheel( e );

			if( VerticalScrollbarVisible )
			{
				int newScrollValue = VScrollbar.Value - e.Delta * System.Windows.Forms.SystemInformation.MouseWheelScrollLines / 120;

				if( newScrollValue < VScrollbar.Minimum )
				{
					newScrollValue = VScrollbar.Minimum;
				}

				if( newScrollValue > VScrollbar.Maximum - VScrollbar.LargeChange )
				{
					newScrollValue = VScrollbar.Maximum - VScrollbar.LargeChange + 1;
				}

				VScrollbar.Value = newScrollValue;
			}
		}

		public override void MouseClick( MouseEventArgs e )
		{
			base.MouseClick( e );

			if( !_mouseClickHandled && e.Button != MouseButtons.Right )
			{
				_mouseClickHandled = true;
				RowSection rowSectionSelected = GetRowSectionFromPoint( new Point( e.X, e.Y ) );
				if( rowSectionSelected == null )
				{
					return;
				}


				RowIdentifier ri = rowSectionSelected.RowIdentifier;
				PositionedRowIdentifier si = new PositionedRowIdentifier( ri, rowSectionSelected.Position );
				SetFocusWithSelectionCheck( si );
			}
		}

		private bool _mouseClickHandled = false;

		public override void MouseDown( MouseEventArgs e )
		{
			RowSection rowSectionSelected = GetRowSectionFromPoint( new Point( e.X, e.Y ) );
			if( rowSectionSelected == null )
			{
				return;
			}


			RowIdentifier ri = rowSectionSelected.RowIdentifier;
			PositionedRowIdentifier si = new PositionedRowIdentifier( ri, rowSectionSelected.Position );
			if( !SelectedItems.Contains( ri ) )
			{
				SetFocusWithSelectionCheck( si );
				_mouseClickHandled = true;
			}
			else
			{
				_mouseClickHandled = false;
				FocusedItem = si;
			}
		}

		public override void PaintBackground( Section.GraphicsSettings gs, Rectangle clipRect )
		{
			gs.Graphics.FillRectangle( SystemBrushes.Window, Rectangle );
			base.PaintBackground( gs, clipRect );
		}

		#region Group Control

		public enum GroupState
		{
			Collapsed,
			Expanded
		} ;

		public GroupState GetGroupState( RowIdentifier ri )
		{
			GroupState opposite = _globalGroupState == GroupState.Expanded ? GroupState.Collapsed : GroupState.Expanded;
			GroupState gs = _groupExpansionState.Contains( ri ) ? opposite : _globalGroupState;
			GroupState gsCopy = gs;
			ListControl.GetGroupedState( ri, ref gs );
			if( gsCopy != gs )
			{
				if( _groupExpansionState.Contains( ri ) )
				{
					_groupExpansionState.Remove( ri );
				}
				else
				{
					_groupExpansionState.Add( ri );
				}
			}
			return gs;
		}

		public void SetGroupState( RowIdentifier ri, GroupState groupState, bool layoutNow )
		{
			System.Diagnostics.Debug.Assert( ri.GroupColumns != null );

			if( FocusedItem != null && FocusedItem.RowIdentifier.GroupColumns.Length == 0 && Array.IndexOf( ri.Items, FocusedItem.RowIdentifier.Items[0] ) != -1 )
			{
				bool isSelected = SelectedItems.Contains( FocusedItem.RowIdentifier );

				PositionedRowIdentifier pri = GetPositionedIdentifierFromObject( ri );
				if( pri != null )
				{
					if( !SelectedItems.Contains( pri.RowIdentifier ) )
					{
						SelectedItems.Add( pri.RowIdentifier );
					}
					FocusedItem = pri;
				}
			}

			if( GetGroupState( ri ) != groupState )
			{
				if( _globalGroupState == groupState )
				{
					_groupExpansionState.Remove( ri );
				}
				else
				{
					_groupExpansionState.Add( ri );
				}
				ClearRowInformation();
				if( layoutNow )
				{
					Layout();
				}
				else
				{
					Host.LazyLayout( this );
				}
			}
		}

		public void SetGlobalGroupState( GroupState globalGroupState )
		{
			_groupExpansionState.Clear();
			_globalGroupState = globalGroupState;
			ClearRowInformation();
			Host.LazyLayout( this );
		}

		#endregion

		public bool HasFocus( RowIdentifier ri )
		{
			return Host.FocusedSection == this && _focusedItem != null && _focusedItem.RowIdentifier == ri;
		}


		internal ItemLists.ItemList ItemList
		{
			get
			{
				return ListControl.ItemList;
			}
		}

		internal ColumnList Columns
		{
			get
			{
				return ListControl.Columns;
			}
		}

		internal void ListUpdated( bool lazyLayout )
		{
			ClearRowInformation();
			if( Host.IsControlCreated )
			{
				if( lazyLayout )
				{
					Host.LazyLayout( this );
				}
				else
				{
					Layout();
				}
			}
		}

        public class GroupIdentifier : RowIdentifier
		{
			public GroupIdentifier( int start, object[] listItems, ColumnList columns, int groupIndex, object item )
			{
				EventingList<Column> groupColumns = columns.GroupedItems;
				_listItems = listItems;
				_groupValues = new object[groupIndex + 1];
				_groupColumns = new Column[groupIndex + 1];
				_hashCode = 0;
				for( int i = 0; i <= groupIndex; i++ )
				{
					string s = groupColumns[i].GroupItemAccessor( item ).ToString();
					_groupValues[i] = s;
					_groupColumns[i] = groupColumns[i];
					_hashCode += s.GetHashCode();
				}
				Start = start;
				End = start;
			}

			/// <summary>
			/// Used only for serialization.
			/// </summary>
			/// <param name="groupValues"></param>
			/// <param name="groupColumn"></param>
			internal GroupIdentifier( object[] groupValues, Column groupColumn )
			{
				_hashCode = 0;
				foreach( object o in groupValues )
				{
					_hashCode += o.GetHashCode();
				}
				Start = -1;
				End = -1;
				_listItems = null;
				_groupValues = groupValues;
				_groupColumns = new Column[] { groupColumn };
			}

			public int Start;
			public int End;

			public override bool Equals( object obj )
			{
				if( ReferenceEquals( this, obj ) )
				{
					return true;
				}

				GroupIdentifier other = obj as GroupIdentifier;
				if( ReferenceEquals( other, null ) || other._groupValues.Length != _groupValues.Length )
				{
					return false;
				}

				for( int i = 0; i < _groupValues.Length; i++ )
				{
					if( !_groupValues[i].Equals( other._groupValues[i] ) )
					{
						return false;
					}
				}

				return true;
			}

			public override int GetHashCode()
			{
				return _hashCode;
			}


			public override Column[] GroupColumns
			{
				get
				{
					return _groupColumns;
				}
			}

			public override object[] GroupValues
			{
				get
				{
					return _groupValues;
				}
			}


			public override object[] Items
			{
				get
				{
					int count = End - Start;
					object[] listItems = new object[count];
					Array.Copy( _listItems, Start, listItems, 0, count );
					return listItems;
				}
			}

			private readonly int _hashCode;
			private readonly object[] _groupValues;
			private readonly Column []_groupColumns;
			private object[] _listItems;
		}

		public class NonGroupRow : RowIdentifier
		{
			public NonGroupRow( object item )
			{
				_item = item;
			}

			public override object[] GroupValues
			{
				get 
				{ 
					return _emptyColumnValues; 
				}
			}

			public override Column[] GroupColumns
			{
				get
				{
					return _emptyColumnList;
				}
			}



			public override object[] Items
			{
				get
				{
					return new object[] { _item };
				}
			}

			public override bool Equals( object obj )
			{
				if( ReferenceEquals( this, obj ) )
				{
					return true;
				}

				NonGroupRow other = obj as NonGroupRow;
				if( ReferenceEquals( other, null ) )
				{
					return false;
				}

				return other._item == _item;
			}

			public override int GetHashCode()
			{
				return _item.GetHashCode();
			}

			private object _item;
			private static readonly Column []_emptyColumnList = new Column[]{};
			private static readonly object[] _emptyColumnValues = new object[] { };
		}


		internal PositionedRowIdentifier FocusedItem
		{
			get
			{
				return Host.FocusedSection == this ? _focusedItem : null;
			}
			set
			{
				if( _focusedItem != value )
				{
					RowSection oldFocusedItem = _focusedItem == null ? null : RowSectionFromRowIdentifier( _focusedItem.RowIdentifier );

					_focusedItem = value;

					if( oldFocusedItem != null )
					{
						oldFocusedItem.LostFocus();
					}

					if( _focusedItem != null )
					{
						RowSection focusedSection = RowSectionFromRowIdentifier( _focusedItem.RowIdentifier );
						Rectangle rowsRect = WorkingRectangle;
						if( focusedSection != null && focusedSection.Rectangle.Top >= rowsRect.Top && focusedSection.Rectangle.Bottom <= rowsRect.Bottom )
						{
							focusedSection.GotFocus();
						}
						else
						{
							//
							// focused item is not in view so we make it visible
							int position = PositionFromRowIdentifier( _focusedItem.RowIdentifier );
							if( position < LineStart )
							{
								_layoutDirection = Direction.Forward;
							}
							else
							{
								_layoutDirection = Direction.Backward;
							}
							LineStart = position;
						}
					}
					ListControl.FireFocusChanged( oldFocusedItem == null ? null : oldFocusedItem.RowIdentifier,
																			 _focusedItem == null ? null : _focusedItem.RowIdentifier );
				}
			}
		}


		private enum Direction
		{
			Forward,
			Backward
		} ;

		private RowSection GetRowSectionFromPoint( Point pt )
		{
			Section section = SectionFromPoint( pt );
			while( section != null && !(section is RowSection) )
			{
				section = section.Parent;
			}
			return (RowSection)section;
		}

		private void SetFocusWithSelectionCheck( PositionedRowIdentifier si )
		{
			FocusedItem = si;

			bool shiftBeingPressed = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
			bool ctrlBeingPressed = (Control.ModifierKeys & Keys.Control) == Keys.Control;


			if( AllowMultiSelect && _lastRowSelected != null && shiftBeingPressed )
			{
				RowIdentifier anchor = _lastRowSelected.RowIdentifier;
				int from;
				int to;
				if( _lastRowSelected.Position < si.Position )
				{
					from = _lastRowSelected.Position;
					to = si.Position;
				}
				else
				{
					to = _lastRowSelected.Position;
					from = si.Position;
				}
				List<RowIdentifier> selectedItems = new List<RowIdentifier>();
				foreach( RowIdentifier rowInfo in GetRowEnumerator( from, Direction.Forward ) )
				{
					selectedItems.Add( rowInfo );

					if( from++ == to )
					{
						break;
					}
				}
				if( ctrlBeingPressed )
				{
					SelectedItems.AddRangeInternal( selectedItems.ToArray() );
				}
				else
				{
					SelectedItems.ClearAndAdd( selectedItems.ToArray() );
				}
			}
			else
			{
				if( ctrlBeingPressed )
				{
					if( SelectedItems.IsSelected( si.RowIdentifier ) )
					{
						SelectedItems.RemoveInternal( si.RowIdentifier );
					}
					else
					{
						if( AllowMultiSelect )
						{
							// if MultiSelect is set to true, add the newly clicked item to the selected items collection
							SelectedItems.AddInternal( si.RowIdentifier );
						}
						else
						{
							// if MutliSelect is set to false, clear the selected items collection and add the newly clicked item.
							SelectedItems.ClearAndAdd( si.RowIdentifier );
						}
					}
				}
				else
				{
					if( !SelectedItems.IsSelected( si.RowIdentifier ) || SelectedItems.Count != 1 )
					{
						SelectedItems.Clear();
						SelectedItems.AddInternal( si.RowIdentifier );
					}
				}
				_lastRowSelected = si;
			}
		}


		private IEnumerable<RowIdentifier> GetRowEnumerator( int lineStart, Direction direction )
		{
			if( ItemList.Count > _scrollbarMax )
			{
				throw new NotImplementedException();
			}
			else
			{
				if( RowInformation == null )
				{
					CalculateListItems();
				}
				switch( direction )
				{
					case Direction.Forward:
						for( int i = lineStart; i < RowInformation.Count; i++ )
						{
							yield return RowInformation[i];
						}
						break;
					case Direction.Backward:
						for( int i = lineStart; i >= 0; i-- )
						{
							if( i < RowInformation.Count )
							{
								yield return RowInformation[i];
							}
						}
						break;
				}
			}
		}

		private static int ItemInNewGroup( Column[] groupColumns, object itemNow, object itemBefore )
		{
			int iColumn = 0;
			foreach( Column column in groupColumns )
			{
				if( itemBefore == null || !column.GroupItemAccessor( itemNow ).Equals( column.GroupItemAccessor( itemBefore ) ) )
				{
					return iColumn;
				}
				iColumn++;
			}
			return -1;
		}

		private ListControl ListControl
		{
			get
			{
				return (ListControl)Host;
			}
		}

		#region ILayoutController Members

		int HeaderSection.ILayoutController.ReservedNearSpace
		{
			get
			{
				return ListControl.Columns.GroupedItems.Count * _groupIndent;
			}
		}

		void HeaderSection.ILayoutController.HeaderLayedOut()
		{
			if( HorizontalScrollbarVisible )
			{
				SetHScrollInfo();
			}
		}

		public int CurrentHorizontalScrollPosition
		{
			get
			{
				return HorizontalScrollbarVisible ? HScrollbar.Value : 0;
			}
			set
			{
				HorizontalScrollbarVisible = true;
				HScrollbar.Value = value;
			}
		}

		#endregion

		private void Layout()
		{
			if( _enforceLazyLayout )
			{
				Host.LazyLayout( this );
			}
			else
			{
				using( Graphics grfx = Host.CreateGraphics() )
				{
					Layout( new GraphicsSettings( grfx ), Rectangle.Size );
					Invalidate();
				}
			}
		}

		protected override void SetVScrollInfo()
		{
			VScrollbar.Minimum = 0;
			VScrollbar.SmallChange = 1;
			VScrollbar.LargeChange = _countDisplayed;

			if( ItemList.Count > _scrollbarMax )
			{
				VScrollbar.Maximum = _scrollbarMax - 1;
				ClearRowInformation();
			}
			else
			{
				VScrollbar.Maximum = CalculateListItems() - 1;
			}

			int required;

			switch( _layoutDirection )
			{
				case Direction.Forward:
					required = LineStart;
					break;
				case Direction.Backward:
					required = LineStart - _countDisplayed + 1;
					break;
				default:
					throw new InvalidOperationException();
			}

			required = Math.Min( Math.Max( required, VScrollbar.Minimum ), VScrollbar.Maximum );

			try
			{
				VScrollbar.Value = required;
			}
			catch( ArgumentOutOfRangeException )
			{
			}
		}

		internal class PositionedRowIdentifier
		{
			public PositionedRowIdentifier( RowIdentifier rowIdentifier, int position )
			{
				RowIdentifier = rowIdentifier;
				Position = position;
			}

			public override bool Equals( object obj )
			{
				return RowIdentifier.Equals( obj );
			}

			public override int GetHashCode()
			{
				return RowIdentifier.GetHashCode();
			}

			public static bool operator ==( PositionedRowIdentifier lhs, PositionedRowIdentifier rhs )
			{
				if( ReferenceEquals( lhs, rhs ) )
				{
					return true;
				}
				if( ReferenceEquals( lhs, null ) )
				{
					return false;
				}
				if( ReferenceEquals( rhs, null ) )
				{
					return false;
				}

				return lhs.RowIdentifier == rhs.RowIdentifier && lhs.Position == rhs.Position;
			}

			public static bool operator !=( PositionedRowIdentifier lhs, PositionedRowIdentifier rhs )
			{
				return !(lhs == rhs);
			}

			public readonly RowIdentifier RowIdentifier;
			public int Position;
		}

		protected override void SetHScrollInfo()
		{
			HScrollbar.LargeChange = Rectangle.Width;
			HScrollbar.Minimum = 0;
			HScrollbar.Maximum = MaxWidth;
			HScrollbar.SmallChange = 1;
		}

		private int MaxWidth
		{
			get
			{
				if( _headerSection.Columns.Count == 0 && !_headerSection.IsVisible )
				{
					int max = 0;
					foreach( Section section in Children )
					{
						if( max < section.Rectangle.Width )
						{
							max = section.Rectangle.Width;
						}
					}
					return max;
				}
				else
				{
					return _headerSection.IdealWidth - 1;
				}
			}
		}

		private void HandleSyncing( RowIdentifier ri, int newPosition, PositionedRowIdentifier[] trackableItems, List<PositionedRowIdentifier> newSelection )
		{
			foreach( PositionedRowIdentifier si in trackableItems )
			{
				if( si != null )
				{
					if( ri.Equals( si.RowIdentifier ) )
					{
						si.Position = newPosition;
					}
				}
			}
			if( SelectedItems.Contains( ri ) )
			{
				newSelection.Add( new PositionedRowIdentifier( ri, newPosition ) );
			}
		}

		protected override void OnVScrollValueChanged( int value )
		{
			int newPosition;
			if( ItemList.Count > _scrollbarMax )
			{
				newPosition = (int)(value / (float)_scrollbarMax * ItemList.Count);
			}
			else
			{
				newPosition = value;
			}
			switch( _layoutDirection )
			{
				case Direction.Forward:
					LineStart = newPosition;
					break;
				case Direction.Backward:
					if( LineStart - _countDisplayed + 1 != newPosition )
					{
						_layoutDirection = Direction.Forward;
						LineStart = newPosition;
					}
					break;
			}
		}


		private void GroupedItems_DataChanged( object sender, EventingList<Column>.EventInfo e )
		{
			//
			// Grouping has changed so clear collapse point, maybe later we can do something to preserve items state.
			_groupExpansionState.Clear();
		}


		private RowSection RowSectionFromRowIdentifier( RowIdentifier ri )
		{
			foreach( Section section in Children )
			{
				RowSection rowSection = section as RowSection;
				if( rowSection != null && rowSection.RowIdentifier == ri )
				{
					return rowSection;
				}
			}
			return null;
		}


		private void SetVScrollValue()
		{
			if( VScrollbar != null )
			{
				switch( _layoutDirection )
				{
					case Direction.Forward:
						VScrollbar.Value = LineStart;
						System.Diagnostics.Debug.Assert( VScrollbar.Value >= 0 );
						break;
					case Direction.Backward:
						VScrollbar.Value = LineStart - _countDisplayed + 1;
						System.Diagnostics.Debug.Assert( VScrollbar.Value >= 0 );
						break;
				}
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			StopScrollTimer();
		}

		private PositionedRowIdentifier SelectedItemFromRowIdentifer( RowIdentifier ri )
		{
			return new PositionedRowIdentifier( ri, PositionFromRowIdentifier( ri ) );
		}

		private class EnforceLazyLayout : IDisposable
		{
			public EnforceLazyLayout( ListSection listSection )
			{
				_listSection = listSection;
				_savedEnforceLazyLayout = listSection._enforceLazyLayout;
				_listSection._enforceLazyLayout = true;
			}

			public void Dispose()
			{
				_listSection._enforceLazyLayout = _savedEnforceLazyLayout;
			}

			private bool _savedEnforceLazyLayout = false;
			private ListSection _listSection;
		}

		private List<RowIdentifier> _lastCalculatedRowInformation = null;

		private static bool IsValidNextRow( RowIdentifier ri, RowIdentifier riCompare, ItemLists.ItemList itemList )
		{
			//
			// Only settle on same type rows.
			if( (ri.GroupColumns.Length == 0 ^ riCompare.GroupColumns.Length == 0 ) == false )
			{
				//
				// Only include items that exist now.
				if( itemList.IndexOf( riCompare.Items[0] ) != -1 )
				{
					return true;
				}
			}
			return false;
		}

		private static RowIdentifier locateNearestRowThatsStillValid( PositionedRowIdentifier current, ItemLists.ItemList itemList, List<RowIdentifier> oldList )
		{
			//
			// Check forward first
			for( int i = current.Position; i < oldList.Count; i++ )
			{
				RowIdentifier ri = oldList[i];
				if( IsValidNextRow( current.RowIdentifier, ri, itemList ) )
				{
					return ri;
				}
			}

			//
			// Check backwards now
			for( int i = current.Position - 1; i >= 0 && i < oldList.Count; i-- )
			{
				RowIdentifier ri = oldList[i];
				if( IsValidNextRow( current.RowIdentifier, ri, itemList ) )
				{
					return ri;
				}
			}

			return null;
		}

		/// <summary>
		/// Returns the number of items in the list adjusted for the group items.
		/// This method will also create a bit array of types <seealso cref="_positionTypes"/>
		/// </summary>
		/// <returns></returns>
		private int CalculateListItems()
		{
			using( new EnforceLazyLayout( this ) )
			{
				if( _rowInformation == null )
				{
					PositionedRowIdentifier[] trackableItems = new PositionedRowIdentifier[3];
					trackableItems[0] = _focusedItem;

					if( _lastCalculatedRowInformation != null )
					{
						if( FocusedItem != null )
						{
							if( LineStart < _lastCalculatedRowInformation.Count )
							{
								trackableItems[1] = new PositionedRowIdentifier( _lastCalculatedRowInformation[LineStart], LineStart );
							}
							RowIdentifier nextValidRow = locateNearestRowThatsStillValid( FocusedItem, ItemList, _lastCalculatedRowInformation );
							if( nextValidRow != null )
							{
								trackableItems[2] = new PositionedRowIdentifier( nextValidRow, 0 );
							}
						}
					}

					Column[] groupColumns = ListControl.Columns.GroupedItems.ToArray();
					object lastItem = null;
					_lastCalculatedRowInformation = _rowInformation = new List<RowIdentifier>( ItemList.Count );
					int hideRowGroupIndex = -1;
					List<PositionedRowIdentifier> newSelection = new List<PositionedRowIdentifier>();


					GroupIdentifier[] activeGroups = new GroupIdentifier[groupColumns.Length];
					object[] listItems = ItemList.ToArray();
					for( int i = 0; i < ItemList.Count; i++ )
					{
						object item = ItemList[i];

						int groupIndex = ItemInNewGroup( groupColumns, item, lastItem );
						if( groupIndex != -1 )
						{
							for( int iGroup = groupIndex; iGroup < groupColumns.Length; iGroup++ )
							{
								GroupIdentifier gi = new GroupIdentifier( i, listItems, Columns, iGroup, item );
								int hideGroupIndex = hideRowGroupIndex;
								if( hideRowGroupIndex == -1 || iGroup <= hideRowGroupIndex )
								{
									if( GetGroupState( gi ) == GroupState.Collapsed )
									{
										hideGroupIndex = iGroup;
										if( groupIndex <= hideRowGroupIndex )
										{
											hideRowGroupIndex = -1;
										}
									}
									else
									{
										hideRowGroupIndex = hideGroupIndex = -1;
									}
								}

								if( hideRowGroupIndex == -1 )
								{
									HandleSyncing( gi, _rowInformation.Count, trackableItems, newSelection );
									_rowInformation.Add( gi );
								}
								if( activeGroups[iGroup] != null )
								{
									activeGroups[iGroup].End = i;
								}
								activeGroups[iGroup] = gi;
								hideRowGroupIndex = hideGroupIndex;
							}
						}
						if( hideRowGroupIndex == -1 )
						{
							RowIdentifier ri = new NonGroupRow( item );
							HandleSyncing( ri, _rowInformation.Count, trackableItems, newSelection );
							_rowInformation.Add( ri );
						}
						lastItem = item;
					}

					foreach( GroupIdentifier gi in activeGroups )
					{
						if( gi != null )
						{
							gi.End = ItemList.Count;
						}
					}
					if( VerticalScrollbarVisible )
					{
						int newMax = _rowInformation.Count == 0 ? 0 : _rowInformation.Count - 1;
						if( VScrollbar.Value >= newMax )
						{
							VScrollbar.Value = newMax;
						}
						VScrollbar.Maximum = newMax;
					}

					if( trackableItems[1] != null )
					{
						LineStart = trackableItems[1].Position;
					}
					if( _focusedItem != null && !IsSelectedItemValid( _focusedItem ) )
					{
						if( trackableItems[2] != null && IsSelectedItemValid( trackableItems[2] ) )
						{
							PositionedRowIdentifier oldFocusedItem = _focusedItem;
							if( _focusedItem != trackableItems[2] )
							{
								_focusedItem = trackableItems[2];
								ListControl.FireFocusChanged( oldFocusedItem == null ? null : oldFocusedItem.RowIdentifier,
																						 _focusedItem == null ? null : _focusedItem.RowIdentifier );
							}
							newSelection.Add( _focusedItem );
						}
						else
						{
							int newPosition;
							if( _focusedItem.Position >= _rowInformation.Count )
							{
								newPosition = _rowInformation.Count - 1;
							}
							else
							{
								newPosition = _focusedItem.Position;
							}
							if( newPosition >= 0 )
							{
								PositionedRowIdentifier si = new PositionedRowIdentifier( _rowInformation[newPosition], newPosition );
								FocusedItem = si;
								newSelection.Add( _focusedItem );
							}
							else
							{
								FocusedItem = null;
							}
						}
					}
					RowIdentifier[] rowItemsToSelect = new RowIdentifier[newSelection.Count];
					int j = 0;
					foreach( PositionedRowIdentifier pri in newSelection )
					{
						rowItemsToSelect[j++] = pri.RowIdentifier;
					}
					SelectedItems.ClearAndAdd( rowItemsToSelect );
					if( SelectedItems.Count == 0 && FocusedItem == null && VScrollbar != null && VScrollbar.Visible )
					{
						VScrollbar.Value = 0;
					}
				}
				_mapOfPositions = null;
			}
			ListControl.OnCalculatedGroupRows( EventArgs.Empty );
			return _rowInformation.Count;
		}

		private bool IsSelectedItemValid( PositionedRowIdentifier item )
		{
			return item != null && (item.Position < _rowInformation.Count && _rowInformation[item.Position] == item.RowIdentifier);
		}

		private PositionedRowIdentifier FindGroupFromRowIdentifier( int position )
		{
			if( position < RowInformation.Count )
			{
				for( int i = position; i >= 0; i-- )
				{
					GroupIdentifier gi = RowInformation[i] as GroupIdentifier;
					if( gi != null )
					{
						return new PositionedRowIdentifier( gi, i );
					}
				}
			}
			return null;
		}

        public List<RowIdentifier> RowInformation
		{
			get
			{
				if( _rowInformation == null )
				{
					CalculateListItems();
				}
				return _rowInformation;
			}
		}

		private void ClearRowInformation()
		{
			_rowInformation = null;
			_mapOfPositions = null;
		}

		private Dictionary<RowIdentifier, int> GetRowPosMap()
		{
			Dictionary<RowIdentifier, int> mapOfPositions = _mapOfPositions == null ? null : (Dictionary<RowIdentifier, int>)_mapOfPositions.Target;
			if( mapOfPositions == null )
			{
				List<RowIdentifier> rowInformation = _rowInformation == null ? _lastCalculatedRowInformation : _rowInformation;

				if( rowInformation == null )
				{
					CalculateListItems();
				}

				mapOfPositions = new Dictionary<RowIdentifier, int>();
				if( rowInformation != null )
				{
					int i = 0;
					foreach( RowIdentifier ri in rowInformation )
					{
						mapOfPositions[ri] = i++;
					}
				}

				_mapOfPositions = new WeakReference( mapOfPositions );
			}
			return mapOfPositions;
		}

		internal PositionedRowIdentifier GetPositionedIdentifierFromObject( RowIdentifier identifier )
		{
			int position;
			if( GetRowPosMap().TryGetValue( identifier, out position ) )
			{
				return new PositionedRowIdentifier( identifier, position );
			}

			return null;
		}

		private int PositionFromRowIdentifier( RowIdentifier ri )
		{
			int position;
			if( GetRowPosMap().TryGetValue( ri, out position ) )
			{
				return position;
			}

			return -1;
		}


		public bool ShowHeaderSection
		{
			get
			{
				return _headerSection.IsVisible;
			}
			set
			{
				if( _headerSection.IsVisible != value )
				{
					_headerSection.IsVisible = value;
					LazyLayout();
				}
			}
		}

		public int CalculatedRowsCount
		{
			get
			{
				return _rowInformation == null ? 0 : _rowInformation.Count;
			}
		}

		private int LineStart
		{
			get
			{
				return _lineStart;
			}
			set
			{
				if( _lineStart != value )
				{
					_lineStart = value;
					Layout();
				}
			}
		}

		private void SelectedItems_DataChanged( object sender, SelectedItemsChangedEventArgs e )
		{
			foreach( Section s in Children )
			{
				RowSection rs = s as RowSection;
				if( rs != null )
				{
					if( SelectedItems.Contains( rs.RowIdentifier ) != rs.DrawnSelected )
					{
						if( rs.NeedsLayoutOnSelection )
						{
							Layout();
							break;
						}
						else
						{
							rs.Invalidate();
						}
					}
				}
			}
		}
		private void _scrollTimer_Tick( object sender, EventArgs e )
		{
			Host.ProcessLayoutsNow();

			if( IsNearStart( Host.CurrentSectionDraggedOver ) && VScrollbar.Value > 0 )
			{
				VScrollbar.Value--;
				return;
			}
			if( IsNearEnd( Host.CurrentSectionDraggedOver ) && VScrollbar.Value < VScrollbar.Maximum )
			{
				VScrollbar.Value++;
				return;
			}
			if( Host.CurrentSectionDraggedOver == null )
			{
				StopScrollTimer();
			}
		}
		private Timer _scrollTimer = null;
		private bool _allowRowDragDrop = false;
		private WeakReference _mapOfPositions = null;
		private List<RowIdentifier> _rowInformation = null;
		private Direction _layoutDirection = Direction.Forward;
		private bool _allowMultiSelect = true;
		private GroupState _globalGroupState = GroupState.Expanded;
		private PositionedRowIdentifier _lastRowSelected = null;
		private PositionedRowIdentifier _focusedItem = null;
		private Set<RowIdentifier> _groupExpansionState = new Set<RowIdentifier>();
		private int _lineStart = 0;
		private int _countDisplayed = 0;
		private const int _scrollbarMax = int.MaxValue;
		private int _groupIndent = 10;
		private HeaderSection _headerSection;
		private bool _enforceLazyLayout = false;
	}
}