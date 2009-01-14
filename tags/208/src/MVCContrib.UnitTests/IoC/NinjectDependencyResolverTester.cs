using MvcContrib.Ninject;
using Ninject.Core;
using NUnit.Framework;

namespace MvcContrib.UnitTests.IoC
{
    [TestFixture]
    public class NinjectDependencyResolverTester : WhenAValidTypeIsPassedBase
    {
        public override void Setup()
        {
            IKernel kernel = new StandardKernel(new TestModule());
            _dependencyResolver = new NinjectDependencyResolver(kernel);
        }

        public override void TearDown()
        {
        }
    }

    public class TestModule : StandardModule
    {
        public override void Load()
        {
            Bind<SimpleDependency>().ToSelf();
            Bind<IDependency>().To<SimpleDependency>();
            Bind<INestedDependency>().To<NestedDependency>();
        }
    }
}
