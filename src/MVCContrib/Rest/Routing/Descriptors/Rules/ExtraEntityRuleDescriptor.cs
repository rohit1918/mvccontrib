using System.Collections.Generic;

namespace MvcContrib.Rest.Routing.Descriptors.Rules
{
	public class ExtraEntityRuleDescriptor : ExtraRuleDescriptor
	{
		public ExtraEntityRuleDescriptor(IRestfulRuleSetDescriptor ruleSet, IRuleFragmentDescriptor ruleFragment)
		{
			RuleSet = ruleSet;
			foreach(IFragmentDescriptor fragment in ruleFragment.Fragments)
			{
				fragment.Rule = this;
				fragment.RuleSet = ruleSet;
				ListOfFragments.Add(fragment);
			}
		}

		protected override IEnumerable<IFragmentDescriptor> DependantFragments
		{
			get { return RuleSet.EntityFragment.Fragments; }
		}
	}
}