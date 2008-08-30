using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UnitTests.XsltViewEngine.Helpers;
using MvcContrib.ViewFactories;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.XsltViewEngine
{
	[TestFixture, Category("XsltViewEngine")]
	public class XsltViewFactoryTest : ViewTestBase
	{
		private const string controller = "MyController";
		private const string view = "MyView";
		private static readonly string BASE_PATH = Path.Combine(Environment.CurrentDirectory, @"..\..\XsltViewEngine\Data");
		private IViewSourceLoader _viewSourceLoader;
		private Controller _fakeController;

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
            _viewSourceLoader = mockRepository.StrictMock<IViewSourceLoader>();
			SetupResult.For(_viewSourceLoader.HasView("MyController/MyView.xslt")).Return(true);
			SetupResult.For(_viewSourceLoader.GetViewSource("MyController/MyView.xslt")).Return(new XsltViewSource());
			mockRepository.Replay(_viewSourceLoader);
            _fakeController = mockRepository.StrictMock<Controller>();
			mockRepository.Replay(_fakeController);
		}

		/*[Test]
		public void CreateView()
		{
			RouteData routeData = new RouteData();
			routeData.Values["controller"] = controller;
			Request.PhysicalApplicationPath = "http://testing/mycontroller/test";
			Identity.Name = "";
			Version version = new Version(1, 1);
			Browser.EcmaScriptVersion = version;
			Browser.Browser = "Firefox 2.0.11";
			Request.UserHostName = "::1";
			Request.RequestType = Request.HttpMethod = "GET";
			Request.QueryString["myQueryString"] = "myQueryStringValue";

			ControllerContext controllerContext = new ControllerContext(HttpContext, routeData, new Controller());

			IViewEngine viewFactory = new XsltViewFactory(_viewSourceLoader);

			IView viewObj = viewFactory.CreateView(controllerContext, view, string.Empty, new XsltViewData());

			Assert.IsNotNull(viewObj);
			Assert.IsTrue(viewObj is XsltView);
		}*/

		/*[Test]
		public void CreateView_DontSetTheAppPath()
		{
			RouteData routeData = new RouteData();
			routeData.Values["controller"] = controller;
			Request.PhysicalApplicationPath = BASE_PATH;
			Identity.Name = "";
			Version version = new Version(1, 1);
			Browser.EcmaScriptVersion = version;
			Browser.Browser = "Firefox 2.0.11";
			Request.UserHostName = "::1";
			Request.RequestType = Request.HttpMethod = "GET";
			Request.QueryString["myQueryString"] = "myQueryStringValue";

			ControllerContext controllerContext = new ControllerContext(HttpContext, routeData, new Controller());

			IViewFactory viewFactory = new XsltViewFactory(_viewSourceLoader);

			IView viewObj = viewFactory.CreateView(controllerContext, view, string.Empty, new XsltViewData());

			Assert.IsNotNull(viewObj);
			Assert.IsTrue(viewObj is XsltView);
		}*/

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void XsltViewFactory_DependsOn_ViewSourceLoader()
		{
			new XsltViewFactory();
			new XsltViewFactory(null);
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void ThrowExceptionWhenDataTypeIsInvalid()
		{
/*
			var routeData = new RouteData();
			var viewContext = new ViewContext(HttpContext, routeData, _fakeController, view, string.Empty, new ViewDataDictionary(new object()), 
			                                          new TempDataDictionary());
				// new ControllerContext(HttpContext, routeData, new Controller());

			IViewEngine viewFactory = new XsltViewFactory(_viewSourceLoader);

			viewFactory.RenderView(viewContext);
*/
			Assert.Fail("Fix me");
		}
	}
}
