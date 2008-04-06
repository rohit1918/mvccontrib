using MvcContrib.Rest.Routing;

namespace MvcContrib.TestHelper.Context.Builders
{
	public class RestfulRoutingFixtureContextAdapter
	{
		private readonly IRoutingEngine _routingEngine;
		private RouteContextBuilder _routeContextBuilder;

		public RestfulRoutingFixtureContextAdapter(IRoutingEngine routingEngine)
		{
			_routingEngine = routingEngine;
		}

		public virtual RouteContextBuilder GetRequest
		{
			get { return RouteContext.Get; }
		}

		public virtual RouteContextBuilder PostRequest
		{
			get { return RouteContext.Post; }
		}

		public virtual RouteContextBuilder RouteContext
		{
			get { return _routeContextBuilder ?? (_routeContextBuilder = new RouteContextBuilder(_routingEngine)); }
		}

		public RouteContextBuilder PutRequest
		{
			get { return RouteContext.Put; }
		}

		public RouteContextBuilder DeleteRequest
		{
			get { return RouteContext.Delete; }
		}
	}
}