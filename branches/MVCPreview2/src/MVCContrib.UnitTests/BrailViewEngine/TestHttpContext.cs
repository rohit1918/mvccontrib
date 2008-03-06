using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Profile;
using System.Web.SessionState;
using HttpSessionStateBase = System.Web.HttpSessionStateBase;

namespace MVCContrib.UnitTests.BrailViewEngine
{
	public class TestHttpContext : HttpContextBase
	{
		private readonly List<Exception> _allErrors = new List<Exception>();
		private HttpApplicationState _Application;
		private HttpApplication _ApplicationInstance;
		private Cache _Cache = new Cache();
		private IHttpHandler _CurrentHandler = new TestHttpHandler();
		private RequestNotification _CurrentNotification = RequestNotification.AcquireRequestState;
		private Exception _Error = null;
		private IHttpHandler _Handler = new TestHttpHandler();
		private bool _IsCustomErrorEnabled = false;
		private bool _IsDebuggingEnabled = false;
		private bool _IsPostNotification = false;
		private IDictionary _Items = new Hashtable();
		private IHttpHandler _PreviousHandler = new TestHttpHandler();
		private ProfileBase _Profile;
		private TestHttpRequest _Request = new TestHttpRequest();
		private TestHttpResponse _Response = new TestHttpResponse();
		private HttpServerUtilityBase _Server = new TestHttpServerUtility();
		private HttpSessionStateBase _Session = new TestHttpSessionState();
		private bool _SkipAuthorization;
		private DateTime _Timestamp = DateTime.Now;
		private TraceContext _Trace = new TraceContext(null);
		private IPrincipal _User;

        //public void AddError(Exception errorInfo)
        //{
        //    _allErrors.Add(errorInfo);
        //}

        //public void ClearError()
        //{
        //    _allErrors.Clear();
        //}

        //public object GetSection(string sectionName)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RewritePath(string path)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RewritePath(string path, bool rebaseClientPath)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RewritePath(string filePath, string pathInfo, string queryString)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath)
        //{
        //    throw new NotImplementedException();
        //}

        //public Exception[] AllErrors
        //{
        //    get { return _allErrors.ToArray(); }
        //}

        //public HttpApplicationState Application
        //{
        //    get { return _Application; }
        //    set { _Application = value; }
        //}

        //public HttpApplication ApplicationInstance
        //{
        //    get { return _ApplicationInstance; }
        //    set { _ApplicationInstance = value; }
        //}

        //public Cache Cache
        //{
        //    get { return _Cache; }
        //    set { _Cache = value; }
        //}

        //public IHttpHandler CurrentHandler
        //{
        //    get { return _CurrentHandler; }
        //    set { _CurrentHandler = value; }
        //}

        //public RequestNotification CurrentNotification
        //{
        //    get { return _CurrentNotification; }
        //    set { _CurrentNotification = value; }
        //}

        //public Exception Error
        //{
        //    get { return _Error; }
        //    set { _Error = value; }
        //}

        //public IHttpHandler Handler
        //{
        //    get { return _Handler; }
        //    set { _Handler = value; }
        //}

        //public bool IsCustomErrorEnabled
        //{
        //    get { return _IsCustomErrorEnabled; }
        //    set { _IsCustomErrorEnabled = value; }
        //}

        //public bool IsDebuggingEnabled
        //{
        //    get { return _IsDebuggingEnabled; }
        //    set { _IsDebuggingEnabled = value; }
        //}

        //public bool IsPostNotification
        //{
        //    get { return _IsPostNotification; }
        //    set { _IsPostNotification = value; }
        //}

        //public IDictionary Items
        //{
        //    get { return _Items; }
        //    set { _Items = value; }
        //}

        //public IHttpHandler PreviousHandler
        //{
        //    get { return _PreviousHandler; }
        //    set { _PreviousHandler = value; }
        //}

