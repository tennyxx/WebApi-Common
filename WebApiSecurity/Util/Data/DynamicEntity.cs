using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace WebApi.Common.Util
{
    public class DynamicEntity : DynamicObject, IDictionary<string, object>
    {
        private readonly Dictionary<string, object> _dictionary;

        public DynamicEntity()
        {
            this._dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        public int Count
        {
            get { return this._dictionary.Count; }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this._dictionary[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this._dictionary.TryGetValue(binder.Name, out result);
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }


        public object this[string key]
        {
            get { return this._dictionary[key]; }

            set { this._dictionary[key] = value; }
        }


        public void Add(string key, object value)
        {
            this._dictionary.Add(key, value);
        }

        public ICollection<string> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(string key)
        {
            this._dictionary.Remove(key);
            return true;
        }

        public bool TryGetValue(string key, out object value)
        {
            try
            {
                value = this._dictionary[key];
            }
            catch
            {
                value = null;
                return false;
            }
            return true;
        }

        public ICollection<object> Values
        {
            get { return this._dictionary.Values; }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            this._dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this._dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return this._dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            this._dictionary.Remove(item.Key);
            return true;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this._dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._dictionary.GetEnumerator();
        }

        #region 扩展方法 2016-08-24

        public string GetValueAsString(string key)
        {
            if (_dictionary.ContainsKey(key))
            {
                return _dictionary[key] == null ? "" : _dictionary[key].ToString();
            }
            return "";
        }

        public decimal GetValueAsDecimal(string key)
        {
            if (_dictionary.ContainsKey(key))
            {
                return _dictionary[key] == null ? 0 : decimal.Parse(_dictionary[key].ToString());
            }
            return 0;
        }

        public int GetValueAsInt(string key)
        {
            if (_dictionary.ContainsKey(key))
            {
                return _dictionary[key] == null ? 0 : int.Parse(_dictionary[key].ToString());
            }
            return 0;
        }

        public short GetValueAsShort(string key)
        {
            if (_dictionary.ContainsKey(key))
            {
                return _dictionary[key] == null ? (short)0 : short.Parse(_dictionary[key].ToString());
            }
            return (short)0;
        }

        public long GetValueAsLong(string key)
        {
            if (_dictionary.ContainsKey(key))
            {
                return _dictionary[key] == null ? 0 : long.Parse(_dictionary[key].ToString());
            }
            return 0;
        }

        #endregion
    }
}
