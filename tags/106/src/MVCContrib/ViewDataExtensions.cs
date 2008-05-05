using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcContrib
{
    public static class ViewDataExtensions
    {
        /// <summary>
        /// Adds an object using the type as the dictionary key
        /// </summary>
        public static IDictionary<string, object> Add<T>(this IDictionary<string, object> bag, T anObject)
        {
            Type type = typeof (T);
            if (bag.ContainsKey(getKey(type)))
            {
                string message = string.Format("You can only add one default object for type '{0}'.", type);
                throw new ArgumentException(message);
            }

            bag.Add(getKey(type), anObject);
            return bag;
        }

        public static IDictionary<string, object> Add<T>(this IDictionary<string, object> bag, string key, T value)
        {
            bag.Add(key, value);
            return bag;
        }

        public static T Get<T>(this IDictionary<string, object> bag)
        {
            return bag.Get<T>(getKey(typeof (T)));
        }

        public static T GetOrDefault<T>(this IDictionary<string, object> bag, string key, T defaultValue)
        {
            if (bag.ContainsKey(key))
                return (T) bag[key];

            return defaultValue;
        }

        public static object Get(this IDictionary<string, object> bag, Type type)
        {
            if (!bag.ContainsKey(getKey(type)))
            {
                string message = string.Format("No object exists that is of type '{0}'.", type);
                throw new ArgumentException(message);
            }

            return bag[getKey(type)];
        }

        private static string getKey(Type type)
        {
            return type.FullName;
        }

        public static bool Contains<T>(this IDictionary<string, object> bag)
        {
            return bag.ContainsKey(getKey(typeof (T)));
        }

        public static bool Contains(this IDictionary<string, object> bag, Type keyType)
        {
            return bag.ContainsKey(getKey(keyType));
        }

        public static T Get<T>(this IDictionary<string, object> bag, string key)
        {
            if (!bag.ContainsKey(key))
            {
                string message = string.Format("No object exists with key '{0}'.", key);
                throw new ArgumentException(message);
            }

            return (T) bag[key];
        }

        public static int GetCount(this IDictionary<string, object> bag, Type type)
        {
            int count = 0;
            foreach (object value in bag.Values)
            {
                if (type.Equals(value.GetType()))
                {
                    count++;
                }
            }

            return count;
        }

        //ViewData extensions

        public static T Get<T>(this ViewData bag)
        {
            return bag.Get<T>(getKey(typeof(T)));
        }

        public static T GetOrDefault<T>(this ViewData bag, string key, T defaultValue)
        {
            if (bag.ContainsDataItem(key))
                return (T)bag[key];

            return defaultValue;
        }

        public static object Get(this ViewData bag, Type type)
        {
            if (!bag.ContainsDataItem(getKey(type)))
            {
                string message = string.Format("No object exists that is of type '{0}'.", type);
                throw new ArgumentException(message);
            }

            return bag[getKey(type)];
        }

        public static bool Contains<T>(this ViewData bag)
        {
            return bag.ContainsDataItem(getKey(typeof(T)));
        }

        public static bool Contains(this ViewData bag, Type keyType)
        {
            return bag.ContainsDataItem(getKey(keyType));
        }

        public static bool Contains(this ViewData bag, string key)
        {
            return bag.ContainsDataItem(key);
        }

        public static T Get<T>(this ViewData bag, string key)
        {
            if (!bag.ContainsDataItem(key))
            {
                string message = string.Format("No object exists with key '{0}'.", key);
                throw new ArgumentException(message);
            }

            return (T)bag[key];
        }
    }
}