        //public ProfileBase Profile
        //{
        //    get { return _Profile; }
        //    set { _Profile = value; }
        //}

		
        //public TestHttpRequest Request
        //{
        //    get { return _Request; }
        //    set { _Request = value; }
        //}

	

        //public TestHttpResponse Response
        //{
        //    get { return _Response; }
        //    set { _Response = value; }
        //}

        //public HttpServerUtilityBase Server
        //{
        //    get { return _Server; }
        //    set { _Server = value; }
        //}

        //public HttpSessionStateBase Session
        //{
        //    get { return _Session; }
        //    set { _Session = value; }
        //}

        //public bool SkipAuthorization
        //{
        //    get { return _SkipAuthorization; }
        //    set { _SkipAuthorization = value; }
        //}

        //public DateTime Timestamp
        //{
        //    get { return _Timestamp; }
        //    set { _Timestamp = value; }
        //}

        //public TraceContext Trace
        //{
        //    get { return _Trace; }
        //    set { _Trace = value; }
        //}

        //public IPrincipal User
        //{
        //    get { return _User; }
        //    set { _User = value; }
        //}

        //public object GetService(Type serviceType)
        //{
        //    throw new NotImplementedException();
        //}
	}

	public class TestHttpHandler : IHttpHandler
	{
		private bool _IsReusable;

		public void ProcessRequest(HttpContext context)
		{
			throw new NotImplementedException();
		}

		public bool IsReusable
		{
			get { return _IsReusable; }
			set { _IsReusable = value; }
		}
	}

	public class TestHttpRequest : HttpRequestBase
	{
		private readonly List<string> _acceptTypes = new List<string>();
		private readonly List<string> _userLanguages = new List<string>();
		private string _ApplicationPath;
		private string _AnonymousID;
		private string _AppRelativeCurrentExecutionFilePath;
		private HttpBrowserCapabilitiesBase _Browser;
		private HttpClientCertificate _ClientCertificate;
		private Encoding _ContentEncoding;
		private int _ContentLength;
		private string _ContentType;
		private HttpCookieCollection _Cookies = new HttpCookieCollection();
		private string _CurrentExecutionFilePath;
		private string _FilePath;
		private HttpFileCollection _Files;
		private Stream _Filter;
		private NameValueCollection _Form = new NameValueCollection();
		private string _HttpMethod;
		private Stream _InputStream;
		private bool _IsAuthenticated;
		private bool _IsSecureConnection;
		private WindowsIdentity _LogonUserIdentity;
		private string _Path;
		private string _PathInfo;
		private string _PhysicalApplicationPath;
		private string _PhysicalPath;
		private string _RawUrl;
		private string _RequestType;
		private NameValueCollection _ServerVariables = new NameValueCollection();
		private int _TotalBytes;
		private Uri _Url;
		private Uri _UrlReferrer;
		private string _UserAgent;
		private string _UserHostAddress;
		private string _UserHostName;
		private NameValueCollection _Headers = new NameValueCollection();
		private NameValueCollection _QueryString = new NameValueCollection();

        //public byte[] BinaryRead(int count)
        //{
        //    throw new NotImplementedException();
        //}

        //public int[] MapImageCoordinates(string imageFieldName)
        //{
        //    throw new NotImplementedException();
        //}

        //public string MapPath(string virtualPath)
        //{
        //    throw new NotImplementedException();
        //}

        //public string MapPath(string virtualPath, string baseVirtualDir, bool allowCrossAppMapping)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ValidateInput()
        //{
        //    throw new NotImplementedException();
        //}

        //public void SaveAs(string filename, bool includeHeaders)
        //{
        //    throw new NotImplementedException();
        //}

        //public string[] AcceptTypes
        //{
        //    get { return _acceptTypes.ToArray(); }
        //}

        //public void AddAcceptType(string value)
        //{
        //    _acceptTypes.Add(value);
        //}

        //public string ApplicationPath
        //{
        //    get { return _ApplicationPath; }
        //    set { _ApplicationPath = value; }
        //}

