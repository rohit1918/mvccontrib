using MvcContrib.Rest.Routing.Descriptors;

namespace MvcContrib.Rest.Routing.Builders
{
	public class RestfulRuleSetBuilderWithClosedListing<TParent, TRoot>
		: BaseRuleSetBuilder<RestfulRuleSetBuilderWithClosedListing<TParent, TRoot>, TRoot>
		where TParent : IRestfulRuleSetBuilder<TParent, TRoot>
		where TRoot : IRootRestfulRuleSetBuilder<TRoot>
	{
		public RestfulRuleSetBuilderWithClosedListing(TParent parent) : base((TRoot)parent.RootBuilder)
		{
		}

		protected override RestfulRuleSetBuilderWithClosedListing<TParent, TRoot> Me
		{
			get { return this; }
		}

		protected override void AddFragment(IFragmentDescriptor fragment)
		{
			Root.AddFragment(fragment);
		}

		public RestfulRuleSetBuilderWithClosedEntity<RestfulRuleSetBuilderWithClosedListing<TParent, TRoot>, TRoot> ToEntity()
		{
			Root.CloseEntity();
			return new RestfulRuleSetBuilderWithClosedEntity<RestfulRuleSetBuilderWithClosedListing<TParent, TRoot>, TRoot>(Me);
		}
	}
}