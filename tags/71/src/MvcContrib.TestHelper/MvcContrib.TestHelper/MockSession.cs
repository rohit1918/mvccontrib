using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;
using IHttpSessionState=System.Web.IHttpSessionState;

namespace MvcTestingFramework
{
    internal class MockSession : IHttpSessionState, ICollection, IEnumerable
    {
        private IDictionary _objects;

        public MockSession()
        {
            // As per reflected HttpStaticObjectsCollection
            _objects = new Hashtable(StringComparer.OrdinalIgnoreCase);
        }

        #region IHttpSessionState Members

        public void Add(string name, object value)
        {
            _objects.Add(name, value);
        }

        public void Clear()
        {
            _objects.Clear();
        }

        //this is perhaps the most ridiculous interface property ever. Reflecting the framework shows that it is never called, and returns 'this'
        public IHttpSessionState Contents
        {
            get { return this; }
        }

        public void Remove(string name)
        {
            _objects.Remove(name);
        }

        public void RemoveAll()
        {
            _objects.Clear();
        }

        public void RemoveAt(int index)
        {
            string key = GetKeyFromIndex(index);
            if(!string.IsNullOrEmpty(key))
                Remove(key);
        }

        public object this[string name]
        {
            get { return _objects[name]; }
            set { _objects[name] = value; }
        }

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

        public void CopyTo(Array array, int index)
        {
            _objects.CopyTo(array, index);
        }

        public int Count
        {
            get { return _objects.Count; }
        }

        public bool IsSynchronized
        {
            get { return _objects.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return _objects.SyncRoot; }
        }

        public IEnumerator GetEnumerator()
        {
            return _objects.GetEnumerator();
        }

        //All interface methods below this are not implemented

        public void Abandon()
        {
            throw new NotImplementedException();
        }

        public string SessionID
        {
            get { throw new NotImplementedException(); }
        }

        public HttpStaticObjectsCollection StaticObjects
        {
            get { throw new NotImplementedException(); }
        }

        public int Timeout
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


        public HttpCookieMode CookieMode
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsCookieless
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsNewSession
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        //NameObjectCollectionBase.KeysCollection.ctor is marked internal
        public NameObjectCollectionBase.KeysCollection Keys
        {
            get { throw new NotImplementedException(); }
        }

        public int LCID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public SessionStateMode Mode
        {
            get { throw new NotImplementedException(); }
        }

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