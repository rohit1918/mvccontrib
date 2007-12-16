using System;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using MvcContrib.ViewFactories;
using NUnit.Framework;
using NVelocity.Runtime;
using Rhino.Mocks;

namespace MVCContrib.UnitTests.ViewFactories
{
	[TestFixture]
	public class NVelocityViewFactoryTester
	{
		private NVelocityViewFactory _factory;
		private ControllerContext _controllerContext;

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

			MockRepository mocks = new MockRepository();
			IHttpContext httpContext = mocks.DynamicMock<IHttpContext>();
			RequestContext requestContext = new RequestContext(httpContext, new RouteData());
			IController controller = mocks.DynamicMock<IController>();
			
			_controllerContext = new ControllerContext(requestContext, controller);
			_controllerContext.RouteData.Values.Add("controller", viewPath);
		}

		[Test]
		public void WillAcceptNullProperties()
		{
			NVelocityViewFactory factory = new NVelocityViewFactory();
			factory = new NVelocityViewFactory(null);
		}

		[Test]
		public void LoadValidView()
		{
			NVelocityView view = _factory.CreateView(_controllerContext, "view", string.Empty, null) as NVelocityView;
			Assert.IsNotNull(view);
			Assert.IsNotNull(view.ViewTemplate);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvalidViewThrows()
		{
			_factory.CreateView(_controllerContext, "nonExistant", string.Empty, null);
		}

		[Test]
		public void LoadValidViewWithMaster()
		{
			NVelocityView view = _factory.CreateView(_controllerContext, "view", "master", null) as NVelocityView;
			Assert.IsNotNull(view);
			Assert.IsNotNull(view.ViewTemplate);
			Assert.IsNotNull(view.MasterTemplate);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvalidMasterThrows()
		{
			_factory.CreateView(_controllerContext, "view", "nonExistant", null);
		}
	}
}