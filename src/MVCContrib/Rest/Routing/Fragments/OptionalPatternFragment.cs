using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Routing;
using MvcContrib.Rest;
using MvcContrib.Rest.Routing.Ext;
using MvcContrib.Rest.Routing.Fragments;

namespace MvcContrib.Rest.Routing.Fragments
{
	public class OptionalPatternFragment : BasePatternFragment
	{
		private readonly IDictionary _acceptedUrlPartsToValues = new HybridDictionary(true);
		private readonly IDictionary _acceptedValuesToParts = new HybridDictionary(true);

		protected OptionalPatternFragment()
		{
			IsOptional = true;
		}

		public OptionalPatternFragment(string name, string defaultValue, IDictionary acceptedValuesAndAliases)
			: this(
				name, defaultValue, new RegexPatternBuilder().BuildPattern(acceptedValuesAndAliases.Values.Cast<string>()),
				CalculateWeight(defaultValue, acceptedValuesAndAliases.Values.Cast<string>()))
		{
			acceptedValuesAndAliases.Cast<DictionaryEntry>()
				.ForEach(entry =>
				{
					_acceptedValuesToParts.Add(entry.Key, entry.Value);
					_acceptedUrlPartsToValues.Add(entry.Value, entry.Key);
				});
		}

		public OptionalPatternFragment(string name, string defaultValue, string pattern, int weight)
		{
			Name = name;
			PatternBuilder = () => new RegexPatternBuilder().BuildOptionalNamedCapturingPattern(name, pattern);
			Weight = weight;
			DefaultValue = defaultValue;
			IsOptional = true;
		}

		private static int CalculateWeight(string defaultValue, IEnumerable<string> acceptedValues)
		{
			return CalculateWeight(defaultValue) + (acceptedValues == null || acceptedValues.Count() == 0 ? 100 : 1);
		}

		private static int CalculateWeight(string defaultValue)
		{
			return string.IsNullOrEmpty(defaultValue) ? 100 : 1;
		}

		public override bool Matches(string urlPart, RouteData routeData)
		{
			if (base.Matches(urlPart, routeData))
			{
				object captured;
				if (routeData.Values.TryGetValue(Name, out captured))
				{
					if (_acceptedUrlPartsToValues.Contains(captured))
					{
						routeData.Values[Name] = _acceptedUrlPartsToValues[captured].ToString();
					}
				}
				return true;
			}
			return false;
		}

		public override bool TryBuildUrl(StringBuilder virtualPath, RequestContext requestContext, RouteValueDictionary routeData, VirtualPathData virtualPathData)
		{
			if (routeData.ContainsKey(Name))
			{
				string value = Convert.ToString(routeData[Name]);
				if (_acceptedValuesToParts.Contains(value))
				{
					routeData[Name] = _acceptedValuesToParts[value];
				}
			}
			return base.TryBuildUrl(virtualPath, requestContext, routeData, virtualPathData);
		}

		public static OptionalPatternFragment ForInt32(string name, int? defaultValue)
		{
			return new OptionalPatternFragment(name, defaultValue.ToString(), RegexPatternBuilder.RequiredPositiveInt32Pattern,
			                                   1 + CalculateWeight(defaultValue.ToString()));
		}

		public static OptionalPatternFragment ForInt64(string name, long? defaultValue)
		{
			return new OptionalPatternFragment(name, defaultValue.ToString(), RegexPatternBuilder.RequiredPositiveInt64Pattern,
			                                   1 + CalculateWeight(defaultValue.ToString()));
		}

		public static OptionalPatternFragment ForGuid(string name, Guid? defaultValue)
		{
			return new OptionalPatternFragment(name, defaultValue.HasValue ? defaultValue.Value.ToString("N") : null,
			                                   RegexPatternBuilder.RequiredGuidPattern,
			                                   1 +
			                                   CalculateWeight(defaultValue.HasValue ? defaultValue.Value.ToString("N") : null));
		}
	}
}