using System;
using System.Collections.Generic;
using System.IO;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Renders a grid as an HTML table.
	/// </summary>
	public class HtmlTableGridRenderer<T> : GridRenderer<T>
	{
		protected override void RenderGridStart()
		{
			throw new NotImplementedException();
		}

		protected override void RenderGridEnd(bool isEmpty)
		{
			throw new NotImplementedException();
		}

		protected override void RenderEmpty()
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Base class for Grid Renderers. 
	/// </summary>
	public abstract class GridRenderer<T> : IGridRenderer<T>
	{
		public void Render(IGridModel<T> gridModel, IEnumerable<T> dataSource, TextWriter output)
		{
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


		protected virtual void RenderItems()
		{
			throw new NotImplementedException();
		}


		protected virtual bool RenderHeader() {
			/*//No items - do not render a header.
			if (Items == null) {
				return false;
			}

			IEnumerator<T> enumerator = Items.GetEnumerator();

			//No items - do not render header.
			if (!enumerator.MoveNext()) {
				return false;
			}

			RenderHeadStart();

			foreach (var column in Columns) {
				//Allow for custom header overrides.
				if (column.CustomHeader != null) {
					column.CustomHeader();
				}
				else {
					//Skip if the custom Column Condition fails.
					if (column.ColumnCondition != null && !column.ColumnCondition()) {
						continue;
					}

					RenderHeaderCellStart(column);

					RenderText(column.Name);

					RenderHeaderCellEnd();
				}
			}

			RenderHeadEnd();

			return true;*/
			throw new NotImplementedException();
		}

		protected abstract void RenderGridStart();
		protected abstract void RenderGridEnd(bool isEmpty);
		protected abstract void RenderEmpty();
	}


}