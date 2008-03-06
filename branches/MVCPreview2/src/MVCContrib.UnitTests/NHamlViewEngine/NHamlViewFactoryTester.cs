using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
		private Dictionary<string, object> _viewData;
		private HttpRequestBase _httpRequest;

		private static readonly string VIEW_ROOT_DIRECTORY = @"NHamlViewEngine\Views";

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();

			_viewData = new Dictionary<string, object>();

			HttpContext httpContext = _mocks.DynamicMock<HttpContextBase>();
			HttpResponse httpResponse = _mocks.DynamicMock<HttpResponseBase>();
			_httpRequest = _mocks.DynamicMock<HttpRequestBase>();
			SetupResult.For(httpContext.Request).Return(_httpRequest);
			SetupResult.For(httpContext.Response).Return(httpResponse);
			RequestContext requestContext = new RequestContext(httpContext, new RouteData());
			IController controller = _mocks.DynamicMock<IController>();

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
			NHamlViewFactory viewFactory = new NHamlViewFactory(viewSourceLoader);

			_mocks.ReplayAll();

			viewFactory.CreateView(_controllerContext, "index", null, _viewData);

			_mocks.VerifyAll();
		}

		[Test]
		public void Can_Compile_View_With_Custom_View_Data()
		{
			IViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			NHamlViewFactory viewFactory = new NHamlViewFactory(viewSourceLoader);

			_mocks.ReplayAll();

			viewFactory.CreateView(_controllerContext, "custom", null, new ViewData("testData"));

			_mocks.VerifyAll();
		}

		[Test]
		public void Can_Compile_View_With_Specific_Master()
		{
			IViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			NHamlViewFactory viewFactory = new NHamlViewFactory(viewSourceLoader);

			_mocks.ReplayAll();

			viewFactory.CreateView(_controllerContext, "index", "specificMaster", _viewData);

			_mocks.VerifyAll();
		}

		[Test]
		public void Can_Compile_View_With_Controller_Master()
		{
			IViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			NHamlViewFactory viewFactory = new NHamlViewFactory(viewSourceLoader);

			_mocks.ReplayAll();

			viewFactory.CreateView(_controllerContext, "index", null, _viewData);

			_mocks.VerifyAll();
		}

		[Test]
		public void Can_Compile_View_With_Application_Master()
		{
			IViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			NHamlViewFactory viewFactory = new NHamlViewFactory(viewSourceLoader);

			_mocks.ReplayAll();

			_controllerContext.RouteData.Values["controller"] = "NHamlApplication";

			viewFactory.CreateView(_controllerContext, "index", null, _viewData);

			_mocks.VerifyAll();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Cant_Compile_Missing_View()
		{
			IViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			NHamlViewFactory viewFactory = new NHamlViewFactory(viewSourceLoader);

			_mocks.ReplayAll();

			viewFactory.CreateView(_controllerContext, "missingView", null, _viewData);

			_mocks.VerifyAll();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Cant_Compile_View_With_Missing_Master()
		{
			IViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader(VIEW_ROOT_DIRECTORY);
			NHamlViewFactory viewFactory = new NHamlViewFactory(viewSourceLoader);

			_mocks.ReplayAll();

			viewFactory.CreateView(_controllerContext, "index", "missingMaster", _viewData);

			_mocks.VerifyAll();
		}
	}
}
