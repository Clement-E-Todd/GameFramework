using System.Collections.Generic;
using UnityEngine;

namespace ClementTodd
{
    public class Properties
    {
        private Dictionary<string, object> properties = new Dictionary<string, object>();

        public T Get<T>(string key)
        {
            object value;
            bool keyWasFound = properties.TryGetValue(key, out value);

            if (!keyWasFound)
            {
                return default(T);
            }

            if (value is T)
            {
                return (T)value;
            }
            else
            {
                Debug.LogWarningFormat("Wrong type requested for property '{0}'. Returning: {1}", key, default(T));
                return default(T);
            }
        }

        public void Set(string key, object value)
        {
            if (properties.ContainsKey(key))
            {
                properties[key] = value;
            }
            else
            {
                properties.Add(key, value);
            }
        }

        public bool CompareType<T>(string key)
        {
            return properties.ContainsKey(key) && properties[key] is T;
        }
    }
}