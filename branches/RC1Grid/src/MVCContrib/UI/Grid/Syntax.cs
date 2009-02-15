using System;
using System.Collections.Generic;

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
		/// Defines additional grid sections to render.
		/// </summary>
		/// <param name="sections"></param>
		/// <returns></returns>
		IGridWithOptions<T> Sections(Action<GridSections<T>> sections);
	}
}