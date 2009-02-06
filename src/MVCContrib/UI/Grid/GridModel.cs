using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.UI.Grid.Syntax;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Default model for grid
	/// </summary>
	public class GridModel<T>  : IGridModel<T> where T : class
	{
		private readonly IList<GridColumn<T>> _columns = new List<GridColumn<T>>();
		private IGridRenderer<T> _renderer = new HtmlTableGridRenderer<T>();
		private string _emptyText;
		private IDictionary<string, object> _attributes = new Dictionary<string, object>();


		IList<GridColumn<T>> IGridModel<T>.Columns
		{
			get { return _columns; }
		}

		IGridRenderer<T> IGridModel<T>.Renderer
		{
			get { return _renderer; }
			set { _renderer = value; }
		}

		string IGridModel<T>.EmptyText
		{
			get { return _emptyText; }
			set { _emptyText = value; }
		}

		IDictionary<string, object> IGridModel<T>.Attributes
		{
			get { return _attributes; }
			set { _attributes = value; }
		}

		/// <summary>
		/// Creates a new instance of the GridModel class
		/// </summary>
		public GridModel()
		{
			_emptyText = "There is no data available.";
		}

		/// <summary>
		/// Defines a column from a particular property
		/// </summary>
		/// <param name="propertySpecifier"></param>
		/// <returns></returns>
		public IGridColumn<T> ColumnFor(Expression<Func<T, object>> propertySpecifier)
		{
			var columnBuilder = new ColumnBuilder<T>();
			var column = columnBuilder.For(propertySpecifier);
			_columns.Add((GridColumn<T>)column);
			return column;
		}

		/// <summary>
		/// Defines a custom column
		/// </summary>
		/// <param name="columnName">Name of column</param>
		public IGridColumn<T> ColumnFor(string columnName)
		{
			var columnBuilder = new ColumnBuilder<T>();
			var column = columnBuilder.For(columnName);
			_columns.Add((GridColumn<T>)column);
			return column;
		}

		/// <summary>
		/// Text that will be displayed when the grid has no data.
		/// </summary>
		/// <param name="emptyText">Text to display</param>
		public void Empty(string emptyText)
		{
			_emptyText = emptyText;
		}

		/// <summary>
		/// Defines additional attributes for the grid.
		/// </summary>
		/// <param name="hash"></param>
		public void Attributes(params Func<object, object>[] hash)
		{
			Attributes(new Hash(hash));
		}

		/// <summary>
		/// Defines additional attributes for the grid
		/// </summary>
		/// <param name="attributes"></param>
		public void Attributes(IDictionary<string, object> attributes)
		{
			_attributes = attributes;
		}
	}
}