using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcContrib.Rest.Routing.Descriptors.Rules
{
	public abstract class BaseRestfulRuleDescriptor : IRestfulRuleDescriptor
	{
		private readonly List<IFragmentDescriptor> _fragments = new List<IFragmentDescriptor>();
		private readonly List<CustomRouteMatcher> _routeMatchers = new List<CustomRouteMatcher>();

		protected BaseRestfulRuleDescriptor(IRestfulRuleSetDescriptor ruleSet)
		{
			RuleSet = ruleSet;
		}

		protected List<IFragmentDescriptor> ListOfFragments
		{
			get { return _fragments; }
		}

		protected List<CustomRouteMatcher> ListOfRouteMatchers
		{
			get { return _routeMatchers; }
		}

		#region IRestfulRuleDescriptor Members

		public IRestfulRuleSetDescriptor RuleSet { get; private set; }

		public virtual IEnumerable<CustomRouteMatcher> CustomRouteMatchers
		{
			get { return _routeMatchers.AsEnumerable(); }
		}

		public IEnumerable<IFragmentDescriptor> PrefixFragments { get; set; }

		public virtual IEnumerable<IFragmentDescriptor> Fragments
		{
			get
			{
				foreach(IFragmentDescriptor fragment in PrefixFragments)
				{
					yield return fragment;
				}
				foreach(IFragmentDescriptor fragment in ListOfFragments)
				{
					yield return fragment;
				}
			}
		}

		public virtual int Weight
		{
			get { return (from fragment in Fragments select fragment.Weight).Sum(); }
		}

		public virtual int Levels
		{
			get { return Fragments.Count(); }
		}

		public IRestfulRuleDescriptor AddFragment(IFragmentDescriptor fragment)
		{
			fragment.Rule = this;
			fragment.RuleSet = RuleSet;
			ListOfFragments.Add(fragment);
			return this;
		}

		#endregion

		#region Custom Route Matchers

		public static IEnumerable<CustomRouteMatcher> MutableEntityActionRouteMatchers
		{
			get
			{
				CustomRouteMatcher routeMatcher = (rule, httpContext, routeData) =>
				                                  	{
				                                  		switch(httpContext.Request.HttpMethod.ToUpperInvariant())
				                                  		{
				                                  			case "PUT":
				                                  				routeData.Values["action"] = "update";
				                                  				return true;
				                                  			case "DELETE":
				                                  				routeData.Values["action"] = "destroy";
				                                  				return true;
				                                  			case "POST":
				                                  				string method = httpContext.Request.Form["_method"];
				                                  				if(string.IsNullOrEmpty(method))
				                                  				{
				                                  					return false;
				                                  				}
				                                  				switch(method.ToUpperInvariant())
				                                  				{
				                                  					case "PUT":
				                                  						routeData.Values["action"] = "update";
				                                  						return true;
				                                  					case "DELETE":
				                                  						routeData.Values["action"] = "destroy";
				                                  						return true;
				                                  				}
				                                  				return false;
				                                  		}
				                                  		return false;
				                                  	};
				yield return routeMatcher;
			}
		}

		public static IEnumerable<CustomRouteMatcher> HttpGetOnlyRouteMatchers
		{
			get
			{
				yield return
					(rule, httpContext, routeData) => httpContext.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase);
			}
		}

		public static IEnumerable<CustomRouteMatcher> CreateNewEntityMatchers
		{
			get
			{
				yield return (rule, httpContext, routeData) =>
				             	{
				             		if(httpContext.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
				             		{
				             			routeData.Values["action"] = "create";
				             			return true;
				             		}
				             		return false;
				             	};
			}
		}

		#endregion
	}
}