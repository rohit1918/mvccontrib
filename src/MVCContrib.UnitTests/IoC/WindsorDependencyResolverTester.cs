using Castle.Windsor;
using MvcContrib.Castle;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.IoC
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

				_dependencyResolver = new WindsorDependencyResolver(container);
			}
		}

		[TestFixture]
		public class WhenDisposeImplementationIsCalled
		{
			private MockRepository _mocks;
			private WindsorDependencyResolver _resolver;
			private IWindsorContainer _container;

			[SetUp]
			public void Setup()
			{
				_mocks = new MockRepository();
				_container = _mocks.DynamicMock<IWindsorContainer>();
				_resolver = new WindsorDependencyResolver(_container);
			}

			[Test]
			public void ThenReleaseShouldBeCalled()
			{
				var obj = new object();
				using(_mocks.Record())
				{
					Expect.Call(() => _container.Release(obj));
				}
				using(_mocks.Playback())
				{
					_resolver.DisposeImplementation(obj);
				}
			}
		} 
	}
}
