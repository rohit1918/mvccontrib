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

		/// <summary>
		/// Defines additional attributes for the cell. 
		/// </summary>
		public static IGridColumn<T> Attributes<T>(this IGridColumn<T> column, params Func<object, object>[] hash)
		{
			return column.Attributes(x => new Hash(hash));
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

		/// <summary>
		/// Specifies that a partial view should be rendered as the start of every row. 
		/// </summary>
		public static void RowStart<T>(this IGridSections<T> sections, string partialName) where T : class
		{
			sections[GridSection.RowStart] = new GridSection<T>((rowmodel, context) => RenderPartialForSection(rowmodel, context, partialName));
		}

		/// <summary>
		/// Specifies that a partial view should be rendered as the end of every row. 
		/// </summary>
		public static void RowEnd<T>(this IGridSections<T> sections, string partialName) where T : class 
		{
			sections[GridSection.RowEnd] = new GridSection<T>((rowmodel, context) => RenderPartialForSection(rowmodel, context, partialName));
		}

		private static void RenderPartialForSection<T>(GridRowViewData<T> viewData, RenderingContext context, string partialName) 
		{
			var view = context.ViewEngines.TryLocatePartial(context.ViewContext, partialName);
			var newViewData = new ViewDataDictionary<GridRowViewData<T>>(viewData);
			var newContext = new ViewContext(context.ViewContext, context.ViewContext.View, newViewData,
											 context.ViewContext.TempData);
			view.Render(newContext, context.Writer);
		}

		/// <summary>
		/// Specifies that a Partial View should be rendered for the start of each row. 
		/// </summary>
		/// <param name="grid">The grid</param>
		/// <param name="partialName">The name of the partial to render</param>
		/// <returns></returns>
		public static IGridWithOptions<T> RowStart<T>(this IGridWithOptions<T> grid, string partialName) where T : class 
		{
			grid.Model.Sections.RowStart(partialName);
			return grid;
		}

		/// <summary>
		/// Specifies that a Partial View should be rendered for the end of each row.
		/// </summary>
		/// <param name="grid">The grid</param>
		/// <param name="partialName">The name of the partial view to render</param>
		/// <returns></returns>
		public static IGridWithOptions<T> RowEnd<T>(this IGridWithOptions<T> grid, string partialName) where T : class 
		{
			grid.Model.Sections.RowEnd(partialName);
			return grid;
		}

		/// <summary>
		/// The HTML that should be used to render the header for the column. This should include TD tags. 
		/// </summary>
		/// <param name="column">The current column</param>
		/// <param name="header">The format to use.</param>
		/// <returns></returns>
		public static IGridColumn<T> Header<T>(this IGridColumn<T> column, string header) where T : class 
		{
			column.CustomHeaderRenderer = c => c.Writer.Write(header);
			return column;
		}


		/// <summary>
		/// Specifies that a partial view should be used to render the column header.
		/// </summary>
		/// <param name="column">The current column</param>
		/// <param name="partialName">Name of the partial to render</param>
		/// <returns></returns>
		public static IGridColumn<T> HeaderPartial<T>(this IGridColumn<T> column, string partialName)  where T : class
		{
			column.CustomHeaderRenderer = context => {
				var view = context.ViewEngines.TryLocatePartial(context.ViewContext, partialName);
				view.Render(context.ViewContext, context.Writer);
			};
			return column;
		}

		/// <summary>
		/// Specifies that a partial view should be used to render the contents of this column.
		/// </summary>
		/// <param name="column">The current column</param>
		/// <param name="partialName">The name of the partial view</param>
		/// <returns></returns>
		public static IGridColumn<T> Partial<T>(this IGridColumn<T> column, string partialName) where T : class {
			column.CustomItemRenderer = (context, item) => {
				var view = context.ViewEngines.TryLocatePartial(context.ViewContext, partialName);
				var newViewData = new ViewDataDictionary<T>(item);
				var newContext = new ViewContext(context.ViewContext, context.ViewContext.View, newViewData, context.ViewContext.TempData);
				view.Render(newContext, context.Writer);
			};
			return column;
		}
	}
}