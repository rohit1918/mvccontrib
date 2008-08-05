using System;
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
	public class NVelocityViewFactoryTester
	{
		private NVelocityViewFactory _factory;
		private ControllerContext _controllerContext;
		private MockRepository _mocks;
		private StringWriter _output;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			_output = new StringWriter();

			string viewPath = "MvcContrib.UnitTests.ViewFactories";

			IDictionary properties = new Hashtable();
			properties["resource.loader"] = "assembly";
			properties["assembly.resource.loader.class"] = "NVelocity.Runtime.Resource.Loader.AssemblyResourceLoader, NVelocity";
			properties["assembly.resource.loader.assembly"] = "MVCContrib.UnitTests";
			properties["master.folder"] = viewPath;
			_factory = new NVelocityViewFactory(properties);

			var httpContext = _mocks.DynamicMock<HttpContextBase>();
			var response = _mocks.DynamicMock<HttpResponseBase>();
			SetupResult.For(httpContext.Response).Return(response);
			SetupResult.For(response.Output).Return(_output);

			var requestContext = new RequestContext(httpContext, new RouteData());
			var controller = _mocks.DynamicMock<IController>();
			
			_mocks.ReplayAll();

			_controllerContext = new ControllerContext(requestContext, controller);
			_controllerContext.RouteData.Values.Add("controller", viewPath);
		}

		[Test]
		public void WillAcceptNullProperties()
		{
			var factory = new NVelocityViewFactory();
			factory = new NVelocityViewFactory(null);
		}

		[Test]
		public void LoadValidView()
		{
			var context = new ViewContext(_controllerContext, "view", string.Empty, null, null);
			NVelocityView view = _factory.CreateView(context);
			Assert.IsNotNull(view);
			Assert.IsNotNull(view.ViewTemplate);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvalidViewThrows()
		{
			var context = new ViewContext(_controllerContext, "nonExistant", string.Empty, null, null);
			_factory.CreateView(context);
		}

		[Test]
		public void LoadValidViewWithMaster()
		{
			var context = new ViewContext(_controllerContext, "view", "master", null, null);
			NVelocityView view = _factory.CreateView(context);
			Assert.IsNotNull(view);
			Assert.IsNotNull(view.ViewTemplate);
			Assert.IsNotNull(view.MasterTemplate);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvalidMasterThrows()
		{
			var context = new ViewContext(_controllerContext, "view", "nonExistant", null, null);
			_factory.CreateView(context);
		}

		[Test]
		public void ShouldRenderView()
		{
			string expected = "Master Template View Template";
			var context = new ViewContext(_controllerContext, "view", "master", null, null);
			_factory.RenderView(context);
			string output = _output.ToString();
			Assert.AreEqual(expected, output);
		}
	}
}
