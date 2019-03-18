/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using BinaryComponents.SuperList.Sections;

namespace BinaryComponents.SuperList
{

	#region SelectedItemsChangedEventArgs

	public class SelectedItemsChangedEventArgs : EventArgs
	{
		public enum TypeOfChange
		{
			Inserted,
			Deleted
		} ;

		internal SelectedItemsChangedEventArgs( RowIdentifier[] items, TypeOfChange changeType )
		{
			_items = items;
			_changeType = changeType;
		}

		public object[] Items
		{
			get
			{
				return _items;
			}
		}

		public TypeOfChange ChangeType
		{
			get
			{
				return _changeType;
			}
		}

		private object[] _items;
		private TypeOfChange _changeType;
	}

	#endregion

	public class SelectedItemsCollection : ICollection<RowIdentifier>
	{
		internal SelectedItemsCollection( ListControl listControl )
		{
			_listControl = listControl;
		}

		public void SelectAll()
		{
			_listControl.ListSection.SelectAll();
		}

		public RowIdentifier this[int i]
		{
			get
			{
				return GetSortedSelectionList()[i];
			}
		}

		public RowIdentifier[] ToArray()
		{
			return GetSortedSelectionList();
		}

		public void Add( object o )
		{
			Add( new ListSection.NonGroupRow( o ) );
		}

		public void AddRange( object[] items )
		{
			foreach( object o in items )
			{
				Add( o );
			}
		}

		#region ICollection<RowIdentifier> Members

		public void Add( RowIdentifier ri )
		{
			int i = GetPositionFromItem( ri );
			if( i == -1 )
			{
				//
				// Not found so force any deferred updates to be processed.
				_listControl.Items.SynchroniseWithUINow();
				i = GetPositionFromItem( ri );
				if( i == -1 )
				{
					throw new ArgumentException( "ri cannot be added to the selected lists as it doens't exist in the main list" );
				}
			}

			_selectedRows.Add( ri, i );
			_listControl.ListSection.LazyLayout();
			_selectedItemsArrayRef = null;
			OnDataChanged( SelectedItemsChangedEventArgs.TypeOfChange.Inserted, ri );
		}

		public void Clear()
		{
			if( DataChanged != null )
			{
				RowIdentifier[] items = GetSortedSelectionList();
				_selectedRows.Clear();
				if( items.Length > 0 )
				{
					OnDataChanged( SelectedItemsChangedEventArgs.TypeOfChange.Deleted, items );
				}
			}
			else
			{
				_selectedRows.Clear();
			}
			ClearCache();
		}

		public bool Contains( RowIdentifier item )
		{
			return _selectedRows.ContainsKey( item );
		}

