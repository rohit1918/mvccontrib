using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Routing;
using MvcContrib.Rest.Routing.Fragments;

namespace MvcContrib.Rest.Routing.Fragments
{
	public abstract class BasePatternFragment : IFragment
	{
		private string _pathFragment;
		private Regex _regex;


		protected BasePatternFragment()
		{
			Weight = 1;
			IsOptional = false;
		}

		protected virtual Regex Regex
		{
			get { return _regex ?? (_regex = new Regex("^" + PathFragment + "$", RegexOptions.Compiled)); }
			set { _regex = value; }
		}

		protected virtual Func<string> PatternBuilder { get; set; }

		protected bool HasDefaultValue
		{
			get { return !string.IsNullOrEmpty(DefaultValue); }
		}

		#region IFragment Members

		public virtual bool IsOptional { get; set; }

		public virtual string PathFragment
		{
			get { return _pathFragment ?? (_pathFragment = PatternBuilder()); }
			set { _pathFragment = value; }
		}

		public virtual string Name { get; protected set; }

		public virtual string DefaultValue { get; protected set; }

		public virtual bool Matches(string urlPart, RouteData routeData)
		{
			if (urlPart == null)
			{
				if (IsOptional)
				{
					if (HasDefaultValue)
					{
						routeData.Values[Name] = DefaultValue;
					}
					return true;
				}
				return false;
			}

			Match match = Regex.Match(urlPart);
			if (!match.Success)
			{
				return false;
			}
			routeData.Values[Name] = match.Groups[Name].Value;
			return true;
		}


		public virtual bool TryBuildUrl(StringBuilder virtualPath, RequestContext requestContext, RouteValueDictionary routeData, VirtualPathData virtualPathData)
		{
			if (!routeData.ContainsKey(Name))
			{
				return IsOptional;
			}

			string value = Convert.ToString(routeData[Name]);
			if (Regex.IsMatch(value))
			{
				if (!HasDefaultValue || !value.Equals(DefaultValue, StringComparison.OrdinalIgnoreCase))
				{
					virtualPath.Append('/').Append(value);
				}
				return true;
			}

			return false;
		}

		public virtual int Weight { get; protected set; }

		#endregion
	}
}