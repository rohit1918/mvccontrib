using MvcContrib.StructureMap;
using NUnit.Framework;
using StructureMap;

namespace MvcContrib.UnitTests.IoC
{
    [TestFixture]
    public class StructureMapDependencyResolverTester
    {
        [TestFixture]
        public  class WhenAValidDependencyTypeIsPassed : WhenAValidTypeIsPassedBase
        {
            public override void TearDown()
            {                
                
            }

            public override void Setup()
            {
                
                StructureMapConfiguration.ResetAll();                
                StructureMapConfiguration.UseDefaultStructureMapConfigFile = false;                
                StructureMapConfiguration.BuildInstancesOf<IDependency>().TheDefaultIsConcreteType<SimpleDependency>();
                StructureMapConfiguration.BuildInstancesOf<SimpleDependency>().TheDefaultIsConcreteType<SimpleDependency>();
                StructureMapConfiguration.BuildInstancesOf<NestedDependency>().TheDefaultIsConcreteType<NestedDependency>();
                ObjectFactory.Reset();
                _dependencyResolver = new StructureMapDependencyResolver();
            }
        }
    }
}
