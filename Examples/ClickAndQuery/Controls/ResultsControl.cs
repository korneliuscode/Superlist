using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using BinaryComponents.SuperList;
using System.Data.SqlTypes;

namespace ClickAndQuery.Controls
{
	public partial class ResultsControl : UserControl
	{
		public ResultsControl()
		{
			InitializeComponent();
			_listControl.Items.ProcessingStyle = BinaryComponents.SuperList.ItemLists.ProcessingStyle.Thread;

			//
			//	Here we set the default object comparer. This is used when two rows that have been compared for
			//	grouping and / or sorting are equal. We still need an order otherwise sometimes you get rows
			//	jumping around when the list refreshes as QSort doesn't guarantee previous order. QSort is the 
			//	algorithm Array.Sort uses.
			_listControl.Items.ObjectComparer = new LastResortComparer();
		}


		public void SerializeState( System.IO.TextWriter writer )
		{
			_listControl.SerializeState( writer );
		}

		public void DeSerializeState( System.IO.TextReader reader )
		{
			if( _listControl.Columns.Count == 0 )
			{
				_deferredSerializationState = reader.ReadToEnd();
			}
			else
			{
				_listControl.DeSerializeState( reader );
				_deferredSerializationState = null;
			}
		}

		public void CopyToClipboard()
		{
			StringBuilder builder = new StringBuilder();

			//
			// Copy header information
			int count = 0;
			foreach( Column c in _listControl.Columns.VisibleItems )
			{
				if( count++ > 0 )
				{
					builder.Append( "\t" );
				}
				builder.Append( c.Name );
			}
			builder.Append( Environment.NewLine );

			List<object[]> selectedRows = new List<object[]>();

			//
			//	Get a list of rows that have been selected. If a group has
			//	been selected then all its rows get added if and only if none
			//	of its subitems have been selected otherwise the group is
			//	ignored in favour of the subitems.
			BinaryComponents.Utility.Collections.Set<object> rowsReadyToAdd = new BinaryComponents.Utility.Collections.Set<object>();
			int speculativeStartIndex = 0;
			foreach( RowIdentifier ri in _listControl.SelectedItems )
			{
				bool first = true;
				foreach( object[] row in ri.Items )
				{
					if( first )
					{
						first = false;
						if( rowsReadyToAdd.Contains( row ) )
						{
							if( speculativeStartIndex < selectedRows.Count )
							{
								selectedRows.RemoveRange( speculativeStartIndex, selectedRows.Count - speculativeStartIndex );
							}
						}
						rowsReadyToAdd.Clear();
						speculativeStartIndex = selectedRows.Count;
					}
					rowsReadyToAdd.Add( row );
					selectedRows.Add( row );
				}
			}

			//
			//	Build the text we're going to use for the Clipboard
			foreach( object[] row in selectedRows )
			{
				bool first = true;
				foreach( Column column in _listControl.Columns.VisibleItems )
				{
					if( first )
					{
						first = false;
					}
					else
					{
						builder.Append( "\t" );
					}
					builder.Append( column.ColumnItemAccessor( row ).ToString() );
				}
				builder.Append( Environment.NewLine );
			}

			Clipboard.SetText( builder.ToString() );
		}
	

		/// <summary>
		/// Given a command a reader will be executed against it. Note the command will be
		/// automatically disposed of after the query has finished or been cancelled.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="behaviour"></param>
		public void ExecuteReader( SqlCommand command, CommandBehavior behaviour )
		{
			CancelQuery();
			_listControl.Items.Clear();
			_initialQuery = true;
			_queryJob = new QueryJob( command, behaviour ) ;
		}

		/// <summary>
		/// Cancels any query that's currently running.
		/// </summary>
		public void CancelQuery()
		{
			if( _queryJob != null )
			{
				_queryJob.Cancel();
				_queryJob.Dispose();
			}
			_queryJob = null;
		}

		public bool IsQueryExecuting
		{
			get
			{
				return _queryJob != null && !_queryJob.IsFinished;
			}
		}

		public bool IsQueryFinished
		{
			get
			{
				return _queryJob == null || _queryJob.IsFinished;
			}
		}

		public Exception LastQueryError
		{
			get
			{
				return _lastQueryError;
			}
			set
			{
				_lastQueryError = null;
			}
		}


		public void Clear()
		{
			_listControl.Items.Clear();
			_listControl.Columns.Clear();
		}

		/// <summary>
		/// Sizes the columns to fit the currnet content.
		/// </summary>
		public void SizeColumnsToFit()
		{
			_listControl.SizeColumnsToFit();
		}

		/// <summary>
		/// Given a DataReader a set of columns are created for the ListControl
		/// </summary>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		private static List<SQLColumn> CreateColumns( System.Data.SqlClient.SqlDataReader dataReader )
		{
			List<SQLColumn> superListColumns = new List<SQLColumn>();

			for( int i = 0; i < dataReader.FieldCount; i++ )
			{
				superListColumns.Add( CreateColumn( dataReader.GetFieldType( i ),  dataReader.GetName( i ), i ) );
			}
			return superListColumns;
		}

