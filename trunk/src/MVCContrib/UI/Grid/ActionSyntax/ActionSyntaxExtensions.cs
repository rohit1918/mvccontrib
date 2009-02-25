using System;
using MvcContrib.UI.Grid.Syntax;

namespace MvcContrib.UI.Grid.ActionSyntax
{
	/// <summary>
	/// Extension methods for the Grid that enable the alternative 'Action' syntax for rendering custom sections.
	/// </summary>
	public static class ActionSyntaxExtensions
	{
		/// <summary>
		/// Executes a delegate that can be used to specify custom HTML to replace the built in rendering of the start of the row.
		/// </summary>
		/// <param name="grid">The grid</param>
		/// <param name="block">Action that renders the HTML.</param>
		public static IGridWithOptions<T> RowStart<T>(this IGridWithOptions<T> grid, Action<T> block) where T : class
		{
			grid.Sections.RowStart(block);
			return grid;
		}


		/// <summary>
		/// Executes a delegate that can be used to specify custom HTML to replace the built in rendering of the start of the row.
		/// </summary>
		/// <param name="grid">The grid</param>
		/// <param name="block">Action that renders the HTML.</param>
		public static IGridWithOptions<T> RowStart<T>(this IGridWithOptions<T> grid, Action<T, GridRowViewData<T>> block) where T : class
		{
			grid.Sections.RowStart(block);
			return grid;
		}

		/// <summary>
		/// Executes a delegate that can be used to specify custom HTML to replace the built in rendering of the end of the row.
		/// </summary>
		/// <param name="grid">The grid</param>
		/// <param name="block">Action that renders the HTML.</param>
		public static IGridWithOptions<T> RowEnd<T>(this IGridWithOptions<T> grid, Action<T> block) where T : class
		{
			grid.Sections.RowEnd(block);
			return grid;
		}

	}
}