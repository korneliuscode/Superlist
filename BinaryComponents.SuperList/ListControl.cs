/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using BinaryComponents.SuperList.ItemLists;
using BinaryComponents.SuperList.Sections;
using BinaryComponents.WinFormsUtility.Controls;
using BinaryComponents.WinFormsUtility.Drawing;

namespace BinaryComponents.SuperList
{
	public class ListControl : SectionContainerControl
	{
		public ListControl( SectionFactory sectionFactory )
			: base( sectionFactory )
		{
			InitializeComponent();

			_itemList = new BufferedList();
			_itemList.ListControl = this;
			_selectedItems = new SelectedItemsCollection( this );

			_customiseListSection = SectionFactory.CreateCustomiseListSection( this );
			_listSection = SectionFactory.CreateListSection( this );

			Canvas.Children.Add( _customiseListSection );
			Canvas.Children.Add( _listSection );
		}

		public ListControl()
			: this( new SectionFactory() )
		{
		}

		public virtual void SerializeState( TextWriter writer )
		{
			ListSection.SerializeState( writer );
		}

		public virtual void DeSerializeState( TextReader reader )
		{
			ListSection.DeSerializeState( reader );
		}

		public void LayoutSections()
		{
			Canvas.Host.LazyLayout( Canvas );
		}

		/// <summary>
		/// Sets the column width to fit the columns contents.
		/// </summary>
		public void SizeColumnsToFit()
		{
			SizeColumnsToFit( Columns.ToArray() );
		}

		/// <summary>
		/// Sets the column width to fit the specified columns contents.
		/// </summary>
		/// <param name="columns">Columns to size</param>
		public void SizeColumnsToFit( Column[] columns )
		{
			ListSection.SizeColumnsToFit( columns );
		}

		public bool MultiSelect
		{
			get
			{
				return _listSection.AllowMultiSelect;
			}
			set
			{
				_listSection.AllowMultiSelect = value;
			}
		}

		public object FocusedItem
		{
			get
			{
				return _listSection.FocusedItem == null ? null : _listSection.FocusedItem.RowIdentifier.Items[0];
			}
			set
			{
				if( value != null )
				{
					ListSection.PositionedRowIdentifier si = _listSection.GetPositionedIdentifierFromObject( new ListSection.NonGroupRow( value ) );

					if( si != null )
					{
						int pos = si.Position - 1;

						while( pos >= 0 )
						{
							RowIdentifier gi = _listSection.RowInformation[pos];

							if( gi is ListSection.GroupIdentifier )
							{
								_listSection.FocusedItem = new ListSection.PositionedRowIdentifier( gi, pos );

								--pos;
							}
							else
							{
								break;
							}
						}

						_listSection.FocusedItem = si;
					}
				}
				else
				{
					_listSection.FocusedItem = null;
				}
			}
		}

		/// <summary>
		/// Override this method if you want to change the expansion state of a grouped item.
		/// </summary>
		/// <param name="ri"></param>
		/// <param name="currentState"></param>
		public virtual void GetGroupedState( RowIdentifier ri, ref ListSection.GroupState currentState )
		{
		}

		#region Customization

		public bool ShowHeaderSection
		{
			get
			{
				return _listSection.ShowHeaderSection;
			}
			set
			{
				_listSection.ShowHeaderSection = value;
			}
		}

		public bool ShowCustomizeSection
		{
			get
			{
				return _customiseListSection != null;
			}
			set
			{
				if( ShowCustomizeSection != value )
				{
					if( _customiseListSection == null )
					{
						_customiseListSection = SectionFactory.CreateCustomiseListSection( this );
						Canvas.Children.Insert( 0, _customiseListSection );
					}
					else
					{
						Canvas.Children.Remove( _customiseListSection );
						_customiseListSection.Dispose();
						_customiseListSection = null;
					}
					Canvas.Host.LazyLayout( Canvas );
				}
			}
		}

