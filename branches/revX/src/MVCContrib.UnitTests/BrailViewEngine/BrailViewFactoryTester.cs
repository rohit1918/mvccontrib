using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.BrailViewEngine;
using MvcContrib.ViewFactories;

namespace MvcContrib.UnitTests.BrailViewEngine
{
	using NUnit.Framework;
using Rhino.Mocks;

	[TestFixture]
	[Category("BrailViewEngine")]
	public class BrailViewFactoryTester
	{
		private BrailViewFactory _viewFactory;
		private ViewContext _viewContext;
		private MockRepository _mocks;

		private static readonly string VIEW_ROOT_DIRECTORY = @"BrailViewEngine\Views";

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			var httpContext = new TestHttpContext();
			var requestContext = new RequestContext(httpContext, new RouteData());
            var controller = _mocks.StrictMock<ControllerBase>();
			_mocks.Replay(controller);
			_viewContext = new ViewContext(httpContext, new RouteData(), controller, null, new ViewDataDictionary(new object()), null);  //new ControllerContext(requestContext, controller);

			var viewEngine = new BooViewEngine
			                 	{
			                 		ViewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY),
			                 		Options = new BooViewEngineOptions()
			                 	};
			viewEngine.Initialize();

			_viewFactory = new BrailViewFactory(viewEngine);
			_mocks.ReplayAll();
		}

		[Test]
		public void Can_Create_Default_ViewFactory()
		{
			var viewFactory = new BrailViewFactory();
			Assert.IsNotNull(viewFactory.ViewEngine);
		}

	/*	[Test]
		public void Can_Create_View()
		{
			_viewContext.RouteData.Values.Add("controller", "home");
//			BrailBase view = _viewFactory.CreateView(_controllerContext, "view", null, null);
//			Assert.IsNotNull(view);
		}*/

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Depends_On_BooViewEngine()
		{
			new BrailViewFactory(null);
		}
	}
}
