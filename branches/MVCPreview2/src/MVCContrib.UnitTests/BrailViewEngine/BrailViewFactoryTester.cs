using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.BrailViewEngine;
using MvcContrib.ViewFactories;

namespace MVCContrib.UnitTests.BrailViewEngine
{
	using NUnit.Framework;

	[TestFixture]
	[Category("BrailViewEngine")]
	public class BrailViewFactoryTester
	{
		private BrailViewFactory _viewFactory;
		private ControllerContext _controllerContext;

		private static readonly string VIEW_ROOT_DIRECTORY = @"BrailViewEngine\Views";

		[SetUp]
		public void SetUp()
		{
			TestHttpContext httpContext = new TestHttpContext();
			RequestContext requestContext = new RequestContext(httpContext, new RouteData());
			IController controller = new Controller();
			_controllerContext = new ControllerContext(requestContext, controller);

			BooViewEngine viewEngine = new BooViewEngine();
			viewEngine.ViewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			viewEngine.Options = new BooViewEngineOptions();
			viewEngine.Initialize();

			_viewFactory = new BrailViewFactory(viewEngine);
		}

		[Test]
		public void Can_Create_Default_ViewFactory()
		{
			BrailViewFactory viewFactory = new BrailViewFactory();
			Assert.IsNotNull(viewFactory.ViewEngine);
		}

		[Test]
		public void Can_Create_View()
		{
			_controllerContext.RouteData.Values.Add("controller", "home");
			IView view = _viewFactory.CreateView(_controllerContext, "view", null, null);
			Assert.IsNotNull(view);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Depends_On_BooViewEngine()
		{
			new BrailViewFactory(null);
		}
	}
}
