using System.Collections;
using System.Collections.Specialized;
using MvcContrib.Rest.Routing.Descriptors;
using MvcContrib.Rest.Routing.Ext;
using MvcContrib.Rest.Routing.Fragments;
using MvcContrib.Rest.Routing.Services;

namespace MvcContrib.Rest.Routing.Services
{
	public class RestfulFragmentConverter : IFragmentConverter
	{
		#region IFragmentConverter Members

		public IFragment ToFragment(IFragmentDescriptor descriptor)
		{
			IDictionary valuesAndAliases = new HybridDictionary();
			descriptor.AcceptedValuesAndAliases.ForEach(pair => valuesAndAliases.Add(pair.Key, pair.Value));

			switch (descriptor.Name)
			{
				case "controller":
					return new ControllerFragment(descriptor.RuleSet.Controllers);
				case "id":
					return new EntityIdFragment("id", descriptor.UrlPart);
				case "action":
					return new ActionFragment(descriptor.DefaultValue, valuesAndAliases, descriptor.IsOptional);
			}

			if (string.IsNullOrEmpty(descriptor.Name))
			{
				return new StaticFragment(descriptor.UrlPart);
			}

			if (descriptor.IsOptional)
			{
				return new OptionalPatternFragment(descriptor.Name, descriptor.DefaultValue, valuesAndAliases);
			}

			if (valuesAndAliases.Count > 0)
			{
				return new RequiredPatternFragment(descriptor.Name, valuesAndAliases);
			}
			return new RequiredPatternFragment(descriptor.Name, descriptor.UrlPart, 1);
		}

		#endregion
	}
}