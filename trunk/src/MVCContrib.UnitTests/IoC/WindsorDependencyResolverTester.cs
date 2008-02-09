using System.Web.Mvc;
using Castle.Windsor;
using MvcContrib.Castle;
using MvcContrib.Interfaces;
using MvcContrib.UnitTests.ControllerFactories;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MVCContrib.UnitTests.IoC
{
	[TestFixture]
	public class WindsorDependencyResolverTester
	{
		[TestFixture]
		public class WhenAValidTypeIsPassed : WhenAValidTypeIsPassedBase
		{
			[Test]
			public void ForCoverage()
			{
				IWindsorContainer container = new WindsorDependencyResolver().Container;
			}

			public override void TearDown()
			{
			}

			public override void Setup()
			{
				IWindsorContainer container = new WindsorContainer();
				container.AddComponent("SimpleDependency",
				                       typeof(
				                       	SimpleDependency));
				container.AddComponent("IDependency",
				                       typeof(
				                       	IDependency),
				                       typeof(
				                       	SimpleDependency));
				container.AddComponent("NestedDependency",
				                       typeof(NestedDependency));

				this._dependencyResolver = new WindsorDependencyResolver(container);
			}
		}
	}
}