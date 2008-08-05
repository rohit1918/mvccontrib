using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Rhino.Mocks;
using HttpSessionStateBase = System.Web.HttpSessionStateBase;
using System.Security.Principal;

namespace MvcContrib.UnitTests
{
	public static class MvcMockHelpers
	{
		public static HttpContextBase DynamicHttpContextBase(this MockRepository mocks)
		{
			return mocks.DynamicHttpContextBase
				(mocks.DynamicHttpBrowserCapabilitiesBase(),
				 mocks.DynamicHttpRequestBase(),
				 mocks.DynamicHttpResponseBase(),
				 mocks.DynamicHttpSessionStateBase(),
				 mocks.DynamicHttpServerUtilityBase(),
				 mocks.DynamicIPrincipal());
		}

		public static HttpContextBase DynamicHttpContextBase(this MockRepository mocks, IPrincipal principal, HttpBrowserCapabilitiesBase browser)
		{
			return mocks.DynamicHttpContextBase
				   (browser,
					mocks.DynamicHttpRequestBase(),
					mocks.DynamicHttpResponseBase(),
					mocks.DynamicHttpSessionStateBase(),
					mocks.DynamicHttpServerUtilityBase(),
					principal);
		}

		public static HttpContextBase DynamicHttpContextBase(this MockRepository mocks,
			HttpBrowserCapabilitiesBase browser,
			HttpRequestBase request,
			HttpResponseBase response,
			HttpSessionStateBase session,
			HttpServerUtilityBase server,
			IPrincipal user)
		{
			var context = mocks.DynamicMock<HttpContextBase>();
			SetupResult.For(context.User).Return(user);
			SetupResult.For(request.Browser).Return(browser);
			SetupResult.For(context.Request).Return(request);
			SetupResult.For(context.Response).Return(response);
			SetupResult.For(context.Session).Return(session);
			SetupResult.For(context.Server).Return(server);
			mocks.Replay(context);
			return context;
		}
		public static HttpBrowserCapabilitiesBase DynamicHttpBrowserCapabilitiesBase(this MockRepository mocks)
		{
			var browser = mocks.DynamicMock<HttpBrowserCapabilitiesBase>();
			return browser;
		}
		public static HttpRequestBase DynamicHttpRequestBase(this MockRepository mocks)
		{
			var request = mocks.DynamicMock<HttpRequestBase>();
			SetupResult.For(request.Form).Return(new NameValueCollection());
			SetupResult.For(request.QueryString).Return(new NameValueCollection());
			return request;
		}
		public static HttpResponseBase DynamicHttpResponseBase(this MockRepository mocks)
		{
			var response = mocks.DynamicMock<HttpResponseBase>();
			SetupResult.For(response.OutputStream).Return(new MemoryStream());
			SetupResult.For(response.Output).Return(new StringWriter());
			SetupResult.For(response.ContentType).PropertyBehavior();
			return response;
		}
		public static HttpSessionStateBase DynamicHttpSessionStateBase(this MockRepository mocks)
		{
			var session = mocks.DynamicMock<HttpSessionStateBase>();
			return session;
		}
		public static HttpServerUtilityBase DynamicHttpServerUtilityBase(this MockRepository mocks)
		{
			var server = mocks.DynamicMock<HttpServerUtilityBase>();
			return server;
		}
		public static IPrincipal DynamicIPrincipal(this MockRepository mocks)
		{
			var principal = mocks.DynamicMock<IPrincipal>();
			return principal;
		}

        public static ViewContext DynamicViewContext(this MockRepository mocks, string viewName)
        {
            var httpContext = DynamicHttpContextBase(mocks);
            var controller = mocks.DynamicMock<IController>();
            mocks.Replay(controller);

            var controllerContext = new ControllerContext(httpContext, new RouteData(), controller);            
            
            return new ViewContext(controllerContext, viewName, "", new ViewDataDictionary(), new TempDataDictionary());
        }
	}
}