		public ColumnList Columns
		{
			get
			{
				return _columns;
			}
		}

		public ItemList Items
		{
			get
			{
				return _itemList;
			}
		}

		public SelectedItemsCollection SelectedItems
		{
			get
			{
				return _selectedItems;
			}
		}

		public void EnsureVisible( object o )
		{
			if( o != null )
			{
				ListSection.PositionedRowIdentifier si = _listSection.GetPositionedIdentifierFromObject( new ListSection.NonGroupRow( o ) );

				if( si != null )
				{
					int pos = si.Position - 1;

					while( pos >= 0 )
					{
						RowIdentifier gi = _listSection.RowInformation[pos];

						if( gi is ListSection.GroupIdentifier )
						{
							_listSection.FocusedItem = new ListSection.PositionedRowIdentifier( gi, pos );

							--pos;
						}
						else
						{
							break;
						}
					}

					_listSection.FocusedItem = si;
				}
			}
			else
			{
				_listSection.FocusedItem = null;
			}
		}

		public Font GroupSectionFont
		{
			get
			{
				return _groupSectionFont;
			}
			set
			{
				_groupSectionFont = value;
			}
		}

		public Color GroupSectionForeColor
		{
			get
			{
				return _groupSectionForeColor;
			}
			set
			{
				_groupSectionForeColor = value;
			}
		}

		public string GroupSectionTextSingular
		{
			get
			{
				return _groupSectionTextSingular;
			}
			set
			{
				_groupSectionTextSingular = value;
			}
		}

		public string GroupSectionTextPlural
		{
			get
			{
				return _groupSectionTextPlural;
			}
			set
			{
				_groupSectionTextPlural = value;
			}
		}

		public Color AlternatingRowColor
		{
			get
			{
				return _alternatingRowColor;
			}
			set
			{
				_alternatingRowColor = value;
			}
		}

		public GdiPlusEx.VAlignment GroupSectionVerticalAlignment
		{
			get
			{
				return _groupSectionVerticalAlignment;
			}
			set
			{
				_groupSectionVerticalAlignment = value;
			}
		}

		public Color SeparatorColor
		{
			get
			{
				return _separatorColor;
			}
			set
			{
				_separatorColor = value;
			}
		}

		public Color IndentColor
		{
			get
			{
				return _indentColor;
			}
			set
			{
				_indentColor = value;
			}
		}

		public bool AllowSorting
		{
			get
			{
				return _allowSorting;
			}
			set
			{
				if( _allowSorting != value )
				{
					_allowSorting = value;
					foreach( Column clm in Columns )
					{
						clm.ShowHeaderSortArrow = _allowSorting;
					}
					LayoutControl();
				}
			}
		}

		public override SectionFactory SectionFactory
		{
			set
			{
				base.SectionFactory = value;

				Canvas.Children.Remove( _customiseListSection );
				Canvas.Children.Remove( _listSection );
				if( _customiseListSection != null )
				{
					_customiseListSection.Dispose();
				}
				bool headerShown = _listSection.ShowHeaderSection;
				_listSection.Dispose();

				if( ShowCustomizeSection )
				{
					_customiseListSection = SectionFactory.CreateCustomiseListSection( this );
					Canvas.Children.Add( _customiseListSection );
				}


				_listSection = SectionFactory.CreateListSection( this );
				_listSection.ShowHeaderSection = headerShown;
				Canvas.Children.Add( _listSection );
			}
		}

		#endregion

		#region Events

		#region RowFocusChangedEvent

		public class RowFocusChangedEventArgs : EventArgs
		{
			public RowFocusChangedEventArgs( RowIdentifier oldFocus, RowIdentifier newFocus )
			{
				OldFocus = oldFocus;
				NewFocus = newFocus;
			}

			public readonly RowIdentifier OldFocus;
			public readonly RowIdentifier NewFocus;
		}

