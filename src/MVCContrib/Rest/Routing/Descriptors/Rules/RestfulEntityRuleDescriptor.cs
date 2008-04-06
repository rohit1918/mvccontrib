using System.Collections.Generic;

namespace MvcContrib.Rest.Routing.Descriptors.Rules
{
	public class RestfulEntityRuleDescriptor : BaseRestfulRuleDescriptor
	{
		public RestfulEntityRuleDescriptor(IRestfulRuleSetDescriptor ruleSet, IEnumerable<CustomRouteMatcher> routeMatchers)
			: base(ruleSet)
		{
			foreach(IFragmentDescriptor fragment in ruleSet.EntityFragment.Fragments)
			{
				fragment.RuleSet = ruleSet;
				fragment.Rule = this;
				ListOfFragments.Add(fragment);
			}
			ListOfRouteMatchers.AddRange(routeMatchers);
		}
	}
}