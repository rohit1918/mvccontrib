using System;
using System.Collections.Generic;
using MvcContrib.Rest.Routing.Descriptors;

namespace MvcContrib.Rest.Routing
{
	public interface IRestfulRuleContainer
	{
		IEnumerable<IRestfulRuleSetDescriptor> RuleSets { get; }

		void Register(IRestfulRuleSetDescriptor ruleSet);

		void Register(IEnumerable<IRestfulRuleSetDescriptor> ruleSets);

		bool IsControllerRegisteredInDifferentScope(string controllerName, string scope);

		IRestfulRuleSetDescriptor GetOrCreateRuleSetFor(Type controllerType);
	}
}