using System.Collections.Generic;

namespace MvcContrib.Rest.Routing.Descriptors.Rules
{
	public class RestfulListingRuleDescriptor : BaseRestfulRuleDescriptor
	{
		public RestfulListingRuleDescriptor(IRestfulRuleSetDescriptor ruleSet, IEnumerable<CustomRouteMatcher> routeMatchers)
			: base(ruleSet)
		{
			foreach(IFragmentDescriptor fragment in ruleSet.ListingFragment.Fragments)
			{
				fragment.RuleSet = ruleSet;
				fragment.Rule = this;
				ListOfFragments.Add(fragment);
			}
			ListOfRouteMatchers.AddRange(routeMatchers);
		}
	}
}