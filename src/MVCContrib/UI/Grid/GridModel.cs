using System;
using MvcContrib.UI.Grid.Syntax;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Default model for grid
	/// </summary>
	public class GridModel  : IGridModel
	{
		IGridRenderer IGridModel.Renderer { get; set; }

		/// <summary>
		/// Creates a new instance of the GridModel class
		/// </summary>
		public GridModel()
		{
		}
	}
}