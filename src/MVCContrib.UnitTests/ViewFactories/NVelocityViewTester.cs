using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
			string viewPath = "MvcContrib.UnitTests.ViewFactories";

			IDictionary properties = new Hashtable();
			properties["resource.loader"] = "assembly";
			properties["assembly.resource.loader.class"] = "NVelocity.Runtime.Resource.Loader.AssemblyResourceLoader, NVelocity";
			properties["assembly.resource.loader.assembly"] = "MVCContrib.UnitTests";
			properties["master.folder"] = viewPath;
			_factory = new NVelocityViewFactory(properties);

			_output = new StringWriter();

			_mocks = new MockRepository();
			HttpContextBase httpContext = _mocks.DynamicMock<HttpContextBase>();
			HttpResponseBase httpResponse = _mocks.DynamicMock<HttpResponseBase>();
			HttpSessionStateBase httpSessionState = _mocks.DynamicMock<HttpSessionStateBase>();
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
			ViewContext viewContext = new ViewContext(_controllerContext, "view", null, new ViewDataDictionary(), null);

			var view = _factory.CreateView(viewContext);

			_mocks.ReplayAll();

			view.RenderView();

			Assert.AreEqual("View Template", _output.ToString());
		}

		[Test]
		public void CanRenderViewWithMaster()
		{
			ViewContext viewContext = new ViewContext(_controllerContext, "view", "master", new ViewDataDictionary(), null);

			var view = _factory.CreateView(viewContext);

			_mocks.ReplayAll();

			view.RenderView();

			Assert.AreEqual("Master Template View Template", _output.ToString());
		}

		[Test]
		public void CanRenderViewWithViewData()
		{
			ViewDataDictionary viewData = new ViewDataDictionary();
			viewData["test"] = "test";
			ViewContext viewContext = new ViewContext(_controllerContext,"view", null, viewData, null);

			var view = _factory.CreateView(viewContext);

			_mocks.ReplayAll();

			view.RenderView();

			Assert.AreEqual("View Template test", _output.ToString());
		}
	}
}
