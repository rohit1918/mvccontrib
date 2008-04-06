using System.Web.Routing;

namespace MvcContrib.Rest.Routing
{
	public interface IMvcRouteBuilder
	{
		void BuildRoutes(IRestfulRuleContainer ruleContainer, RouteCollection routingRuleContainer);
	}
}