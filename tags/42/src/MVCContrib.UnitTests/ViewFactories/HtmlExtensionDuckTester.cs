using System;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Castle;
using NUnit.Framework;
using NVelocity.Runtime;
using NVelocity.Util.Introspection;
using Rhino.Mocks;

namespace MVCContrib.UnitTests.ViewFactories
{
	[TestFixture, Category("NVelocityViewEngine")]
	public class HtmlExtensionDuckTester
	{
		private MockRepository _mocks;
		private HtmlExtensionDuck _htmlHelperDuck;
		private HtmlHelper _htmlHelper;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			IHttpContext httpContext = _mocks.DynamicMock<IHttpContext>();
			IHttpResponse httpResponse = _mocks.DynamicMock<IHttpResponse>();
			IHttpSessionState httpSessionState = _mocks.DynamicMock<IHttpSessionState>();
			SetupResult.For(httpContext.Session).Return(httpSessionState);
			SetupResult.For(httpContext.Response).Return(httpResponse);
			RequestContext requestContext = new RequestContext(httpContext, new RouteData());
			IController controller = _mocks.DynamicMock<IController>();
			ControllerContext controllerContext = new ControllerContext(requestContext, controller);
			_mocks.ReplayAll();
			ViewContext viewContext = new ViewContext(controllerContext, new Hashtable(), new TempDataDictionary(controllerContext.HttpContext));

			_htmlHelper = new HtmlHelper(viewContext);
			_htmlHelperDuck = new HtmlExtensionDuck(_htmlHelper);

			_htmlHelperDuck.Introspector = new Introspector(new Logger());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Depends_On_HtmlHelper()
		{
			new HtmlExtensionDuck((HtmlHelper)null);
		}

		[Test]
		public void Invokes_Methods_On_HtmlHelper()
		{
			string expected = _htmlHelper.ActionLink("linkText", "actionName");
			string actual = _htmlHelperDuck.Invoke("ActionLink", new object[] {"linkText", "actionName"}) as string;

			Assert.AreEqual(expected, actual); 
		}

		[Test]
		public void Invokes_Methods_On_HtmlHelper_Extension_Classes()
		{
			string expected = _htmlHelper.TextBox("htmlName");
			string actual = _htmlHelperDuck.Invoke("TextBox", new object[] {"htmlName"}) as string;

			Assert.AreEqual(expected, actual); 
		}

		[Test]
		public void Returns_Null_For_Unresolved_Methods()
		{
			Assert.IsNull(_htmlHelperDuck.Invoke("UnresolvedMethod", new object[] {}));
		}

		class Logger : IRuntimeLogger
		{
			public void Warn(object message)
			{
			}

			public void Info(object message)
			{
			}

			public void Error(object message)
			{
			}

			public void Debug(object message)
			{
			}
		}
	}
}
