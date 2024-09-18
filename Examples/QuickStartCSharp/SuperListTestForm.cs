/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////
//
// (c) 2006 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using BinaryComponents.SuperList;
using BinaryComponents.SuperList.Sections;
using BinaryComponents.Utility.Collections;
using ListControl = BinaryComponents.SuperList.ListControl;

namespace SuperListTest
{
	public partial class SuperListTestForm : Form
	{
		public SuperListTestForm()
		{
			InitializeComponent();
			_surnameColumn = new Column( "surname", "Surname", 120, delegate( object item )
																															 {
																																 return ((Person)item).Surname;
																															 } );
			_firstnameColumn = new Column( "firstname", "Firstname", 120, delegate( object item )
																																		 {
																																			 return ((Person)item).Firstname;
																																		 } );
			_phoneColumn = new Column( "phone", "Phone", 100, delegate( object item )
																												 {
																													 return ((Person)item).Phone;
																												 } );
			_cityColumn = new Column( "city", "City", 60, delegate( object item )
																										 {
																											 return ((Person)item).City;
																										 } );
			_stateColumn = new Column( "state", "State", 70, delegate( object item )
																												{
																													return ((Person)item).State;
																												} );
			_dateColumn = new Column( "date", "Date", 110, delegate( object item )
																															{
																																return ((Person)item).Date.ToString();
																															} );
			_dateColumn.GroupItemAccessor = GroupValueFromItem;
			_dateColumn.MoveBehaviour = Column.MoveToGroupBehaviour.Copy;

			_dateColumn.GroupSortOrder = SortOrder.Descending;
			_surnameColumn.SortOrder = SortOrder.Ascending;

			_imageColumn = new Column( "State", " State", 16, delegate( object item )
																															 {
																																 return ((Person)item).Open ? _imageList.Images[1] : _imageList.Images[0];
																															 } );
			_imageColumn.Comparitor = delegate( object x, object y )
			{
				Person xPerson = (Person)x;
				Person yPerson = (Person)y;
				if( xPerson.Open == yPerson.Open )
				{
					return 0;
				}
				return xPerson.Open ? 1 : -1;
			};
			_imageColumn.GroupItemAccessor = delegate( object item )
			{
				return ((Person)item).Open ? "Opened" : "Closed";
			};

			_superList.Columns.Add( _imageColumn );
			_superList.Columns.Add( _firstnameColumn );
			_superList.Columns.Add( _phoneColumn );
			_superList.Columns.Add( _stateColumn );
			_superList.Columns.Add( _cityColumn );
			_superList.Columns.Add( _dateColumn );
			_superList.Columns.GroupedItems.Add( _dateColumn );
			_superList.Columns.GroupedItems.Add( _stateColumn );

			_superList.SelectedItems.DataChanged += SelectedItems_DataChanged;

			const int iterationCount = 1; // Change this if you want to increas the number of items in the list
			for( int i = 0; i < iterationCount; i++ )
			{
				_superList.Items.AddRange( Person.GetData() );
			}
			_superList.SelectedItems.Add( _superList.Items[0] );
		}


		private static string GroupValueFromItem( object o )
		{
			DateTime date = ((Person)o).Date;

			DateTime publicationDate = date;
			DateTime now = DateTime.Now.Date;
			DateTime weekStart = now.AddDays( -(int)now.DayOfWeek );
			DateTime monthStart = now.AddDays( -now.Day );

			double days = now.Subtract( publicationDate ).TotalDays;

			if( days < 1 )
			{
				return "Today";
			}
			else if( days < 2 )
			{
				return "Yesterday";
			}
			else if( publicationDate > weekStart )
			{
				return "This Week";
			}
			else if( publicationDate > weekStart.AddDays( -7 ) )
			{
				return "Last Week";
			}
			else if( publicationDate > monthStart )
			{
				return "This Month";
			}
			else if( publicationDate > monthStart.AddMonths( -1 ).AddDays( 1 ) )
			{
				return "Last Month";
			}
			else
			{
				return "Older";
			}
		}

