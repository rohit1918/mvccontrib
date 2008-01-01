using System.Web;
using Rhino.Mocks;
using IHttpSessionState = System.Web.IHttpSessionState;
using System.Security.Principal;

namespace MVCContrib.UnitTests.XsltViewEngine.Helpers
{
    public static class MvcMockHelpers
    {
        public static IHttpContext DynamicIHttpContext(this MockRepository mocks)
        {
            return mocks.DynamicIHttpContext
                (mocks.DynamicIHttpBrowserCapabilities(),
                 mocks.DynamicIHttpRequest(),
                 mocks.DynamicIHttpResponse(),
                 mocks.DynamicIHttpSessionState(),
                 mocks.DynamicIHttpServerUtility(),
                 mocks.DynamicIPrincipal());
        }

        public static IHttpContext DynamicIHttpContext(this MockRepository mocks,IPrincipal principal,IHttpBrowserCapabilities browser)
    {
        return mocks.DynamicIHttpContext
               (browser,
                mocks.DynamicIHttpRequest(),
                mocks.DynamicIHttpResponse(),
                mocks.DynamicIHttpSessionState(),
                mocks.DynamicIHttpServerUtility(),
                principal);
    }

        public static IHttpContext DynamicIHttpContext(this MockRepository mocks,
            IHttpBrowserCapabilities browser,
            IHttpRequest request,
            IHttpResponse response,
            IHttpSessionState session,
            IHttpServerUtility server,
            IPrincipal user)
        {
            IHttpContext context = mocks.DynamicMock<IHttpContext>();
            SetupResult.For(context.User).Return(user);
            SetupResult.For(request.Browser).Return(browser);
            SetupResult.For(context.Request).Return(request);
            SetupResult.For(context.Response).Return(response);
            SetupResult.For(context.Session).Return(session);
            SetupResult.For(context.Server).Return(server);
            mocks.Replay(context);
            return context;
        }
        public static IHttpBrowserCapabilities DynamicIHttpBrowserCapabilities(this MockRepository mocks)
        {
            IHttpBrowserCapabilities browser = mocks.DynamicMock<IHttpBrowserCapabilities>();
            return browser;
        }
        public static IHttpRequest DynamicIHttpRequest(this MockRepository mocks)
        {
            IHttpRequest request = mocks.DynamicMock<IHttpRequest>();
            return request;
        }
        public static IHttpResponse DynamicIHttpResponse(this MockRepository mocks)
        {
            IHttpResponse response = mocks.DynamicMock<IHttpResponse>();
            return response;
        }
        public static IHttpSessionState DynamicIHttpSessionState(this MockRepository mocks)
        {
            IHttpSessionState session = mocks.DynamicMock<IHttpSessionState>();
            return session;
        }
        public static IHttpServerUtility DynamicIHttpServerUtility(this MockRepository mocks)
        {
            IHttpServerUtility server = mocks.DynamicMock<IHttpServerUtility>();
            return server;
        }
        public static IPrincipal DynamicIPrincipal(this MockRepository mocks)
        {
            IPrincipal principal = mocks.DynamicMock<IPrincipal>();
            return principal;
        }


    }

   

  
}
