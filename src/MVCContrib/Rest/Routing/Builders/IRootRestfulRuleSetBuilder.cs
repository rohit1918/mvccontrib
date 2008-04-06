using System;
using System.Collections.Generic;
using MvcContrib.Rest.Routing.Descriptors;
using MvcContrib.Rest.Routing.Descriptors.Fragments;

namespace MvcContrib.Rest.Routing.Builders
{
	/// <summary>Interface for the Root rule set builder.</summary>
	/// <typeparam name="TBuilder">The real type of the root.</typeparam>
	/// <remarks>Hackish, but implement interface explicitly to hide methods
	/// from the public intellisense view of the real root builder.</remarks>
	public interface IRootRestfulRuleSetBuilder<TBuilder> : IRestfulRuleSetBuilder<TBuilder, TBuilder>
		where TBuilder : IRootRestfulRuleSetBuilder<TBuilder>
	{
		IRestfulRuleContainer Container { get; }
		IRuleFragmentDescriptor ListingRule { get; }
		IRuleFragmentDescriptor EntityRule { get; }
		RequiredFragmentDescriptor EntityIdFragment { get; }
		List<IRuleFragmentDescriptor> ExtraEntityRules { get; }
		List<IRuleFragmentDescriptor> ExtraListingRules { get; }
		IEnumerable<ControllerRoutingDescriptor> Controllers { get; }
		void AddFragment(IFragmentDescriptor fragment);
		void RegisterExtraEntityRule(IRuleFragmentDescriptor ruleFragment);
		void RegisterExtraListingRule(IRuleFragmentDescriptor ruleFragment);
		void CloseListing();
		void CloseEntity();
		void AddController(Type controller);
	}
}