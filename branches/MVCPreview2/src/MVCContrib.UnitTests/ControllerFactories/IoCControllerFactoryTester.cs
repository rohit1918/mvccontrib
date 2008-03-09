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
        public class WhenCreatedWithoutADependencyResolverContainer:SpecBase
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
                using(Record())
                {
                    Expect.Call(_dependencyResolver.GetImplementationOf<IController>(typeof(IocTestController))).Return(
                        new IocTestController() as IController);
                }

                IController controller;

                using(Playback())
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
    }

    internal class IocTestController : IController
    {
        #region IController Members

        public void Execute(ControllerContext controllerContext)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}