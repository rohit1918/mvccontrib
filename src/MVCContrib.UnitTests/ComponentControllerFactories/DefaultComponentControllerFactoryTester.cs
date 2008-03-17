using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using MvcContrib.ExtendedComponentController;

namespace MvcContrib.UnitTests.ComponentControllerFactories
{
	[TestFixture]
	public class DefaultComponentControllerFactoryTester
	{
		private IComponentControllerFactory componentControllerFactory;

		[SetUp]
		public void SetUp()
		{
			componentControllerFactory = new DefaultComponentControllerFactory();
		}
		
		[Test]
		public void WhenCreateComponentControllerIsCalledWithGenerics()
		{
			var component=componentControllerFactory.CreateComponentController<SampleComponentController>();
			Assert.IsNotNull(component);
			Assert.IsAssignableFrom(typeof(SampleComponentController),component);
		}

		[Test]
		public void WhenCreateComponentControllerIsCalledWithType()
		{
			var component = componentControllerFactory.CreateComponentController(typeof(SampleComponentController));
			Assert.IsNotNull(component);
			Assert.IsAssignableFrom(typeof(SampleComponentController), component);
		}

		[Test]
		[ExpectedException(typeof(MissingMethodException))]
		public void WhenCreateComponentControllerIsCalledWithDefaultConstructorlessComponent()
		{
			var component = componentControllerFactory.CreateComponentController(typeof(SampleComponentController2));
		}

		[Test]
		public void DisposeWorks()
		{
			var component = componentControllerFactory.CreateComponentController(typeof(SampleComponentController));
			componentControllerFactory.DisposeComponentController(component);
		}

		[TearDown]
		public void TearDown()
		{
			
		}
	}
}
