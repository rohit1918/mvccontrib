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
		private const string Empty_Text_Key = "empty";
		private const string Pagination_Format_Text_Key = "paginationFormat";
		private const string Page_Query_Name_Text_Key = "page";
		private const string Pagination_Single_Format_Text_Key = "paginationSingleFormat";
		private const string Pagination_First_Text_Key = "first";
		private const string Pagination_Prev_Text_Key = "prev";
		private const string Pagination_Next_Text_Key = "next";
		private const string Pagination_Last_Text_Key = "last";

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
			var grid = new Grid<T>();
			
			grid.Render(
				GetDataSourceFromViewData<T>(viewDataKey, helper.ViewContext),
				CreateColumnBuilder(columns, sections),
				new GridOptions().LoadFromDictionary(htmlAttributes), 
				htmlAttributes,
				helper.ViewContext
			);

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
			var grid = new Grid<T>();

			grid.Render(
				dataSource,
				CreateColumnBuilder(columns, sections),
				new GridOptions().LoadFromDictionary(htmlAttributes),
				htmlAttributes,
				helper.ViewContext
			);

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

		public static GridOptions LoadFromDictionary(this GridOptions options, IDictionary dictionary) {
			if (dictionary == null) return options;

			if (dictionary.Contains(Empty_Text_Key)) {
				options.EmptyMessageText = dictionary[Empty_Text_Key] as string;
				dictionary.Remove(Empty_Text_Key);
			}

			if (dictionary.Contains(Pagination_Format_Text_Key)) {
				options.PaginationFormat = dictionary[Pagination_Format_Text_Key] as string;
				dictionary.Remove(Pagination_Format_Text_Key);
			}

			if (dictionary.Contains(Pagination_Single_Format_Text_Key)) {
				options.PaginationSingleFormat = dictionary[Pagination_Single_Format_Text_Key] as string;
				dictionary.Remove(Pagination_Single_Format_Text_Key);
			}

			if (dictionary.Contains(Pagination_First_Text_Key)) {
				options.PaginationFirst = dictionary[Pagination_First_Text_Key] as string;
				dictionary.Remove(Pagination_First_Text_Key);
			}

			if (dictionary.Contains(Pagination_Prev_Text_Key)) {
				options.PaginationPrev = dictionary[Pagination_Prev_Text_Key] as string;
				dictionary.Remove(Pagination_Prev_Text_Key);
			}

			if (dictionary.Contains(Pagination_Next_Text_Key)) {
				options.PaginationNext = dictionary[Pagination_Next_Text_Key] as string;
				dictionary.Remove(Pagination_Next_Text_Key);
			}

			if (dictionary.Contains(Pagination_Last_Text_Key)) {
				options.PaginationLast = dictionary[Pagination_Last_Text_Key] as string;
				dictionary.Remove(Pagination_Last_Text_Key);
			}

			if (dictionary.Contains(Page_Query_Name_Text_Key)) {
				options.PageQueryName = dictionary[Page_Query_Name_Text_Key] as string;
				dictionary.Remove(Page_Query_Name_Text_Key);
			}

			return options;
		}
	}
}