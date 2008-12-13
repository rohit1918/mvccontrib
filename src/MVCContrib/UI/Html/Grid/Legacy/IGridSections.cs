using System;

namespace MvcContrib.UI.Html.Grid.Legacy
{
	/// <summary>
	/// Used to provide custon sections to the grid.
	/// </summary>
	public interface IGridSections<T>
	{
		/// <summary>
		/// Executes a delegate that can be used to specify custom HTML to replace the built in rendering of the start of the row.
		/// </summary>
		/// <param name="block">Action that renders the HTML.</param>
		void RowStart(Action<T> block);
		/// <summary>
		/// Executes a delegate that can be used to specify custom HTML to replace the built in rendering of the start of the row with alternate row as additional parameter.
		/// </summary>
		/// <param name="block">Action that renders the HTML.</param>
		void RowStart(Action<T, bool> block);
		/// <summary>
		/// Executes a delegate that can be used to specify custom HTML to replace the built in rendering of the end of the row.
		/// </summary>
		/// <param name="block">Action that renders the HTML.</param>
		void RowEnd(Action<T> block);
	}
}