using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcContrib.ExtendedComponentController;
using NUnit.Framework;

namespace MvcContrib.UnitTests.ComponentControllerFactories
{
	[TestFixture]
	public class ComponentControllerBuilderTester
	{
		[SetUp]
		public void SetUp()
		{
			
		}

		[Test]
		public void CheckIfDefaultComponentControllerIsCorrect()
		{
			ComponentControllerBuilder.Current.SetComponentControllerFactory((IComponentControllerFactory)null);
			var v = ComponentControllerBuilder.Current.GetComponentControllerFactory();
			Assert.IsAssignableFrom(typeof(DefaultComponentControllerFactory),v);
		}

		[Test]
		public void CheckIfAssignmentWorks()
		{
			var factory = new DefaultComponentControllerFactory();
			ComponentControllerBuilder.Current.SetComponentControllerFactory(factory);
			var actual = ComponentControllerBuilder.Current.GetComponentControllerFactory();
			Assert.AreEqual(factory,actual);
		}

		[Test]
		public void SetToDefaultWhenNullIsUsed()
		{
			ComponentControllerBuilder.Current.SetComponentControllerFactory((Type)null);
			var actual = ComponentControllerBuilder.Current.GetComponentControllerFactory();
			Assert.IsAssignableFrom(typeof(DefaultComponentControllerFactory),actual);

			ComponentControllerBuilder.Current.SetComponentControllerFactory((IComponentControllerFactory)null);
			actual = ComponentControllerBuilder.Current.GetComponentControllerFactory();
			Assert.IsAssignableFrom(typeof(DefaultComponentControllerFactory), actual);
		}

	}
}