using System.Collections;
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
			var httpContext = _mocks.DynamicMock<HttpContextBase>();
			var httpResponse = _mocks.DynamicMock<HttpResponseBase>();
			var httpSessionState = _mocks.DynamicMock<HttpSessionStateBase>();
			Expect.Call(httpContext.Session).Repeat.Any().Return(httpSessionState);
			Expect.Call(httpContext.Response).Repeat.Any().Return(httpResponse);
			Expect.Call(httpResponse.Output).Repeat.Any().Return(_output);

			var requestContext = new RequestContext(httpContext, new RouteData());
			var controller = _mocks.DynamicMock<ControllerBase>();

			_controllerContext = new ControllerContext(requestContext, controller);
			_controllerContext.RouteData.Values.Add("controller", viewPath);
		}

		[Test]
		public void CanRenderView()
		{
			var viewContext = new ViewContext(_controllerContext, "view", new ViewDataDictionary(), null);

			var view = _factory.FindView(_controllerContext, "view", null).View;

			_mocks.ReplayAll();

			view.Render(viewContext, _output);

			Assert.AreEqual("View Template", _output.ToString());
		}

		[Test]
		public void CanRenderViewWithMaster()
		{
			var viewContext = new ViewContext(_controllerContext, "view", new ViewDataDictionary(), null);

			var view = _factory.FindView(_controllerContext, "view", "master").View;

			_mocks.ReplayAll();

			view.Render(viewContext, _output);

			Assert.AreEqual("Master Template View Template", _output.ToString());
		}

		[Test]
		public void CanRenderViewWithViewData()
		{
			var viewData = new ViewDataDictionary();
			viewData["test"] = "test";
			var viewContext = new ViewContext(_controllerContext,"view", viewData, null);

			var view = _factory.FindView(_controllerContext, "view", null).View;

			_mocks.ReplayAll();

			view.Render(viewContext, _output);

			Assert.AreEqual("View Template test", _output.ToString());
		}
	}
}
