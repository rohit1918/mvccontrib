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

		
		IList<GridColumn<T>> IGridModel<T>.Columns
		{
			get { return columns; }
		}

		IGridRenderer<T> IGridModel<T>.Renderer { get; set; }
		string IGridModel<T>.EmptyText { get; set; }
		IDictionary<string, object> IGridModel<T>.Attributes { get; set; }

		/// <summary>
		/// Creates a new instance of the GridModel class
		/// </summary>
		public GridModel()
		{
			((IGridModel<T>)this).EmptyText = "There is no data available.";
		}
	}
}