using System.Collections.Generic;

namespace MvcContrib.Rest.Routing.Attributes
{
	public interface IRestfulParameterAttribute : IRestfulNodePositionAttribute
	{
		string Name { get; set; }

		PatternType PatternType { get; set; }

		bool IsOptional { get; set; }

		string CustomPattern { get; set; }

		string[] AcceptedValues { get; set; }

		string DefaultValue { get; set; }

		IEnumerable<KeyValuePair<string, string>> AcceptedValuesAndAliases { get; }
	}
}