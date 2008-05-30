namespace MvcContrib.UI.Html.Grid
{
	using System;
	using System.Linq.Expressions;

	/// <summary>
	/// Used in the construction of grid columns.
	/// </summary>
	/// <typeparam name="T">Type of object to generate grid rows for.</typeparam>
	public interface IRootGridColumnBuilder<T> where T : class
	{
		/// <summary>
		/// Uses a lambda expression to specify which property the column should be rendered for.
		/// </summary>
		/// <param name="expression">Lambda expression for the property.</param>
		/// <returns>A Column builder.</returns>
		IExpressionColumnBuilder<T> For(Expression<Func<T, object>> expression);
		/// <summary>
		/// Uses a lambda expression to specify which property the column should be rendered for.
		/// </summary>
		/// <param name="func">Lambda expression for the property.</param>
		/// <param name="name">Custom column heading.</param>
		/// <returns>A Column builder.</returns>
		INestedGridColumnBuilder<T> For(Func<T, object> func, string name);
		/// <summary>
		/// Specifies a string representation of a property name for which a column should be created. 
		/// Using this approach will resort to using reflection to obtain the property value.
		/// </summary>
		/// <param name="name">Property name to generate the column for.</param>
		/// <returns>A Column builder</returns>
		ISimpleColumnBuilder<T> For(string name);
		/// <summary>
		/// Executes a delegate that can be used to specify custom HTML to replace the built in rendering of the start of the row.
		/// </summary>
		/// <param name="block">Action that renders the HTML.</param>
		void RowStart(Action<T> block);
		/// <summary>
		/// Executes a delegate that can be used to specify custom HTML to replace the built in rendering of the end of the row.
		/// </summary>
		/// <param name="block">Action that renders the HTML.</param>
		void RowEnd(Action<T> block);
	}
}