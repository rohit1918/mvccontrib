using System;
using System.IO;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Renders a grid as an HTML table.
	/// </summary>
	public class HtmlTableGridRenderer<T> : GridRenderer<T>
	{
	}

	/// <summary>
	/// Base class for Grid Renderers. 
	/// </summary>
	public abstract class GridRenderer<T> : IGridRenderer<T>
	{
		public void Render(IGridModel<T> gridModel, TextWriter output)
		{
		}
	}
}