		private void SelectedItems_DataChanged( object sender, SelectedItemsChangedEventArgs e )
		{
			//Debug.WriteLine( string.Format( "Selection event {0}", e.ChangeType ) );
			foreach( RowIdentifier ri in e.Items )
			{
				string s;

				if( ri.GroupColumns.Length == 0 )
				{
					s = string.Format( "\tSelection row item: {0}", ri.Items[0] );
				}
				else
				{
					s = "\tSelection group item:";
					int index = ri.GroupColumns.Length-1;
					for( int i = 0; i < _superList.Columns.GroupedItems.Count; i++ )
					{
						if( i > 0 )
						{
							s += "->";
						}
						s += _superList.Columns.GroupedItems[i].GroupItemAccessor( ri.Items[0] );
					}
				}
				//Trace.WriteLine( s );
			}
		}

		private void _fileExitMenuItem_Click( object sender, EventArgs e )
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void startTimedAdditionsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _timer == null )
			{
				_timer = new Timer();
				_timer.Interval = 1000;
				_timer.Tick += new EventHandler( _timer_Tick );
				_timer.Start();
			}
			else
			{
				_timer.Stop();
				_timer.Dispose();
				_timer = null;
			}
		}

		private void _timer_Tick( object sender, EventArgs e )
		{
			_superList.Items.AddRange( Person.GetData() );
		}

		private Timer _timer;

		private void clearListToolStripMenuItem_Click( object sender, EventArgs e )
		{
			_superList.Items.Clear();
		}

		private void selectAllToolStripMenuItem_Click( object sender, EventArgs e )
		{
			_superList.SelectedItems.SelectAll();
		}


		private void _superList_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Delete )
			{
				List<object> selectedItems = new List<object>();

				Set<object> groupRows = new Set<object>();
				Column groupColumn = null;
				foreach( RowIdentifier ri in _superList.SelectedItems )
				{
					if( ri.GroupColumns.Length == 0 && groupRows.Contains( ri.Items[0] ) )
					{
						groupRows.Clear();
					}

					foreach( object newsFeedItemNode in ri.Items )
					{
						if( groupColumn != ri.LastColumn )
						{
							selectedItems.AddRange( groupRows.ToArray() );
							groupColumn = ri.LastColumn;
						}

						if( ri.GroupColumns.Length == 0 )
						{
							selectedItems.Add( newsFeedItemNode );
						}
						else
						{
							groupRows.Add( newsFeedItemNode );
						}
					}
				}
				selectedItems.AddRange( groupRows.ToArray() );

				foreach( object o in selectedItems )
				{
					_superList.Items.Remove( o );
				}
			}
		}

		private const string _fileFilter = "SuperList config file(*.xml)|*.xml";

		private void _ensureItemVisibleToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SelectItemForm sif = new SelectItemForm( _superList );
			if( sif.ShowDialog( this ) == DialogResult.OK && sif.SelectedItems.Length > 0 )
			{
				MessageBox.Show( this, sif.SelectedItems[0].ToString() );
				_superList.FocusedItem = sif.SelectedItems[0];
			}
		}

		private void _dumpListItemsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			foreach( Person p in _superList.Items.ToArray() )
			{
				//Debug.WriteLine( p );
			}
		}

		private void _dirtyItemsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SelectItemForm sif = new SelectItemForm( _superList );
			if( sif.ShowDialog( this ) == DialogResult.OK )
			{
				_superList.Items.ItemsChanged( sif.SelectedItems );
			}
		}

		private void selectItemsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SelectItemForm sif = new SelectItemForm( _superList );
			if( sif.ShowDialog( this ) == DialogResult.OK )
			{
				_superList.SelectedItems.AddRange( sif.SelectedItems );
			}
		}


		private void toggleCustomAreaVisibilityToolStripMenuItem_Click( object sender, EventArgs e )
		{
			_superList.ShowCustomizeSection = !_superList.ShowCustomizeSection;
		}

		private void toggleHeaderVisibilityToolStripMenuItem_Click( object sender, EventArgs e )
		{
			_superList.ShowHeaderSection = !_superList.ShowHeaderSection;
		}

		private void clearSelectionToolStripMenuItem_Click( object sender, EventArgs e )
		{
			_superList.SelectedItems.Clear();
			_superList.AllowSorting = !_superList.AllowSorting;
		}

		private void loadConfigToolStripMenuItem_Click_1( object sender, EventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.CheckFileExists = true;
			ofd.AddExtension = true;
			ofd.Filter = _fileFilter;
			ofd.Title = "Load config file";
			if( ofd.ShowDialog( this ) == DialogResult.OK )
			{
				try
				{
					using( TextReader textReader = File.OpenText( ofd.FileName ) )
					{
						_superList.DeSerializeState( textReader );
					}
				}
				catch( Exception exception )
				{
					MessageBox.Show( exception.Message );
				}
			}
		}

		private void saveConfigToolStripMenuItem_Click_1( object sender, EventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.CheckFileExists = false;
			ofd.AddExtension = true;
			ofd.Filter = _fileFilter;
			ofd.Title = "Save config file";
			if( ofd.ShowDialog( this ) == DialogResult.OK )
			{
				try
				{
					using( TextWriter textWriter = File.CreateText( ofd.FileName ) )
					{
						_superList.SerializeState( textWriter );
					}
				}
				catch( Exception exception )
				{
					MessageBox.Show( exception.Message );
				}
			}
		}

		#region Example of overriding rows

		/// <summary>
		/// Storage area for the row override.
		/// </summary>
		private RowOverrideExample _rowOverride;


		private void toggleRowPaintingOverrideToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if( _rowOverride == null )
			{
				//
				// Start overrride.
				_rowOverride = new RowOverrideExample( _superList );
			}
			else
			{
				//
				// Clear override.
				_rowOverride.Dispose();
				_rowOverride = null;
			}
		}

		/// <summary>
		/// Example of overriding rows giving a gradient fill look.
		/// </summary>
		private class RowOverrideExample : IDisposable
		{
			public RowOverrideExample( ListControl listControl )
			{
				_oldFactory = listControl.SectionFactory; // store old factory as we want to leave as we came.
				_listControl = listControl;

				//
				// Replace the current SectionFactory with our override.
				listControl.SectionFactory = new MySectionFactory(); // 
				_listControl.LayoutSections();
			}


			public void Dispose()
			{
				if( _oldFactory != null ) // put things back as they were
				{
					_listControl.SectionFactory = _oldFactory;
					_listControl.LayoutSections();
				}
			}

			private class MySectionFactory : SectionFactory
			{
				public override RowSection CreateRowSection(
						ListControl listControl,
						RowIdentifier rowIdentifier,
						HeaderSection headerSection,
						int position )
				{
					return new MyRowSection( listControl, rowIdentifier, headerSection, position );
				}
			}

			private class MyRowSection : RowSection
			{
				public MyRowSection(
						ListControl listControl,
						RowIdentifier rowIdentifier,
						HeaderSection headerSection,
						int position )
					: base( listControl, rowIdentifier, headerSection, position )
				{
					_position = position;
				}

				public override void PaintBackground( GraphicsSettings gs, Rectangle clipRect )
				{
					Color from, to;
					if( _position % 2 == 0 )
					{
						from = Color.White;
						to = Color.LightBlue;
					}
					else
					{
						to = Color.White;
						from = Color.LightBlue;
					}
					using( LinearGradientBrush lgb = new LinearGradientBrush(
							Rectangle,
							from,
							to,
							LinearGradientMode.Horizontal ) )
					{
						gs.Graphics.FillRectangle( lgb, Rectangle );
					}
				}


				private readonly int _position;
			}

			private readonly ListControl _listControl;
			private readonly SectionFactory _oldFactory;
		}

		#endregion

		#region Example of Email like preview

		private void ClearPreview()
		{
			if( _emailPreviewExample != null )
			{
				_emailPreviewExample.Dispose();
				_emailPreviewExample = null;
			}
		}

		private void emailLikePreviewToolStripMenuItemNone_Click( object sender, EventArgs e )
		{
			ClearPreview();
		}

		private void emailLikePreviewToolStripMenuItemSelectedOnly_Click( object sender, EventArgs e )
		{
			ClearPreview();
			_emailPreviewExample = new EmailPreviewExample( _superList, EmailPreviewExample.Style.SelectedOnly );
		}

		private void emailLikePreviewToolStripMenuItemAll_Click( object sender, EventArgs e )
		{
			ClearPreview();
			_emailPreviewExample = new EmailPreviewExample( _superList, EmailPreviewExample.Style.AllRows );
		}

		private EmailPreviewExample _emailPreviewExample = null;

		/// <summary>
		/// Example of overriding rows giving a gradient fill look.
		/// </summary>
		private class EmailPreviewExample : IDisposable
		{
			public enum Style
			{
				AllRows,
				SelectedOnly
			} ;

			public EmailPreviewExample( ListControl listControl, Style style )
			{
				_oldFactory = listControl.SectionFactory; // store old factory as we want to leave as we came.
				_listControl = listControl;

				//
				// Replace the current SectionFactory with our override.
				listControl.SectionFactory = new MySectionFactory( style ); // 
				_listControl.LayoutSections();
			}


			public void Dispose()
			{
				if( _oldFactory != null ) // put things back as they were
				{
					_listControl.SectionFactory = _oldFactory;
					_listControl.LayoutSections();
				}
			}

			private class MySectionFactory : SectionFactory
			{
				public MySectionFactory( Style style )
				{
					_style = style;
				}

				public override RowSection CreateRowSection(
						ListControl listControl,
						RowIdentifier rowIdentifier,
						HeaderSection headerSection,
						int position )
				{
					return new MyRowSection( listControl, rowIdentifier, headerSection, position, _style );
				}

				private Style _style;
			}

			private class MyRowSection : RowSection
			{
				public MyRowSection(
						ListControl listControl,
						RowIdentifier rowIdentifier,
						HeaderSection headerSection,
						int position,
						Style style )
					: base( listControl, rowIdentifier, headerSection, position )
				{
					_style = style;
				}

				private Font AutoPreviewFont
				{
					get
					{
						return new Font( Host.Font, FontStyle.Italic );
					}
				}

				public override bool NeedsLayoutOnSelection
				{
					get
					{
						return _style == Style.SelectedOnly;
					}
				}

				public override void Layout( GraphicsSettings gs, Size maximumSize )
				{
					base.Layout( gs, maximumSize );

					if( _style == Style.AllRows || IsSelected )
					{
						using( Font autoPreviewFont = AutoPreviewFont )
						{
							int top = Size.Height;
							int autoHeight = autoPreviewFont.Height * 2;
							int indent = IndentWidth + 5;
							Person person = (Person)Item;
							string description = person.Description;

							if( !string.IsNullOrEmpty( description ) )
							{
								_rect = new Rectangle( indent, top, Size.Width - indent - 20, autoHeight );

								Size = new Size( Size.Width, Size.Height + autoHeight + 4 );
							}
						}
					}
				}


				public override void Paint( GraphicsSettings gs, Rectangle clipRect )
				{
					base.Paint( gs, clipRect );

					if( _style == Style.AllRows || IsSelected )
					{
						using( Font autoPreviewFont = AutoPreviewFont )
						{
							Person person = (Person)Item;
							string description = person.Description;

							Rectangle rect = _rect;

							rect.Offset( HostBasedRectangle.Location );

							Color color;

							if( IsSelected )
							{
								color = SystemColors.HighlightText;
							}
							else
							{
								color = SystemColors.ControlDarkDark;
							}

							BinaryComponents.WinFormsUtility.Drawing.GdiPlusEx.DrawString
									( gs.Graphics, description, AutoPreviewFont, color, rect
									 , BinaryComponents.WinFormsUtility.Drawing.GdiPlusEx.TextSplitting.MultiLine, BinaryComponents.WinFormsUtility.Drawing.GdiPlusEx.Ampersands.Display );
						}
					}
				}

				private Rectangle _rect;
				private Style _style;
			}

			private ListControl _listControl;
			private SectionFactory _oldFactory;
		}

		#endregion

		private void _sizeColumnsToFitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			_superList.SizeColumnsToFit();
		}

		private void allowDragDrop_CheckStateChanged( object sender, EventArgs e )
		{
			if( _superList.AllowRowDragDrop != allowDragDrop.Checked )
			{
				_superList.AllowRowDragDrop = allowDragDrop.Checked;
				if( _superList.AllowRowDragDrop )
				{
					_superList.DragOverEx += new SuperlistDragEventHandler( _superList_DragOver );
					_superList.DragDropEx += new SuperlistDragEventHandler( _superList_DragDropEx );
					MessageBox.Show( this, "This example allows you to drag and drop rows on group rows Dev comment(Example works now but I need to still do a little work on the transparancy of the dragged image )" );
				}
				else
				{
					_superList.DragOverEx -= new SuperlistDragEventHandler( _superList_DragOver );
					_superList.DragDropEx -= new SuperlistDragEventHandler( _superList_DragDropEx );
				}
			}
		}
		private Column _imageColumn;
		private Column _surnameColumn;
		private Column _firstnameColumn;
		private Column _phoneColumn;
		private Column _cityColumn;
		private Column _stateColumn;
		private Column _dateColumn;

		private delegate void Apply();
		private void ApplyDragDropChanges( Column[] groupColumns, Person source, Person p )
		{
			Dictionary<Column, Apply> columnActions = new Dictionary<Column, Apply>();
			columnActions.Add( _surnameColumn, delegate { p.Surname = source.Surname; } );
			columnActions.Add( _firstnameColumn, delegate{ p.Firstname = source.Firstname ;} );
			columnActions.Add( _phoneColumn, delegate{ p.Phone = source.Phone;} );
			columnActions.Add( _cityColumn, delegate{ p.City = source.City;} );
			columnActions.Add( _stateColumn, delegate{ p.State = source.State;} );
			columnActions.Add( _dateColumn, delegate{ p.Date = source.Date;} );
			foreach( Column c in groupColumns )
			{
				if( c.GroupedComparitor( source, p ) != 0 ) // we only change the property if its grouped value is different
				{
					columnActions[c]();
				}
			}
		}

		void _superList_DragDropEx( object sender, SuperlistDragEventArgs ea )
		{
			bool changed = false;
			GroupSection gs = ea.SectionOver as GroupSection;
			if( gs != null )
			{
				IDataObject dataObject = ea.DragEventArgs.Data;
				string[] formats = dataObject.GetFormats();

				foreach( string format in formats )
				{
					object[] items = dataObject.GetData( format ) as object[];
					foreach( object item in items )
					{
						RowIdentifier ri = item as RowIdentifier;
						if( ri != null )
						{
							foreach( Person p in ri.Items )
							{
								ApplyDragDropChanges( gs.RowIdentifier.GroupColumns, (Person)gs.RowIdentifier.Items[0], p );
								changed = true;
							}
						}
					}
				}
			}
			if( changed )
			{
				_superList.Items.Sort();
			}
		}


		void _superList_DragOver( object sender, SuperlistDragEventArgs e )
		{
			//
			//	In this example we're interested if the section over has a grouped section
			//	for an ancestor. If it does then we allow dropping on the section
			GroupSection gs = e.SectionOver.GetAncestor<GroupSection>( true );
			if( gs != null )
			{
				e.SectionAllowedToDropOn = gs;
				return;
			}
			e.DragEventArgs.Effect = DragDropEffects.None;
		}
	}
}