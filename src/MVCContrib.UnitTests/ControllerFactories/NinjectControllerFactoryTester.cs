using System;
using System.Web.Mvc;
using MvcContrib.Ninject;
using Ninject.Conditions;
using Ninject.Core;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.ControllerFactories
{
    [TestFixture]
    public class NinjectControllerFactoryTester
    {
        [SetUp]
        public void SetUp()
        {
            NinjectKernel.Initialize(new TestModule());
        }

        [Test]
        public void ShouldGetNinjaControllerFromNinjectControllerFactoryWhenControllerNameIsNinja()
        {
            var factory = new NinjectControllerFactory();

            IController ninjaController = factory.CreateController(null, "Ninja");

            Assert.That(ninjaController, Is.Not.Null);
            Assert.That(ninjaController, Is.AssignableFrom(typeof(NinjaController)));
        }

        [Test]
        public void NinjectControllerFacotryShouldDisposeController()
        {
            var factory = new NinjectControllerFactory();
            var disposableController = new DisposableNinjaController();
            factory.DisposeController(disposableController);
            Assert.That(disposableController.IsDisposed);
        }

        private class TestModule : StandardModule
        {
            public override void Load()
            {
                Bind<IController>().To<NinjaController>().Only(When.Context.Variable("controllerName").EqualTo("Ninja"));
                Bind<IController>().To<DisposableNinjaController>().Only(When.Context.Variable("controllerName").EqualTo("DisposableNinja"));
            }
        }

        private class NinjaController : IController
        {
            public void Execute(ControllerContext controllerContext)
            {
                throw new NotImplementedException();
            }
        }

        private class DisposableNinjaController : IDisposable, IController
        {
            public bool IsDisposed;
            public void Dispose()
            {
                IsDisposed = true;
            }

            public void Execute(ControllerContext controllerContext)
            {
                throw new NotImplementedException();
            }
        }
    }
}
