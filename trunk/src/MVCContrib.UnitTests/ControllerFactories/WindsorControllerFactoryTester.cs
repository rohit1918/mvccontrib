using System;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;
using MvcContrib.Castle;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.Castle
{
	[TestFixture]
	public class WindsorControllerFactoryTester
	{
		private MockRepository _mocks;
		private IWindsorContainer _container;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();

			_container = new WindsorContainer();
			_container.AddComponent("simplecontroller", typeof(WindsorSimpleController));
			_container.AddComponent("StubDependency", typeof(IDependency), typeof(StubDependency));
			_container.AddComponent("dependencycontroller", typeof(WindsorDependencyController));
		}

		[Test]
		public void ShouldReturnTheController()
		{
			IControllerFactory factory = new WindsorControllerFactory(_container);

			IController controller = factory.CreateController(null, "Simple");

			Assert.That(controller, Is.Not.Null);
			Assert.That(controller, Is.AssignableFrom(typeof(WindsorSimpleController)));
		}

		[Test]
		public void ShouldReturnControllerWithDependencies()
		{
			IControllerFactory factory = new WindsorControllerFactory(_container);

			IController controller = factory.CreateController(null, "Dependency");

			Assert.That(controller, Is.Not.Null);
			Assert.That(controller, Is.AssignableFrom(typeof(WindsorDependencyController)));

			var dependencyController = (WindsorDependencyController)controller;
			Assert.That(dependencyController._dependency, Is.Not.Null);
			Assert.That(dependencyController._dependency, Is.AssignableFrom(typeof(StubDependency)));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowExceptionWhenContainerIsNull()
		{
			new WindsorControllerFactory(null);
		}

		[Test]
		public void ShouldDisposeOfController()
		{
			IControllerFactory factory = new WindsorControllerFactory(_container);
			var controller = new WindsorDisposableController();
			factory.DisposeController(controller);
			Assert.That(controller.IsDisposed);
		}

		[Test]
		public void ShouldReleaseController()
		{
			var mockContainer = _mocks.DynamicMock<IWindsorContainer>();
			var controller = new WindsorSimpleController();
			using(_mocks.Record())
			{
				Expect.Call(() => mockContainer.Release(controller));
			}
			using(_mocks.Playback())
			{
				var factory = new WindsorControllerFactory(mockContainer);
				factory.DisposeController(controller);
			}
		}

		public class WindsorDisposableController : IDisposable, IController
		{
			public bool IsDisposed;

			public WindsorDisposableController()
			{
				IsDisposed = false;
			}

			public void Dispose()
			{
				IsDisposed = true;
			}

			public void Execute(ControllerContext controllerContext)
			{
			}
		}

		public class MockApplication : HttpApplication, IContainerAccessor
		{
			private readonly IWindsorContainer _container;

			public MockApplication(IWindsorContainer container)
			{
				_container = container;
			}

			public IWindsorContainer Container
			{
				get { return _container; }
			}
		}

		public class WindsorSimpleController : IController
		{
			public void Execute(ControllerContext controllerContext)
			{
				throw new NotImplementedException();
			}
		}

		public class WindsorDependencyController : IController
		{
			public IDependency _dependency;

			public WindsorDependencyController(IDependency dependency)
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
