using System.Linq;
using MvcContrib.Rest.Routing;
using MvcContrib.Rest.Routing.Descriptors;
using MvcContrib.Rest.Routing.Services;

namespace MvcContrib.Rest.Routing.Services
{
	public class RestfulRuleConverter : IRestfulRuleConverter
	{
		private readonly IRestfulRuleContainer _container;

		public RestfulRuleConverter(IRestfulRuleContainer container, IFragmentConverter fragmentConverter)
		{
			FragmentConverter = fragmentConverter;
			_container = container;
		}

		protected IFragmentConverter FragmentConverter { get; set; }

		#region IRestfulRuleConverter Members

		public RestfulRoutingRule ToRestfulRoutingRule(IRestfulRuleDescriptor rule)
		{
			var route = new RestfulRoutingRule();
			route.Container = _container;
			route.Scope = rule.RuleSet.Scope;

			foreach (IFragmentDescriptor fragmentDescriptor in rule.Fragments)
			{
				route.AddFragment(FragmentConverter.ToFragment(fragmentDescriptor));
			}
			foreach (CustomRouteMatcher matcher in rule.CustomRouteMatchers)
			{
				route.AddCustomRouteMatcher(matcher);
			}

			AddExcludeRegisteredControllerCheck(route, rule);
			return route;
		}

		#endregion

		private void AddExcludeRegisteredControllerCheck(RestfulRoutingRule route, IRestfulRuleDescriptor rule)
		{
			if (rule.RuleSet.Controllers.Count() == 0)
			{
				route.AddCustomRouteMatcher((routingRule, context, match) =>
				{
					if (match.Values.ContainsKey("controller"))
					{
						return !routingRule.Container.IsControllerRegisteredInDifferentScope((string)match.Values["controller"], routingRule.Scope);
					}
					return true;
				});
			}
		}
	}
}