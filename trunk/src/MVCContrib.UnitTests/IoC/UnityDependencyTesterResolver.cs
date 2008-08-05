using NUnit.Framework;
using Microsoft.Practices.Unity;
using MvcContrib.Unity;

namespace MvcContrib.UnitTests.IoC
{
	public class UnityDependencyResolverTester
	{
		[TestFixture]
		public class WhenAValidTypeIsPassed : WhenAValidTypeIsPassedBase
		{
			[Test]
			public void ForCoverage()
			{
				IUnityContainer container = new UnityDependencyResolver().Container;
			}
			public override void TearDown()
			{

			}
			public override void Setup()
			{
				IUnityContainer container = new UnityContainer()
					.RegisterType<SimpleDependency, SimpleDependency>()
					.RegisterType<IDependency, SimpleDependency>()
					.RegisterType<NestedDependency, NestedDependency>();

				_dependencyResolver = new UnityDependencyResolver(container);
			}

		}
	}
}
