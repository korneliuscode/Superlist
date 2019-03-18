using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BinaryComponents.SuperList;
using BinaryComponents.SuperList.Sections;
using BinaryComponents.Utility.Collections;
using ListControl = BinaryComponents.SuperList.ListControl;

namespace DragDrop
{
	public partial class DragDropForm : Form
	{
		public DragDropForm()
		{
			InitializeComponent();
			CreateColumns( _listControl1 );
			CreateColumns( _listControl2 );

			PopulateList( _listControl1 );

			_listControl1.AllowRowDragDrop = true;
			_listControl2.AllowRowDragDrop = true;

			_listControl1.DragOverEx += new SuperlistDragEventHandler( _listControl_DragOverEx );
			_listControl2.DragOverEx += new SuperlistDragEventHandler( _listControl_DragOverEx );
			_listControl1.DragDropEx += new SuperlistDragEventHandler( _listControl_DragDropEx );
			_listControl2.DragDropEx += new SuperlistDragEventHandler( _listControl_DragDropEx );
		}

		void _listControl_DragDropEx( object sender, SuperlistDragEventArgs ea )
		{
			ListControl senderControl = sender as ListControl;
			if( senderControl != null )
			{
				Person[] persons = PersonsFromDataObject( ea.DragEventArgs.Data );

				if( sender == _listControl1 ) // we remove the persons from the other list in this example and add to the sender
				{
					_listControl2.Items.RemoveRange( persons );
				}
				else
				{
					_listControl1.Items.RemoveRange( persons );
				}
				senderControl.Items.AddRange( persons );
			}
		}

		void _listControl_DragOverEx( object sender, SuperlistDragEventArgs ea )
		{
			ListControl senderControl = sender as ListControl;
			if( senderControl != null  )
			{
                Person[] items = PersonsFromDataObject(ea.DragEventArgs.Data);
                foreach (Person p in items)
                {
                    if (senderControl.Items.IndexOf(p) != -1)
                    {
                        return; // don't allow items to be dropped on ourself.
                    }
                }
				ea.SectionAllowedToDropOn = ea.SectionOver;
				return;
			}
			ea.DragEventArgs.Effect = DragDropEffects.None;
		}

		Person[] PersonsFromDataObject( IDataObject dataObject )
		{
			List<Person> persons = new List<Person>();
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
							persons.Add( p );
						}
					}
				}
			}
			return persons.ToArray();
		}


		private void PopulateList( ListControl listControl )
		{
			const int iterationCount = 1; // Change this if you want to increas the number of items in the list
			for( int i = 0; i < iterationCount; i++ )
			{
				listControl.Items.AddRange( Person.GetData() );
			}
		}


		#region Column Definitions

		private void CreateColumns( ListControl listControl )
		{
			Column surnameColumn;
			Column firstnameColumn;
			Column phoneColumn;
			Column cityColumn;
			Column stateColumn;
			Column dateColumn;

			surnameColumn = new Column( "surname", "Surname", 120, delegate( object item )
																															 {
																																 return ((Person)item).Surname;
																															 } );
			firstnameColumn = new Column( "firstname", "Firstname", 120, delegate( object item )
																																		 {
																																			 return ((Person)item).Firstname;
																																		 } );
			phoneColumn = new Column( "phone", "Phone", 100, delegate( object item )
																												 {
																													 return ((Person)item).Phone;
																												 } );
			cityColumn = new Column( "city", "City", 60, delegate( object item )
																										 {
																											 return ((Person)item).City;
																										 } );
			stateColumn = new Column( "state", "State", 70, delegate( object item )
																												{
																													return ((Person)item).State;
																												} );
			dateColumn = new Column( "date", "Date", 110, delegate( object item )
																															{
																																return ((Person)item).Date.ToString();
																															} );
			dateColumn.GroupItemAccessor = GroupValueFromItem;
			dateColumn.MoveBehaviour = Column.MoveToGroupBehaviour.Copy;

			dateColumn.GroupSortOrder = SortOrder.Descending;
			surnameColumn.SortOrder = SortOrder.Ascending;

			listControl.Columns.Add( firstnameColumn );
			listControl.Columns.Add( phoneColumn );
			listControl.Columns.Add( stateColumn );
			listControl.Columns.Add( cityColumn );
			listControl.Columns.Add( dateColumn );
			listControl.Columns.GroupedItems.Add( dateColumn );
			listControl.Columns.GroupedItems.Add( stateColumn );
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

		#endregion
	}
}