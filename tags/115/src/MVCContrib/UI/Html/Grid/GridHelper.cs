namespace MvcContrib.UI.Html.Grid
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;

	/// <summary>
	/// Extension methods on the HtmlHelper for creating Grid instances.
	/// </summary>
	public static class GridExtensions
	{
		public static void Grid<T>(this HtmlHelper helper, string viewDataKey, Action<IRootGridColumnBuilder<T>> columns) where T : class
		{
			Grid<T>(helper, viewDataKey, null, columns);
		}


		public static void Grid<T>(this HtmlHelper helper, string viewDataKey, IDictionary htmlAttributes, Action<IRootGridColumnBuilder<T>> columns) where T : class
		{
			var grid = new Grid<T>(
				viewDataKey,
				helper.ViewContext,
				CreateColumnBuilder(columns),
				htmlAttributes,
				helper.ViewContext.HttpContext.Response.Output
			);

			grid.Render();
		}

		public static void Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource, Action<IRootGridColumnBuilder<T>> columns) where T : class
		{
			Grid<T>(helper, dataSource, null, columns);
		}

		public static void Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource, IDictionary htmlAttributes, Action<IRootGridColumnBuilder<T>> columns) where T : class
		{
			var grid = new Grid<T>(
				dataSource,
				CreateColumnBuilder(columns),
				htmlAttributes,
				helper.ViewContext.HttpContext.Response.Output,
				helper.ViewContext.HttpContext
			);

			grid.Render();
		}

		private static GridColumnBuilder<T> CreateColumnBuilder<T>(Action<IRootGridColumnBuilder<T>> columns) where T : class
		{
			var builder = new GridColumnBuilder<T>();
			if (columns != null)
				columns(builder);

			return builder;
		}
	}
}