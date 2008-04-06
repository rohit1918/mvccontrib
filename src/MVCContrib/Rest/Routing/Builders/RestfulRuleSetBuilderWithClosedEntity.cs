namespace MvcContrib.Rest.Routing.Builders
{
	public class RestfulRuleSetBuilderWithClosedEntity<TParent, TRoot> :
		IRestfulRuleSetBuilder<RestfulRuleSetBuilderWithClosedEntity<TParent, TRoot>, TRoot>
		where TParent : IRestfulRuleSetBuilder<TParent, TRoot>
		where TRoot : IRootRestfulRuleSetBuilder<TRoot>
	{
		private readonly TParent _parent;

		public RestfulRuleSetBuilderWithClosedEntity(TParent builder)
		{
			_parent = builder;
		}

		#region IRestfulRuleSetBuilder<RestfulRuleSetBuilderWithClosedEntity<TParent,TRoot>,TRoot> Members

		public void Register()
		{
			_parent.RootBuilder.Register();
		}

		IRootRestfulRuleSetBuilder<TRoot>
			IRestfulRuleSetBuilder<RestfulRuleSetBuilderWithClosedEntity<TParent, TRoot>, TRoot>.RootBuilder
		{
			get { return _parent.RootBuilder; }
		}

		#endregion

		public ExtraEntityRuleBuilder<RestfulRuleSetBuilderWithClosedEntity<TParent, TRoot>, TRoot> WithExtraEntityRoute()
		{
			return new ExtraEntityRuleBuilder<RestfulRuleSetBuilderWithClosedEntity<TParent, TRoot>, TRoot>(this);
		}

		public RestfulRuleSetBuilderWithClosedEntity<TParent, TRoot> WithExtraEntityRouteTo(string action)
		{
			return WithExtraEntityRoute().ToAction(action).Register();
		}

		public ExtraListingRuleBuilder<RestfulRuleSetBuilderWithClosedEntity<TParent, TRoot>, TRoot> WithExtraListingRoute()
		{
			return new ExtraListingRuleBuilder<RestfulRuleSetBuilderWithClosedEntity<TParent, TRoot>, TRoot>(this);
		}

		public RestfulRuleSetBuilderWithClosedEntity<TParent, TRoot> WithExtraListingRouteToAction(string action)
		{
			return WithExtraListingRoute().ToAction(action).Register();
		}

		public IdBuilder<RestfulRuleSetBuilderWithClosedEntity<TParent, TRoot>, TRoot> UsingId()
		{
			return new IdBuilder<RestfulRuleSetBuilderWithClosedEntity<TParent, TRoot>, TRoot>(this);
		}

		public RestfulRuleSetBuilderWithClosedEntity<TParent, TRoot> UsingId(string idName)
		{
			return UsingId().Named(idName).Return();
		}
	}
}