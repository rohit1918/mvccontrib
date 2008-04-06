using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using MvcContrib.Rest.Routing.Descriptors;
using MvcContrib.Rest.Routing.Descriptors.Fragments;
using MvcContrib.Rest.Routing.Ext;

namespace MvcContrib.Rest.Routing.Builders
{
	public abstract class BaseRuleSetBuilder<TBuilder, TRootBuilder> : IRestfulRuleSetBuilder<TBuilder, TRootBuilder>
		where TBuilder : IRestfulRuleSetBuilder<TBuilder, TRootBuilder>
		where TRootBuilder : IRootRestfulRuleSetBuilder<TRootBuilder>
	{
		private TRootBuilder _root;

		protected BaseRuleSetBuilder()
		{
		}

		protected BaseRuleSetBuilder(TRootBuilder rootBuilder)
		{
			Root = rootBuilder;
		}

		protected abstract TBuilder Me { get; }

		protected IRootRestfulRuleSetBuilder<TRootBuilder> Root
		{
			get { return ((IRestfulRuleSetBuilder<TBuilder, TRootBuilder>)this).RootBuilder; }
			set { _root = (TRootBuilder)value; }
		}

		#region IRestfulRuleSetBuilder<TBuilder,TRootBuilder> Members

		public virtual void Register()
		{
			Root.Register();
		}

		IRootRestfulRuleSetBuilder<TRootBuilder> IRestfulRuleSetBuilder<TBuilder, TRootBuilder>.RootBuilder
		{
			get { return _root; }
		}

		#endregion

		protected abstract void AddFragment(IFragmentDescriptor fragment);

		public virtual TBuilder AddControllerToRuleSet(Type controllerType)
		{
			Root.AddController(controllerType);
			return Me;
		}

		public virtual TBuilder ToRequiredParameter(string name, IEnumerable<string> acceptedValues)
		{
			AddFragment(new RequiredFragmentDescriptor(null, name, acceptedValues));
			return Me;
		}

		public TBuilder ToRequiredParameter(string name, IEnumerable<KeyValuePair<string, string>> acceptedValuesAndAliases)
		{
			IDictionary hash = CreateAcceptedValuesAndAliases(acceptedValuesAndAliases);
			AddFragment(new RequiredFragmentDescriptor(null, name, hash));
			return Me;
		}

		public TBuilder ToRequiredParameter(string name, IDictionary acceptedValuesAndAliases)
		{
			AddFragment(new RequiredFragmentDescriptor(null, name, acceptedValuesAndAliases));
			return Me;
		}

		public TBuilder ToOptionalParameter(string name, string defaultValue, IEnumerable<string> acceptedValues)
		{
			AddFragment(new OptionalFragmentDescriptor(null, name, defaultValue, acceptedValues));
			return Me;
		}

		public TBuilder ToOptionalParameter(string name, string defaultValue,
		                                    IEnumerable<KeyValuePair<string, string>> acceptedValuesAndAliases)
		{
			IDictionary hash = CreateAcceptedValuesAndAliases(acceptedValuesAndAliases);
			AddFragment(new OptionalFragmentDescriptor(null, name, defaultValue, hash));
			return Me;
		}

		public TBuilder ToOptionalParameter(string name, string defaultValue, IDictionary acceptedValuesAndAliases)
		{
			AddFragment(new OptionalFragmentDescriptor(null, name, defaultValue, acceptedValuesAndAliases));
			return Me;
		}

		public ExtraEntityRuleBuilder<TBuilder, TRootBuilder> WithExtraEntityRoute()
		{
			return new ExtraEntityRuleBuilder<TBuilder, TRootBuilder>(Me);
		}

		public TBuilder WithExtraEntityRouteTo(string action)
		{
			return WithExtraEntityRoute().ToAction(action).Register();
		}

		public ExtraListingRuleBuilder<TBuilder, TRootBuilder> WithExtraListingRoute()
		{
			return new ExtraListingRuleBuilder<TBuilder, TRootBuilder>(Me);
		}

		public TBuilder WithExtraListingRouteToAction(string action)
		{
			return WithExtraListingRoute().ToAction(action).Register();
		}

		public IdBuilder<TBuilder, TRootBuilder> UsingId()
		{
			return new IdBuilder<TBuilder, TRootBuilder>(Me);
		}

		public TBuilder UsingId(string idName)
		{
			return UsingId().Named(idName).Return();
		}

		protected virtual IDictionary CreateAcceptedValuesAndAliases(IEnumerable<KeyValuePair<string, string>> acceptedValuesAndAliases)
		{
			IDictionary hash = new HybridDictionary(true);
			if (acceptedValuesAndAliases != null)
			{
				acceptedValuesAndAliases.ForEach(pair => hash.Add(pair.Key, pair.Value));
			}
			return hash;
		}
	}
}