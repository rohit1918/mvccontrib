using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Castle;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ViewFactories
{
	[TestFixture]
	public class NVelocityViewTester
	{
		private NVelocityViewFactory _factory;
		private ControllerContext _controllerContext;
		private MockRepository _mocks;
		private StringWriter _output;

		[SetUp]
		public void SetUp()
		{
			string viewPath = "MVCContrib.UnitTests.ViewFactories";

			IDictionary properties = new Hashtable();
			properties["resource.loader"] = "assembly";
			properties["assembly.resource.loader.class"] = "NVelocity.Runtime.Resource.Loader.AssemblyResourceLoader, NVelocity";
			properties["assembly.resource.loader.assembly"] = "MVCContrib.UnitTests";
			properties["master.folder"] = viewPath;
			_factory = new NVelocityViewFactory(properties);

			_output = new StringWriter();

			_mocks = new MockRepository();
			IHttpContext httpContext = _mocks.DynamicMock<IHttpContext>();
			IHttpResponse httpResponse = _mocks.DynamicMock<IHttpResponse>();
			IHttpSessionState httpSessionState = _mocks.DynamicMock<IHttpSessionState>();
			Expect.Call(httpContext.Session).Repeat.Any().Return(httpSessionState);
			Expect.Call(httpContext.Response).Repeat.Any().Return(httpResponse);
			Expect.Call(httpResponse.Output).Repeat.Any().Return(_output);

			RequestContext requestContext = new RequestContext(httpContext, new RouteData());
			IController controller = _mocks.DynamicMock<IController>();

			_controllerContext = new ControllerContext(requestContext, controller);
			_controllerContext.RouteData.Values.Add("controller", viewPath);
		}

		[Test]
		public void CanRenderView()
		{
			IView view = _factory.CreateView(_controllerContext, "view", null, new Hashtable());

			_mocks.ReplayAll();

			ViewContext viewContext = new ViewContext(_controllerContext, new Hashtable(), new TempDataDictionary(_controllerContext.HttpContext));

			view.RenderView(viewContext);

			Assert.AreEqual("View Template", _output.ToString());
		}

		[Test]
		public void CanRenderViewWithMaster()
		{
			IView view = _factory.CreateView(_controllerContext, "view", "master", new Hashtable());

			_mocks.ReplayAll();

			ViewContext viewContext = new ViewContext(_controllerContext, new Hashtable(), new TempDataDictionary(_controllerContext.HttpContext));

			view.RenderView(viewContext);

			Assert.AreEqual("Master Template View Template", _output.ToString());
		}

		[Test]
		public void CanRenderViewWithViewData()
		{
			Dictionary<string, object> viewData = new Dictionary<string, object>();
			viewData["test"] = "test";

			IView view = _factory.CreateView(_controllerContext, "view", null, viewData);

			_mocks.ReplayAll();

			ViewContext viewContext = new ViewContext(_controllerContext, new Hashtable(), new TempDataDictionary(_controllerContext.HttpContext));

			view.RenderView(viewContext);

			Assert.AreEqual("View Template test", _output.ToString());
		}
	}
}