using System;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.Rest.Routing;
using MvcContrib.Rest.Routing.Descriptors;
using MvcContrib.Rest.Routing.Descriptors.Fragments;
using MvcContrib.Rest.Routing.Descriptors.Rules;
using MvcContrib.Rest.Routing.Ext;

namespace MvcContrib.Rest.Routing.Builders
{
	public class RestfulRuleSetBuilder : BaseRuleSetBuilder<RestfulRuleSetBuilder, RestfulRuleSetBuilder>,
	                                     IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder>
	{
		private readonly IRestfulRuleContainer _container;
		private readonly IRestfulRuleSetDescriptor _ruleSet;
		private bool _controllerClosed;
		private bool _entityClosed;
		private bool _listingClosed;


		public RestfulRuleSetBuilder(IRestfulRuleContainer container) : this(container, new RestfulRuleSetDescriptor())
		{
		}

		public RestfulRuleSetBuilder(IRestfulRuleContainer container, IRestfulRuleSetDescriptor existingRuleSet)
		{
			_container = container;
			Root = this;
			_ruleSet = existingRuleSet;
		}

		protected IRestfulRuleSetDescriptor RuleSet
		{
			get { return _ruleSet; }
		}

		protected override RestfulRuleSetBuilder Me
		{
			get { return this; }
		}

		#region IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder> Members

		IRestfulRuleContainer IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder>.Container
		{
			get { return _container; }
		}

		IRuleFragmentDescriptor IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder>.ListingRule
		{
			get { return _ruleSet.ListingFragment; }
		}

		IRuleFragmentDescriptor IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder>.EntityRule
		{
			get { return _ruleSet.EntityFragment; }
		}

		RequiredFragmentDescriptor IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder>.EntityIdFragment
		{
			get { return _ruleSet.EntityIdFragment; }
		}

		List<IRuleFragmentDescriptor> IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder>.ExtraEntityRules
		{
			get { return _ruleSet.ExtraEntityRules; }
		}

		List<IRuleFragmentDescriptor> IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder>.ExtraListingRules
		{
			get { return _ruleSet.ExtraListingRules; }
		}

		void IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder>.AddFragment(IFragmentDescriptor fragment)
		{
			AddFragment(fragment);
		}

		IEnumerable<ControllerRoutingDescriptor> IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder>.Controllers
		{
			get { return _ruleSet.Controllers; }
		}

		IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder>
			IRestfulRuleSetBuilder<RestfulRuleSetBuilder, RestfulRuleSetBuilder>.RootBuilder
		{
			get { return this; }
		}

		public override void Register()
		{
			_container.Register(CreateRuleSet());
		}

		void IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder>.RegisterExtraEntityRule(IRuleFragmentDescriptor ruleFragment)
		{
			if(ruleFragment != null)
			{
				RuleSet.ExtraEntityRules.Add(ruleFragment);
			}
		}

		void IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder>.RegisterExtraListingRule(IRuleFragmentDescriptor ruleFragment)
		{
			if(ruleFragment != null)
			{
				RuleSet.ExtraListingRules.Add(ruleFragment);
			}
		}

		void IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder>.CloseListing()
		{
			BuildListingFragment();
		}

		void IRootRestfulRuleSetBuilder<RestfulRuleSetBuilder>.CloseEntity()
		{
			BuildEntityFragment();
		}

		public void AddController(Type controller)
		{
			if(!_ruleSet.Controllers.Any(descriptor => descriptor.ControllerType == controller))
			{
				_ruleSet.AddController(controller);
			}
		}

		#endregion

		protected override void AddFragment(IFragmentDescriptor fragment)
		{
			if(_listingClosed && _entityClosed)
			{
				throw new InvalidOperationException(
					"Unable to add fragment to rule set because both the listing and entity fragments are closed.");
			}

			if(_listingClosed)
			{
				_ruleSet.EntityFragment.AddFragment(fragment);
			}
			else
			{
				_ruleSet.ListingFragment.AddFragment(fragment);
			}
		}

		protected IRestfulRuleSetDescriptor CreateRuleSet()
		{
			IRestfulRuleSetDescriptor ruleSet = CreateRuleSetDescriptor();
			AddBaseRestfulRules(ruleSet);
			return ruleSet;
		}

		protected void AddBaseRestfulRules(IRestfulRuleSetDescriptor ruleSet)
		{
			ruleSet.AddRule(new RestfulEntityRuleDescriptor(ruleSet, BaseRestfulRuleDescriptor.MutableEntityActionRouteMatchers));

			ruleSet.AddRule(new RestfulEntityRuleDescriptor(ruleSet, BaseRestfulRuleDescriptor.HttpGetOnlyRouteMatchers)
			                	.AddFragment(new OptionalFragmentDescriptor(null, "action", "show", new[] {"show", "edit", "delete"})));

			ruleSet.AddRule(new RestfulListingRuleDescriptor(ruleSet, BaseRestfulRuleDescriptor.HttpGetOnlyRouteMatchers)
			                	.AddFragment(new OptionalFragmentDescriptor(null, "action", "index", new[] {"index", "new"})));

			ruleSet.AddRule(new RestfulListingRuleDescriptor(ruleSet, BaseRestfulRuleDescriptor.CreateNewEntityMatchers));
		}

		private IRestfulRuleSetDescriptor CreateRuleSetDescriptor()
		{
			var ruleSet = (RestfulRuleSetDescriptor)_ruleSet;
			BuildListingFragment();
			BuildEntityFragment();
			AddExtraRules(RuleSet);
			return ruleSet;
		}

		private void AddExtraRules(IRestfulRuleSetDescriptor ruleSet)
		{
			ruleSet.ExtraEntityRules.ForEach(
				ruleFragment => ruleSet.AddRule(new ExtraEntityRuleDescriptor(ruleSet, ruleFragment)));
			ruleSet.ExtraListingRules.ForEach(
				ruleFragment => ruleSet.AddRule(new ExtraListingRuleDescriptor(ruleSet, ruleFragment)));
		}

		private void BuildEntityFragment()
		{
			BuildListingFragment();
			if (!_entityClosed)
			{
				RuleSet.ListingFragment.Fragments.ForEach(fragment => RuleSet.EntityFragment.AddFragment(fragment));
				RuleSet.EntityFragment.AddFragment(RuleSet.EntityIdFragment);
			}
			_entityClosed = true;
		}

		private void BuildListingFragment()
		{
			BuildControllerFragment();
			_listingClosed = true;
		}

		private void BuildControllerFragment()
		{
			if(!_controllerClosed)
				RuleSet.ListingFragment.AddFragment(new ControllerFragmentDescriptor(null));
			_controllerClosed = true;
		}

		public RestfulRuleSetBuilder FromPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return this;
			}

