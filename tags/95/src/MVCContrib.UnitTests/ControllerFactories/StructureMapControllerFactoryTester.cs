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
			    StructureMapConfiguration.BuildInstancesOf<IController>();
				StructureMapConfiguration.BuildInstancesOf<StructureMapSimpleController>().TheDefaultIsConcreteType
					<StructureMapSimpleController>();
				StructureMapConfiguration.BuildInstancesOf<IDependency>().TheDefaultIsConcreteType<StubDependency>();
				StructureMapConfiguration.BuildInstancesOf<StructureMapDependencyController>().TheDefaultIsConcreteType
					<StructureMapDependencyController>();
                ObjectFactory.Reset();
			}

			[Test]
			public void ShouldReturnTheController()
			{
				IControllerFactory factory = new StructureMapControllerFactory();
				IController controller = factory.CreateController(null, "StructureMapSimple");

				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.AssignableFrom(typeof(StructureMapSimpleController)));
			}

			[Test]
			public void ShouldReturnControllerWithDependencies()
			{
				IControllerFactory factory = new StructureMapControllerFactory();
				IController controller = factory.CreateController(null, "StructureMapDependency");

				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.AssignableFrom(typeof(StructureMapDependencyController)));

				StructureMapDependencyController dependencyController = (StructureMapDependencyController)controller;
				Assert.That(dependencyController._dependency, Is.Not.Null);
				Assert.That(dependencyController._dependency, Is.AssignableFrom(typeof(StubDependency)));
			}

		    public class StructureMapDependencyController : IController
			{
				public IDependency _dependency;

				public StructureMapDependencyController(IDependency dependency)
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

	    public class StructureMapSimpleController : IController
	    {
	        public void Execute(ControllerContext controllerContext)
	        {
	            throw new NotImplementedException();
	        }
	    }
	}
}
