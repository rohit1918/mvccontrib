using System;
using System.Web.Mvc;
using MvcContrib.ControllerFactories;
using MvcContrib.Interfaces;
using MvcContrib.Services;
using MvcContrib.UnitTests.ControllerFactories;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ControllerFactories.IoCControllerFactoryTester
{
	[TestFixture]
	public class IoCControllerFactoryTester
	{
		[TestFixture]
		public class WhenCreatedWithoutADependencyResolverContainer : SpecBase
		{
			private IDependencyResolver _dependencyResolver;

			[Test]
			public void Should_call_into_the_static_resolver_to_create_a_controller()
			{
				using (Record())
				{
					Expect.Call(_dependencyResolver.GetImplementationOf(typeof(IocTestController))).Return(
						new IocTestController() as IController);
				}

				IController controller;

				using (Playback())
				{
					IControllerFactory controllerFactory = new IoCControllerFactory();
					controller = controllerFactory.CreateController(null, "IocTest");
				}

				Assert.That(controller.GetType().Equals(typeof(IocTestController)));
			}

			protected override void BeforeEachSpec()
			{
				_dependencyResolver = _mocks.CreateMock<IDependencyResolver>();
				DependencyResolver.InitializeWith(_dependencyResolver);
			}

			protected override void AfterEachSpec()
			{
				_dependencyResolver = null;
				DependencyResolver.InitializeWith(null);
			}
		}

		[TestFixture]
		public class WhenCreatedWithADependencyResolverCcontainer : SpecBase
		{
			private IDependencyResolver _dependencyResolver;

			[Test]
			public void Should_call_into_the_resolver_to_create_a_controller()
			{
				using (Record())
				{
					Expect.Call(_dependencyResolver.GetImplementationOf<IController>(typeof(IocTestController))).Return(
						new IocTestController() as IController);
				}

				IController controller;

				using (Playback())
				{
					IControllerFactory controllerFactory = new IoCControllerFactory(_dependencyResolver);
					controller = controllerFactory.CreateController(null, "IocTest");
				}

				Assert.That(controller.GetType().Equals(typeof(IocTestController)));
			}

			[Test]
			[ExpectedException(typeof(System.ArgumentNullException))]
			public void Should_throw_an_argument_null_exception_when_the_resolver_is_null()
			{
				IControllerFactory controllerFactory = new IoCControllerFactory(null);
			}

			[Test, ExpectedException(typeof(ArgumentNullException))]
			public void Should_throw_if_controllerName_is_null()
			{
				IControllerFactory controllerFactory = new IoCControllerFactory(_dependencyResolver);
				controllerFactory.CreateController(null, null);
			}


			[Test, ExpectedException(typeof(Exception), ExpectedMessage = "Could not find a type for the controller name 'DoesNotExist'")]
			public void Should_throw_if_controller_type_cannot_be_resolved()
			{
				IControllerFactory controllerFactory = new IoCControllerFactory(_dependencyResolver);
				controllerFactory.CreateController(null, "DoesNotExist");
			}

			protected override void BeforeEachSpec()
			{
				_dependencyResolver = _mocks.CreateMock<IDependencyResolver>();
				DependencyResolver.InitializeWith(_dependencyResolver);
			}

			protected override void AfterEachSpec()
			{
				_dependencyResolver = null;
				DependencyResolver.InitializeWith(null);
			}
		}

		[TestFixture]
		public class WhenDisposeControllerIsCalled : SpecBase
		{
			private IDependencyResolver _dependencyResolver;

			[Test]
			public void Then_ReleaseImplementation_should_be_called_on_the_specified_resolver()
			{
				var controller = new IocTestController();
				using(Record())
				{
					Expect.Call(() => _dependencyResolver.DisposeImplementation(controller));
				}
				using(Playback())
				{
					IControllerFactory factory = new IoCControllerFactory(_dependencyResolver);
					factory.DisposeController(controller);
				}
			}

			[Test]
			public void Then_ReleaseImplementation_should_be_called_on_the_default_resolver()
			{
				DependencyResolver.InitializeWith(_dependencyResolver);
				var controller = new IocTestController();
				using (Record())
				{
					Expect.Call(() => _dependencyResolver.DisposeImplementation(controller));
				}
				using (Playback())
				{
					IControllerFactory factory = new IoCControllerFactory();
					factory.DisposeController(controller);
				}
			}

			[Test]
			public void And_controller_implements_IDisposable_then_dispose_should_be_called()
			{
				var controller = new DisposableIocTestController();

				using(Record())
				{
				}

				using (Playback())
				{
					IControllerFactory factory = new IoCControllerFactory(_dependencyResolver);
					factory.DisposeController(controller);
				}

				Assert.IsTrue(controller.IsDisposed);
			}

			protected override void AfterEachSpec()
			{
				
			}

			protected override void BeforeEachSpec()
			{
				_dependencyResolver = base._mocks.DynamicMock<IDependencyResolver>();	
			}
		}
	}

	public class IocTestController : IController
	{
		#region IController Members

		public void Execute(ControllerContext controllerContext)
		{
			throw new NotImplementedException();
		}

		#endregion
	}

	public class DisposableIocTestController : IController, IDisposable
	{
		public bool IsDisposed;

		public void Execute(ControllerContext controllerContext)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			IsDisposed = true;
		}
	}
}