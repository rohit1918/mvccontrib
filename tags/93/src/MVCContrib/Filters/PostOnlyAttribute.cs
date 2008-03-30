using System;
using System.Web.Mvc;
using MvcContrib.MetaData;

namespace MvcContrib.Filters
{
	public class PostOnlyAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(FilterExecutingContext filterContext)
		{
			if (filterContext.HttpContext.Request.RequestType != "POST")
				throw new InvalidOperationException(
					string.Format("Action '{0}' can only be accessed using an HTTP Post.", filterContext.ActionMethod.Name));
		}
	}
}
