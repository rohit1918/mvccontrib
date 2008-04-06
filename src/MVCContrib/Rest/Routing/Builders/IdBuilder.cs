namespace MvcContrib.Rest.Routing.Builders
{
	public class IdBuilder<TReturnBuilder, TRoot>
		where TReturnBuilder : IRestfulRuleSetBuilder<TReturnBuilder, TRoot>
		where TRoot : IRootRestfulRuleSetBuilder<TRoot>
	{
		private readonly TReturnBuilder _returnBuilder;

		public IdBuilder(TReturnBuilder returnBuilder)
		{
			_returnBuilder = returnBuilder;
		}

		public TReturnBuilder Return()
		{
			return _returnBuilder;
		}

		/// <summary>Set the name of of the id fragment parameter.</summary>
		/// <param name="name">The name of the parameter.</param>
		/// <returns>The current <see cref="IdBuilder{TReturnBuilder,TRoot}"/></returns>
		public IdBuilder<TReturnBuilder, TRoot> Named(string name)
		{
			_returnBuilder.RootBuilder.EntityIdFragment.Name = name;
			return this;
		}

		/// <summary>Restrict the id fragment to only accept <c>0</c> and positive <see cref="int"/>'s</summary>
		/// <returns>The current <see cref="IdBuilder{TReturnBuilder,TRoot}"/></returns>
		public IdBuilder<TReturnBuilder, TRoot> RestrictedToPositiveInt32()
		{
			_returnBuilder.RootBuilder.EntityIdFragment.UrlPart = RegexPatternBuilder.RequiredPositiveInt32Pattern;
			return this;
		}

		public IdBuilder<TReturnBuilder, TRoot> RestrictedToPositiveInt64()
		{
			_returnBuilder.RootBuilder.EntityIdFragment.UrlPart = RegexPatternBuilder.RequiredPositiveInt64Pattern;
			return this;
		}

		public IdBuilder<TReturnBuilder, TRoot> RestrictedToGuid()
		{
			_returnBuilder.RootBuilder.EntityIdFragment.UrlPart = RegexPatternBuilder.RequiredGuidPattern;
			return this;
		}

		public IdBuilder<TReturnBuilder, TRoot> RestrictedToPattern(string regularExpressionPattern)
		{
			_returnBuilder.RootBuilder.EntityIdFragment.UrlPart = regularExpressionPattern;
			return this;
		}
	}
}