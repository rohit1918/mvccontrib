using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcContrib.UI.Html.Grid.Legacy
{
	/// <summary>
	/// Extension methods on the HtmlHelper for creating Grid instances.
	/// </summary>
	public static class LegacyGridExtensions
	{
		public static void Grid<T>(this HtmlHelper helper, string viewDataKey, Action<IRootGridColumnBuilder<T>> columns) where T : class
		{
			Grid(helper, viewDataKey, null, columns, null);
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
				GetDataSourceFromViewData<T>(viewDataKey, helper.ViewContext),
				CreateColumnBuilder(columns, sections),
				htmlAttributes,
				helper.ViewContext.HttpContext.Response.Output,
				helper.ViewContext.HttpContext
				);

			grid.Render();
		}

		public static void Grid<T>(this HtmlHelper helper, IEnumerable<T> dataSource, Action<IRootGridColumnBuilder<T>> columns) where T : class
		{
			Grid(helper, dataSource, null, columns, null);
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

		private static IEnumerable<T> GetDataSourceFromViewData<T>(string key, ViewContext context) 
		{
			if(key == null) return null;

			object items = context.ViewData.Eval(key);
			IEnumerable<T> collection = null;

			if (items != null) 
			{
				collection = items as IEnumerable<T>;

				if (collection == null) 
				{
					collection = (items as IEnumerable).Cast<T>();
				}
			}

			return collection;
		}

	}
}