using System;
using System.Collections.Generic;

namespace Personalized_Online_Fashion_Shop_Desktop_App
{
    public static class Session
    {
        private static readonly Dictionary<string, string> data = new Dictionary<string, string>();

        public static void Set(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            data[key] = value;
        }

        public static string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            return data.TryGetValue(key, out string value) ? value : null;
        }

        public static void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            data.Remove(key);
        }

        public static void Clear()
        {
            data.Clear();
        }

        public static bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            return data.ContainsKey(key);
        }
    }
}