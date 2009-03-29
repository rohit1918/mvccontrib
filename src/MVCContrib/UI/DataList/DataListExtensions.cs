using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MvcContrib.UI.DataList
{
	public static class DataListExtensions
	{
		public static DataList<T> DataList<T>(this HtmlHelper helper, IEnumerable<T> dataSource, params Func<object, object>[] tableAttributes)
		{
			DataList<T> list = new DataList<T>(dataSource, 3, RepeatDirection.Vertical, helper.ViewContext.HttpContext.Response.Output, new Hash(tableAttributes));
			return list;
		}
		public static DataList<T> DataList<T>(this HtmlHelper helper, IEnumerable<T> dataSource)
		{
			DataList<T> list = new DataList<T>(dataSource, 3, RepeatDirection.Vertical, helper.ViewContext.HttpContext.Response.Output);
			return list;
		}
	}
}