using System.Web.Mvc;

namespace MVCContrib.SimplyRestful
{
	public interface IRestfulActionResolver
	{
		RestfulAction ResolveAction(RequestContext context);
	}
}