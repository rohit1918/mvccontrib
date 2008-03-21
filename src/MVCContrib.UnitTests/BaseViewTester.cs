using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Rhino.Mocks;

namespace MvcContrib.UnitTests
{
	public class BaseViewTester
	{
		protected MockRepository mocks;
		protected StringWriter _output;
		protected ViewContext _viewContext;

		protected virtual void Setup()
		{
			_output = new StringWriter();

			mocks = new MockRepository();
			HttpContextBase httpContext = mocks.DynamicMock<HttpContextBase>();
			HttpResponseBase httpResponse = mocks.DynamicMock<HttpResponseBase>();
			HttpSessionStateBase httpSessionState = mocks.DynamicMock<HttpSessionStateBase>();
			SetupResult.For(httpContext.Session).Return(httpSessionState);
			SetupResult.For(httpContext.Response).Return(httpResponse);
			SetupResult.For(httpResponse.Output).Return(_output);
			SetupResult.For(httpResponse.ContentEncoding).Return(Encoding.UTF8);
			SetupResult.For(httpContext.Items).Return(new Hashtable());
			RequestContext requestContext = new RequestContext(httpContext, new RouteData());
			IController controller = mocks.DynamicMock<IController>();
			ControllerContext controllerContext = new ControllerContext(requestContext, controller);

			mocks.ReplayAll();

			_viewContext = new ViewContext(controllerContext,"index","", new Hashtable(), new TempDataDictionary(controllerContext.HttpContext));
			
		}
	}
}
