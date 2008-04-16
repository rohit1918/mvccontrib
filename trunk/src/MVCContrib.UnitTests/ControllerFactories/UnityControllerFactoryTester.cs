using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Unity;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using Microsoft.Practices.Unity;

namespace MvcContrib.UnitTests.ControllerFactories
{
	[TestFixture, Category("UnityFactory")]
	public class UnityControllerFactoryTester
	{
		private MockRepository _mocks;
		private IUnityContainer _container;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();

			_container = new UnityContainer();
			_container.RegisterType<UnitySimpleController, UnitySimpleController>();
			_container.RegisterType<IDependency, StubDependency>();
			_container.RegisterType<UnityDependencyController, UnityDependencyController>();
		}

		[Test]
		public void ShouldReturnTheController()
		{
			HttpContextBase mockContext = _mocks.PartialMock<HttpContextBase>();
			MockApplication application = new MockApplication(_container);
			Expect.Call(mockContext.ApplicationInstance).Return(application);
			RequestContext context = new RequestContext(mockContext, new RouteData());
			_mocks.ReplayAll();

			IControllerFactory factory = new UnityControllerFactory();

			IController controller = factory.CreateController(context, "UnitySimple"); //typeof(SimpleController));

			Assert.That(controller, Is.Not.Null);
			Assert.That(controller, Is.AssignableFrom(typeof(UnitySimpleController)));
		}

		[Test]
		public void ShouldReturnControllerWithDependencies()
		{
			HttpContextBase mockContext = _mocks.DynamicMock<HttpContextBase>();
			MockApplication application = new MockApplication(_container);
			Expect.Call(mockContext.ApplicationInstance).Return(application);
			RequestContext context = new RequestContext(mockContext, new RouteData());
			_mocks.ReplayAll();

			IControllerFactory factory = new UnityControllerFactory();

			IController controller = factory.CreateController(context, "UnityDependency"); //typeof(DependencyController));

			Assert.That(controller, Is.Not.Null);
			Assert.That(controller, Is.AssignableFrom(typeof(UnityDependencyController)));

			UnityDependencyController dependencyController = (UnityDependencyController)controller;
			Assert.That(dependencyController._dependency, Is.Not.Null);
			Assert.That(dependencyController._dependency, Is.AssignableFrom(typeof(StubDependency)));
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShouldThrowExceptionWhenContainerIsNull()
		{
			HttpContextBase mockContext = _mocks.DynamicMock<HttpContextBase>();
			MockApplication application = new MockApplication(null);
			Expect.Call(mockContext.ApplicationInstance).Return(application);
			RequestContext context = new RequestContext(mockContext, new RouteData());
			_mocks.ReplayAll();

			IControllerFactory factory = new UnityControllerFactory();

			IController controller = factory.CreateController(context, "UnitySimple");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShouldThrowExceptionWhenApplicationDoesNotImplementIContainerAccessor()
		{
			HttpContextBase mockContext = _mocks.DynamicMock<HttpContextBase>();
			HttpApplication application = new HttpApplication();
			Expect.Call(mockContext.ApplicationInstance).Return(application);
			RequestContext context = new RequestContext(mockContext, new RouteData());
			_mocks.ReplayAll();

			IControllerFactory factory = new UnityControllerFactory();

			IController controller = factory.CreateController(context, "UnitySimple");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShouldThrowExceptionWhenControllerDoesNotExist()
		{
			HttpContextBase mockContext = _mocks.DynamicMock<HttpContextBase>();
			HttpApplication application = new HttpApplication();
			Expect.Call(mockContext.ApplicationInstance).Return(application);
			RequestContext context = new RequestContext(mockContext, new RouteData());
			_mocks.ReplayAll();

			IControllerFactory factory = new UnityControllerFactory();

			IController controller = factory.CreateController(context, "ControllerThatDoesNotExist");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowExceptionWhenRequestContextIsNull()
		{
			IControllerFactory factory = new UnityControllerFactory();

			IController controller = factory.CreateController(null, "UnitySimple");
		}

		public class MockApplication : HttpApplication, IUnityContainerAccessor
		{
			private readonly IUnityContainer _container;

			public MockApplication(IUnityContainer container)
			{
				_container = container;
			}

			public IUnityContainer Container
			{
				get { return _container; }
			}
		}

		public class UnitySimpleController : IController
		{
			public void Execute(ControllerContext controllerContext)
			{
				throw new NotImplementedException();
			}
		}

		public class UnityDependencyController : IController
		{
			public IDependency _dependency;

			public UnityDependencyController(IDependency dependency)
			{
				_dependency = dependency;
			}

			public void Execute(ControllerContext controllerContext)
			{
				throw new NotImplementedException();
			}
		}

		public interface IDependency
		{
		}

		public class StubDependency : IDependency
		{
		}
	}
}