        //public string AnonymousID
        //{
        //    get { return _AnonymousID; }
        //    set { _AnonymousID = value; }
        //}

        //public string AppRelativeCurrentExecutionFilePath
        //{
        //    get { return _AppRelativeCurrentExecutionFilePath; }
        //    set { _AppRelativeCurrentExecutionFilePath = value; }
        //}

        //public HttpBrowserCapabilitiesBase Browser
        //{
        //    get { return _Browser; }
        //    set { _Browser = value; }
        //}

        //public HttpClientCertificate ClientCertificate
        //{
        //    get { return _ClientCertificate; }
        //    set { _ClientCertificate = value; }
        //}

        //public Encoding ContentEncoding
        //{
        //    get { return _ContentEncoding; }
        //    set { _ContentEncoding = value; }
        //}

        //public int ContentLength
        //{
        //    get { return _ContentLength; }
        //    set { _ContentLength = value; }
        //}

        //public string ContentType
        //{
        //    get { return _ContentType; }
        //    set { _ContentType = value; }
        //}

        //public HttpCookieCollection Cookies
        //{
        //    get { return _Cookies; }
        //    set { _Cookies = value; }
        //}

        //public string CurrentExecutionFilePath
        //{
        //    get { return _CurrentExecutionFilePath; }
        //    set { _CurrentExecutionFilePath = value; }
        //}

        //public string FilePath
        //{
        //    get { return _FilePath; }
        //    set { _FilePath = value; }
        //}

        //public HttpFileCollection Files
        //{
        //    get { return _Files; }
        //    set { _Files = value; }
        //}

        //public Stream Filter
        //{
        //    get { return _Filter; }
        //    set { _Filter = value; }
        //}

        //public NameValueCollection Form
        //{
        //    get { return _Form; }
        //    set { _Form = value; }
        //}

        //public string HttpMethod
        //{
        //    get { return _HttpMethod; }
        //    set { _HttpMethod = value; }
        //}

        //public Stream InputStream
        //{
        //    get { return _InputStream; }
        //    set { _InputStream = value; }
        //}

        //public bool IsAuthenticated
        //{
        //    get { return _IsAuthenticated; }
        //    set { _IsAuthenticated = value; }
        //}

        //public bool IsSecureConnection
        //{
        //    get { return _IsSecureConnection; }
        //    set { _IsSecureConnection = value; }
        //}

        //public WindowsIdentity LogonUserIdentity
        //{
        //    get { return _LogonUserIdentity; }
        //    set { _LogonUserIdentity = value; }
        //}

        //public string Path
        //{
        //    get { return _Path; }
        //    set { _Path = value; }
        //}

        //public string PathInfo
        //{
        //    get { return _PathInfo; }
        //    set { _PathInfo = value; }
        //}

        //public string PhysicalApplicationPath
        //{
        //    get { return _PhysicalApplicationPath; }
        //    set { _PhysicalApplicationPath = value; }
        //}

        //public string PhysicalPath
        //{
        //    get { return _PhysicalPath; }
        //    set { _PhysicalPath = value; }
        //}

        //public string RawUrl
        //{
        //    get { return _RawUrl; }
        //    set { _RawUrl = value; }
        //}

        //public string RequestType
        //{
        //    get { return _RequestType; }
        //    set { _RequestType = value; }
        //}

        //public NameValueCollection ServerVariables
        //{
        //    get { return _ServerVariables; }
        //    set { _ServerVariables = value; }
        //}

        //public int TotalBytes
        //{
        //    get { return _TotalBytes; }
        //    set { _TotalBytes = value; }
        //}

        //public Uri Url
        //{
        //    get { return _Url; }
        //    set { _Url = value; }
        //}

        //public Uri UrlReferrer
        //{
        //    get { return _UrlReferrer; }
        //    set { _UrlReferrer = value; }
        //}

        //public string UserAgent
        //{
        //    get { return _UserAgent; }
        //    set { _UserAgent = value; }
        //}

