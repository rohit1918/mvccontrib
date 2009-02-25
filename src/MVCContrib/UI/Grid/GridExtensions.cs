using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcContrib.UI.Grid.Syntax;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Extension methods related to the Grid
	/// </summary>
	public static class GridExtensions
	{
		private const string CouldNotFindView =
			"The view '{0}' or its master could not be found. The following locations were searched:{1}";

		/// <summary>
		/// Creates a grid using the specified datasource.
		/// </summary>
		/// <typeparam name="T">Type of datasouce element</typeparam>
		/// <returns></returns>
		public static IGrid<T> Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource) where T : class
		{
			return new Grid<T>(dataSource, helper.ViewContext.HttpContext.Response.Output, helper.ViewContext);
		}

		/// <summary>
		/// Creates a grid from an entry in the viewdata.
		/// </summary>
		/// <typeparam name="T">Type of element in the grid datasource.</typeparam>
		/// <returns></returns>
		public static IGrid<T> Grid<T>(this HtmlHelper helper, string viewDataKey) where T : class
		{
			var dataSource = helper.ViewContext.ViewData.Eval(viewDataKey) as IEnumerable<T>;

			if (dataSource == null)
			{
				throw new InvalidOperationException(string.Format(
														"Item in ViewData with key '{0}' is not an IEnumerable of '{1}'.", viewDataKey,
														typeof(T).Name));
			}

			return helper.Grid(dataSource);
		}

		/// <summary>
		/// Defines additional attributes for the column heading.
		/// </summary>
		/// <returns></returns>
		public static IGridColumn<T> HeaderAttributes<T>(this IGridColumn<T> column, params Func<object, object>[] hash)
		{
			return column.HeaderAttributes(new Hash(hash));
		}

		/// <summary>
		/// Defines additional attributes for a grid.
		/// </summary>
		/// <returns></returns>
		public static IGridWithOptions<T> Attributes<T>(this IGridWithOptions<T> grid, params Func<object, object>[] hash)
			where T : class
		{
			return grid.Attributes(new Hash(hash));
		}

		public static IView TryLocatePartial(this ViewEngineCollection engines, ViewContext context, string viewName)
		{
			var viewResult = engines.FindPartialView(context, viewName);

			if (viewResult.View == null)
			{
				var locationsText = new StringBuilder();
				foreach (var location in viewResult.SearchedLocations)
				{
					locationsText.AppendLine();
					locationsText.Append(location);
				}

				throw new InvalidOperationException(string.Format(CouldNotFindView, viewName, locationsText));
			}

			return viewResult.View;
		}
	}
}