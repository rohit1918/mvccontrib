using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.UI.Grid.Syntax;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Extension methods related to the Grid
	/// </summary>
	public static class GridExtensions
	{
		/// <summary>
		/// Creates a grid using the specified datasource.
		/// </summary>
		/// <typeparam name="T">Type of datasouce element</typeparam>
		/// <returns></returns>
		public static IGrid<T> Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource) where T : class
		{
			return new Grid<T>(dataSource, helper.ViewContext.HttpContext.Response.Output);
		}

		/// <summary>
		/// Creates a grid from an entry in the viewdata.
		/// </summary>
		/// <typeparam name="T">Type of element in the grid datasource.</typeparam>
		/// <returns></returns>
		public static IGrid<T> Grid<T>(this HtmlHelper helper, string viewDataKey) where T : class
		{
			var dataSource = helper.ViewContext.ViewData.Eval(viewDataKey) as IEnumerable<T>;

			if(dataSource == null)
			{
				throw new InvalidOperationException(string.Format("Item in ViewData with key '{0}' is not an IEnumerable of '{1}'.", viewDataKey, typeof(T).Name));
			}

			return helper.Grid(dataSource);
		}
	}
}