        //public string[] UserLanguages
        //{
        //    get { return _userLanguages.ToArray(); }
        //}

        //public void AddUserLanguage(string value)
        //{
        //    _userLanguages.Add(value);
        //}

        //public string UserHostAddress
        //{
        //    get { return _UserHostAddress; }
        //    set { _UserHostAddress = value; }
        //}

        //public string UserHostName
        //{
        //    get { return _UserHostName; }
        //    set { _UserHostName = value; }
        //}

        //public NameValueCollection Headers
        //{
        //    get { return _Headers; }
        //    set { _Headers = value; }
        //}

        //public NameValueCollection QueryString
        //{
        //    get { return _QueryString; }
        //    set { _QueryString = value; }
        //}

        //public string this[string key]
        //{
        //    get { throw new NotImplementedException(); }
        //}
	}

	public class TestHttpResponse : HttpResponseBase{
		private bool _Buffer;
		private bool _BufferOutput;
		private HttpCachePolicyBase _Cache;
		private string _CacheControl;
		private string _Charset;
		private Encoding _ContentEncoding;
		private string _ContentType;
		private HttpCookieCollection _Cookies;
		private int _Expires;
		private DateTime _ExpiresAbsolute;
		private Stream _Filter;
		private NameValueCollection _Headers;
		private Encoding _HeaderEncoding;
		private bool _IsClientConnected;
		private bool _IsRequestBeingRedirected;
		private StringWriter _Output = new StringWriter();
		private string _RedirectLocation;
		private string _Status;
		private int _StatusCode;
		private string _StatusDescription;
		private int _SubStatusCode;
		private bool _SuppressContent;
		private bool _TrySkipIisCustomErrors;

        //public void AddCacheItemDependency(string cacheKey)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddCacheItemDependencies(ArrayList cacheKeys)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddCacheItemDependencies(string[] cacheKeys)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddCacheDependency(params CacheDependency[] dependencies)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddFileDependency(string filename)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddFileDependencies(ArrayList filenames)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddFileDependencies(string[] filenames)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AppendCookie(HttpCookie cookie)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AppendHeader(string name, string value)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AppendToLog(string param)
        //{
        //    throw new NotImplementedException();
        //}

        //public string ApplyAppPathModifier(string virtualPath)
        //{
        //    throw new NotImplementedException();
        //}

        //public void BinaryWrite(byte[] buffer)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Clear()
        //{
        //    throw new NotImplementedException();
        //}

        //public void ClearContent()
        //{
        //    throw new NotImplementedException();
        //}

        //public void ClearHeaders()
        //{
        //    throw new NotImplementedException();
        //}

        //public void DisableKernelCache()
        //{
        //    throw new NotImplementedException();
        //}

        //public void End()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Flush()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Pics(string value)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Redirect(string url)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Redirect(string url, bool endResponse)
        //{
        //    throw new NotImplementedException();
        //}

        //public void SetCookie(HttpCookie cookie)
        //{
        //    throw new NotImplementedException();
        //}

        //public TextWriter SwitchWriter(TextWriter writer)
        //{
        //    throw new NotImplementedException();
        //}

        //public void TransmitFile(string filename)
        //{
        //    throw new NotImplementedException();
        //}

        //public void TransmitFile(string filename, long offset, long length)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Write(char[] buffer, int index, int count)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Write(object obj)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Write(string s)
        //{
        //    throw new NotImplementedException();
        //}

        //public void WriteFile(string filename)
        //{
        //    throw new NotImplementedException();
        //}

        //public void WriteFile(string filename, bool readIntoMemory)
        //{
        //    throw new NotImplementedException();
        //}

        //public void WriteFile(string filename, long offset, long size)
        //{
        //    throw new NotImplementedException();
        //}

        //public void WriteFile(IntPtr fileHandle, long offset, long size)
        //{
        //    throw new NotImplementedException();
        //}

