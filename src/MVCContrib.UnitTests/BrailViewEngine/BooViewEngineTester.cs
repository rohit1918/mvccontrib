using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Web.Mvc;
using MvcContrib.BrailViewEngine;
using MvcContrib.ViewFactories;
using NUnit.Framework;

namespace MVCContrib.UnitTests.BrailViewEngine
{
	[TestFixture]
	[Category("BrailViewEngine")]
	public class BooViewEngineTester
	{
		private BooViewEngine _viewEngine;
		private ViewContext _viewContext;
		private TestHttpContext _httpContext;

		private static readonly string VIEW_ROOT_DIRECTORY = @"BrailViewEngine\Views";

		[SetUp]
		public void SetUp()
		{
			_httpContext = new TestHttpContext();
			RequestContext requestContext = new RequestContext(_httpContext, new RouteData());
			IController controller = new Controller();
			ControllerContext controllerContext = new ControllerContext(requestContext, controller);
			_viewContext =
				new ViewContext(controllerContext, new Hashtable(StringComparer.InvariantCultureIgnoreCase),
				                new TempDataDictionary(controllerContext.HttpContext));

			_viewEngine = new BooViewEngine();
			_viewEngine.ViewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			_viewEngine.Options = new BooViewEngineOptions();
			_viewEngine.Initialize();
		}

		[Test]
		public void Can_Render_View_With_Master_And_SubView()
		{
			string expected = "Master View SubView";
			string actual = GetViewOutput("view", "/Master");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Request_ApplicationPath_Is_Placed_In_ViewData_With_SiteRoot_Key()
		{
			_httpContext.Request.ApplicationPath = "/ApplictionPath";

			string expected = "Current apppath is /ApplictionPath/";
			string actual = GetViewOutput("apppath");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Changing_View_Causes_Recompile()
		{
			_httpContext.Request.ApplicationPath = "/ApplictionPath";

			IViewSource viewSource = _viewEngine.ViewSourceLoader.GetViewSource("apppath.brail");
			string originalSource;
			using(TextReader reader = File.OpenText(viewSource.FullName))
			{
				originalSource = reader.ReadToEnd();
			}

			string expected = "Current apppath is /ApplictionPath/";
			string actual = GetViewOutput("apppath");
			Assert.AreEqual(expected, actual);

			string newSource = "newSource";
			using(TextWriter writer = File.CreateText(viewSource.FullName))
			{
				writer.Write(newSource);
			}

			Thread.Sleep(100);
			_httpContext.Response.ClearOutput();
			actual = GetViewOutput("apppath");

			try
			{
				Assert.AreEqual(newSource, actual);
			}
			finally
			{
				using (TextWriter writer = File.CreateText(viewSource.FullName))
				{
					writer.Write(originalSource);
				}
			}
		}

		[Test]
		public void Can_Render_SubView_with_custom_ViewData()
		{
			string expected = "View Test";
			string actual = GetViewOutput("view_CustomViewData");

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Layout_And_View_Should_Have_ViewContext()
		{
			BrailBase view = _viewEngine.Process(_httpContext.Response.Output, "view", "/Master");
			view.RenderView(_viewContext);
			Assert.IsNotNull(view.ViewContext);
			Assert.AreEqual(view.ViewContext, view.Layout.ViewContext);
		}

		[Test]
		public void Should_Use_Custom_Base_Class()
		{
			_viewEngine.Options.AssembliesToReference.Add(System.Reflection.Assembly.Load("MVCContrib.UnitTests"));
			_viewEngine.Options.BaseType = "MVCContrib.UnitTests.BrailViewEngine.TestBrailBase";
			BrailBase view = _viewEngine.Process(_httpContext.Response.Output, "view", null);
			Assert.IsInstanceOfType(typeof(TestBrailBase), view);
		}

		private string GetViewOutput(string viewName)
		{
			return GetViewOutput(viewName, null);
		}

		private string GetViewOutput(string viewName, string masterName)
		{
			BrailBase view = _viewEngine.Process(_httpContext.Response.Output, viewName, masterName);
			view.RenderView(_viewContext);
			return _httpContext.Response.Output.ToString();
		}
	}

	public abstract class TestBrailBase : BrailBase
	{
		protected TestBrailBase(BooViewEngine viewEngine, TextWriter output) : base(viewEngine, output)
		{
		}
	}
}
