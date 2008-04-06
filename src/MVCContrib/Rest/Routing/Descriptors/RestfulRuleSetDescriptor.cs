using System;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.Rest.Routing.Descriptors.Fragments;
using MvcContrib.Rest.Routing.Ext;

namespace MvcContrib.Rest.Routing.Descriptors
{
	public class RestfulRuleSetDescriptor : IRestfulRuleSetDescriptor
	{
		private readonly List<ControllerRoutingDescriptor> _controllers = new List<ControllerRoutingDescriptor>();
		private readonly IRuleFragmentDescriptor _entityFragment = new RuleFragmentDescriptor();

		private readonly RequiredFragmentDescriptor _entityIdFragment = new RequiredFragmentDescriptor(null, "id",
		                                                                                               RegexPatternBuilder.
		                                                                                               	RequiredPositiveInt32Pattern);

		private readonly List<IRuleFragmentDescriptor> _extraEntityRules = new List<IRuleFragmentDescriptor>();
		private readonly List<IRuleFragmentDescriptor> _extraListingRules = new List<IRuleFragmentDescriptor>();
		private readonly IRuleFragmentDescriptor _listingFragment = new RuleFragmentDescriptor();
		private readonly IRuleFragmentDescriptor _remoteFragment = new RuleFragmentDescriptor();
		private readonly List<IRestfulRuleDescriptor> _rules = new List<IRestfulRuleDescriptor>();

		private IList<IRestfulRuleDescriptor> ListOfRules
		{
			get { return _rules; }
		}

		#region IRestfulRuleSetDescriptor Members

		public IEnumerable<ControllerRoutingDescriptor> Controllers
		{
			get { return _controllers.AsEnumerable(); }
		}

		public IRuleFragmentDescriptor RemoteFragment
		{
			get { return _remoteFragment; }
		}

		public IRuleFragmentDescriptor EntityFragment
		{
			get { return _entityFragment; }
		}

		public IRuleFragmentDescriptor ListingFragment
		{
			get { return _listingFragment; }
		}

		public IEnumerable<IRestfulRuleDescriptor> Rules
		{
			get { return ListOfRules.AsEnumerable(); }
		}

		public RequiredFragmentDescriptor EntityIdFragment
		{
			get { return _entityIdFragment; }
		}

		public List<IRuleFragmentDescriptor> ExtraEntityRules
		{
			get { return _extraEntityRules; }
		}

		public List<IRuleFragmentDescriptor> ExtraListingRules
		{
			get { return _extraListingRules; }
		}

		public string Scope
		{
			get { return _remoteFragment.Fragments.Concat(fragment => fragment.SplitPrefix + fragment.UrlPart); }
		}

		public void AddRule(IRestfulRuleDescriptor rule)
		{
			rule.PrefixFragments = _remoteFragment.Fragments;
			ListOfRules.Add(rule);
		}

		public void AddController(Type controller)
		{
			if(!_controllers.Any(descriptor => descriptor.ControllerType == controller))
			{
				_controllers.Add(new ControllerRoutingDescriptor(controller));
			}
		}

		#endregion

		public void AddControllers(IEnumerable<ControllerRoutingDescriptor> controllers)
		{
			_controllers.AddRange(controllers);
		}

		public void AddPrefixFragments(IEnumerable<IFragmentDescriptor> fragments)
		{
			foreach(IFragmentDescriptor fragment in fragments)
			{
				fragment.RuleSet = this;
				_remoteFragment.AddFragment(fragment);
			}
		}
	}
}