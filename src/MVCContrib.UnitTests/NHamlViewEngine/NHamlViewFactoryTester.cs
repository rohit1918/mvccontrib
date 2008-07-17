using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.NHamlViewEngine;
using MvcContrib.ViewFactories;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.NHamlViewEngine
{
	[TestFixture]
	public class NHamlViewFactoryTester
	{
		private MockRepository _mocks;
		private ControllerContext _controllerContext;
		private ViewDataDictionary _viewData;
		private HttpRequestBase _httpRequest;
		private StringWriter _output;

		private static readonly string VIEW_ROOT_DIRECTORY = @"NHamlViewEngine\Views";

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();

			_output = new StringWriter();
			_viewData = new ViewDataDictionary();

			var httpContext = _mocks.DynamicMock<HttpContextBase>();
			var httpResponse = _mocks.DynamicMock<HttpResponseBase>();
			_httpRequest = _mocks.DynamicMock<HttpRequestBase>();
			SetupResult.For(httpContext.Request).Return(_httpRequest);
			SetupResult.For(httpContext.Response).Return(httpResponse);
			SetupResult.For(httpResponse.Output).Return(_output);
			var requestContext = new RequestContext(httpContext, new RouteData());
			var controller = _mocks.DynamicMock<IController>();

			_controllerContext = new ControllerContext(requestContext, controller);
			_controllerContext.RouteData.Values["controller"] = "NHamlController";

			NHamlViewFactory.ClearViewCache();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NHamlViewFactory_DependsOn_ViewSourceLoader()
		{
			new NHamlViewFactory(null);
		}

		[Test]
		public void Can_Compile_View()
		{
			IViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			var viewFactory = new NHamlViewFactory(viewSourceLoader);

			_mocks.ReplayAll();
			var context = new ViewContext(_controllerContext, "index", null, _viewData,
			                              new TempDataDictionary());
			viewFactory.RenderView(context);

			_mocks.VerifyAll();
		}

		[Test]
		public void Can_Compile_View_With_Custom_View_Data()
		{
			IViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			var viewFactory = new NHamlViewFactory(viewSourceLoader);

			_mocks.ReplayAll();

			var context = new ViewContext(_controllerContext, "custom", null, _viewData,
			                              new TempDataDictionary());
			viewFactory.RenderView(context);

			_mocks.VerifyAll();
		}

		[Test]
		public void Can_Compile_View_With_Specific_Master()
		{
			IViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			var viewFactory = new NHamlViewFactory(viewSourceLoader);

			_mocks.ReplayAll();

			var context = new ViewContext(_controllerContext, "index", "specificMaster", _viewData,
			                              new TempDataDictionary());
			viewFactory.RenderView(context);
			_mocks.VerifyAll();
		}

		[Test]
		public void Can_Compile_View_With_Controller_Master()
		{
			IViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			var viewFactory = new NHamlViewFactory(viewSourceLoader);

			_mocks.ReplayAll();

			var context = new ViewContext(_controllerContext, "index", null, _viewData,
			                              new TempDataDictionary());
			viewFactory.RenderView(context);

			_mocks.VerifyAll();
		}

		[Test]
		public void Can_Compile_View_With_Application_Master()
		{
			IViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			var viewFactory = new NHamlViewFactory(viewSourceLoader);

			_mocks.ReplayAll();

			_controllerContext.RouteData.Values["controller"] = "NHamlApplication";
			var context = new ViewContext(_controllerContext, "index", null, _viewData,
			                              new TempDataDictionary());
			viewFactory.RenderView(context);

			_mocks.VerifyAll();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Cant_Compile_Missing_View()
		{
			IViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			var viewFactory = new NHamlViewFactory(viewSourceLoader);

			_mocks.ReplayAll();
			var context = new ViewContext(_controllerContext, "missingView", null, _viewData,
			                              new TempDataDictionary());

			viewFactory.RenderView(context);
			_mocks.VerifyAll();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Cant_Compile_View_With_Missing_Master()
		{
			IViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			var viewFactory = new NHamlViewFactory(viewSourceLoader);

			_mocks.ReplayAll();

			var context = new ViewContext(_controllerContext, "index", "missingMaster", _viewData,
			                              new TempDataDictionary());

			viewFactory.RenderView(context);

			_mocks.VerifyAll();
		}
	}
}