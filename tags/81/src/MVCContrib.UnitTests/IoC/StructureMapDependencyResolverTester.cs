using System;
using System.Web.Mvc;
using MvcContrib.ControllerFactories;
using MvcContrib.Interfaces;
using MvcContrib.StructureMap;
using MvcContrib.UnitTests.ControllerFactories;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using StructureMap;

namespace MVCContrib.UnitTests.IoC
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
                StructureMap.ObjectFactory.Reset();
                _dependencyResolver = new StructureMapDependencyResolver();
            }
        }
    }
}