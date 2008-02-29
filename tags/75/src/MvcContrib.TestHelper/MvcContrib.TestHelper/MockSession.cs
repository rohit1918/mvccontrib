using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;
using IHttpSessionState=System.Web.IHttpSessionState;

namespace MvcContrib.TestHelper
{
    internal class MockSession : IHttpSessionState, ICollection, IEnumerable
    {
        private IDictionary _objects;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockSession"/> class.
        /// </summary>
        public MockSession()
        {
            // As per reflected HttpStaticObjectsCollection
            _objects = new Hashtable(StringComparer.OrdinalIgnoreCase);
        }

        #region IHttpSessionState Members

        /// <summary>
        /// Adds the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Add(string name, object value)
        {
            _objects.Add(name, value);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _objects.Clear();
        }

        //this is perhaps the most ridiculous interface property ever. Reflecting the framework shows that it is never called, and returns 'this'
        /// <summary>
        /// Gets the contents.
        /// </summary>
        /// <value>The contents.</value>
        public IHttpSessionState Contents
        {
            get { return this; }
        }

        /// <summary>
        /// Removes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Remove(string name)
        {
            _objects.Remove(name);
        }

        /// <summary>
        /// Removes all.
        /// </summary>
        public void RemoveAll()
        {
            _objects.Clear();
        }

        /// <summary>
        /// Removes an object at the index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            string key = GetKeyFromIndex(index);
            if(!string.IsNullOrEmpty(key))
                Remove(key);
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified name.
        /// </summary>
        /// <value></value>
        public object this[string name]
        {
            get { return _objects[name]; }
            set { _objects[name] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        /// <value></value>
        public object this[int index]
        {
            get
            {
                string key = GetKeyFromIndex(index);
                return _objects[key];
            }
            set
            {
                string key = GetKeyFromIndex(index);
                _objects[key] = value;
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="array"/> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="array"/> is multidimensional.-or- <paramref name="index"/> is equal to or greater than the length of <paramref name="array"/>.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception>
        /// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception>
        public void CopyTo(Array array, int index)
        {
            _objects.CopyTo(array, index);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.</returns>
        public int Count
        {
            get { return _objects.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <value></value>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
        public bool IsSynchronized
        {
            get { return _objects.IsSynchronized; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
        public object SyncRoot
        {
            get { return _objects.SyncRoot; }
        }

        public IEnumerator GetEnumerator()
        {
            return _objects.GetEnumerator();
        }

        //All interface methods below this are not implemented

        /// <summary>
        /// Not Implemented
        /// </summary>
        public void Abandon()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public string SessionID
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public HttpStaticObjectsCollection StaticObjects
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public int Timeout
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public HttpCookieMode CookieMode
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public bool IsCookieless
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public bool IsNewSession
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        //NameObjectCollectionBase.KeysCollection.ctor is marked internal
        public NameObjectCollectionBase.KeysCollection Keys
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public int LCID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public SessionStateMode Mode
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public int CodePage
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        protected string GetKeyFromIndex(int index)
        {
            if(index < 0 || index >= _objects.Count)
                throw new ArgumentOutOfRangeException();
            int i = _objects.Count - 1; //Standard session implements as list, hashtable implements as stack
            foreach(object key in _objects.Keys)
            {
                if(i-- == index)
                {
                    return key as string;
                }
            }
            return null;
        }
    }
}
