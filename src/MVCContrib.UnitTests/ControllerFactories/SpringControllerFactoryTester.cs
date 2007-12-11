using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using MVCContrib.ControllerFactories;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Spring.Core.IO;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Xml;

namespace MVCContrib.UnitTests.ControllerFactories
{
	[TestFixture]
	public class SpringControllerFactoryTester
	{
		[TestFixture]
		public class WhenAValidControllerTypeIsPassed
		{
			[SetUp]
			public void Setup()
			{
				//still investigating configuring objects without using xml for unit test

				string objectXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> " +
				                   "  <objects xmlns=\"http://www.springframework.net\" " +
				                   "    xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
				                   "    xsi:schemaLocation=\"http://www.springframework.net http://www.springframework.net/xsd/spring-objects.xsd\"> " +
				                   "    <object id=\"SimpleController\" singleton=\"true\" type=\"MVCContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+SimpleController\"/> " +
				                   "    <object id=\"DependencyController\" singleton=\"true\" type=\"MVCContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+DependencyController\" > " +
				                   "      <constructor-arg> " +
				                   "        <object type=\"MVCContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+StubDependency\" /> " +
				                   "      </constructor-arg> " +
				                   "    </object> " +
				                   "  </objects>";
				Stream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(objectXml));
				IResource resource = new InputStreamResource(stream, "In memory xml");
				IObjectFactory factory = new XmlObjectFactory(resource);
				SpringControllerFactory.Configure(factory);
			}

			[Test]
			public void ShouldReturnTheController()
			{
				IControllerFactory factory = new SpringControllerFactory();
				IController controller = factory.CreateController(null, typeof(SimpleController));

				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.AssignableFrom(typeof(SimpleController)));
			}

			[Test]
			public void ShouldReturnControllerWithDependencies()
			{
				IControllerFactory factory = new SpringControllerFactory();
				IController controller = factory.CreateController(null, typeof(DependencyController));

				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.AssignableFrom(typeof(DependencyController)));

				DependencyController dependencyController = (DependencyController)controller;
				Assert.That(dependencyController._dependency, Is.Not.Null);
				Assert.That(dependencyController._dependency, Is.AssignableFrom(typeof(StubDependency)));
			}

			public class SimpleController : IController
			{
				public void Execute(ControllerContext controllerContext)
				{
					throw new NotImplementedException();
				}
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
	}
}