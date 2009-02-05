using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Base class for Grid Renderers. 
	/// </summary>
	public abstract class GridRenderer<T> : IGridRenderer<T>
	{
		protected IGridModel<T> GridModel { get; private set; }
		protected IEnumerable<T> DataSource { get; private set; }
		private TextWriter _writer;

		public void Render(IGridModel<T> gridModel, IEnumerable<T> dataSource, TextWriter output)
		{
			_writer = output;
			GridModel = gridModel;
			DataSource = dataSource;

			RenderGridStart();
			bool headerRendered = RenderHeader();

			if (headerRendered) {
				RenderItems();
			}
			else {
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
			foreach (var item in DataSource) {
				RenderRowStart(item, isAlternate);

				foreach (var column in GridModel.Columns) {

					if(! column.Visible)
					{
						continue;
					}

					//A custom item section has been specified - render it and continue to the next iteration.
//					if (column.Name != null && column.CustomRenderer != null) {
//						column.CustomRenderer(item);
//						continue;
//					}

					RenderStartCell(column);

					//Invoke the delegate to retrieve the value to be displayed in the cell.
					object value = column.GetValue(item);

					if (value != null) {
						RenderText(value.ToString());
					}

					RenderEndCell();
				}

				RenderRowEnd(item);

				isAlternate = !isAlternate;
			}
		}

		protected virtual bool RenderHeader() {
			//No items - do not render a header.
			if (DataSource == null) {
				return false;
			}

			IEnumerator<T> enumerator = DataSource.GetEnumerator();

			//No items - do not render header.
			if (!enumerator.MoveNext()) {
				return false;
			}

			RenderHeadStart();

			foreach (var column in GridModel.Columns) {
				//Allow for custom header overrides.
//				if (column.CustomHeader != null) {
//					column.CustomHeader();
//				}
				//else {
					//Skip if the custom Column Condition fails.
					if (! column.Visible) {
						continue;
					}

					RenderHeaderCellStart(column);
					RenderText(column.Name);
					RenderHeaderCellEnd();
				}
			//}

			RenderHeadEnd();

			return true;
		}

		protected virtual void RenderRowStart(T item, bool isAlternate)
		{
			//If there's a custom delegate for rendering the start of the row, invoke it.
			//Otherwise fall back to the default rendering.
//			if(Columns.RowStartBlock != null)
//			{
//				Columns.RowStartBlock(item);
//			}
//			else if(Columns.RowStartWithAlternateBlock != null)
//			{
//				Columns.RowStartWithAlternateBlock(item, isAlternate);
//			}
//			else
//			{
				RenderRowStart(isAlternate);
//			}
		}

		protected virtual void RenderRowEnd(T item) {
			//If there's a custom delegate for rendering the end of the row, invoke it.
			//Otherwise fall back to the default rendering.
//			if (Columns.RowEndBlock != null) {
//				Columns.RowEndBlock(item);
//			}
//			else {
				RenderRowEnd();
//			}
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