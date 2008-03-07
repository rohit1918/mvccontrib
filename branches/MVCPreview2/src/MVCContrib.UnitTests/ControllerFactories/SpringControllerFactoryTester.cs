using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Spring;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Spring.Context;
using Spring.Context.Support;
using Spring.Core.IO;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Xml;

namespace MvcContrib.UnitTests.ControllerFactories
{
	public class SpringControllerFactoryTester
	{
		[TestFixture]
		public class WhenConfigureNotCalled
		{
            IControllerFactory factory;
			[SetUp]
			public void Setup()
			{
				//make sure instance variable was not set in another
				//test fixture.  this needs to be done because of the 
				//static field and we need to be sure that test fixtures
				//can be run in any order.
				Type springFactoryType = typeof(SpringControllerFactory);
				FieldInfo factoryField = springFactoryType.GetField("_objectFactory", BindingFlags.NonPublic | BindingFlags.Static);

				
			    factory = new SpringControllerFactory();
			    factoryField.SetValue(factory, null);
			}

			[Test, ExpectedException(typeof(ArgumentException))]
			public void ShouldThrowExceptionForNoConfig()
			{
				
				IController controller = factory.CreateController(null, "Simple"); 
                //                                                  Type.GetType(
                //                                                    "MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+SimpleController"));
			}
		}

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
				                   "    <object id=\"SimpleController\" singleton=\"true\" type=\"MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+SimpleController\"/> " +
				                   "    <object id=\"DependencyController\" singleton=\"true\" type=\"MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+DependencyController\" > " +
				                   "      <constructor-arg> " +
				                   "        <object type=\"MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+StubDependency\" /> " +
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
				IController controller = factory.CreateController(null,"Simple");

				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.AssignableFrom(typeof(SimpleController)));
			}

			[Test]
			public void ShouldReturnControllerWithDependencies()
			{
				IControllerFactory factory = new SpringControllerFactory();
				IController controller = factory.CreateController(null, "Dependency");

				Assert.That(controller, Is.Not.Null);
				Assert.That(controller, Is.AssignableFrom(typeof(DependencyController)));

				DependencyController dependencyController = (DependencyController)controller;
				Assert.That(dependencyController._dependency, Is.Not.Null);
				Assert.That(dependencyController._dependency, Is.AssignableFrom(typeof(StubDependency)));
			}

            [Test, ExpectedException(typeof(NullReferenceException))]
			public void ShouldThrowExceptionForInvalidController()
			{
				IControllerFactory factory = new SpringControllerFactory();
			    IController controller = factory.CreateController(null, "NonValid");//typeof(NonValidController));
			}

            [Test, ExpectedException(typeof(NullReferenceException))]
			public void ShouldThrowExceptionForNullControllerType()
			{
				IControllerFactory factory = new SpringControllerFactory();
				IController controller = factory.CreateController(null, null);
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

			public class NonValidController : IController
			{
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

		[TestFixture]
		public class WhenPassedApplicationContext
		{
			[Test]
			public void ShouldConfigureFactory()
			{
				string objectXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> " +
				                   "  <objects xmlns=\"http://www.springframework.net\" " +
				                   "    xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
				                   "    xsi:schemaLocation=\"http://www.springframework.net http://www.springframework.net/xsd/spring-objects.xsd\"> " +
				                   "    <object id=\"SimpleController\" singleton=\"true\" type=\"MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+SimpleController\"/> " +
				                   "    <object id=\"DependencyController\" singleton=\"true\" type=\"MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+DependencyController\" > " +
				                   "      <constructor-arg> " +
				                   "        <object type=\"MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+StubDependency\" /> " +
				                   "      </constructor-arg> " +
				                   "    </object> " +
				                   "  </objects>";
				Stream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(objectXml));
				IResource resource = new InputStreamResource(stream, "In memory xml");
				GenericApplicationContext ctx = new GenericApplicationContext();
				XmlObjectDefinitionReader reader = new XmlObjectDefinitionReader(ctx);
				reader.LoadObjectDefinitions(resource);
				ctx.Refresh();
				SpringControllerFactory.Configure(ctx as IApplicationContext);
				IControllerFactory factory = new SpringControllerFactory();
			    IController controller = factory.CreateController(null, "Simple");
				                                                  //Type.GetType("MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+SimpleController"));

				Assert.That(controller, Is.Not.Null);
				Assert.That(controller,
				            Is.AssignableFrom(
				            	Type.GetType(
				            		"MvcContrib.UnitTests.ControllerFactories.SpringControllerFactoryTester+WhenAValidControllerTypeIsPassed+SimpleController")));
			}
		}
	}
}
