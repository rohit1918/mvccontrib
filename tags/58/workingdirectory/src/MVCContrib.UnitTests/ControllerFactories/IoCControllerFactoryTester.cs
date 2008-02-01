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
                    Expect.Call(_dependencyResolver.GetImplementationOf<IController>(typeof(TestController))).Return(
                        new TestController() as IController);
                }

                IController controller;

                using (Playback())
                {
                    IControllerFactory controllerFactory = new IoCControllerFactory();
                    controller = controllerFactory.CreateController(null, typeof(TestController));
                }

                Assert.That(controller.GetType().Equals(typeof(TestController)));
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
                    Expect.Call(_dependencyResolver.GetImplementationOf<IController>(typeof(TestController))).Return(
                        new TestController() as IController);
                }

                IController controller;

                using(Playback())
                {
                    IControllerFactory controllerFactory = new IoCControllerFactory(_dependencyResolver);
                    controller = controllerFactory.CreateController(null, typeof(TestController));
                }

                Assert.That(controller.GetType().Equals(typeof(TestController)));
            }

            [Test]
            [ExpectedException(typeof(System.ArgumentNullException))]
            public void Should_throw_an_argument_null_exception_when_the_controller_type_is_null()
            {
                IControllerFactory controllerFactory = new IoCControllerFactory(null);
                controllerFactory.CreateController(null, null); 
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

    internal class TestController : IController
    {
        #region IController Members

        public void Execute(ControllerContext controllerContext)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}