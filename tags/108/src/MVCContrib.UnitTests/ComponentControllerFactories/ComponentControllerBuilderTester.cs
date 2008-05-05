using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
			FieldInfo fieldInfo = typeof(ComponentControllerBuilder).GetField("_current",
													  BindingFlags.Static | BindingFlags.NonPublic);
			fieldInfo.SetValue(null, null);
		}

		[Test]
		public void CheckIfAInstanceAssignmentWorks()
		{
			var factory = new DefaultComponentControllerFactory();
			ComponentControllerBuilder.Current.SetComponentControllerFactory(factory);
			var actual = ComponentControllerBuilder.Current.GetComponentControllerFactory();
			Assert.AreSame(factory, actual);
		}

		[Test]
		public void CheckIfCurrentReturnsNonNull()
		{
			Assert.IsNotNull(ComponentControllerBuilder.Current);

		}

		[Test]
		public void GetComponentControllerFactoryReturnsNonNull()
		{
			Assert.IsNotNull(ComponentControllerBuilder.Current.GetComponentControllerFactory());
		}

		[Test]
		public void CheckIfTypeAssignmentWorks()
		{
			var previous = ComponentControllerBuilder.Current.GetComponentControllerFactory();
			ComponentControllerBuilder.Current.SetComponentControllerFactory(typeof(DefaultComponentControllerFactory));
			var current = ComponentControllerBuilder.Current.GetComponentControllerFactory();
			Assert.IsAssignableFrom(typeof(DefaultComponentControllerFactory),current);
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

		[TearDown]
		public void TearDown()
		{

		}
	}
}
