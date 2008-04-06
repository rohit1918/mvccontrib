using MvcContrib.Rest.Routing.Descriptors;

namespace MvcContrib.Rest.Routing.Builders
{
	public class RestfulRuleSetBuilderWithClosedController :
		BaseRuleSetBuilder<RestfulRuleSetBuilderWithClosedController, RestfulRuleSetBuilder>
	{
		public RestfulRuleSetBuilderWithClosedController(RestfulRuleSetBuilder rootBuilder) : base(rootBuilder)
		{
		}

		protected override RestfulRuleSetBuilderWithClosedController Me
		{
			get { return this; }
		}

		protected override void AddFragment(IFragmentDescriptor fragment)
		{
			Root.AddFragment(fragment);
		}

		public RestfulRuleSetBuilderWithClosedListing<RestfulRuleSetBuilderWithClosedController, RestfulRuleSetBuilder>
			ToListing()
		{
			Root.CloseListing();
			return
				new RestfulRuleSetBuilderWithClosedListing<RestfulRuleSetBuilderWithClosedController, RestfulRuleSetBuilder>(Me);
		}

		public RestfulRuleSetBuilderWithClosedEntity<RestfulRuleSetBuilderWithClosedController, RestfulRuleSetBuilder>
			ToEntity()
		{
			Root.CloseEntity();
			return new RestfulRuleSetBuilderWithClosedEntity<RestfulRuleSetBuilderWithClosedController, RestfulRuleSetBuilder>(Me);
		}
	}
}