		protected override void OnHandleCreated( EventArgs e )
		{
			base.OnHandleCreated( e );

			Application.Idle += new EventHandler( Application_Idle );
		}

		protected override void OnHandleDestroyed( EventArgs e )
		{
			base.OnHandleDestroyed( e );
			CancelQuery();
			Application.Idle -= new EventHandler( Application_Idle );
		}

		private void Application_Idle( object sender, EventArgs e )
		{
			//
			//	Add any new rows into the Superlist from our QueryJob
			if( _queryJob != null )
			{
				object [][]rows = _queryJob.DequeueRows();
				if( rows != null && rows.Length > 0 )
				{
					//
					//	We can only deserialize the list settings after column have been added to the 
					//	list, so we do the deserialize here.
					if( _initialQuery )
					{
						_newColumns = PopulateColumns( _queryJob.Columns );
						if( _deferredSerializationState != null )
						{
							using( StringReader stringReader = new StringReader( _deferredSerializationState ) )
							{
								DeSerializeState( stringReader );
							}
						}
					}

					_listControl.Items.AddRange( rows );

					//
					// Size the columns on our initial batch of rows
					if( _initialQuery ) 
					{
						_listControl.SizeColumnsToFit( _newColumns );
						_initialQuery = false;
					}
				}
				else if( _queryJob.IsFinished )
				{
					_lastQueryError = _queryJob.LastError;
					_queryJob.Dispose();
					_queryJob = null;
				}
			}

		}


		private SQLColumn[] PopulateColumns( SQLColumn[] columnsToAdd )
		{
			List<SQLColumn> newColumns = new List<SQLColumn>();
			//
			//	Run through our new columns and if any of our old ones match contiguously
			//	from the start then we keep them (and their settings). 
			int i = 0;
			for( ; i < columnsToAdd.Length && i < _listControl.Columns.Count; i++ )
			{
				SQLColumn oldColumn = (SQLColumn)_listControl.Columns[i];
				if( columnsToAdd[i].IsSameColumn( oldColumn ) )
				{
					columnsToAdd[i] = oldColumn;
				}
				else
				{
					newColumns.Add( columnsToAdd[i] );
				}
			}
			if( i < columnsToAdd.Length )
			{
				SQLColumn []toCopy = new SQLColumn[columnsToAdd.Length - i];
				Array.Copy( columnsToAdd, i, toCopy, 0, toCopy.Length );
				newColumns.AddRange( toCopy );
			}
			_listControl.Columns.Clear();
			_listControl.Columns.AddRange( columnsToAdd );
			return newColumns.ToArray();
		}



		/// <summary>
		///	This is used when two rows that have been compared for grouping and / or sorting are equal. 
		///	We still need an order otherwise sometimes you get rows jumping around 
		///	when the list refreshes as QSort  doesn't guarantee previous order. QSort is the 
		///	algorithm Array.Sort uses.
		/// </summary>
		private class LastResortComparer : System.Collections.IComparer
		{
			#region IComparer Members

			public int Compare( object x, object y )
			{
				object[] xRow = (object[])x;
				object[] yRow = (object[])y;
				return (int)xRow[xRow.Length - 1] - (int)yRow[yRow.Length - 1];
			}

			#endregion
		}

		/// <summary>
		/// For a given type you can create your own SQLColumn bye registering your own ColumnCreator
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="columnIndex"></param>
		/// <returns></returns>
		private delegate SQLColumn ColumnCreator( string columnName, int columnIndex );

		/// <summary>
		/// Type to Column factory map.
		/// </summary>
		private static Dictionary<Type, ColumnCreator> _dataTypeMapToColumnFactory = new Dictionary<Type, ColumnCreator>();
		static ResultsControl()
		{
			_dataTypeMapToColumnFactory.Add( typeof( DateTime ),
				delegate( string columnName, int columnIndex ) { return new DateColumn( columnName, columnIndex ); } );
		}
		private static SQLColumn CreateColumn( Type type, string columnName, int columnIndex )
		{
			ColumnCreator creator;
			if( _dataTypeMapToColumnFactory.TryGetValue( type, out creator ) )
			{
				return creator( columnName, columnIndex );
			}
			return new SQLColumn( columnName, columnIndex );
		}

