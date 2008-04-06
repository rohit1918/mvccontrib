using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using MvcContrib.Rest.Routing.Ext;

namespace MvcContrib.Rest.Routing.Descriptors.Fragments
{
	public abstract class BaseFragmentDescriptor : IFragmentDescriptor
	{
		private readonly IDictionary _acceptedUrlPartsToValues = new HybridDictionary(true);
		private readonly IDictionary _acceptedValuesToParts = new HybridDictionary(true);

		protected BaseFragmentDescriptor(IRestfulRuleDescriptor rule)
		{
			SplitPrefix = SplitPrefix.Slash;
			Weight = 1;
			IsOptional = false;
			Rule = rule;
			RuleSet = rule != null ? rule.RuleSet : null;
		}

		#region IFragmentDescriptor Members

		public IRestfulRuleSetDescriptor RuleSet { get; set; }

		public IRestfulRuleDescriptor Rule { get; set; }

		public virtual int Weight { get; set; }

		public virtual string UrlPart { get; set; }

		public virtual string Name { get; set; }

		public virtual string DefaultValue { get; set; }

		public virtual bool IsOptional { get; set; }

		public virtual SplitPrefix SplitPrefix { get; set; }

		public IEnumerable<KeyValuePair<string, string>> AcceptedValuesAndAliases
		{
			get
			{
				return
					from entry in _acceptedValuesToParts.Cast<DictionaryEntry>()
					select new KeyValuePair<string, string>(entry.Key.ToString(), entry.Value.ToString());
			}
		}

		#endregion

		protected void AddAcceptedValues(IDictionary valuesAndAliases)
		{
			if (valuesAndAliases == null || valuesAndAliases.Count == 0)
			{
				return;
			}
			valuesAndAliases.Cast<DictionaryEntry>().ForEach(
				entry => AddAcceptedValue(entry.Key.ToString(), entry.Value.ToString()));
		}

		protected void AddAcceptedValues(IEnumerable<string> values)
		{
			if (values == null)
			{
				return;
			}
			values.ForEach(value => AddAcceptedValue(value));
		}

		protected void AddAcceptedValue(string value)
		{
			AddAcceptedValue(value, value);
		}

		protected void AddAcceptedValue(string value, string urlAlias)
		{
			_acceptedValuesToParts[value] = urlAlias;
			_acceptedUrlPartsToValues[urlAlias] = value;
		}
	}
}