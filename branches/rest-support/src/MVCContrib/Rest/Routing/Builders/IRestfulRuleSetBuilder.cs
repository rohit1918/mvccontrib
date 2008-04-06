namespace MvcContrib.Rest.Routing.Builders
{
	public interface IRestfulRuleSetBuilder<TBuilder, TRoot>
		where TBuilder : IRestfulRuleSetBuilder<TBuilder, TRoot>
		where TRoot : IRootRestfulRuleSetBuilder<TRoot>
	{
		IRootRestfulRuleSetBuilder<TRoot> RootBuilder { get; }
		void Register();
	}
}