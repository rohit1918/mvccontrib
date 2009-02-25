using System;
using System.Collections.Generic;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Grid Column fluent interface
	/// </summary>
	public interface IGridColumn<T>
	{
		/// <summary>
		/// Specified an explicit name for the column.
		/// </summary>
		/// <param name="name">Name of column</param>
		/// <returns></returns>
		IGridColumn<T> Named(string name);
		/// <summary>
		/// If the property name is PascalCased, it should not be split part.
		/// </summary>
		/// <returns></returns>
		IGridColumn<T> DoNotSplit();
		/// <summary>
		/// A custom format to use when building the cell's value
		/// </summary>
		/// <param name="format">Format to use</param>
		/// <returns></returns>
		IGridColumn<T> Format(string format);
		/// <summary>
		/// Delegate used to hide the contents of the cells in a column.
		/// </summary>
		IGridColumn<T> CellCondition(Func<T, bool> func);

		/// <summary>
		/// Determines whether the column should be displayed
		/// </summary>
		/// <param name="isVisible"></param>
		/// <returns></returns>
		IGridColumn<T> Visible(bool isVisible);

		/// <summary>
		/// Do not HTML Encode the output
		/// </summary>
		/// <returns></returns>
		IGridColumn<T> DoNotEncode();

		/// <summary>
		/// Defines additional attributes for the column heading.
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		IGridColumn<T> HeaderAttributes(IDictionary<string, object> attributes);

		/// <summary>
		/// The HTML that should be used to render the header for the column. This should include TD tags. 
		/// </summary>
		/// <param name="header">The format to use.</param>
		/// <returns></returns>
		IGridColumn<T> Header(string header);

		/// <summary>
		/// Specifies that a partial view should be used to render the column header.
		/// </summary>
		/// <param name="partialName"></param>
		/// <returns></returns>
		IGridColumn<T> HeaderPartial(string partialName);


		/// <summary>
		/// Specifies that an action should be used to render the column header.
		/// </summary>
		/// <param name="action">The action to render</param>
		/// <returns></returns>
		IGridColumn<T> HeaderAction(Action action);

		/// <summary>
		/// Specifies that a partial view should be used to render the contents of this column.
		/// </summary>
		/// <param name="partialName">The name of the partial view</param>
		/// <returns></returns>
		IGridColumn<T> Partial(string partialName);


		/// <summary>
		/// Specifies that an action should be used to render the contents of this column.
		/// </summary>
		/// <param name="action">The action to render</param>
		/// <returns></returns>
		IGridColumn<T> Action(Action<T> action);
	}
}