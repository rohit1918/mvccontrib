using System;
using System.Web;
using System.Collections.Generic;
using System.IO;

namespace MvcContrib.UI.Html.Grid
{

	/// <summary>
	/// Base class for SmartGrid functionality.
	/// </summary>
	/// <typeparam name="T">The type of object for each row in the grid.</typeparam>
	public abstract class GridBase<T> where T : class
	{
		/// <summary>
		/// The items to be displayed in the grid.
		/// </summary>
		protected IEnumerable<T> Items { get; set; }

		/// <summary>
		/// The columns to generate.
		/// </summary>
		protected GridColumnBuilder<T> Columns { get; set; }

		/// <summary>
		/// The writer to output the results to.
		/// </summary>
		protected TextWriter Writer { get; private set; }

		/// <summary>
		/// Message to be displayed if the Items property is empty.
		/// </summary>
		public string EmptyMessageText { get; set; }

		protected GridBase(IEnumerable<T> items, GridColumnBuilder<T> columns, TextWriter writer)
		{
			Items = items;
			Columns = columns;
			Writer = writer;
			EmptyMessageText = "There is no data available.";
		}

		/// <summary>
		/// Renders text to the output stream
		/// </summary>
		/// <param name="text">The text to render</param>
		protected void RenderText(string text)
		{
			Writer.Write(text);
		}

		/// <summary>
		/// Performs the rendering of the grid.
		/// </summary>
		public virtual void Render()
		{
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

			RenderGridEnd(! headerRendered);
		}

		/// <summary>
		/// Iterates over the items collection, builds up the markup for each row and outputs the results.
		/// </summary>
		protected virtual void RenderItems()
		{
			bool isAlternate = false;
			foreach(var item in Items)
			{
				RenderRowStart(item, isAlternate);

				foreach(var column in Columns)
				{
					Action renderStartCell = () => RenderStartCell(column);
					Action renderEndCell = RenderEndCell;

					column.Render(item, Writer, renderStartCell, renderEndCell);
				}

				RenderRowEnd(item);

				isAlternate = !isAlternate;
			}
		}

		/// <summary>
		/// If there are items to display, iterate over each column and output the results. 
		/// If there are no items to display, return false to indicate execution of RenderItems should not take place.
		/// </summary>
		/// <returns>boolean indication whether or not items should be rendered.</returns>
		protected virtual bool RenderHeader()
		{
			//No items - do not render a header.
			if(Items == null)
			{
				return false;
			}

			IEnumerator<T> enumerator = Items.GetEnumerator();

			//No items - do not render header.
			if(!enumerator.MoveNext())
			{
				return false;
			}

			RenderHeadStart();

			foreach(var column in Columns)
			{
				Action cellStart = () => RenderHeaderCellStart(column);
				Action cellEnd = RenderHeaderCellEnd;

				column.RenderHeader(Writer, cellStart, cellEnd);
			}

			RenderHeadEnd();

			return true;
		}


		/// <summary>
		/// Renders the start of a row to the output stream.
		/// </summary>
		/// <param name="item">The item to be rendered into this row.</param>
		/// <param name="isAlternate">Whether the row is an alternate row</param>
		protected virtual void RenderRowStart(T item, bool isAlternate)
		{
			//If there's a custom delegate for rendering the start of the row, invoke it.
			//Otherwise fall back to the default rendering.
			if(Columns.RowStartBlock != null)
			{
				Columns.RowStartBlock(item);
			}
			else if(Columns.RowStartWithAlternateBlock != null)
			{
				Columns.RowStartWithAlternateBlock(item, isAlternate);
			}
			else
			{
				RenderRowStart(isAlternate);
			}
		}

		/// <summary>
		/// Renders the end of a row to the output stream.
		/// </summary>
		/// <param name="item">The item being rendered in to this row.</param>
		protected virtual void RenderRowEnd(T item)
		{
			//If there's a custom delegate for rendering the end of the row, invoke it.
			//Otherwise fall back to the default rendering.
			if(Columns.RowEndBlock != null)
			{
				Columns.RowEndBlock(item);
			}
			else
			{
				RenderRowEnd();
			}
		}

		protected abstract void RenderHeaderCellEnd();
		protected abstract void RenderHeaderCellStart(IGridColumn<T> column);
		protected abstract void RenderRowStart(bool isAlternate);
		protected abstract void RenderRowEnd();
		protected abstract void RenderEndCell();
		protected abstract void RenderStartCell(IGridColumn<T> column);
		protected abstract void RenderHeadStart();
		protected abstract void RenderHeadEnd();
		protected abstract void RenderGridStart();
		protected abstract void RenderGridEnd(bool isEmpty);
		protected abstract void RenderEmpty();
	}
}
