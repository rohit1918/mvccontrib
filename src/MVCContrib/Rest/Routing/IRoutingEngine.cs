using System.Collections;
using System.Web;
using System.Web.Routing;

namespace MvcContrib.Rest.Routing
{
	public interface IRoutingEngine
	{
		RouteData FindMatch(HttpContextBase httpContext);

		RouteCollection Rules { get; set; }

		string CreateUrl(RequestContext request, IDictionary routeParameters);
	}

	/// <summary>A default wrapper around the MS MVC RouteCollection</summary>
	public class RoutingEngine : IRoutingEngine
	{
		public RoutingEngine()
			: this(new RouteCollection())
		{
		}

		public RoutingEngine(RouteCollection rules)
		{
			Rules = rules;
		}

		public RouteData FindMatch(HttpContextBase httpContext)
		{
			return Rules.GetRouteData(httpContext);
		}

		public RouteCollection Rules { get; set; }

		public string CreateUrl(RequestContext request, IDictionary routeParameters)
		{
			RouteValueDictionary routeValues = new RouteValueDictionary();
			foreach(DictionaryEntry entry in routeParameters)
			{
				routeValues.Add(entry.Key.ToString(), entry.Value);
			}
			return Rules.GetVirtualPath(request, routeValues).VirtualPath;
		}
	}
}