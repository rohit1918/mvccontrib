using System.Collections.Generic;
using MvcContrib.UI.Grid.Syntax;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Defines a grid model
	/// </summary>
	public interface IGridModel<T> where T: class 
	{
		IGridRenderer<T> Renderer { get; set; }
		ICollection<GridColumn<T>> Columns { get; }
		IDictionary<GridSection, GridSection<T>> Sections { get; }
		string EmptyText { get; set; }
		IDictionary<string, object> Attributes { get; set; }
	}
}