		public delegate void RowFocusChangedEventArgsHandler( object sender, RowFocusChangedEventArgs eventArgs );

		public event RowFocusChangedEventArgsHandler RowFocusChanged;

		protected virtual void OnRowFocusChanged( RowFocusChangedEventArgs eventArgs )
		{
			if( RowFocusChanged != null )
			{
				RowFocusChanged( this, eventArgs );
			}
		}

		#endregion

		#region RowMouseClickedEvent

		public class RowEventArgs : EventArgs
		{
			public RowEventArgs( RowIdentifier row )
			{
				Row = row;
			}

			public readonly RowIdentifier Row;
		}

		public delegate void RowMouseClickedEventArgsHandler( object sender, RowEventArgs eventArgs );

		public event RowMouseClickedEventArgsHandler MouseClickedRow;

		protected virtual void OnMouseClickedRow( RowEventArgs eventArgs )
		{
			if( MouseClickedRow != null )
			{
				MouseClickedRow( this, eventArgs );
			}
		}

		#endregion

		#region RowDoubleClickedEvent

		public event EventHandler RowDoubleClicked;

		protected virtual void OnRowDoubleClicked( RowSection rowSection )
		{
			if( RowDoubleClicked != null )
			{
				RowDoubleClicked( rowSection, EventArgs.Empty );
			}
		}

		#endregion

		#region CellMouseClickedEvent

		public class CellEventArgs : EventArgs
		{
			public CellEventArgs( CellSection cell )
			{
				Cell = cell;
			}

			public readonly CellSection Cell;
		}

		public delegate void CellMouseClickedEventArgsHandler( object sender, CellEventArgs eventArgs );

		public event CellMouseClickedEventArgsHandler MouseClickedCell;

		protected virtual void OnMouseClickedCell( CellEventArgs eventArgs )
		{
			if( MouseClickedCell != null )
			{
				MouseClickedCell( this, eventArgs );
			}
		}

		#endregion

		#endregion

		#region Drag & Drop
		public bool AllowRowDragDrop
		{
			get
			{
				return _listSection.AllowRowDragDrop;
			}
			set
			{
				_listSection.AllowRowDragDrop = value;
			}
		}
		#endregion

		#region Implementation

		public void PerformMouseWheel( int delta )
		{
			if( _listSection != null )
			{
				_listSection.MouseWheel( new MouseEventArgs( MouseButtons.None, 0, 0, 0, delta ) );
			}
		}

		protected override void OnMouseWheel( MouseEventArgs e )
		{
			base.OnMouseWheel( e );

			if( _listSection != null )
			{
				_listSection.MouseWheel( e );
			}
		}

		protected internal ItemList ItemList
		{
			get
			{
				return _itemList;
			}
		}

		protected override void OnHandleCreated( EventArgs e )
		{
			base.OnHandleCreated( e );

			_timer = new Timer();
			_timer.Interval = 50;
			_timer.Tick += _timer_Tick;
			_timer.Start();
		}

		protected override void OnHandleDestroyed( EventArgs e )
		{
			base.OnHandleDestroyed( e );

			_timer.Tick -= _timer_Tick;
			_timer.Stop();
			_timer.Dispose();
			_timer = null;
		}

		private void _timer_Tick( object sender, EventArgs e )
		{
			if( ControlUtils.IsClientRectangleVisible( this, ClientRectangle ) )
			{
				ItemList.DoHouseKeeping();
			}
		}

		protected override bool IsInputKey( Keys keyData )
		{
			Keys[] keysWereInterestedIn = new Keys[] { Keys.Space, Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Multiply, Keys.Subtract };

			foreach( Keys key in keysWereInterestedIn )
			{
				if( (keyData & key) == key )
				{
					return true;
				}
			}
			return false;
		}

		public ListSection ListSection
		{
			get
			{
				return _listSection;
			}
		}

		public event EventHandler CalculatedGroupRows;

