using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Rest.Routing.Ext;
using MvcContrib.Rest.Routing.Fragments;

namespace MvcContrib.Rest.Routing
{
	/// <summary>A route matching predicate that can also take action and modify the <paramref name="routeMatch"/></summary>
	/// <param name="rule">The <see cref="RestfulRoutingRule"/> that is calling the custom matcher.</param>
	/// <param name="routeContext">The <see cref="HttpContextBase"/></param>
	/// <param name="routeMatch">The <see cref="RouteData"/></param>
	/// <returns><c>true</c> if the rule is considered a match, otherwise <c>false</c> to stop matching and attempt the next rule in the container.</returns>
	public delegate bool CustomRouteMatcher(RestfulRoutingRule rule, HttpContextBase routeContext, RouteData routeMatch);

	/// <summary>Pendant</summary>
	/// <param name="rule"></param>
	/// <param name="url"></param>
	/// <param name="parameters"></param>
	/// <returns></returns>
	public delegate bool CustomUrlBuilder(RestfulRoutingRule rule, StringBuilder url, IDictionary parameters);

	/// <summary>A transformer to convert a parameter value to a url part when creating a url.</summary>
	/// <param name="rule"></param>
	/// <param name="fragment"></param>
	/// <param name="name"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public delegate string ParameterToUrlPartTransformer(
		RestfulRoutingRule rule, IFragment fragment, string name, string value);

	/// <summary>A transformer to convert a url part to a <see cref="RouteData"/> parameter when matching a url.</summary>
	/// <param name="rule"></param>
	/// <param name="fragment"></param>
	/// <param name="name"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public delegate string UrlPartToParameterTransformer(
		RestfulRoutingRule rule, IFragment fragment, string name, string value);

	public class RestfulRoutingRule : RouteBase
	{
		private static readonly char[] PathSplitChar = { '/', '.' };
		private readonly List<CustomRouteMatcher> _customRouteMatchers = new List<CustomRouteMatcher>();
		private readonly List<CustomUrlBuilder> _customUrlBuilders = new List<CustomUrlBuilder>();
		private readonly List<IFragment> _fragments = new List<IFragment>();


		public string RouteName { get; set; }

		public IRestfulRuleContainer Container { get; set; }

		public string Scope { get; set; }

		public void AddFragment(IFragment fragment)
		{
			_fragments.Add(fragment);
		}

		public void AddCustomUrlBuilder(CustomUrlBuilder builder)
		{
			_customUrlBuilders.Add(builder);
		}

		public void AddCustomRouteMatcher(CustomRouteMatcher matcher)
		{
			_customRouteMatchers.Add(matcher);
		}

		public override RouteData GetRouteData(HttpContextBase httpContext)
		{
			RouteData routeData = new RouteData(this, new MvcRouteHandler());

			string[] urlParts = httpContext.Request.Url.AbsolutePath.Split(PathSplitChar, StringSplitOptions.RemoveEmptyEntries);
			if (_fragments.DoWhileWithIndex((fragment, index) => fragment.Matches(urlParts.AtOrDefault(index), routeData)))
			{
				if (_customRouteMatchers.DoWhile(customMatcher => customMatcher(this, httpContext, routeData)))
				{
					return routeData;
				}
			}
			return null;
		}

		public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
		{
			VirtualPathData virtualPathData = new VirtualPathData(this, "");
			var url = new StringBuilder();

			if (_fragments.DoWhile(fragment => fragment.TryBuildUrl(url, requestContext, values, virtualPathData)))
			{
				//return _customUrlBuilders.DoWhile(customBuilder => customBuilder(this, url, values)) ? url.ToString() : null;
				virtualPathData.VirtualPath = url.ToString().Substring(1);
				return virtualPathData;
			}
			return null;
		}
	}
}