using System.IO;

namespace MvcContrib.UI.Grid
{
    /// <summary>
    /// A renderer responsible for rendering a grid.
    /// </summary>
	public interface IGridRenderer
	{
		/// <summary>
		/// Renders a grid
		/// </summary>
		/// <param name="gridModel">The grid model to render</param>
		/// <param name="output">The TextWriter to which the grid should be rendered/</param>
		void Render(IGridModel gridModel, TextWriter output);
	}
}