		void ICollection<RowIdentifier>.CopyTo( RowIdentifier[] array, int arrayIndex )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		public int Count
		{
			get
			{
				return _selectedRows.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public bool Remove( RowIdentifier item )
		{
			return RemoveInternal( item );
		}

		#endregion

		#region IEnumerable<RowIdentifier> Members

		IEnumerator<RowIdentifier> IEnumerable<RowIdentifier>.GetEnumerator()
		{
			foreach( RowIdentifier ri in GetSortedSelectionList() )
			{
				yield return ri;
			}
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetSortedSelectionList().GetEnumerator();
		}

		#endregion

		public delegate void DataChangedHandler( object sender, SelectedItemsChangedEventArgs e );

		public event DataChangedHandler DataChanged;

		#region Implementation

		internal bool IsSelected( RowIdentifier rowIdentifier )
		{
			return _selectedRows.ContainsKey( rowIdentifier );
		}

		internal int IndexFromRowIdentifier( RowIdentifier ri )
		{
			return _selectedRows[ri];
		}


		/// <summary>
		/// Symantically clears the list and adds the items given, but will
		/// only call data changed on the differences in terms of the final 
		/// outcome of deleted / inserts items.
		/// </summary>
		/// <param name="items"></param>
		/// <returns>The number of rows changed</returns>
		internal int ClearAndAdd( params RowIdentifier[] items )
		{
			List<RowIdentifier> deletedItems = new List<RowIdentifier>();
			List<RowIdentifier> insertedItems = new List<RowIdentifier>();

			Dictionary<RowIdentifier, int> selectedRows = new Dictionary<RowIdentifier, int>();
			int itemsAlreadyInListCount = 0;
			foreach( RowIdentifier item in items )
			{
				bool inList = selectedRows.ContainsKey( item );
				if( !inList )
				{
					int pos = GetPositionFromItem( item );
					if( pos != -1 )
					{
						selectedRows.Add( item, pos );
					}
				}
				if( _selectedRows.ContainsKey( item ) )
				{
					itemsAlreadyInListCount++;
				}
				else
				{
					if( !inList )
					{
						insertedItems.Add( item );
					}
				}
			}
			if( itemsAlreadyInListCount < _selectedRows.Count )
			{
				foreach( RowIdentifier item in _selectedRows.Keys )
				{
					if( !selectedRows.ContainsKey( item ) )
					{
						deletedItems.Add( item );
					}
				}
			}
			_selectedRows = selectedRows;
			ClearCache();
			if( deletedItems.Count > 0 )
			{
				OnDataChanged( SelectedItemsChangedEventArgs.TypeOfChange.Deleted, deletedItems.ToArray() );
			}
			if( insertedItems.Count > 0 )
			{
				OnDataChanged( SelectedItemsChangedEventArgs.TypeOfChange.Inserted, insertedItems.ToArray() );
			}
			return deletedItems.Count + insertedItems.Count;
		}

		internal void AddInternal( RowIdentifier item )
		{
			ClearCache();

			if( !_selectedRows.ContainsKey( item ) )
			{
				int pos = GetPositionFromItem( item );
				if( pos != -1 )
				{
					_selectedRows.Add( item, pos );
					OnDataChanged( SelectedItemsChangedEventArgs.TypeOfChange.Inserted, item );
				}
			}
		}

		private int GetPositionFromItem( RowIdentifier ri )
		{
			return _listControl.Items.IndexOf( ri.Items[0] );
		}

		private int GetPositionFromItem( object item )
		{
			return _listControl.Items.IndexOf( item );
		}

		internal void AddRangeInternal( RowIdentifier[] selectedItems )
		{
			ClearCache();
			if( DataChanged == null )
			{
				foreach( RowIdentifier ri in selectedItems )
				{
					if( !_selectedRows.ContainsKey( ri ) )
					{
						int pos = GetPositionFromItem( ri );
						if( pos != -1 )
						{
							_selectedRows.Add( ri, pos );
						}
					}
				}
			}
			else
			{
				List<RowIdentifier> items = new List<RowIdentifier>( selectedItems.Length );
				foreach( RowIdentifier ri in selectedItems )
				{
					if( !_selectedRows.ContainsKey( ri ) )
					{
						int pos = GetPositionFromItem( ri );
						if( pos != -1 )
						{
							_selectedRows.Add( ri, pos );
							items.Add( ri );
						}
					}
				}
				if( items.Count > 0 )
				{
					items.Sort( RowIdentifierComparison );
					OnDataChanged( SelectedItemsChangedEventArgs.TypeOfChange.Inserted, items.ToArray() );
				}
			}
		}

		private int RowIdentifierComparison( RowIdentifier x, RowIdentifier y )
		{
			if( x.Equals( y ) )
			{
				return 0;
			}

			int posX = _selectedRows[x];
			int posY = _selectedRows[y];
			if( posX != posY )
			{
				return posX - posY;
			}

			//
			//	Positions are the same so use the group positions if they have one. 
			//	Groups are always first compared with non grouped items.
			int groupPosX = x.GroupColumns.Length-1;
			int groupPosY = y.GroupColumns.Length-1;
			if( groupPosX > 0 && groupPosY == -1 )
			{
				return -1;
			}
			if( groupPosY > 0 && groupPosX == -1 )
			{
				return 1;
			}
			return groupPosX - groupPosY;
		}

		internal bool RemoveInternal( RowIdentifier rowIdentifier )
		{
			ClearCache();
			if( _selectedRows.ContainsKey( rowIdentifier ) )
			{
				_selectedRows.Remove( rowIdentifier );
				OnDataChanged( SelectedItemsChangedEventArgs.TypeOfChange.Deleted, rowIdentifier );
				return true;
			}
			return false;
		}

		internal void RemoveRangeInternal( RowIdentifier[] rowIdentifiers )
		{
			ClearCache();
			if( DataChanged == null )
			{
				foreach( RowIdentifier ri in rowIdentifiers )
				{
					_selectedRows.Remove( ri );
				}
			}
			else
			{
				List<RowIdentifier> items = new List<RowIdentifier>( rowIdentifiers.Length );
				List<object> keys = new List<object>( rowIdentifiers.Length );
				foreach( RowIdentifier ri in rowIdentifiers )
				{
					int position;
					if( _selectedRows.TryGetValue( ri, out position ) )
					{
						items.Add( ri );
						keys.Add( position );
						_selectedRows.Remove( ri );
					}
				}
				RowIdentifier[] itemsArray = items.ToArray();
				Array.Sort( keys.ToArray(), itemsArray );
				OnDataChanged( SelectedItemsChangedEventArgs.TypeOfChange.Deleted, itemsArray );
			}
		}

		private RowIdentifier[] GetSortedSelectionList()
		{
			RowIdentifier[] selectedItemsArray = _selectedItemsArrayRef == null ? null : (RowIdentifier[])_selectedItemsArrayRef.Target;
			if( selectedItemsArray == null )
			{
				List<RowIdentifier> items = new List<RowIdentifier>( _selectedRows.Count );
				List<object> keys = new List<object>( _selectedRows.Count );
				foreach( KeyValuePair<RowIdentifier, int> kv in _selectedRows )
				{
					items.Add( kv.Key );
					keys.Add( kv.Value );
				}
				RowIdentifier[] itemsArray = items.ToArray();
				Array.Sort( keys.ToArray(), itemsArray );
				selectedItemsArray = items.ToArray();
				_selectedItemsArrayRef = new WeakReference( selectedItemsArray );
			}
			return selectedItemsArray;
		}

		private void OnDataChanged( SelectedItemsChangedEventArgs.TypeOfChange changeType, params RowIdentifier[] items )
		{
			if( DataChanged != null )
			{
				SelectedItemsChangedEventArgs eventArgs = new SelectedItemsChangedEventArgs( items, changeType );
				DataChanged( _listControl, eventArgs );
			}
		}

		private void ClearCache()
		{
			_selectedItemsArrayRef = null;
		}

		private Dictionary<RowIdentifier, int> _selectedRows = new Dictionary<RowIdentifier, int>();
		private readonly ListControl _listControl;
		private WeakReference _selectedItemsArrayRef = null;

		#endregion
	}
}