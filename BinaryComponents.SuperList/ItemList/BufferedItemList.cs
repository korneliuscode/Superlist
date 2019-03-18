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
using System.Threading;
using System.Windows.Forms;
using BinaryComponents.Utility.Collections;

namespace BinaryComponents.SuperList.ItemLists
{
    public class BufferedList : ItemList
    {
        public override int Count
        {
            get
            {
                lock (_lockObject)
                {
                    return _sortedListItems.Length;
                }
            }
        }

        public override void Clear()
        {
            lock (_lockObject)
            {
                _sortedListItems = new object[0];
                _deferredAdditions.Clear();
                _deferredDeletions.Clear();
								_sortNeeded = true;
            }
        }

        public override void Add(object item)
        {
            lock (_lockObject)
            {
                if (!_deferredAdditions.Contains(item))
                {
                    _deferredAdditions.Add(item);
                }
            }
        }

        public override void AddRange(IEnumerable items)
        {
            foreach (object item in items)
            {
                Add(item);
            }
        }

        public override int IndexOf(object o)
        {
            Dictionary<object, int> mapItems = null;
            lock (_lockObject)
            {
                if (_mapItems != null)
                {
                    mapItems = (Dictionary<object, int>) _mapItems.Target;
                }
                if (mapItems == null)
                {
                    mapItems = new Dictionary<object, int>();
                    if (_sortedListItems != null)
                    {
                        for (int i = 0; i < _sortedListItems.Length; i++)
                        {
                            mapItems[_sortedListItems[i]] = i;
                        }
                    }
                    _mapItems = new WeakReference(mapItems);
                }
            }
            int result;
            if (!mapItems.TryGetValue(o, out result))
            {
                result = -1;
            }
            return result;
        }

        private WeakReference _mapItems = null;

        public override void Remove(object item)
        {
            lock (_lockObject)
            {
                _deferredAdditions.Remove(item);
                if (!_deferredDeletions.Contains(item))
                {
                    _deferredDeletions.Add(item);
                }
            }
        }

        public override void ItemsChanged(params object[] items)
        {
            bool reLayout = false;
            //
            // only need to be concerned if the sort order changes
            if ((_jobGroupColumns != null && _jobGroupColumns.Length > 0) || (_jobSortColumns != null && _jobSortColumns.Length > 0))
            {
                lock (_lockObject)
                {
                    ColumnComparer comparer = new ColumnComparer(_jobGroupColumns, _jobSortColumns, this);

                    //
                    //	Check to see if changes have an effect of the sort order.
                    //	if they do then we add them to the deletes and adds.
                    //	Otherwise we don't need to do anything.
                    foreach (object o in items)
                    {
                        int searchResult = Array.BinarySearch(_sortedListItems, o, comparer);
                        if (searchResult < 0)
                        {
                            searchResult = ~searchResult;
                        }
                        if (searchResult < _sortedListItems.Length)
                        {
                            if (_sortedListItems[searchResult] == o)
                            {
                                reLayout = true; // visually may have changed so we layout
                            }
                            else
                            {
                                _deferredDeletions.Add(o);
                                _deferredAdditions.Add(o);
                            }
                        }
                    }
                }
            }
            if (reLayout)
            {
                ListControl.UpdateListSection(true);
            }
        }

        public override void RemoveRange(IEnumerable items)
        {
            foreach (object item in items)
            {
                Remove(item);
            }
        }

        public override object[] ToArray()
        {
            return _sortedListItems;
        }

        public override void SynchroniseWithUINow()
        {
            DoHouseKeeping(false);
        }

        #region Implementation

        #region ListControl communication methods

        public override object this[int index]
        {
            get
            {
                lock (_lockObject)
                {
                    DoHouseKeeping(false);
                    return _sortedListItems[index];
                }
            }
        }

        public override void Sort()
        {
            _sortNeeded = true;
        }


        protected internal override void DoHouseKeeping()
        {
            DoHouseKeeping(ProcessingStyle == ProcessingStyle.Thread);
        }

        #endregion

