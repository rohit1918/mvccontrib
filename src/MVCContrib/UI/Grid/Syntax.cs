using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace MvcContrib.UI.Grid.Syntax
{
	public interface IGrid<T> : IGridWithOptions<T> where T: class 
	{
		/// <summary>
		/// Specifies a custom GridModel to use.
		/// </summary>
		/// <param name="model">The GridModel storing information about this grid</param>
		/// <returns></returns>
		IGridWithOptions<T> WithModel(IGridModel<T> model);
	}

	public interface IGridWithOptions<T> where T : class 
	{
		/// <summary>
		/// Custom grid sections
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)] //hide from fluent interface
		IGridSections<T> Sections { get; }

		/// <summary>
		/// Specifies that the grid should be rendered using a specified renderer.
		/// </summary>
		/// <param name="renderer">Renderer to use</param>
		/// <returns></returns>
		IGridWithOptions<T> RenderUsing(IGridRenderer<T> renderer);

		/// <summary>
		/// Specifies the columns to use. 
		/// </summary>
		/// <param name="columnBuilder"></param>
		/// <returns></returns>
		IGridWithOptions<T> Columns(Action<ColumnBuilder<T>> columnBuilder);

		/// <summary>
		/// Text to render when grid is empty.
		/// </summary>
		/// <param name="emptyText">Empty Text</param>
		/// <returns></returns>
		IGridWithOptions<T> Empty(string emptyText);

		/// <summary>
		/// Additional custom attributes
		/// </summary>
		/// <returns></returns>
		IGridWithOptions<T> Attributes(IDictionary<string, object> attributes);

		/// <summary>
		/// Specifies that a Partial View should be rendered for the start of each row. 
		/// </summary>
		/// <param name="partialName">The name of the partial to render</param>
		/// <returns></returns>
		IGridWithOptions<T> RowStart(string partialName);

		/// <summary>
		/// Specifies that a Partial View should be rendered for the end of each row.
		/// </summary>
		/// <param name="partialName">The name of the partial view to render</param>
		/// <returns></returns>
		IGridWithOptions<T> RowEnd(string partialName);

		/// <summary>
		/// Renders the grid to the TextWriter specified at creation
		/// </summary>
		/// <returns></returns>
		void Render();
	}
}