        //public void WriteSubstitution(HttpResponseSubstitutionCallback callback)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool Buffer
        //{
        //    get { return _Buffer; }
        //    set { _Buffer = value; }
        //}

        //public bool BufferOutput
        //{
        //    get { return _BufferOutput; }
        //    set { _BufferOutput = value; }
        //}

        ////public IHttpCachePolicy Cache
        ////{
        ////    get { return _Cache; }
        ////    set { _Cache = value; }
        ////}

        //public string CacheControl
        //{
        //    get { return _CacheControl; }
        //    set { _CacheControl = value; }
        //}

        //public string Charset
        //{
        //    get { return _Charset; }
        //    set { _Charset = value; }
        //}

        //public Encoding ContentEncoding
        //{
        //    get { return _ContentEncoding; }
        //    set { _ContentEncoding = value; }
        //}

        //public string ContentType
        //{
        //    get { return _ContentType; }
        //    set { _ContentType = value; }
        //}

        //public HttpCookieCollection Cookies
        //{
        //    get { return _Cookies; }
        //    set { _Cookies = value; }
        //}

        //public int Expires
        //{
        //    get { return _Expires; }
        //    set { _Expires = value; }
        //}

        //public DateTime ExpiresAbsolute
        //{
        //    get { return _ExpiresAbsolute; }
        //    set { _ExpiresAbsolute = value; }
        //}

        //public Stream Filter
        //{
        //    get { return _Filter; }
        //    set { _Filter = value; }
        //}

        //public NameValueCollection Headers
        //{
        //    get { return _Headers; }
        //    set { _Headers = value; }
        //}

        //public Encoding HeaderEncoding
        //{
        //    get { return _HeaderEncoding; }
        //    set { _HeaderEncoding = value; }
        //}

        //public bool IsClientConnected
        //{
        //    get { return _IsClientConnected; }
        //    set { _IsClientConnected = value; }
        //}

        //public bool IsRequestBeingRedirected
        //{
        //    get { return _IsRequestBeingRedirected; }
        //    set { _IsRequestBeingRedirected = value; }
        //}

        //TextWriter HttpResponseBase.Output
        //{
        //    get { return Output; }
        //}

        //public StringWriter Output
        //{
        //    get { return _Output; }
        //    set { _Output = value; }
        //}

        //public void ClearOutput()
        //{
        //    _Output.GetStringBuilder().Length = 0;
        //}

        //public string RedirectLocation
        //{
        //    get { return _RedirectLocation; }
        //    set { _RedirectLocation = value; }
        //}

        //public string Status
        //{
        //    get { return _Status; }
        //    set { _Status = value; }
        //}

        //public int StatusCode
        //{
        //    get { return _StatusCode; }
        //    set { _StatusCode = value; }
        //}

        //public string StatusDescription
        //{
        //    get { return _StatusDescription; }
        //    set { _StatusDescription = value; }
        //}

        //public int SubStatusCode
        //{
        //    get { return _SubStatusCode; }
        //    set { _SubStatusCode = value; }
        //}

        //public bool SuppressContent
        //{
        //    get { return _SuppressContent; }
        //    set { _SuppressContent = value; }
        //}

        //public bool TrySkipIisCustomErrors
        //{
        //    get { return _TrySkipIisCustomErrors; }
        //    set { _TrySkipIisCustomErrors = value; }
        //}
	}

	public class TestHttpServerUtility : HttpServerUtilityBase
	{
        //public void ClearError()
        //{
        //    throw new NotImplementedException();
        //}

        //public object CreateObject(string progID)
        //{
        //    throw new NotImplementedException();
        //}

        //public object CreateObject(Type type)
        //{
        //    throw new NotImplementedException();
        //}

        //public object CreateObjectFromClsid(string clsid)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Execute(string path)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Execute(string path, TextWriter writer)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Execute(string path, bool preserveForm)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Execute(string path, TextWriter writer, bool preserveForm)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Execute(IHttpHandler handler, TextWriter writer, bool preserveForm)
        //{
        //    throw new NotImplementedException();
        //}

