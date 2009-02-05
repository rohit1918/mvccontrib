using System;
using System.Collections.Generic;
using MvcContrib.UI.Grid.Syntax;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Default model for grid
	/// </summary>
	public class GridModel<T>  : IGridModel<T>
	{
		private readonly IList<GridColumn<T>> columns = new List<GridColumn<T>>();

		IGridRenderer<T> IGridModel<T>.Renderer { get; set; }
		
		IList<GridColumn<T>> IGridModel<T>.Columns
		{
			get { return columns; }
		}

		/// <summary>
		/// Creates a new instance of the GridModel class
		/// </summary>
		public GridModel()
		{
		}
	}
}