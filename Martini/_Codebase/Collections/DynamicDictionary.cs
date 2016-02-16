using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Martini.Collections
{
    internal class DynamicKeyValuePair<TKey, TValue> : DynamicObject
    {
        private readonly string _keyName;
        private readonly string _valueName;

        private readonly KeyValuePair<TKey, TValue> _keyValuePair;

        public DynamicKeyValuePair(KeyValuePair<TKey, TValue> keyValuePair, string keyName, string valueName)
        {
            _keyValuePair = keyValuePair;
            _keyName = keyName;
            _valueName = valueName;
        }

        public TKey Key => _keyValuePair.Key;

        public TValue Value => _keyValuePair.Value;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder.Name == _keyName)
            {
                result = _keyValuePair.Key;
                return true;
            }
            if (binder.Name == _valueName)
            {
                result = _keyValuePair.Value;
                return true;
            }

            result = null;
            return false;
        }
    }

    internal class DynamicDictionary<TKey, TValue> : ICollection<DynamicKeyValuePair<TKey, TValue>>, IEnumerable<DynamicKeyValuePair<TKey, TValue>>, IEnumerable
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        private readonly string _keyName;
        private readonly string _valueName;

        public DynamicDictionary(string keyName, string valueName)
        {
            _keyName = keyName;
            _valueName = valueName;
        }

        public IEnumerator<DynamicKeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.Select(x => new DynamicKeyValuePair<TKey, TValue>(x, _keyName, _valueName)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(DynamicKeyValuePair<TKey, TValue> item)
        {
            _dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(DynamicKeyValuePair<TKey, TValue> item)
        {
            return _dictionary.ContainsKey(item.Key);
        }

        public void CopyTo(DynamicKeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {

        }

        public bool Remove(DynamicKeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Remove(item.Key);
        }

        public int Count => _dictionary.Count;

        public bool IsReadOnly => false;

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get { return this[key]; }
            set { this[key] = value; }
        }

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<TValue> Values => _dictionary.Values;
    }
}