        private void DoHouseKeeping(bool runInBackground)
        {
            if (runInBackground)
            {
                CheckForJobCompletionResults();
                CheckForSortJob(runInBackground);
            }
            else
            {
                //
                // Spin wait if we have a queued job
                while (true)
                {
                    lock (_lockObject)
                    {
                        if (!_jobQueued)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(50);
                }
                CheckForSortJob(false);
                CheckForJobCompletionResults();
            }
        }

        private void CheckForJobCompletionResults()
        {
            lock (_lockObject)
            {
                if (!_jobQueued)
                {
                    if (_jobSortedList != null)
                    {
                        GroupColumns = _jobGroupColumns;
                        _sortedListItems = _jobSortedList.ToArray();
                        _jobSortedList = null;
                        _mapItems = null;
                        ListUpdated(false);
                    }
                }
            }
        }

        private void CheckForSortJob(bool runInBackground)
        {
            lock (_lockObject)
            {
                if (!_jobQueued)
                {
                    if (_deferredAdditions.Count > 0 || _deferredDeletions.Count > 0 || _sortNeeded)
                    {
                        _jobGroupColumns = GetGroupColumnsCopy();
                        _jobSortColumns = GetSortColumnsCopy();
                        _jobSortNeeded = _sortNeeded;
                        _sortNeeded = false;
                        _jobQueued = true;
                        if (runInBackground)
                        {
                            ThreadPool.QueueUserWorkItem(UpdateJob);
                        }
                        else
                        {
                            Cursor previousCursor = Cursors.Default;
                            try
                            {
                                if (ListControl.Cursor != Cursors.WaitCursor)
                                {
                                    previousCursor = ListControl.Cursor;
                                    ListControl.Cursor = Cursors.WaitCursor;
                                }
                                UpdateJob(null);
                            }
                            finally
                            {
                                if (previousCursor != Cursors.WaitCursor)
                                {
                                    ListControl.Cursor = previousCursor;
                                }
                            }
                        }
                    }
                }
            }
        }


        private void UpdateJob(object state)
        {
            try
            {
                object[] additions;
                object[] oldSortedList;
                Set<object> deferredDeletions = new Set<object>();

                bool sortNeeded;
                lock (_lockObject) // nip in to the shared areas and take a copy of the data 
                {
                    additions = _deferredAdditions.ToArray();
                    deferredDeletions = new Set<object>(_deferredDeletions);
                    oldSortedList = (object[]) _sortedListItems.Clone();
                    _deferredAdditions.Clear();
                    _deferredDeletions.Clear();
                    sortNeeded = _jobSortNeeded;
                    _jobSortNeeded = false;
                }

                List<object> newSortedList = new List<object>(_sortedListItems.Length + additions.Length);


                //
                // Copy the necessary lists
                if (deferredDeletions.Count > 0)
                {
                    for (int i = 0; i < oldSortedList.Length; i++)
                    {
                        object item = oldSortedList[i];
                        if (!deferredDeletions.Contains(item))
                        {
                            newSortedList.Add(item);
                        }
                    }
                }
                else
                {
                    newSortedList = new List<object>(_sortedListItems);
                }

                if (_jobGroupColumns.Length > 0 || _jobSortColumns.Length > 0)
                {
                    ColumnComparer comparer = new ColumnComparer(_jobGroupColumns, _jobSortColumns, this);

                    //
                    // Perform sort.
                    if (!sortNeeded && additions.Length != 0 && newSortedList.Count > additions.Length)
                    {
                        //
                        //	We insert the new entries using BinarySearch since the
                        //	default sort for the Clr is QuickSort which works best on
                        //	unsorted lists.
                        foreach (object item in additions)
                        {
                            int searchResult = newSortedList.BinarySearch(item, comparer);
                            if (searchResult < 0)
                            {
                                searchResult = ~searchResult;
                            }
                            newSortedList.Insert(searchResult, item);
                        }
                    }
                    else
                    {
                        newSortedList.AddRange(additions);
                        newSortedList.Sort(comparer);
                    }
                }
                else
                {
                    newSortedList.AddRange(additions);
                }
                lock (_lockObject)
                {
                    _jobSortedList = newSortedList;
                }
            }
            finally
            {
                _jobQueued = false;
            }
        }

        #region ColumnComparer

        private class ColumnComparer : IComparer<object>
        {
            public ColumnComparer(Column[] groupByColumns, Column[] sortByColumns, BufferedList list)
            {
                _list = list;
                _groupByColumns = groupByColumns;
                _sortByColumns = sortByColumns;
            }

            public int Compare(object x, object y)
            {
                if (x == y)
                {
                    return 0;
                }

                foreach (Column _column in _groupByColumns)
                {
                    int result = _column.GroupedComparitor(x, y);
                    if (result != 0)
                    {
                        return _column.GroupSortOrder == SortOrder.Descending ? -result : result;
                    }
                }

                if (_sortByColumns.Length == 0)
                {
                    foreach (Column column in _groupByColumns)
                    {
                        int result = column.Comparitor(x, y);
                        if (result != 0)
                        {
                            return column.GroupSortOrder == SortOrder.Descending ? -result : result;
                        }
                    }
                }
                else
                {
                    foreach (Column column in _sortByColumns)
                    {
                        int result = column.Comparitor(x, y);
                        if (result != 0)
                        {
                            return column.SortOrder == SortOrder.Descending ? -result : result;
                        }
                    }
                }

                if (x is IComparable)
                {
                    return ((IComparable) x).CompareTo(y);
                }

                if (_list.ObjectComparer == null)
                {
                    throw new InvalidOperationException("ItemList.ObjectComparer must be set. This is needed so that all rows have an order, otherwise rows can be prone to jumping around.");
                }

                return _list.ObjectComparer.Compare(x, y);
            }

            private readonly BufferedList _list;
            private readonly Column[] _groupByColumns;
            private readonly Column[] _sortByColumns;
        }

        #endregion

        private Column[] GetSortColumnsCopy()
        {
            List<Column> columns = new List<Column>();

            foreach (Column column in ListControl.Columns.VisibleItems)
            {
                if (column.SortOrder != SortOrder.None)
                {
                    columns.Add(new Column(column));
                }
            }
            return columns.ToArray();
        }

        private Column[] GetGroupColumnsCopy()
        {
            List<Column> columns = new List<Column>();

            foreach (Column column in ListControl.Columns.GroupedItems)
            {
                columns.Add(new Column(column));
            }
            return columns.ToArray();
        }

        private enum ActionType
        {
            Add,
            Remove
        } ;

        private struct Action
        {
            public Action(ActionType actionType, object item)
            {
                ActionType = actionType;
                Item = item;
            }

            public readonly ActionType ActionType;
            public readonly object Item;
        }

        private readonly Set<object> _deferredAdditions = new Set<object>();
        private readonly Set<object> _deferredDeletions = new Set<object>();

        private bool _sortNeeded = false;
        private bool _jobQueued = false; // trigger for job to be run
        private bool _jobSortNeeded = false;
        private Column[] _jobSortColumns = null;
        private Column[] _jobGroupColumns = null;
        private List<object> _jobSortedList = null;

        private readonly object _lockObject = new object();

        private object[] _sortedListItems = new object[0];

        #endregion
    }
}