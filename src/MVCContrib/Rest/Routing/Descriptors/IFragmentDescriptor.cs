using System.Collections.Generic;

namespace MvcContrib.Rest.Routing.Descriptors
{
	public interface IFragmentDescriptor
	{
		IRestfulRuleSetDescriptor RuleSet { get; set; }

		IRestfulRuleDescriptor Rule { get; set; }

		int Weight { get; set; }

		string UrlPart { get; set; }

		string Name { get; set; }

		string DefaultValue { get; set; }

		bool IsOptional { get; set; }

		IEnumerable<KeyValuePair<string, string>> AcceptedValuesAndAliases { get; }

		SplitPrefix SplitPrefix { get; set; }
	}
}