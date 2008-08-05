using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
			_container.RegisterTypeMapping<ObjectBuilderSimpleController, ObjectBuilderSimpleController>();
			_container.RegisterTypeMapping<IDependency, StubDependency>();
			_container.RegisterTypeMapping<ObjectBuilderDependencyController, ObjectBuilderDependencyController>();
		}

		[Test]
		public void ShouldReturnTheController()
		{
			var mockContext = _mocks.PartialMock<HttpContextBase>();
			var application = new MockApplication(_container);
			Expect.Call(mockContext.ApplicationInstance).Return(application);
			var context = new RequestContext(mockContext, new RouteData());
			_mocks.ReplayAll();

			IControllerFactory factory = new ObjectBuilderControllerFactory();

			IController controller = factory.CreateController(context, "ObjectBuilderSimple"); //typeof(SimpleController));

			Assert.That(controller, Is.Not.Null);
			Assert.That(controller, Is.AssignableFrom(typeof(ObjectBuilderSimpleController)));
		}

		[Test]
		public void ShouldReturnControllerWithDependencies()
		{
			var mockContext = _mocks.DynamicMock<HttpContextBase>();
			var application = new MockApplication(_container);
			Expect.Call(mockContext.ApplicationInstance).Return(application);
			var context = new RequestContext(mockContext, new RouteData());
			_mocks.ReplayAll();

			IControllerFactory factory = new ObjectBuilderControllerFactory();

			IController controller = factory.CreateController(context, "ObjectBuilderDependency"); //typeof(DependencyController));

			Assert.That(controller, Is.Not.Null);
			Assert.That(controller, Is.AssignableFrom(typeof(ObjectBuilderDependencyController)));

			var dependencyController = (ObjectBuilderDependencyController)controller;
			Assert.That(dependencyController._dependency, Is.Not.Null);
			Assert.That(dependencyController._dependency, Is.AssignableFrom(typeof(StubDependency)));
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShouldThrowExceptionWhenContainerIsNull()
		{
			var mockContext = _mocks.DynamicMock<HttpContextBase>();
			var application = new MockApplication(null);
			Expect.Call(mockContext.ApplicationInstance).Return(application);
			var context = new RequestContext(mockContext, new RouteData());
			_mocks.ReplayAll();

			IControllerFactory factory = new ObjectBuilderControllerFactory();

			IController controller = factory.CreateController(context, "ObjectBuilderSimple");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShouldThrowExceptionWhenApplicationDoesNotImplementIContainerAccessor()
		{
			var mockContext = _mocks.DynamicMock<HttpContextBase>();
			var application = new HttpApplication();
			Expect.Call(mockContext.ApplicationInstance).Return(application);
			var context = new RequestContext(mockContext, new RouteData());
			_mocks.ReplayAll();

			IControllerFactory factory = new ObjectBuilderControllerFactory();

			IController controller = factory.CreateController(context, "ObjectBuilderSimple");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowExceptionWhenRequestContextIsNull()
		{
			IControllerFactory factory = new ObjectBuilderControllerFactory();

			IController controller = factory.CreateController(null, "ObjectBuilderSimple");
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

		public class ObjectBuilderSimpleController : IController
		{
			public void Execute(ControllerContext controllerContext)
			{
				throw new NotImplementedException();
			}
		}

		public class ObjectBuilderDependencyController : IController
		{
			public IDependency _dependency;

			public ObjectBuilderDependencyController(IDependency dependency)
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
