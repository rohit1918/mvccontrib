using System;
using System.Web.Mvc;
using MvcContrib.MetaData;

namespace MvcContrib.Filters
{
	public class PostOnlyAttribute : FilterAttribute
	{
		public PostOnlyAttribute() : base(typeof(PostOnlyFilterImpl))
		{
		}
	}

	public class PostOnlyFilterImpl : IFilter
	{
		public bool Execute(ControllerContext context, ActionMetaData action)
		{
			if (context.HttpContext.Request.RequestType != "POST")
				throw new InvalidOperationException(
					string.Format("Action '{0}' can only be accessed using an HTTP Post.", action.Name));

			return true;
		}
	}
}
