using System;
using System.Collections.Generic;
using MvcContrib.Rest.Routing.Descriptors.Fragments;

namespace MvcContrib.Rest.Routing.Descriptors
{
	public interface IRestfulRuleSetDescriptor
	{
		IEnumerable<ControllerRoutingDescriptor> Controllers { get; }

		IRuleFragmentDescriptor RemoteFragment { get; }

		IRuleFragmentDescriptor EntityFragment { get; }

		IRuleFragmentDescriptor ListingFragment { get; }

		IEnumerable<IRestfulRuleDescriptor> Rules { get; }

		RequiredFragmentDescriptor EntityIdFragment { get; }

		List<IRuleFragmentDescriptor> ExtraEntityRules { get; }

		List<IRuleFragmentDescriptor> ExtraListingRules { get; }

		string Scope { get; }

		void AddRule(IRestfulRuleDescriptor descriptor);

		void AddController(Type controller);
	}
}