using System.Collections.Generic;
using MvcContrib.Rest.Routing.Descriptors;

namespace MvcContrib.Rest.Routing.Services
{
	public interface IRestfulRuleSorter
	{
		IEnumerable<IRestfulRuleDescriptor> Sort(IEnumerable<IRestfulRuleSetDescriptor> ruleSets);
	}

	public interface IRestfulRuleCompactor
	{
		IEnumerable<IRestfulRuleDescriptor> Compact(IEnumerable<IRestfulRuleDescriptor> rules);
	}
}