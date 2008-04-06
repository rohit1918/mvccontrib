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
	public class RequiredPatternFragment : BasePatternFragment
	{
		private readonly IDictionary _acceptedUrlPartsToValues = new HybridDictionary(true);
		private readonly IDictionary _acceptedValuesToParts = new HybridDictionary(true);

		protected RequiredPatternFragment() {}

		public RequiredPatternFragment(string name, IDictionary acceptedValuesAndAliases)
			: this(
				name, new RegexPatternBuilder().BuildPattern(acceptedValuesAndAliases.Values.Cast<string>()),
				CalculateWeight(acceptedValuesAndAliases.Values.Cast<string>()))
		{
			acceptedValuesAndAliases.Cast<DictionaryEntry>()
				.ForEach(entry =>
				{
					_acceptedValuesToParts.Add(entry.Key, entry.Value);
					_acceptedUrlPartsToValues.Add(entry.Value, entry.Key);
				});
		}

		public RequiredPatternFragment(string name, string pattern, int weight)
		{
			Name = name;
			PatternBuilder = () => new RegexPatternBuilder().BuildRequiredNamedCapturingPattern(name, pattern);
			Weight = weight;
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

		protected static int CalculateWeight(IEnumerable<string> acceptedValues)
		{
			return acceptedValues == null || acceptedValues.Count() == 0 ? 100 : 1;
		}

		public static RequiredPatternFragment ForInt32(string name)
		{
			return new RequiredPatternFragment(name, RegexPatternBuilder.RequiredPositiveInt32Pattern, 1);
		}

		public static RequiredPatternFragment ForInt64(string name)
		{
			return new RequiredPatternFragment(name, RegexPatternBuilder.RequiredPositiveInt64Pattern, 1);
		}

		public static RequiredPatternFragment ForGuid(string name)
		{
			return new RequiredPatternFragment(name, RegexPatternBuilder.RequiredGuidPattern, 1);
		}
	}
}