		protected internal virtual void OnCalculatedGroupRows( EventArgs e )
		{
			if( CalculatedGroupRows != null )
			{
				CalculatedGroupRows( this, e );
			}
		}


		internal void UpdateListSection( bool lazyLayout )
		{
			_listSection.ListUpdated( lazyLayout );
		}

		protected override void OnMouseClick( MouseEventArgs e )
		{
			base.OnMouseClick( e );

			Section section = SectionFromPoint( e.Location );
			while( section != null && !(section is RowSection) )
			{
				section = section.Parent;
			}

			if( section != null )
			{
				RowSection rs = section as RowSection;
				OnMouseClickedRow( new RowEventArgs( rs.RowIdentifier ) );

				CellSection cs = rs.CellSectionFromPoint( new Point( e.X, e.Y ) );
				if( cs != null )
				{
					OnMouseClickedCell( new CellEventArgs( cs ) );
				}
			}
		}

		protected override void OnDoubleClick( EventArgs e )
		{
			base.OnDoubleClick( e );
			if( RowDoubleClicked != null )
			{
				Section section = SectionFromPoint( PointToClient( MousePosition ) );
				while( section != null )
				{
					if( section is RowSection )
					{
						OnRowDoubleClicked( (RowSection)section );
						break;
					}
					section = section.Parent;
				}
			}
		}

		private void InitializeComponent()
		{
			SuspendLayout();
			Name = "ListControl";
			KeyDown += new System.Windows.Forms.KeyEventHandler( ListControl_KeyDown );
			ResumeLayout( false );
		}

		internal void FireFocusChanged( RowIdentifier oldFocusItem, RowIdentifier newFocusItem )
		{
			OnRowFocusChanged( new RowFocusChangedEventArgs( oldFocusItem, newFocusItem ) );
		}


		private Timer _timer;
		private ListSection _listSection;
		private Section _customiseListSection;
		private ItemList _itemList;
		private ColumnList _columns = new ColumnList();
		private SelectedItemsCollection _selectedItems;
		private Color _separatorColor = Color.FromArgb( 123, 164, 224 );
		private Color _indentColor = Color.LightGoldenrodYellow;
		private bool _allowSorting = true;
		private Font _groupSectionFont;
		private Color _alternatingRowColor = Color.Empty;
		private Color _groupSectionForeColor = SystemColors.WindowText;
		private string _groupSectionTextSingular = "Item";
		private string _groupSectionTextPlural = "Items";
		private GdiPlusEx.VAlignment _groupSectionVerticalAlignment = GdiPlusEx.VAlignment.Top;

		private void ListControl_KeyDown( object sender, KeyEventArgs e )
		{
			try
			{
				if( e.Control )
				{
					switch( e.KeyCode )
					{
						case Keys.C:
							if( SelectedItems.Count > 0 )
							{
								StringBuilder strBldr = new StringBuilder();
								foreach( RowIdentifier row in SelectedItems )
								{
									if( row.Items.Length > 0 )
									{
										if (row is ListSection.GroupIdentifier)
										{
											strBldr.Append(row.LastColumn.GroupItemAccessor(row.Items[0]));
										}
										else
										{
											foreach (Column c in Columns)
											{
												object obj = "";
												if (c.ColumnItemFormattedAccessor != null)
													obj = c.ColumnItemFormattedAccessor(row.Items[0]);
												else
													obj = c.ColumnItemAccessor(row.Items[0]);
												if (obj != null)
													strBldr.Append(obj.ToString());

												strBldr.Append("\t");
											}
										}
										strBldr.Append(Environment.NewLine);
									}
								}
								Clipboard.SetDataObject( strBldr.ToString() );
							}
							break;
                        case Keys.A:
							if (MultiSelect)
							{
								SelectedItems.SelectAll();
							}
							break;
					}
				}
			}
			catch( Exception )
			{
			}
		}

		#endregion
	}
}
