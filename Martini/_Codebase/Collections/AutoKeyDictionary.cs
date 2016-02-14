using System;
using System.Collections;
using System.Collections.Generic;

namespace Martini.Collections
{
    internal class AutoKeyDictionary<TKey, T> : IEnumerable<T>
    {
        private readonly Func<T, TKey> _getKey;
        private readonly Dictionary<TKey, T> _items = new Dictionary<TKey, T>();

        public AutoKeyDictionary(Func<T, TKey> getKey)
        {
            _getKey = getKey;
        }

        public T this[TKey key]
        {
            get { return _items[key]; }
            set { _items[key] = value; }
        }

        public bool TryGetValue(TKey key, out T value)
        {
            return _items.TryGetValue(key, out value);
        }

        public bool Contains(T item)
        {
            return _items.ContainsKey(_getKey(item));
        }

        public void Add(T item)
        {
            _items[_getKey(item)] = item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}