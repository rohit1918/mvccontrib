using System.Collections;
using System.Text;
using System.Web.Routing;
using MvcContrib.Rest.Routing.Fragments;

namespace MvcContrib.Rest.Routing.Fragments
{
	public class FragmentDecorator : IFragment
	{
		protected IFragment Inner { get; set; }

		#region IFragment Members

		public string PathFragment
		{
			get { return Inner.PathFragment; }
		}

		public int Weight
		{
			get { return Inner.Weight; }
		}

		public string Name
		{
			get { return Inner.Name; }
		}

		public string DefaultValue
		{
			get { return Inner.DefaultValue; }
		}

		public bool IsOptional
		{
			get { return Inner.IsOptional; }
		}

		public bool Matches(string urlPart, RouteData routeData)
		{
			return Inner.Matches(urlPart, routeData);
		}

		public virtual bool TryBuildUrl(StringBuilder virtualPath, RequestContext requestContext, RouteValueDictionary routeData, VirtualPathData virtualPathData)
		{
			return Inner.TryBuildUrl(virtualPath, requestContext, routeData, virtualPathData);
		}

		#endregion
	}
}