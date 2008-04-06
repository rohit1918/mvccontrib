using System.Text;
using System.Web.Routing;

namespace MvcContrib.Rest.Routing.Fragments
{
	/// <summary>A fragment or part of a <see cref="RestfulRoutingRule"/>.</summary>
	public interface IFragment
	{
		/// <summary>The path fragment</summary>
		string PathFragment { get; }

		/// <summary>A weight associated with the given fragment.</summary>
		int Weight { get; }

		/// <summary>The name of the fragment.</summary>
		string Name { get; }

		/// <summary>A default value.</summary>
		string DefaultValue { get; }

		/// <summary><see langword="true" /> if the fragment is not required for the url.</summary>
		bool IsOptional { get; }

		/// <summary>Checks if the given <paramref name="urlPart"/> matches for this fragment.</summary>
		/// <param name="urlPart">The piece of the url to match against.</param>
		/// <param name="routeData">The current <see cref="RouteData"/> for the request.</param>
		/// <returns><see langword="true" /> if the <paramref name="urlPart"/> matches, otherwise false.</returns>
		bool Matches(string urlPart, RouteData routeData);

		/// <summary>Tries to build up a virtual path and url.</summary>
		/// <param name="virtualPath">The virtual path that is being built up.</param>
		/// <param name="requestContext">The current request context.</param>
		/// <param name="routeData">The current route data.</param>
		/// <param name="virtualPathData">The current virtual path data for the url.</param>
		/// <returns><see langword="true" /> if it can continue to build the url, otherwise <see langword="false" /></returns>
		bool TryBuildUrl(StringBuilder virtualPath, RequestContext requestContext, RouteValueDictionary routeData,
		                 VirtualPathData virtualPathData);
	}
}