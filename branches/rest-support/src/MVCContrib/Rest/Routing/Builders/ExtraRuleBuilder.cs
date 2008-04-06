using System;
using MvcContrib.Rest.Routing.Descriptors;
using MvcContrib.Rest.Routing.Descriptors.Fragments;
using MvcContrib.Rest.Routing.Ext;

namespace MvcContrib.Rest.Routing.Builders
{
	public abstract class ExtraRuleBuilder<TReturnBuilder, TRoot, TBuilder> : BaseRuleSetBuilder<TBuilder, TRoot>
		where TBuilder : ExtraRuleBuilder<TReturnBuilder, TRoot, TBuilder>
		where TReturnBuilder : IRestfulRuleSetBuilder<TReturnBuilder, TRoot>
		where TRoot : IRootRestfulRuleSetBuilder<TRoot>
	{
		protected readonly IRuleFragmentDescriptor _ruleFragment = new RuleFragmentDescriptor();
		protected readonly TReturnBuilder _ruleSetBuilder;

		protected ExtraRuleBuilder(TReturnBuilder ruleSetBuilder) : base((TRoot)ruleSetBuilder.RootBuilder)
		{
			_ruleSetBuilder = ruleSetBuilder;
		}

		protected abstract void DoRegister(IRuleFragmentDescriptor ruleFragment);

		protected override void AddFragment(IFragmentDescriptor fragment)
		{
			_ruleFragment.AddFragment(fragment);
		}

		public new virtual TReturnBuilder Register()
		{
			DoRegister(ToRuleFragment());
			return _ruleSetBuilder;
		}

		/// <summary>Creates the <see cref="IRuleFragmentDescriptor"/> from the builder.</summary>
		/// <returns>The <see cref="IRuleFragmentDescriptor"/>.</returns>
		public virtual IRuleFragmentDescriptor ToRuleFragment()
		{
			return _ruleFragment;
		}

		/// <summary>Creates <see cref="StaticFragmentDescriptor"/>s for all the parts of the path and adds them to the rule.</summary>
		/// <param name="path">The path which must not contain any dots.</param>
		/// <returns>The builder.</returns>
		/// <remarks>Leading and trailing slashes are stripped, slashes in the middle of the path are split and so muliple
		/// <see cref="StaticFragmentDescriptor"/>s are created and applied to the rule..</remarks>
		public TBuilder ToPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return (TBuilder)this;
			}

			if (path.Contains("."))
			{
				throw new ArgumentException("path cannot contain any periods.", "path");
			}

			path = path.Trim().Trim('/');

			string[] pathParts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			pathParts.ForEach(pathPart => _ruleFragment.AddFragment(new StaticFragmentDescriptor(null, pathPart)));
			return Me;
		}

		/// <summary>Adds the action to execute to the rule.</summary>
		/// <param name="action">The name of the action on the controller to execute.</param>
		/// <returns>The builder.</returns>
		public TBuilder ToAction(string action)
		{
			_ruleFragment.AddFragment(new RequiredFragmentDescriptor(null, "action", new[] {action}));
			return Me;
		}

		/// <summary>Adds the action to execute to the rule with a url alias for the action.</summary>
		/// <param name="actionName">The name of the action on the controller to execute.</param>
		/// <param name="urlName">An alias to match in the url instead of the actual action name.</param>
		/// <returns>The builder</returns>
		public TBuilder ToAction(string actionName, string urlName)
		{
			var values = new Hash();
			values.Add(actionName, urlName);

			_ruleFragment.AddFragment(new RequiredFragmentDescriptor(null, "action", values));
			return Me;
		}
	}
}