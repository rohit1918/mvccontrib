using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.SparkViewEngine;
using MvcContrib.ViewFactories;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.SparkViewEngine
{
	[TestFixture]
	[Category("SparkViewEngine")]
	public class SparkViewFactoryTester
	{
		private MockRepository _mocks;
		private ControllerContext _context;
		private IViewSourceLoader _viewSourceLoader;

		[SetUp]
		public void Init()
		{
			_mocks = new MockRepository();

			var httpContext = _mocks.DynamicMock<HttpContextBase>();
			var requestContext = new RequestContext(httpContext, new RouteData());
			var controller = _mocks.DynamicMock<ControllerBase>();
			_context = new ControllerContext(requestContext, controller);

			_viewSourceLoader = new FileSystemViewSourceLoader("SparkViewEngine\\Views");
		}

		[Test]
		public void Can_Create_Default_ViewFactory()
		{
			var viewFactory = new SparkViewFactory();
			Assert.IsNotNull(viewFactory.Engine);
		}

		[Test]
		public void ViewSourceLoader_Used_By_Engine()
		{
			_context.RouteData.Values.Add("controller", "home");

			_mocks.ReplayAll();
			var viewFactory = new SparkViewFactory(_viewSourceLoader);
			var resultExists = viewFactory.FindPartialView(_context, "sparkpartial");
			Assert.IsNotNull(resultExists.View);

			var resultMissing = viewFactory.FindPartialView(_context, "nosuchfile");
			Assert.IsNull(resultMissing.View);
			Assert.IsNotNull(resultMissing.SearchedLocations);
			Assert.AreNotEqual(0, resultMissing.SearchedLocations.Count());

			_mocks.VerifyAll();
		}

		[Test]
		public void Default_Conventions_Are_In_Effect()
		{
			_context.RouteData.Values.Add("controller", "home");

			_mocks.ReplayAll();
			var viewFactory = new SparkViewFactory(_viewSourceLoader);
			var results = viewFactory.FindView(_context, "list", null);
			Assert.IsNotNull(results.View);

			var writer = new StringWriter();
			results.View.Render(new ViewContext(_context, "list", null, null), writer);

			var clippedOutput = writer.ToString().Replace(" ", "").Replace("\t", "").Replace("\r\n", "");
			Assert.AreEqual("<div><p>begin</p><p>Listing</p><p>end</p></div>", clippedOutput);

			_mocks.VerifyAll();

		}
	}
}