			if (path.Contains("."))
			{
				throw new ArgumentException("path cannot contain any periods.", "path");
			}

			path = path.Trim().Trim('/');

			string[] pathParts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			pathParts.ForEach(pathPart => RuleSet.RemoteFragment.AddFragment(new StaticFragmentDescriptor(null, pathPart)));
			return this;
		}

		public RestfulRuleSetBuilderWithClosedController ToController()
		{
			BuildControllerFragment();
			return new RestfulRuleSetBuilderWithClosedController(this);
		}

		public RestfulRuleSetBuilderWithClosedListing<RestfulRuleSetBuilder, RestfulRuleSetBuilder> ToListing()
		{
			BuildListingFragment();
			return new RestfulRuleSetBuilderWithClosedListing<RestfulRuleSetBuilder, RestfulRuleSetBuilder>(this);
		}

		public RestfulRuleSetBuilderWithClosedEntity<RestfulRuleSetBuilder, RestfulRuleSetBuilder> ToEntity()
		{
			BuildEntityFragment();
			return new RestfulRuleSetBuilderWithClosedEntity<RestfulRuleSetBuilder, RestfulRuleSetBuilder>(this);
		}

		public RestfulRuleSetBuilder FromRestfulParent<TParent>()
		{
			return FromRestfulParent(typeof(TParent));
		}

		public RestfulRuleSetBuilder FromRestfulParent(Type parentControllerType)
		{
			IRestfulRuleSetDescriptor ruleSet = _container.GetOrCreateRuleSetFor(parentControllerType);
			ControllerRoutingDescriptor parentDescriptor = null;
			foreach(ControllerRoutingDescriptor descriptor in ruleSet.Controllers)
			{
				if(descriptor.ControllerType == parentControllerType)
				{
					parentDescriptor = descriptor;
				}
			}
			var parentFragment = new RestfulParentRouteRuleFragmentDescriptor(parentDescriptor, ruleSet);
			RuleSet.RemoteFragment.AddNestedRule(parentFragment);
			return this;
		}
	}
}