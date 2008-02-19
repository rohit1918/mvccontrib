using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MVCContrib.UnitTests;
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
			IHttpContext httpContext = mocks.DynamicMock<IHttpContext>();
			IHttpResponse httpResponse = mocks.DynamicMock<IHttpResponse>();
			IHttpSessionState httpSessionState = mocks.DynamicMock<IHttpSessionState>();
			SetupResult.For(httpContext.Session).Return(httpSessionState);
			SetupResult.For(httpContext.Response).Return(httpResponse);
			SetupResult.For(httpResponse.Output).Return(_output);
			SetupResult.For(httpContext.Items).Return(new Hashtable());
			RequestContext requestContext = new RequestContext(httpContext, new RouteData());
			IController controller = mocks.DynamicMock<IController>();
			ControllerContext controllerContext = new ControllerContext(requestContext, controller);

			mocks.ReplayAll();

			_viewContext = new ViewContext(controllerContext, new Hashtable(), new TempDataDictionary(controllerContext.HttpContext));
			
		}
	}
}