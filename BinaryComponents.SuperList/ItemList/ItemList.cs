/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Windows.Forms;
using BinaryComponents.Utility.Collections;

namespace BinaryComponents.SuperList.ItemLists
{
    /// <summary>
    /// Sometimes the list needs to sort the items. This can be done either in
    /// a separate thread or OnIdle.
    /// </summary>
    public enum ProcessingStyle
    {
        /// <summary>
        /// Processes sort jobs in a separate thread.
        /// </summary>
        Thread,

        /// <summary>
        /// Processes sort jobs when the application is idle.
        /// </summary>
        OnIdle
    }

    /// <summary>
    /// Base class for implementing list storage mechanism
    /// </summary>
    public abstract class ItemList
    {
        /// <summary>
        /// Returns the count of items this list holds.
        /// </summary>
        public abstract int Count
        {
            get;
        }

        public abstract void Add(object item);
        public abstract void AddRange(IEnumerable items);
        public abstract void Remove(object item);
        public abstract void RemoveRange(IEnumerable items);
        public abstract void Clear();
        public abstract object[] ToArray();
        public abstract void ItemsChanged(params object[] items);
        public abstract int IndexOf(object o);

        public abstract void SynchroniseWithUINow();

        /// <summary>
        /// The must not return the same same sort order for any given two items otherwise otherwise after sorting 
        /// items can jump around as dotnet sort doesn't guarentee original order placement where items have the 
        /// same sort order value.
        /// </summary>
        public IComparer ObjectComparer
        {
            get
            {
                return _objectComparer;
            }
            set
            {
                _objectComparer = value;
            }
        }


        /// <summary>
        /// Sometimes the list needs to sort the items. This can be done either in
        /// a separate thread or OnIdle. Default is OnIdle.
        /// </summary>
        public ProcessingStyle ProcessingStyle
        {
            get
            {
                return _processingStyle;
            }
            set
            {
                _processingStyle = value;
            }
        }


        /// <summary>
        /// Called by this class if the data needs to be re-sorted.
        /// </summary>
        public abstract void Sort();


        /// <summary>
        /// Returns the the item at <paramref name="index"/>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract object this[int index]
        {
            get;
        }


        /// <summary>
        /// Called by the list to tell the list to do it internal updating
        /// </summary>
        protected internal abstract void DoHouseKeeping();

        /// <summary>
        /// Returns the attached list control.
        /// </summary>
        protected internal ListControl ListControl
        {
            get
            {
                return _listControl;
            }
            set
            {
                if (_listControl != value)
                {
                    if (_listControl != null)
                    {
                        _listControl.Columns.DataChanged -= Columns_DataChanged;
                        DetachFromColumnsEvents(_listControl.Columns.ToArray());
                    }
                    _listControl = value;
                    if (_listControl != null)
                    {
                        _listControl.Columns.DataChanged += Columns_DataChanged;
                        AttachToColumnsEvents(_listControl.Columns.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Called by derived classes when they have updated information.
        /// </summary>
        protected void ListUpdated(bool lazyLayout)
        {
            ListControl.UpdateListSection(lazyLayout);
        }

        private void Columns_DataChanged(object sender, EventingList<Column>.EventInfo e)
        {
            switch (e.EventType)
            {
                case EventingList<Column>.EventType.Deleted:
                    DetachFromColumnsEvents(e.Items);
                    break;
                case EventingList<Column>.EventType.Added:
                    AttachToColumnsEvents(e.Items);
                    break;
            }
        }

        private void AttachToColumnsEvents(Column[] columns)
        {
            foreach (Column column in columns)
            {
                column.DataChanged += column_DataChanged;
            }
        }

        private void DetachFromColumnsEvents(Column[] columns)
        {
            foreach (Column column in columns)
            {
                column.DataChanged -= column_DataChanged;
            }
        }

        private void column_DataChanged(object sender, Column.ColumnDataChangedEventArgs e)
        {
            switch (e.WhatChanged)
            {
                case Column.WhatPropertyChanged.IsGroup:
                    Sort();
                    break;
                case Column.WhatPropertyChanged.IsVisible:
                    if (e.Column.SortOrder != SortOrder.None)
                    {
                        Sort();
                    }
                    break;
                case Column.WhatPropertyChanged.SortOrder:
                    Sort();
                    break;
            }
        }

        /// <summary>
        /// Represents the grouping used when the list was created.
        /// </summary>
        internal Column[] GroupColumns
        {
            get
            {
                return _groupColumns;
            }
            set
            {
                _groupColumns = value;
            }
        }


        private ProcessingStyle _processingStyle = ProcessingStyle.OnIdle;
        private IComparer _objectComparer = null;
        private Column[] _groupColumns;
        private ListControl _listControl;
    }
}