using System.Collections;
using System.Text;
using System.Web.Routing;
using MvcContrib.Rest.Routing.Fragments;

namespace MvcContrib.Rest.Routing.Fragments
{
	public class StaticFragment : IFragment
	{
		public StaticFragment(string pathFragment)
		{
			PathFragment = pathFragment;
		}

		#region IFragment Members

		public string PathFragment { get; private set; }

		public int Weight
		{
			get { return 0; }
		}

		public string Name
		{
			get { return null; }
		}

		public string DefaultValue
		{
			get { return null; }
		}

		public bool IsOptional
		{
			get { return false; }
		}

		public bool Matches(string urlPart, RouteData routeData)
		{
			return urlPart == PathFragment;
		}

		public bool TryBuildUrl(StringBuilder virtualPath, RequestContext requestContext, RouteValueDictionary routeData,
		                        VirtualPathData virtualPathData)
		{
			virtualPath.Append('/').Append(PathFragment);
			return true;
		}

		#endregion
	}
}