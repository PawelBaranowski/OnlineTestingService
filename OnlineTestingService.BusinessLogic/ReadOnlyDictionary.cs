using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineTestingService.BusinessLogic
{
    // The following class definition comes from http://stackoverflow.com
    // It was posted by dbkk at Aug 30th 2008
    // The original post can be found under this permanent link:
    // http://stackoverflow.com/a/35666
    // 
    // Many thanks for dbkk for sharing hers/his code!

    /// <summary>
    /// A dictionary that can only be queried for keys and values.
    /// </summary>
    /// <typeparam name="TKey">key type</typeparam>
    /// <typeparam name="TValue">value type</typeparam>
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private IDictionary<TKey, TValue> _dict;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> backingDict)
        {
            _dict = backingDict;
        }

        public void Add(TKey key, TValue value)
        {
            throw new InvalidOperationException();
        }

        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return _dict.Keys; }
        }

        public bool Remove(TKey key)
        {
            throw new InvalidOperationException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dict.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return _dict.Values; }
        }

        public TValue this[TKey key]
        {
            get { return _dict[key]; }
            set { throw new InvalidOperationException(); }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new InvalidOperationException();
        }

        public void Clear()
        {
            throw new InvalidOperationException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dict.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dict.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _dict.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new InvalidOperationException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        System.Collections.IEnumerator
               System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)_dict).GetEnumerator();
        }
    }
}