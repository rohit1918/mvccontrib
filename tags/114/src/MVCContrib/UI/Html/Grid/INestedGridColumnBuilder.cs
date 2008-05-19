using System;

namespace MvcContrib.UI.Html.Grid
{
	/// <summary>
	/// Used in the construction of grid columns.
	/// </summary>
	/// <typeparam name="T">Type of object to generate grid rows for.</typeparam>
	public interface INestedGridColumnBuilder<T> where T : class 
	{
		/// <summary>
		/// Specifies that when rendered, the value for the column should have the specified string formatting applied to it.
		/// </summary>
		/// <param name="format">The format to apply.</param>
		/// <returns></returns>
		INestedGridColumnBuilder<T> Formatted(string format);
		/// <summary>
		/// By default, all output is HTML-Encoded. Specifying DoNotEncode will disable the encoding for all cells in this column.
		/// </summary>
		/// <returns></returns>
		INestedGridColumnBuilder<T> DoNotEncode();
		/// <summary>
		/// Can be used to hide the contents of all the cells in the current column. If the specified func returns false then the contents of the cells will not be rendered.
		/// </summary>
		/// <param name="condition">Delegate that will be invoked to determine whether the contents of the cells should be rendered.</param>
		/// <returns></returns>
		INestedGridColumnBuilder<T> CellCondition(Func<T, bool> condition);
		/// <summary>
		/// Can be used to hide the entire column. If the specified func returns false then the column will not be rendered.
		/// </summary>
		/// <param name="condition">Delegate that will be invoked to determine whether the column should be rendered.</param>
		/// <returns></returns>
		INestedGridColumnBuilder<T> ColumnCondition(Func<bool> condition);
		/// <summary>
		/// Can be used to perform custom rendering for a column's heading.
		/// </summary>
		/// <param name="block">Delegate to invoke to perform the rendering.</param>
		/// <returns></returns>
		INestedGridColumnBuilder<T> Header(Action block);
	}
}