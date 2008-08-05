using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcContrib.UI.Html.Grid
{

	/// <summary>
	/// Extension methods on the HtmlHelper for creating Grid instances.
	/// </summary>
	public static class GridExtensions
	{
		public static void Grid<T>(this HtmlHelper helper, string viewDataKey, Action<IRootGridColumnBuilder<T>> columns) where T : class
		{
			Grid<T>(helper, viewDataKey, null, columns, null);
		}

		public static void Grid<T>(this HtmlHelper helper, string viewDataKey, Action<IRootGridColumnBuilder<T>> columns, Action<IGridSections<T>> sections) where T : class
		{
			Grid(helper, viewDataKey, null, columns, sections);
		}

		public static void Grid<T>(this HtmlHelper helper, string viewDataKey, IDictionary htmlAttributes, Action<IRootGridColumnBuilder<T>> columns) where T : class 
		{
			Grid(helper, viewDataKey, htmlAttributes, columns, null);
		}

		public static void Grid<T>(this HtmlHelper helper, string viewDataKey, IDictionary htmlAttributes, Action<IRootGridColumnBuilder<T>> columns, Action<IGridSections<T>> sections) where T : class
		{
			var grid = new Grid<T>(
				viewDataKey,
				helper.ViewContext,
				CreateColumnBuilder(columns, sections),
				htmlAttributes,
				helper.ViewContext.HttpContext.Response.Output
			);

			grid.Render();
		}

		public static void Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource, Action<IRootGridColumnBuilder<T>> columns) where T : class
		{
			Grid<T>(helper, dataSource, null, columns, null);
		}

		public static void Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource, Action<IRootGridColumnBuilder<T>> columns, Action<IGridSections<T>> sections) where T : class 
		{
			Grid(helper, dataSource, null, columns, sections);
		}

		public static void Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource, IDictionary htmlAttributes, Action<IRootGridColumnBuilder<T>> columns) where T : class 
		{
			Grid(helper, dataSource, htmlAttributes, columns, null);
		}

		public static void Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource, IDictionary htmlAttributes, Action<IRootGridColumnBuilder<T>> columns, Action<IGridSections<T>> sections) where T : class
		{
			var grid = new Grid<T>(
				dataSource,
				CreateColumnBuilder(columns, sections),
				htmlAttributes,
				helper.ViewContext.HttpContext.Response.Output,
				helper.ViewContext.HttpContext
			);

			grid.Render();
		}

		private static GridColumnBuilder<T> CreateColumnBuilder<T>(Action<IRootGridColumnBuilder<T>> columns, Action<IGridSections<T>> sections) where T : class
		{
			var builder = new GridColumnBuilder<T>();

			if (columns != null)
			{
				columns(builder);
			}

			if(sections != null)
			{
				sections(builder);	
			}

			return builder;
		}
	}
}
