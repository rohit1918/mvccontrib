using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using MvcContrib.Rest.Routing.Ext;

namespace MvcContrib.Rest.Routing.Descriptors.Fragments
{
	public class RestfulParentRouteRuleFragmentDescriptor : IRuleFragmentDescriptor
	{
		private readonly ControllerRoutingDescriptor _parentController;
		private readonly IRestfulRuleSetDescriptor _parentRuleSet;

		public RestfulParentRouteRuleFragmentDescriptor(ControllerRoutingDescriptor parentController,
																										IRestfulRuleSetDescriptor parentRuleSet)
		{
			_parentRuleSet = parentRuleSet;
			_parentController = parentController;
		}

		#region IRuleFragmentDescriptor Members

		public void AddFragment(IFragmentDescriptor fragment)
		{
			throw new NotImplementedException();
		}

		public void AddNestedRule(IRuleFragmentDescriptor rule)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IFragmentDescriptor> Fragments
		{
			get
			{
				foreach (IFragmentDescriptor fragment in _parentRuleSet.EntityFragment.Fragments)
				{
					if (fragment == _parentRuleSet.EntityIdFragment)
					{
						var existingIdFragment = (RequiredFragmentDescriptor)fragment;
						if (existingIdFragment.AcceptedValuesAndAliases.Count() == 0)
						{
							yield return
								new RequiredFragmentDescriptor(null, _parentController.EntityName + "Id",
																							 existingIdFragment.UrlPart.Replace("<" + existingIdFragment.Name + ">",
																																									"<" + _parentController.EntityName +
																																									"Id>"));
						}
						else
						{
							IDictionary newValuesAndAliases = new HybridDictionary(true);
							existingIdFragment.AcceptedValuesAndAliases.ForEach(pair => newValuesAndAliases.Add(pair.Key, pair.Value));
							yield return
								new RequiredFragmentDescriptor(null, _parentController.EntityName + "Id", newValuesAndAliases);
						}
					}
					else if (!string.IsNullOrEmpty(fragment.Name) && fragment.Name.Equals("controller"))
					{
						yield return new StaticFragmentDescriptor(null, _parentController.Name.ToSeoFriendlyUrlFragment());
					}
					else
					{
						yield return fragment;
					}
				}
			}
		}
		#endregion
	}
}