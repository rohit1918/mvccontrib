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
		public void WhenCreateComponentControllerIsCalled()
		{
			var component=componentControllerFactory.CreateComponentController<SampleComponentController>();
			Assert.IsNotNull(component);
		}


		[TearDown]
		public void TearDown()
		{
			
		}
	}
}