using System;
using System.Web.Mvc;
using MvcContrib.ControllerFactories;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using StructureMap;

namespace MvcContrib.UnitTests.ControllerFactories
{
	[TestFixture]
	public class StructureMapControllerFactoryTester
	{
		[TestFixture]
		public class WhenAValidControllerTypeIsPassed
		{
			[SetUp]
			public void Setup()
			{
				StructureMapConfiguration.ResetAll();
				StructureMapConfiguration.UseDefaultStructureMapConfigFile = false;
				StructureMapConfiguration.BuildInstancesOf<SimpleController>().TheDefaultIsConcreteType
					<SimpleController>();
				StructureMapConfiguration.BuildInstancesOf<IDependency>().TheDefaultIsConcreteType<StubDependency>();
				StructureMapConfiguration.BuildInstancesOf<DependencyController>().TheDefaultIsConcreteType
					<DependencyController>();
                ObjectFactory.Reset();
			}

			[Test]
			public void ShouldReturnTheController()
			{
				IControllerFactory factory = new StructureMapControllerFactory();
				IController controller = factory.CreateController(null, "Simple");

				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.AssignableFrom(typeof(SimpleController)));
			}

			[Test]
			public void ShouldReturnControllerWithDependencies()
			{
				IControllerFactory factory = new StructureMapControllerFactory();
				IController controller = factory.CreateController(null, "Dependency");

				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.AssignableFrom(typeof(DependencyController)));

				DependencyController dependencyController = (DependencyController)controller;
				Assert.That(dependencyController._dependency, Is.Not.Null);
				Assert.That(dependencyController._dependency, Is.AssignableFrom(typeof(StubDependency)));
			}

		    public class DependencyController : IController
			{
				public IDependency _dependency;

				public DependencyController(IDependency dependency)
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

	    public class SimpleController : IController
	    {
	        public void Execute(ControllerContext controllerContext)
	        {
	            throw new NotImplementedException();
	        }
	    }
	}
}
