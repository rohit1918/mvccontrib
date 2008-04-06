using System.Collections.Generic;
using MvcContrib.Rest.Routing.Descriptors.Fragments;

namespace MvcContrib.Rest.Routing.Descriptors.Rules
{
	public class RestfulCreateEntityRuleDescriptor : BaseRestfulRuleDescriptor
	{
		public RestfulCreateEntityRuleDescriptor(IRestfulRuleSetDescriptor ruleSet) : base(ruleSet)
		{
			ListOfFragments.Add(new ControllerFragmentDescriptor(this));
		}

		public override IEnumerable<CustomRouteMatcher> CustomRouteMatchers
		{
			get { return CreateNewEntityMatchers; }
		}
	}
}