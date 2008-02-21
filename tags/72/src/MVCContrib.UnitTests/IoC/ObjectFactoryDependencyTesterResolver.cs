using System;
using MvcContrib.Interfaces;
using MvcContrib.ObjectBuilder;
using NUnit.Framework;

namespace MVCContrib.UnitTests.IoC
{
    public class ObjectFactoryDependencyResolverTester
    {
        [TestFixture]
        public class WhenAValidTypeIsPassed : WhenAValidTypeIsPassedBase
        {
            public override void TearDown()
            {
                
            }
            public override void Setup()
            {
		        IDependencyContainer container = new DependencyContainer();
		        container.RegisterTypeMapping<SimpleDependency, SimpleDependency>();
		        container.RegisterTypeMapping<IDependency, SimpleDependency>();
		        container.RegisterTypeMapping<NestedDependency, NestedDependency>();
                
                _dependencyResolver = new ObjectFactoryDependencyResolver(container);
            }
        }
    }
}
