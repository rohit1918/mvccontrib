using System.Collections.Generic;

namespace MvcContrib.Rest.Routing.Descriptors
{
	///<summary>
	///</summary>
	public interface IRestfulRuleDescriptor
	{
		IRestfulRuleSetDescriptor RuleSet { get; }

		int Weight { get; }

		int Levels { get; }

		IEnumerable<IFragmentDescriptor> Fragments { get; }

		IEnumerable<CustomRouteMatcher> CustomRouteMatchers { get; }

		IEnumerable<IFragmentDescriptor> PrefixFragments { get; set; }
		IRestfulRuleDescriptor AddFragment(IFragmentDescriptor fragment);
	}
}