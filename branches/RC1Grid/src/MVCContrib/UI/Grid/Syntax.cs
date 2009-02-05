namespace MvcContrib.UI.Grid.Syntax
{
	public interface IGrid : IGridWithColumnsOptions
	{
		/// <summary>
		/// Specifies a custom GridModel to use.
		/// </summary>
		/// <param name="model">The GridModel storing information about this grid</param>
		/// <returns></returns>
		IGridWithOptions WithModel(IGridModel model);
	}

	public interface IGridWithColumnsOptions : IGridWithColumns, IGridWithOptions
	{
		
	}

	public interface IGridWithColumns
	{
		
	}

	public interface IGridWithOptions
	{
		/// <summary>
		/// Specifies that the grid should be rendered using a specified renderer.
		/// </summary>
		/// <param name="renderer">Renderer to use</param>
		/// <returns></returns>
		IGridWithOptions RenderUsing(IGridRenderer renderer);
	}
}