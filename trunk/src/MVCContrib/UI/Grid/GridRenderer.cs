using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Base class for Grid Renderers. 
	/// </summary>
	public abstract class GridRenderer<T> : IGridRenderer<T> where T : class 
	{
		protected IGridModel<T> GridModel { get; private set; }
		protected IEnumerable<T> DataSource { get; private set; }
		protected ViewContext Context { get; private set; }
		private TextWriter _writer;
		private readonly ViewEngineCollection _engines;

		protected GridRenderer() : this(ViewEngines.Engines) {}

		protected GridRenderer(ViewEngineCollection engines)
		{
			_engines = engines;
		}

		public void Render(IGridModel<T> gridModel, IEnumerable<T> dataSource, TextWriter output, ViewContext context)
		{
			_writer = output;
			GridModel = gridModel;
			DataSource = dataSource;
			Context = context;

			RenderGridStart();
			bool headerRendered = RenderHeader();

			if(headerRendered)
			{
				RenderItems();
			}
			else
			{
				RenderEmpty();
			}

			RenderGridEnd(!headerRendered);
		}


		protected void RenderText(string text)
		{
			_writer.Write(text);
		}

		protected virtual void RenderItems()
		{
			bool isAlternate = false;
			foreach(var item in DataSource)
			{
				RenderItem(item, isAlternate);
				isAlternate = !isAlternate;
			}
		}

		protected virtual void RenderItem(T item, bool isAlternate)
		{
			RenderRowStart(item, isAlternate);

			foreach(var column in VisibleColumns())
			{
				//A custom item section has been specified - render it and continue to the next iteration.
				if (column.CustomItemRenderer != null)
				{
					column.CustomItemRenderer(new RenderingContext(_writer, Context, _engines), item);
					continue;
				}

				RenderStartCell(column);
				RenderCellValue(column, item);
				RenderEndCell();
			}

			RenderRowEnd(item, isAlternate);
		}

		protected virtual void RenderCellValue(GridColumn<T> column, T item)
		{
			var cellValue = column.GetValue(item);

			if(cellValue != null)
			{
				RenderText(cellValue.ToString());
			}
		}

		protected virtual bool RenderHeader()
		{
			//No items - do not render a header.
			if(IsDataSourceEmpty()) return false;

			RenderHeadStart();

			foreach(var column in VisibleColumns())
			{
				//Allow for custom header overrides.
				if(column.CustomHeaderRenderer != null)
				{
					column.CustomHeaderRenderer(new RenderingContext(_writer, Context, _engines));
				}
				else
				{
					RenderHeaderCellStart(column);
					RenderText(column.Name);
					RenderHeaderCellEnd();
				}
			}

			RenderHeadEnd();

			return true;
		}

		protected bool IsDataSourceEmpty()
		{
			return DataSource == null || !DataSource.Any();
		}

		protected IEnumerable<GridColumn<T>> VisibleColumns()
		{
			return GridModel.Columns.Where(x => x.Visible);
		}

		protected virtual void RenderRowStart(T item, bool isAlternate)
		{
			//If there's a custom section for rendering the start of the row, invoke it.
			//Otherwise fall back to the default rendering.

			var section = GridModel.Sections[GridSection.RowStart];

			if(section != null)
			{
				section.Render(new RenderingContext(_writer, Context, _engines), item, isAlternate);
			}
			else
			{
				RenderRowStart(isAlternate);				
			}
		}

		protected virtual void RenderRowEnd(T item, bool isAlternate)
		{
			var section = GridModel.Sections[GridSection.RowEnd];
            
			if(section != null)
			{
				section.Render(new RenderingContext(_writer, Context, _engines), item, isAlternate);
			}
			else
			{
				RenderRowEnd();
			}
		}

		protected abstract void RenderHeaderCellEnd();
		protected abstract void RenderHeaderCellStart(GridColumn<T> column);
		protected abstract void RenderRowStart(bool isAlternate);
		protected abstract void RenderRowEnd();
		protected abstract void RenderEndCell();
		protected abstract void RenderStartCell(GridColumn<T> column);
		protected abstract void RenderHeadStart();
		protected abstract void RenderHeadEnd();
		protected abstract void RenderGridStart();
		protected abstract void RenderGridEnd(bool isEmpty);
		protected abstract void RenderEmpty();
	}
}
