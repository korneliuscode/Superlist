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
    public sealed class Set<T> : IEnumerable<T>
    {
        public Set()
        {
            _values = new Dictionary<T, int>();
        }

        public Set(IEqualityComparer<T> comparer)
        {
            _values = new Dictionary<T, int>(comparer);
        }

        public Set(IEnumerable<T> ts)
            : this()
        {
            foreach (T t in ts)
            {
                if (!Contains(t))
                {
                    Add(t);
                }
            }
        }

        public int Count
        {
            get
            {
                return _values.Count;
            }
        }

        public void Clear()
        {
            _values.Clear();
        }

        public void Add(T t)
        {
            _values.Add(t, 0);
        }

        public void AddRange(IEnumerable<T> ts)
        {
            foreach (T t in ts)
            {
                Add(t);
            }
        }

        public void Remove(T t)
        {
            _values.Remove(t);
        }

        public void RemoveRange(IEnumerable<T> ts)
        {
            foreach (T t in ts)
            {
                Remove(t);
            }
        }

        public bool Contains(T t)
        {
            return _values.ContainsKey(t);
        }

        public Set<T> ShallowCopy()
        {
            Set<T> copy = new Set<T>();

            foreach (T t in this)
            {
                copy.Add(t);
            }

            return copy;
        }

        public T[] ToArray()
        {
            T[] ts = new T[Count];
            int pos = 0;

            foreach (T t in this)
            {
                ts[pos] = t;
                ++pos;
            }

            return ts;
        }

        public static Set<T> Intersect(params Set<T>[] sets)
        {
            if (sets == null)
            {
                throw new ArgumentNullException("sets");
            }
            if (sets.Length == 0)
            {
                return new Set<T>();
            }

            if (sets[0] == null)
            {
                throw new ArgumentNullException("sets[0]");
            }

            Set<T> counted = sets[0].ShallowCopy();

            for (int i = 1; i < sets.Length; ++i)
            {
                Set<T> set = sets[i];

                if (set == null)
                {
                    throw new ArgumentNullException(string.Format("sets[{0}]", i));
                }

                foreach (T t in set)
                {
                    int count;

                    if (counted._values.TryGetValue(t, out count))
                    {
                        counted._values[t] = count + 1;
                    }
                }
            }

            Set<T> intersection = new Set<T>();
            int c = sets.Length - 1;

            foreach (KeyValuePair<T, int> kvp in counted._values)
            {
                if (kvp.Value == c)
                {
                    intersection.Add(kvp.Key);
                }
            }

            return intersection;
        }

        public static void Differences(Set<T> first, Set<T> second, out T[] onlyInFirst, out T[] inBoth, out T[] onlyInSecond)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }

            List<T> listOnlyInFirst = new List<T>();
            List<T> listInBoth = new List<T>();
            List<T> listOnlyInSecond = new List<T>();

            foreach (T t in first)
            {
                if (second.Contains(t))
                {
                    listInBoth.Add(t);
                }
                else
                {
                    listOnlyInFirst.Add(t);
                }
            }
            foreach (T t in second)
            {
                if (!first.Contains(t))
                {
                    listOnlyInSecond.Add(t);
                }
            }

            onlyInFirst = listOnlyInFirst.ToArray();
            inBoth = listInBoth.ToArray();
            onlyInSecond = listOnlyInSecond.ToArray();
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            foreach (KeyValuePair<T, int> kvp in _values)
            {
                yield return kvp.Key;
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private Dictionary<T, int> _values;
    }
}