		#region SQL Columns
		public class SQLColumn : Column
		{
			public SQLColumn( string columnName, int columnIndex )
				: base( columnName, columnName, 0, null )
			{
				_columnIndex = columnIndex;
				this.IsVisible = true;
				ColumnItemAccessor = delegate( object item ) 
				{
					object value = GetFieldValue( item );
					return value == null ? value == DBNull.Value : value; 
				};

				this.Comparitor = delegate( object x, object y )
													{
														object cellItemX = ColumnItemAccessor( x );
														object cellItemY = ColumnItemAccessor( y );
														if( cellItemX == cellItemY )
														{
															return 0;
														}
														if( cellItemX == DBNull.Value )
														{
															return 1;
														}
														if( cellItemY == DBNull.Value )
														{
															return -1;
														}
														IComparable comparer = cellItemX as IComparable;
														if( comparer == null )
														{
															string error = string.Format( "Cell item for column '{0}' doesn't support comparing. You will need to supply a comparer by setting Column.Comparitor", Name );
															throw new NotSupportedException( error );
														}
														return comparer.CompareTo( cellItemY );
													};

			}

			public virtual object GetFieldValue( object rowItem )  
			{
				object [] row = (object[])rowItem;
				return row[_columnIndex];
			}

			public bool IsSameColumn( SQLColumn otherColumn )
			{
				return this.Name == otherColumn.Name && _columnIndex == otherColumn._columnIndex;
			}

			protected int ColumnIndex
			{
				get
				{
					return _columnIndex;
				}
			}

			protected readonly string NullValue = "(null)";

			private int _columnIndex;
		}
		private class DateColumn : SQLColumn
		{
			public DateColumn( string columnName, int columnIndex )
				: base( columnName, columnIndex )
			{
				this.GroupItemAccessor = GroupValueFromItem;
			}
			private string GroupValueFromItem( object o )
			{
				object item = ((object[])o)[ColumnIndex];
				if( item == null )
				{
					return NullValue;
				}

				DateTime date = (DateTime)item;
				DateTime now = DateTime.Now.Date;
				DateTime weekStart = now.AddDays( -(int)now.DayOfWeek );
				DateTime monthStart = now.AddDays( -now.Day );

				double days = now.Subtract( date ).TotalDays;

				if( date.Year == now.Year && date.Month == now.Month && now.Day - date.Day < 2 )
				{
					if( now.Day == date.Day )
					{
						return "Today";
					}
					else if( now.Day - 1 == date.Day )
					{
						return "Yesterday";
					}
					else
					{
						return "2 days ago";
					}
				}
				else if( date > weekStart )
				{
					return "This Week";
				}
				else if( date > weekStart.AddDays( -7 ) )
				{
					return "Last Week";
				}
				else if( date > monthStart )
				{
					return "This Month";
				}
				else if( date > monthStart.AddMonths( -1 ).AddDays( 1 ) )
				{
					return "Last Month";
				}
				else
				{
					return "Older";
				}
			}
		}
		#endregion

		#region QueryJob
		public class QueryJob : IDisposable
		{
			public QueryJob( SqlCommand command, CommandBehavior behaviour )
			{
				_command = command;
				_command.BeginExecuteReader( ExecutionResultsHandler, _command, behaviour );
			}

			public SQLColumn[] Columns			
			{
				get
				{
					return _columns;
				}
			}


			public object[][] DequeueRows()
			{
				lock( _lockObject )
				{
					if( _rows.Count > 0 )
					{
						object[][] rows = _rows.ToArray();
						_rows.Clear();
						return rows;
					}
				}
				return null;
			}

			public void Cancel()
			{
				_command.Cancel();
				_cancel = true;

				//
				// We wait here for the reader (if running) to stop.
				while( !_finished )
				{
					System.Threading.Thread.Sleep( 1 );
				}
			}

			public bool IsFinished
			{
				get
				{
					return _finished;
				}
			}

			public Exception LastError
			{
				get
				{
					return _lastError;
				}
			}

			private void ExecutionResultsHandler( IAsyncResult result )
			{
				try
				{
					using( SqlDataReader reader = _command.EndExecuteReader( result ) )
					{
						System.Diagnostics.Debug.WriteLine( "Reader Started" );
						_columns = CreateColumns( reader ).ToArray();
						int count = 0;
						while( !_cancel && reader.Read() )
						{
							object[] row = new object[reader.FieldCount + 1];
							reader.GetValues( row );
							row[reader.FieldCount] = count++;

							lock( _lockObject )
							{
								_rows.Add( row );
							}
						}
					}
				}
				catch( Exception e )
				{
					if( !_cancel ) // dont care about errors if cancelled
					{
						_lastError = e;
					}
				}
				finally
				{
					_finished = true;
				}
			}
			public void Dispose()
			{
				if( _command != null )
				{
					_command.Dispose();
					_command = null;
				}
			}


			private object _lockObject = new object();
			private SqlCommand _command;
			private Exception _lastError;
			private bool _finished = false;
			private SQLColumn[] _columns;
			private bool _cancel = false;
			private List<object[]> _rows = new List<object[]>();
		}
		#endregion

		private string _deferredSerializationState;
		private SQLColumn[] _newColumns;
		private bool _initialQuery = true;
		private QueryJob _queryJob = null;
		private Exception _lastQueryError = null;
	}
}
