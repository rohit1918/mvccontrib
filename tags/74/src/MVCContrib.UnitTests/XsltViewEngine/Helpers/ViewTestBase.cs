using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using MvcContrib.ViewFactories;
using Rhino.Mocks;
using System.Web;
using System.Security.Principal;
using System.Collections.Specialized;

namespace MvcContrib.UnitTests.XsltViewEngine.Helpers
{
    public abstract class ViewTestBase
    {
			
			protected XmlDocument LoadXmlDocument(string path)
			{
				string assemblyPath = "MVCContrib.UnitTests.XsltViewEngine.Data." + path;

				XmlDocument xmlDoc = new XmlDocument();

				using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyPath))
				{
					
					xmlDoc.Load(stream);
				}

				return xmlDoc;
			}
        
        protected readonly MockRepository mockRepository = new MockRepository();
        
        public ViewTestBase()
        {
            Browser = mockRepository.DynamicMock<MockBrowserCapabilities>();
            Request = mockRepository.DynamicMock<MockRequest>();
            Response = mockRepository.DynamicIHttpResponse();
            Session = mockRepository.DynamicIHttpSessionState();
            Server = mockRepository.DynamicIHttpServerUtility();
            User = mockRepository.DynamicMock<MockUser>();
            HttpContext = mockRepository.DynamicMock<IHttpContext>();
            Identity = mockRepository.DynamicMock<MockIdentity>();
            SetupResult.For(HttpContext.User).Return(User);
            SetupResult.For(HttpContext.Request).Return(Request);
            SetupResult.For(HttpContext.Response).Return(Response);
            SetupResult.For(HttpContext.Session).Return(Session);
            SetupResult.For(HttpContext.Server).Return(Server);
        	ResponseOutput = new StringWriter();
        	SetupResult.For(Response.Output).Return(ResponseOutput);



            mockRepository.ReplayAll();
            Request.Browser = Browser;
            User.Identity = Identity;
            
        }

				protected StringWriter ResponseOutput { get; set; }
        protected MockBrowserCapabilities Browser { get; set; }
        protected MockRequest Request { get; set; }
        protected IHttpResponse Response {get;set;}
        protected IHttpContext HttpContext { get; set; }
        protected IHttpSessionState Session { get; set; }
        protected IHttpServerUtility Server { get; set; }
        protected MockUser User { get; set; }
        protected MockIdentity Identity { get; set; }

        public class MockUser:IPrincipal
        {

            #region IPrincipal Members

            public IIdentity Identity
            {
                get;
                set;
            }

            public bool IsInRole(string role)
            {
                throw new NotImplementedException();
            }

            #endregion
        }
        
        public class MockIdentity:IIdentity
        {

            #region IIdentity Members

            public string AuthenticationType
            {
                get;
                set;
            }

            public bool IsAuthenticated
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }

            #endregion
        }
        
        public class MockRequest:Dictionary<string,string>, IHttpRequest
        {
            private NameValueCollection querystrings = new NameValueCollection();
            private NameValueCollection formVariables = new NameValueCollection();
            private NameValueCollection serverVariables = new NameValueCollection();
            private NameValueCollection headers = new NameValueCollection();

            #region IHttpRequest Members

            public string[] AcceptTypes
            {
               get;set;
            }

            public string AnonymousID
            {
               get;set;
            }

            public string AppRelativeCurrentExecutionFilePath
            {
               get;set;
            }

            public string ApplicationPath
            {
               get;set;
            }

            public virtual byte[] BinaryRead(int count)
            {
                throw new NotImplementedException();
            }

            public IHttpBrowserCapabilities Browser
            {
               get;set;
            }

            public HttpClientCertificate ClientCertificate
            {
               get;set;
            }

            public Encoding ContentEncoding
            {
                get;set;
            }

            public int ContentLength
            {
               get;set;
            }

            public string ContentType
            {
                get;set;
            }

            public HttpCookieCollection Cookies
            {
               get;set;
            }

            public string CurrentExecutionFilePath
            {
               get;set;
            }

            public string FilePath
            {
               get;set;
            }

            public HttpFileCollection Files
            {
               get;set;
            }

            public System.IO.Stream Filter
            {
                get;set;
            }

            public NameValueCollection Form
            {
                get
                {
                    return formVariables;
                }
                set
                {
                    formVariables = value;
                }
            }

            public NameValueCollection Headers
            {
                get
                {
                    return headers;
                }
                set
                {
                    headers = value;
                }
            }

            public string HttpMethod
            {
               get;set;
            }

            public System.IO.Stream InputStream
            {
               get;set;
            }

            public bool IsAuthenticated
            {
               get;set;
            }

            public bool IsSecureConnection
            {
               get;set;
            }

            public WindowsIdentity LogonUserIdentity
            {
               get;set;
            }

            public virtual int[] MapImageCoordinates(string imageFieldName)
            {
                throw new NotImplementedException();
            }

            public virtual string MapPath(string virtualPath, string baseVirtualDir, bool allowCrossAppMapping)
            {
                throw new NotImplementedException();
            }

            public virtual string MapPath(string virtualPath)
            {
                throw new NotImplementedException();
            }

            public string Path
            {
               get;set;
            }

            public string PathInfo
            {
               get;set;
            }

            public string PhysicalApplicationPath
            {
               get;set;
            }

            public string PhysicalPath
            {
               get;set;
            }

            public NameValueCollection QueryString
            {
                get
                {
                    return querystrings;
                }
                set
                {
                    querystrings = value;
                }
            }

            public string RawUrl
            {
               get;set;
            }

            public string RequestType
            {
                get;set;
            }

            public virtual void SaveAs(string filename, bool includeHeaders)
            {
                throw new NotImplementedException();
            }

            public NameValueCollection ServerVariables
            {
               get
               {
                   return serverVariables;
               }
               set
               {
                 serverVariables = value;   
               }
            }

            public int TotalBytes
            {
               get;set;
            }

            public Uri Url
            {
               get;set;
            }

            public Uri UrlReferrer
            {
               get;set;
            }

            public string UserAgent
            {
               get;set;
            }

            public string UserHostAddress
            {
               get;set;
            }

            public string UserHostName
            {
               get;set;
            }

            public string[] UserLanguages
            {
               get;set;
            }

            public virtual void ValidateInput()
            {
                throw new NotImplementedException();
            }

            #endregion
        }
        
        public class MockBrowserCapabilities:Dictionary<string,string>,IHttpBrowserCapabilities
        {

            #region IHttpBrowserCapabilities Members

            public bool AOL
            {
               get;set;
            }

            public bool ActiveXControls
            {
               get;set;
            }

            public System.Collections.IDictionary Adapters
            {
               get;set;
            }

            public void AddBrowser(string browserName)
            {
                throw new NotImplementedException();
            }

            public bool BackgroundSounds
            {
               get;set;
            }

            public bool Beta
            {
               get;set;
            }

            public string Browser
            {
               get;set;
            }

            public System.Collections.ArrayList Browsers
            {
               get;set;
            }

            public bool CDF
            {
               get;set;
            }

            public bool CanCombineFormsInDeck
            {
               get;set;
            }

            public bool CanInitiateVoiceCall
            {
               get;set;
            }

            public bool CanRenderAfterInputOrSelectElement
            {
               get;set;
            }

            public bool CanRenderEmptySelects
            {
               get;set;
            }

            public bool CanRenderInputAndSelectElementsTogether
            {
               get;set;
            }

            public bool CanRenderMixedSelects
            {
               get;set;
            }

            public bool CanRenderOneventAndPrevElementsTogether
            {
               get;set;
            }

            public bool CanRenderPostBackCards
            {
               get;set;
            }

            public bool CanRenderSetvarZeroWithMultiSelectionList
            {
               get;set;
            }

            public bool CanSendMail
            {
               get;set;
            }

            public System.Collections.IDictionary Capabilities
            {
                get;
                set;
            }

            public Version ClrVersion
            {
               get;set;
            }

            public bool Cookies
            {
               get;set;
            }

            public bool Crawler
            {
               get;set;
            }

            public System.Web.UI.HtmlTextWriter CreateHtmlTextWriter(System.IO.TextWriter w)
            {
                throw new NotImplementedException();
            }

            public int DefaultSubmitButtonLimit
            {
               get;set;
            }

            public void DisableOptimizedCacheKey()
            {
                throw new NotImplementedException();
            }

            public Version EcmaScriptVersion
            {
               get;set;
            }

            public bool Frames
            {
               get;set;
            }

            public int GatewayMajorVersion
            {
               get;set;
            }

            public double GatewayMinorVersion
            {
               get;set;
            }

            public string GatewayVersion
            {
               get;set;
            }

            public Version[] GetClrVersions()
            {
                throw new NotImplementedException();
            }

            public bool HasBackButton
            {
               get;set;
            }

            public bool HidesRightAlignedMultiselectScrollbars
            {
               get;set;
            }

            public string HtmlTextWriter
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public string Id
            {
               get;set;
            }

            public string InputType
            {
               get;set;
            }

            public bool IsBrowser(string browserName)
            {
                throw new NotImplementedException();
            }

            public bool IsColor
            {
               get;set;
            }

            public bool IsMobileDevice
            {
               get;set;
            }

            public Version JScriptVersion
            {
               get;set;
            }

            public bool JavaApplets
            {
               get;set;
            }

            public Version MSDomVersion
            {
               get;set;
            }

            public int MajorVersion
            {
               get;set;
            }

            public int MaximumHrefLength
            {
               get;set;
            }

            public int MaximumRenderedPageSize
            {
               get;set;
            }

            public int MaximumSoftkeyLabelLength
            {
               get;set;
            }

            public double MinorVersion
            {
               get;set;
            }

            public string MinorVersionString
            {
               get;set;
            }

            public string MobileDeviceManufacturer
            {
               get;set;
            }

            public string MobileDeviceModel
            {
               get;set;
            }

            public int NumberOfSoftkeys
            {
               get;set;
            }

            public string Platform
            {
               get;set;
            }

            public string PreferredImageMime
            {
               get;set;
            }

            public string PreferredRenderingMime
            {
               get;set;
            }

            public string PreferredRenderingType
            {
               get;set;
            }

            public string PreferredRequestEncoding
            {
               get;set;
            }

            public string PreferredResponseEncoding
            {
               get;set;
            }

            public bool RendersBreakBeforeWmlSelectAndInput
            {
               get;set;
            }

            public bool RendersBreaksAfterHtmlLists
            {
               get;set;
            }

            public bool RendersBreaksAfterWmlAnchor
            {
               get;set;
            }

            public bool RendersBreaksAfterWmlInput
            {
               get;set;
            }

            public bool RendersWmlDoAcceptsInline
            {
               get;set;
            }

            public bool RendersWmlSelectsAsMenuCards
            {
               get;set;
            }

            public string RequiredMetaTagNameValue
            {
               get;set;
            }

            public bool RequiresAttributeColonSubstitution
            {
               get;set;
            }

            public bool RequiresContentTypeMetaTag
            {
               get;set;
            }

            public bool RequiresControlStateInSession
            {
               get;set;
            }

            public bool RequiresDBCSCharacter
            {
               get;set;
            }

            public bool RequiresHtmlAdaptiveErrorReporting
            {
               get;set;
            }

            public bool RequiresLeadingPageBreak
            {
               get;set;
            }

            public bool RequiresNoBreakInFormatting
            {
               get;set;
            }

            public bool RequiresOutputOptimization
            {
               get;set;
            }

            public bool RequiresPhoneNumbersAsPlainText
            {
               get;set;
            }

            public bool RequiresSpecialViewStateEncoding
            {
               get;set;
            }

            public bool RequiresUniqueFilePathSuffix
            {
               get;set;
            }

            public bool RequiresUniqueHtmlCheckboxNames
            {
               get;set;
            }

            public bool RequiresUniqueHtmlInputNames
            {
               get;set;
            }

            public bool RequiresUrlEncodedPostfieldValues
            {
               get;set;
            }

            public int ScreenBitDepth
            {
               get;set;
            }

            public int ScreenCharactersHeight
            {
               get;set;
            }

            public int ScreenCharactersWidth
            {
               get;set;
            }

            public int ScreenPixelsHeight
            {
               get;set;
            }

            public int ScreenPixelsWidth
            {
               get;set;
            }

            public bool SupportsAccesskeyAttribute
            {
               get;set;
            }

            public bool SupportsBodyColor
            {
               get;set;
            }

            public bool SupportsBold
            {
               get;set;
            }

            public bool SupportsCacheControlMetaTag
            {
               get;set;
            }

            public bool SupportsCallback
            {
               get;set;
            }

            public bool SupportsCss
            {
               get;set;
            }

            public bool SupportsDivAlign
            {
               get;set;
            }

            public bool SupportsDivNoWrap
            {
               get;set;
            }

            public bool SupportsEmptyStringInCookieValue
            {
               get;set;
            }

            public bool SupportsFontColor
            {
               get;set;
            }

            public bool SupportsFontName
            {
               get;set;
            }

            public bool SupportsFontSize
            {
               get;set;
            }

            public bool SupportsIModeSymbols
            {
               get;set;
            }

            public bool SupportsImageSubmit
            {
               get;set;
            }

            public bool SupportsInputIStyle
            {
               get;set;
            }

            public bool SupportsInputMode
            {
               get;set;
            }

            public bool SupportsItalic
            {
               get;set;
            }

            public bool SupportsJPhoneMultiMediaAttributes
            {
               get;set;
            }

            public bool SupportsJPhoneSymbols
            {
               get;set;
            }

            public bool SupportsQueryStringInFormAction
            {
               get;set;
            }

            public bool SupportsRedirectWithCookie
            {
               get;set;
            }

            public bool SupportsSelectMultiple
            {
               get;set;
            }

            public bool SupportsUncheck
            {
               get;set;
            }

            public bool SupportsXmlHttp
            {
               get;set;
            }

            public bool Tables
            {
               get;set;
            }

            public Type TagWriter
            {
               get;set;
            }

            public string Type
            {
               get;set;
            }

            public bool UseOptimizedCacheKey
            {
               get;set;
            }

            public bool VBScript
            {
               get;set;
            }

            public string Version
            {
               get;set;
            }

            public Version W3CDomVersion
            {
               get;set;
            }

            public bool Win16
            {
               get;set;
            }

            public bool Win32
            {
               get;set;
            }



            #endregion

            #region IFilterResolutionService Members

            public int CompareFilters(string filter1, string filter2)
            {
                throw new NotImplementedException();
            }

            public bool EvaluateFilter(string filterName)
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }

		public class XsltViewSource : IViewSource
		{
			public Stream OpenViewStream()
			{
				string xsltViewPath = "MVCContrib.UnitTests.XsltViewEngine.Data.Views.MyController.MyView.xslt";

				return Assembly.GetExecutingAssembly().GetManifestResourceStream(xsltViewPath);
			}

			public string FullName
			{
				get { throw new NotImplementedException(); }
			}

			public long LastModified
			{
				get { throw new NotImplementedException(); }
			}
		}
}
