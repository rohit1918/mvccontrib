using MvcContrib.Rest.Routing;
using MvcContrib.TestHelper.Context.Builders;
using NUnit.Framework;
namespace MvcContrib.UnitTests.Rest.Routing
{
	public class RestfulRoutingFixture
	{
		protected int points;
		protected IRoutingEngine _routingEngine;
		protected RestfulRoutingBuilder _restfulRoutingBuilder;
		private RestfulRoutingFixtureContextAdapter _with;

		[SetUp]
		protected virtual void InitializeTest()
		{
			BaseFixtureSetUp();
			SetUp();
		}

		protected virtual void BaseFixtureSetUp()
		{
			_routingEngine = new RoutingEngine();
			_restfulRoutingBuilder = new RestfulRoutingBuilder();
			_with = new RestfulRoutingFixtureContextAdapter(_routingEngine);
		}

		protected virtual void SetUp()
		{
		}

		protected virtual RestfulRoutingFixtureContextAdapter With
		{
			get { return _with; }
		}

	}
}