        //public Exception GetLastError()
        //{
        //    throw new NotImplementedException();
        //}

        //public string HtmlDecode(string s)
        //{
        //    throw new NotImplementedException();
        //}

        //public void HtmlDecode(string s, TextWriter output)
        //{
        //    throw new NotImplementedException();
        //}

        //public string HtmlEncode(string s)
        //{
        //    throw new NotImplementedException();
        //}

        //public void HtmlEncode(string s, TextWriter output)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Transfer(string path, bool preserveForm)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Transfer(string path)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Transfer(IHttpHandler handler, bool preserveForm)
        //{
        //    throw new NotImplementedException();
        //}

        //public void TransferRequest(string path)
        //{
        //    throw new NotImplementedException();
        //}

        //public void TransferRequest(string path, bool preserveForm)
        //{
        //    throw new NotImplementedException();
        //}

        //public void TransferRequest(string path, bool preserveForm, string method, NameValueCollection headers)
        //{
        //    throw new NotImplementedException();
        //}

        //public string UrlEncode(string s)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UrlEncode(string s, TextWriter output)
        //{
        //    throw new NotImplementedException();
        //}

        //public string UrlDecode(string s)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UrlDecode(string s, TextWriter output)
        //{
        //    throw new NotImplementedException();
        //}

        //public string MachineName
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public int ScriptTimeout
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}
	}

	public class TestHttpSessionState :  HttpSessionStateBase
	{
		public TestHttpSessionState()
			
		{
		}

		private int _CodePage;
		private HttpSessionStateBase _Contents;
		private HttpCookieMode _CookieMode;
		private bool _IsCookieless;
		private bool _IsNewSession;
		private int _LCID;
		private SessionStateMode _Mode;
		private string _SessionID;
		private HttpStaticObjectsCollection _StaticObjects;
		private int _Timeout;
		private object _SyncRoot;
		private bool _IsSynchronized;

        //public void Abandon()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Add(string name, object value)
        //{
        //    base.Add(name, value);
        //}

        //public void RemoveAll()
        //{
        //    Clear();
        //}

        //public void Remove(string key)
        //{
        //    base.Remove(key);
        //}

        //public int CodePage
        //{
        //    get { return _CodePage; }
        //    set { _CodePage = value; }
        //}

        //public HttpSessionStateBase Contents
        //{
        //    get { return _Contents; }
        //    set { _Contents = value; }
        //}

        //public HttpCookieMode CookieMode
        //{
        //    get { return _CookieMode; }
        //    set { _CookieMode = value; }
        //}

        //public bool IsCookieless
        //{
        //    get { return _IsCookieless; }
        //    set { _IsCookieless = value; }
        //}

        //public bool IsNewSession
        //{
        //    get { return _IsNewSession; }
        //    set { _IsNewSession = value; }
        //}

        //new public NameObjectCollectionBase.KeysCollection Keys
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public int LCID
        //{
        //    get { return _LCID; }
        //    set { _LCID = value; }
        //}

        //public SessionStateMode Mode
        //{
        //    get { return _Mode; }
        //    set { _Mode = value; }
        //}

        //public string SessionID
        //{
        //    get { return _SessionID; }
        //    set { _SessionID = value; }
        //}

        //public HttpStaticObjectsCollection StaticObjects
        //{
        //    get { return _StaticObjects; }
        //    set { _StaticObjects = value; }
        //}

        //public int Timeout
        //{
        //    get { return _Timeout; }
        //    set { _Timeout = value; }
        //}

        //public object this[string name]
        //{
        //    get { return base[name]; }
        //    set { base[name] = value; }
        //}

        //public object SyncRoot
        //{
        //    get { return _SyncRoot; }
        //    set { _SyncRoot = value; }
        //}

        //public bool IsSynchronized
        //{
        //    get { return _IsSynchronized; }
        //    set { _IsSynchronized = value; }
        //}
	}
}
