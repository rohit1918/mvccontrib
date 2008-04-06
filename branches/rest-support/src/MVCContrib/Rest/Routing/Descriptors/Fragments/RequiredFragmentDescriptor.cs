using System.Collections;
using System.Collections.Generic;

namespace MvcContrib.Rest.Routing.Descriptors.Fragments
{
	public class RequiredFragmentDescriptor : BaseFragmentDescriptor
	{
		public RequiredFragmentDescriptor(IRestfulRuleDescriptor rule, string name, string pattern) : base(rule)
		{
			Name = name;
			UrlPart = pattern;
			Weight = 2;
			IsOptional = false;
		}

		public RequiredFragmentDescriptor(IRestfulRuleDescriptor rule, string name, IEnumerable<string> acceptedValues)
			: base(rule)
		{
			Name = name;
			IsOptional = false;
			AddAcceptedValues(acceptedValues);
			Weight = 1;
		}

		public RequiredFragmentDescriptor(IRestfulRuleDescriptor rule, string name, IDictionary valuesAndAliases)
			: base(rule)
		{
			Name = name;
			IsOptional = false;
			AddAcceptedValues(valuesAndAliases);
			Weight = 1;
		}
	}
}