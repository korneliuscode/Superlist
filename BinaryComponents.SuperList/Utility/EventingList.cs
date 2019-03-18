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

namespace BinaryComponents.Utility.Collections
{
    /// <summary>
    /// List style class that fires events when items are added / deleted.
    /// </summary>
    public class EventingList<T> : IList<T>
    {
        public enum EventType
        {
            Deleted,
            Added
        } ;

        public class EventInfo : EventArgs
        {
            public EventInfo(EventType eventType, T item)
            {
                EventType = eventType;
                Items = new T[] {item};
            }

            public EventInfo(EventType eventType, T[] items)
            {
                EventType = eventType;
                Items = items;
            }

            public readonly EventType EventType;
            public readonly T[] Items;
        }

        public T[] ToArray()
        {
            return _items.ToArray();
        }

        public event EventHandler<EventInfo> PreDataChanged;
        public event EventHandler<EventInfo> DataChanged;

        protected virtual void OnPreDataChanged(EventInfo eventInfo)
        {
            if (PreDataChanged != null)
            {
                PreDataChanged(this, eventInfo);
            }
        }

        protected virtual void OnDataChanged(EventInfo eventInfo)
        {
            if (DataChanged != null)
            {
                DataChanged(this, eventInfo);
            }
        }

        protected List<T> UnderlyingList
        {
            get
            {
                return _items;
            }
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            return _items.IndexOf(item);
        }


        public void Insert(int index, T item)
        {
            OnPreDataChanged(new EventInfo(EventType.Added, item));
            _items.Insert(index, item);
            OnDataChanged(new EventInfo(EventType.Added, item));
        }

        public void RemoveAt(int index)
        {
            T item = _items[index];
            OnPreDataChanged(new EventInfo(EventType.Deleted, item));
            _items.RemoveAt(index);
            OnDataChanged(new EventInfo(EventType.Deleted, item));
        }

        public void MoveItem(int from, int to)
        {
            if (from < 0 || from >= _items.Count)
            {
                throw new ArgumentOutOfRangeException("from");
            }
            if (to < 0 || to >= _items.Count)
            {
                throw new ArgumentOutOfRangeException("to");
            }

            T temp = _items[from];
            if (to > from)
            {
                to--;
            }
            _items.RemoveAt(from);
            _items.Insert(to, temp);
        }

        public T this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                if (!_items[index].Equals(value))
                {
                    T itemToRemove = _items[index];

                    OnPreDataChanged(new EventInfo(EventType.Deleted, itemToRemove));
                    OnPreDataChanged(new EventInfo(EventType.Added, value));

                    _items[index] = value;

                    OnDataChanged(new EventInfo(EventType.Deleted, itemToRemove));
                    OnDataChanged(new EventInfo(EventType.Added, value));
                }
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            OnPreDataChanged(new EventInfo(EventType.Added, item));
            _items.Add(item);
            OnDataChanged(new EventInfo(EventType.Added, item));
        }

        public void AddRange(T[] items)
        {
            OnPreDataChanged(new EventInfo(EventType.Added, items));
            _items.AddRange(items);
            OnDataChanged(new EventInfo(EventType.Added, items));
        }


        public void Clear()
        {
            if (_items.Count > 0)
            {
                T[] items = _items.ToArray();
                OnPreDataChanged(new EventInfo(EventType.Deleted, items));
                _items.Clear();
                OnDataChanged(new EventInfo(EventType.Deleted, items));
            }
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void RemoveRange(int start, int count)
        {
            T[] items = _items.GetRange(start, count).ToArray();
            OnPreDataChanged(new EventInfo(EventType.Deleted, items));
            _items.RemoveRange(start, count);
            OnDataChanged(new EventInfo(EventType.Deleted, items));
        }

        public void RemoveRange(T[] items)
        {
            if (items.Length == 0)
            {
                return;
            }

            OnPreDataChanged(new EventInfo(EventType.Deleted, items));

            Dictionary<T, int> itemIndexes = new Dictionary<T, int>(_items.Count);
            int i = 0;
            foreach (T item in _items)
            {
                itemIndexes[item] = i++;
            }
            List<int> removeList = new List<int>();
            foreach (T item in items)
            {
                int index;
                if (itemIndexes.TryGetValue(item, out index))
                {
                    itemIndexes.Remove(item); // only allow to be deleted once
                    removeList.Add(index);
                }
            }
            if (removeList.Count == _items.Count)
            {
                _items.Clear();
            }
            else
            {
                removeList.Sort();
                int start = -1;
                int count = 0;
                for (i = removeList.Count - 1; i >= 0; i--)
                {
                    int removeAt = removeList[i];
                    if (removeAt + 1 == start)
                    {
                        start = removeAt;
                        count++;
                    }
                    else
                    {
                        if (start != -1)
                        {
                            _items.RemoveRange(start, count);
                        }
                        start = removeAt;
                        count = 1;
                    }
                }
                if (start != -1 && count > 0)
                {
                    _items.RemoveRange(start, count);
                }
            }
            OnDataChanged(new EventInfo(EventType.Deleted, items));
        }

        public bool Remove(T item)
        {
            int i = _items.IndexOf(item);
            if (i != -1)
            {
                RemoveAt(i);
                return true;
            }

            return false;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion

        private List<T> _items = new List<T>();
    }
}