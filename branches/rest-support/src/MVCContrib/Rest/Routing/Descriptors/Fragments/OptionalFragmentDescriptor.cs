using System.Collections;
using System.Collections.Generic;

namespace MvcContrib.Rest.Routing.Descriptors.Fragments
{
	public class OptionalFragmentDescriptor : BaseFragmentDescriptor
	{
		public OptionalFragmentDescriptor(IRestfulRuleDescriptor rule, string name, string defaultValue,
		                                  IEnumerable<string> acceptedValues) : base(rule)
		{
			Name = name;
			DefaultValue = defaultValue;
			AddAcceptedValues(acceptedValues);
			Weight = 3;
			IsOptional = true;
		}

		public OptionalFragmentDescriptor(IRestfulRuleDescriptor rule, string name, string defaultValue,
		                                  IDictionary valuesAndAliases) : base(rule)
		{
			Name = name;
			IsOptional = true;
			DefaultValue = defaultValue;
			AddAcceptedValues(valuesAndAliases);
			Weight = 1;
		}
	}
}