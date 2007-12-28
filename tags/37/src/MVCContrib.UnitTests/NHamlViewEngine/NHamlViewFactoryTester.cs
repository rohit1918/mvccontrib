using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MvcContrib.ViewFactories;
using NUnit.Framework;
using Rhino.Mocks;

namespace MVCContrib.UnitTests.NHamlViewEngine
{
	[TestFixture, Category("Integration")]
	public class NHamlViewFactoryTester
	{
		private MockRepository _mocks;
		private ControllerContext _controllerContext;
		private Dictionary<string, object> _viewData;
		private IHttpRequest _httpRequest;

		private static readonly string VIEW_ROOT_DIRECTORY = Path.Combine(Environment.CurrentDirectory, @"..\..\NHamlViewEngine\Views");

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();

			_viewData = new Dictionary<string, object>();

			IHttpContext httpContext = _mocks.DynamicMock<IHttpContext>();
			IHttpResponse httpResponse = _mocks.DynamicMock<IHttpResponse>();
			_httpRequest = _mocks.DynamicMock<IHttpRequest>();
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
			NHamlViewFactory viewFactory = new NHamlViewFactory(null);
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
	}
}
