using System.Collections.Generic;
using System.Linq;
using MvcContrib.Rest.Routing.Descriptors;
using MvcContrib.Rest.Routing.Ext;
using MvcContrib.Rest.Routing.Services;

namespace MvcContrib.Rest.Routing.Services
{
	public class RestfulRuleSorter : IRestfulRuleSorter
	{
		#region IRestfulRuleSorter Members

		public IEnumerable<IRestfulRuleDescriptor> Sort(IEnumerable<IRestfulRuleSetDescriptor> ruleSets)
		{
			var rules = new List<IRestfulRuleDescriptor>();

			ruleSets.ForEach(ruleSet => rules.AddRange(ruleSet.Rules));
			rules.Sort((lhs, rhs) =>
			{
				if (lhs.Levels == rhs.Levels)
				{
					return lhs.Weight.CompareTo(rhs.Weight);
				}
				return rhs.Levels.CompareTo(lhs.Levels);
			});

			return rules.AsEnumerable();
		}

		#endregion
	}
}