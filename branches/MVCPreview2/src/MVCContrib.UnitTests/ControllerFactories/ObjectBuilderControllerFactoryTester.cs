using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ControllerFactories;
using MvcContrib.ObjectBuilder;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ControllerFactories
{
	[TestFixture, Category("ObjectBuilderFactory")]
	public class ObjectBuilderControllerFactoryTester
	{
		private MockRepository _mocks;
		private IDependencyContainer _container;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();

			_container = new DependencyContainer();
			_container.RegisterTypeMapping<SimpleController, SimpleController>();
			_container.RegisterTypeMapping<IDependency, StubDependency>();
			_container.RegisterTypeMapping<DependencyController, DependencyController>();
		}

		[Test]
		public void ShouldReturnTheController()
		{
			HttpContextBase mockContext = _mocks.PartialMock<HttpContextBase>();
			MockApplication application = new MockApplication(_container);
			Expect.Call(mockContext.ApplicationInstance).Return(application);
			RequestContext context = new RequestContext(mockContext, new RouteData());
			_mocks.ReplayAll();

			IControllerFactory factory = new ObjectBuilderControllerFactory();

			IController controller = factory.CreateController(context, "Simple"); //typeof(SimpleController));

			Assert.That(controller, Is.Not.Null);
			Assert.That(controller, Is.AssignableFrom(typeof(SimpleController)));
		}

		[Test]
		public void ShouldReturnControllerWithDependencies()
		{
			HttpContextBase mockContext = _mocks.DynamicMock<HttpContextBase>();
			MockApplication application = new MockApplication(_container);
			Expect.Call(mockContext.ApplicationInstance).Return(application);
			RequestContext context = new RequestContext(mockContext, new RouteData());
			_mocks.ReplayAll();

			IControllerFactory factory = new ObjectBuilderControllerFactory();

			IController controller = factory.CreateController(context, "Dependency"); //typeof(DependencyController));

			Assert.That(controller, Is.Not.Null);
			Assert.That(controller, Is.AssignableFrom(typeof(DependencyController)));

			DependencyController dependencyController = (DependencyController)controller;
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

			IControllerFactory factory = new ObjectBuilderControllerFactory();

			IController controller = factory.CreateController(context, "Simple");
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

			IControllerFactory factory = new ObjectBuilderControllerFactory();

			IController controller = factory.CreateController(context, "Simple");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowExceptionWhenRequestContextIsNull()
		{
			IControllerFactory factory = new ObjectBuilderControllerFactory();

			IController controller = factory.CreateController(null, "Simple");
		}

		public class MockApplication : HttpApplication, IDependencyContainerAccessor
		{
			private readonly IDependencyContainer _container;

			public MockApplication(IDependencyContainer container)
			{
				_container = container;
			}

			public IDependencyContainer Container
			{
				get { return _container; }
			}
		}

		public class SimpleController : IController
		{
			public void Execute(ControllerContext controllerContext)
			{
				throw new NotImplementedException();
			}
		}

		public class DependencyController : IController
		{
			public IDependency _dependency;

			public DependencyController(IDependency dependency)
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
