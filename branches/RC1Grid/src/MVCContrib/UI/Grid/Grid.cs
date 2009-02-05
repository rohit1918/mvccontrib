using System;
using System.Collections.Generic;
using System.IO;
using MvcContrib.UI.Grid.Syntax;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Defines a grid to be rendered.
	/// </summary>
	/// <typeparam name="T">Type of datasource for the grid</typeparam>
	public class Grid<T> : IGrid<T> where T : class 
	{
		private readonly TextWriter _writer;
		private IGridModel<T> _gridModel = new GridModel<T>();
		/// <summary>
		/// Creates a new instance of the Grid class.
		/// </summary>
		/// <param name="dataSource">The datasource for the grid</param>
		/// <param name="writer">The TextWriter where the grid should be rendered</param>
		public Grid(IEnumerable<T> dataSource, TextWriter writer)
		{
			_writer = writer;
			DataSource = dataSource;
		}

		/// <summary>
		/// The datasource for the grid.
		/// </summary>
		public IEnumerable<T> DataSource { get; private set; }

		public IGridWithOptions<T> RenderUsing(IGridRenderer<T> renderer)
		{
			_gridModel.Renderer = renderer;
			return this;
		}

		public IGridWithOptions<T> Columns(Action<ColumnBuilder<T>> columnBuilder)
		{
			var builder = new ColumnBuilder<T>();
			columnBuilder(builder);
            
			foreach(var column in builder)
			{
				_gridModel.Columns.Add(column);
			}

			return this;
		}

		public IGridWithOptions<T> Empty(string emptyText)
		{
			_gridModel.EmptyText = emptyText;
			return this;
		}

		public IGridWithOptions<T> Attributes(IDictionary<string, object> attributes)
		{
			_gridModel.Attributes = attributes;
			return this;
		}

		public IGridWithOptions<T> WithModel(IGridModel<T> model)
		{
			_gridModel = model;
			return this;
		}

		public override string ToString() 
		{
			/*if(_gridModel.Renderer == null)
			{
				_gridModel.Renderer = new HtmlTableGridRenderer<T>();
			}*/

			_gridModel.Renderer.Render(_gridModel, DataSource, _writer);
			return null;
		}
	}
}