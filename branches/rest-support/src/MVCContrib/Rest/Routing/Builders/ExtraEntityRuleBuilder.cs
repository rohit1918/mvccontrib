using MvcContrib.Rest.Routing.Descriptors;

namespace MvcContrib.Rest.Routing.Builders
{
	public class ExtraEntityRuleBuilder<TReturnBuilder, TRoot>
		: ExtraRuleBuilder<TReturnBuilder, TRoot, ExtraEntityRuleBuilder<TReturnBuilder, TRoot>>
		where TReturnBuilder : IRestfulRuleSetBuilder<TReturnBuilder, TRoot>
		where TRoot : IRootRestfulRuleSetBuilder<TRoot>
	{
		public ExtraEntityRuleBuilder(TReturnBuilder ruleSetBuilder) : base(ruleSetBuilder)
		{
		}

		protected override ExtraEntityRuleBuilder<TReturnBuilder, TRoot> Me
		{
			get { return this; }
		}

		protected override void DoRegister(IRuleFragmentDescriptor ruleFragment)
		{
			Root.RegisterExtraEntityRule(ruleFragment);
		}
	}
}