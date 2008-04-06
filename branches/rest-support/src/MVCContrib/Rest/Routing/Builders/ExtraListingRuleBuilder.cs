using MvcContrib.Rest.Routing.Descriptors;

namespace MvcContrib.Rest.Routing.Builders
{
	public class ExtraListingRuleBuilder<TReturnBuilder, TRoot>
		: ExtraRuleBuilder<TReturnBuilder, TRoot, ExtraListingRuleBuilder<TReturnBuilder, TRoot>>
		where TReturnBuilder : IRestfulRuleSetBuilder<TReturnBuilder, TRoot>
		where TRoot : IRootRestfulRuleSetBuilder<TRoot>
	{
		public ExtraListingRuleBuilder(TReturnBuilder ruleSetBuilder) : base(ruleSetBuilder)
		{
		}

		protected override ExtraListingRuleBuilder<TReturnBuilder, TRoot> Me
		{
			get { return this; }
		}

		protected override void DoRegister(IRuleFragmentDescriptor ruleFragment)
		{
			Root.RegisterExtraListingRule(ToRuleFragment());
		}
	}
}