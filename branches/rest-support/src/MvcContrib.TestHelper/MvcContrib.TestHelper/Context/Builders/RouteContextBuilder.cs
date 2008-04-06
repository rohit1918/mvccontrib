using System.Collections;
using System.Web;
using System.Web.Routing;
using MvcContrib.Rest.Routing;
using MvcContrib.TestHelper.Stubs;

namespace MvcContrib.TestHelper.Context.Builders
{
	public class RouteContextBuilder : MvcRequestBuilder<RouteContextBuilder>
	{
		protected ITestHttpContext _httpContext;
		protected string _applicationPath = "/";
		protected IDictionary _contextItems = Hash.Empty;
		protected IRoutingEngine _routingEngine;

		public RouteContextBuilder(IRoutingEngine routingEngine)
		{
			_routingEngine = routingEngine;
		}

		/// <summary>Perform a match on the current routing engine with the current http context.</summary>
		/// <returns></returns>
		public RouteData Match()
		{
			return _routingEngine.FindMatch(ToHttpContext());
		}

		public RouteData Match(string url)
		{
			_httpContext.Request.Url = url;
			return _routingEngine.FindMatch(ToHttpContext());
		}
	}
}