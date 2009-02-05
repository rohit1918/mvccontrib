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
	public class Grid<T> : IGrid
	{
		private readonly TextWriter _writer;
		private IGridModel _gridModel = new GridModel();
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

		public IGridWithOptions RenderUsing(IGridRenderer renderer)
		{
			_gridModel.Renderer = renderer;
			return this;
		}

		public IGridWithOptions WithModel(IGridModel model)
		{
			_gridModel = model;
			return this;
		}

		public override string ToString() 
		{
			if(_gridModel.Renderer == null)
			{
				_gridModel.Renderer = new HtmlTableGridRenderer();
			}

			_gridModel.Renderer.Render(_gridModel, _writer);
			return null;
		}
	}
}