using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web.Mvc;
using MvcContrib.Pagination;

namespace MvcContrib.UI.Html.Grid
{
	public class HtmlGridRenderer<T> : GridRendererBase<T> where T : class
	{
		private const string Default_Css_Class = "grid";

		protected override void RenderHeaderCellEnd()
		{
			Writer.Write("</th>");
		}

		protected override void Render()
		{
			if(!HtmlAttributes.Contains("class"))
			{
				HtmlAttributes["class"] = Default_Css_Class;
			}
			base.Render();
		}

		protected override void RenderHeaderCellStart(IGridColumn<T> column)
		{
			string attrs = BuildHtmlAttributes(column.HeaderAttributes);
			if(attrs.Length > 0)
				attrs = " " + attrs;

			Writer.Write(string.Format("<th{0}>", attrs));
		}

		protected override void RenderRowStart(bool isAlternate)
		{
			if(isAlternate)
			{
				Writer.Write("<tr class=\"gridrow_alternate\">");
			}
			else
			{
				Writer.Write("<tr class=\"gridrow\">");
			}
		}

		protected override void RenderRowEnd()
		{
			Writer.Write("</tr>");
		}

		protected override void RenderEndCell()
		{
			Writer.Write("</td>");
		}

		protected override void RenderStartCell(IGridColumn<T> column)
		{
			Writer.Write("<td>");
		}

		protected override void RenderHeadStart()
		{
			Writer.Write("<thead><tr>");
		}

		protected override void RenderHeadEnd()
		{
			Writer.Write("</tr></thead>");
		}

		protected override void RenderGridStart()
		{
			string attrs = BuildHtmlAttributes(HtmlAttributes);
			if(attrs.Length > 0)
				attrs = " " + attrs;

			Writer.Write(string.Format("<table{0}>", attrs));
		}

		protected override void RenderGridEnd(bool isEmpty)
		{
			Writer.Write("</table>");

			if(!isEmpty)
			{
				var pagination = DataSource as IPagination;
				if(pagination != null)
				{
					RenderPagination(pagination);
				}
			}
		}

		/// <summary>
		/// Renders the pagination section of the grid.
		/// Eg "Showing 1 - 10 of 20 | last, prev, next, last"
		/// </summary>
		/// <param name="pagedList"></param>
		protected virtual void RenderPagination(IPagination pagedList)
		{
			var builder = new StringBuilder();
			builder.Append("<div class='pagination'>");
			builder.Append("<span class='paginationLeft'>");
			if(pagedList.PageSize == 1)
			{
				builder.AppendFormat(Options.PaginationSingleFormat, pagedList.FirstItem, pagedList.TotalItems);
			}
			else
			{
				builder.AppendFormat(Options.PaginationFormat, pagedList.FirstItem, pagedList.LastItem, pagedList.TotalItems);
			}
			builder.Append("</span>");
			builder.Append("<span class='paginationRight'>");

			if(pagedList.PageNumber == 1)
			{
				builder.Append(Options.PaginationFirst);
			}
			else
			{
				builder.Append(CreatePageLink(1, Options.PaginationFirst));
			}

			builder.Append(" | ");

			if(pagedList.HasPreviousPage)
			{
				builder.Append(CreatePageLink(pagedList.PageNumber - 1, Options.PaginationPrev));
			}
			else
			{
				builder.Append(Options.PaginationPrev);
			}


			builder.Append(" | ");

			if(pagedList.HasNextPage)
			{
				builder.Append(CreatePageLink(pagedList.PageNumber + 1, Options.PaginationNext));
			}
			else
			{
				builder.Append(Options.PaginationNext);
			}


			builder.Append(" | ");

			int lastPage = pagedList.TotalPages;

			if(pagedList.PageNumber < lastPage)
			{
				builder.Append(CreatePageLink(lastPage, Options.PaginationLast));
			}
			else
			{
				builder.Append(Options.PaginationLast);
			}


			builder.Append(@"</span></div>");


			Writer.Write(builder.ToString());
		}

		/// <summary>
		/// Creates a pagination link and includes and curren
		/// </summary>
		/// <param name="pageNumber"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		protected virtual string CreatePageLink(int pageNumber, string text)
		{
			string queryString = CreateQueryString(Context.HttpContext.Request.QueryString);
			string filePath = Context.HttpContext.Request.FilePath;
			return string.Format("<a href=\"{0}?{1}={2}{3}\">{4}</a>", filePath, Options.PageQueryName, pageNumber, queryString,
			                     text);
		}

		protected virtual string CreateQueryString(NameValueCollection values)
		{
			var builder = new StringBuilder();

			foreach(string key in values.Keys)
			{
				if(key == "page")
					//Don't re-add any existing 'page' variable to the querystring - this will be handled in CreatePageLink.
				{
					continue;
				}

				foreach(var value in values.GetValues(key))
				{
					builder.AppendFormat("&amp;{0}={1}", key, value);
				}
			}

			return builder.ToString();
		}

		protected override void RenderEmpty()
		{
			Writer.Write("<tr><td>" + Options.EmptyMessageText + "</td></tr>");
		}


		/// <summary>
		/// Converts the specified attributes dictionary of key-value pairs into a string of HTML attributes. 
		/// </summary>
		/// <returns></returns>
		private static string BuildHtmlAttributes(IDictionary attributes)
		{
			if(attributes == null || attributes.Count == 0)
			{
				return string.Empty;
			}

			var attributeSB = new StringBuilder();

			foreach(DictionaryEntry entry in attributes)
			{
				attributeSB.AppendFormat("{0}=\"{1}\"", entry.Key, entry.Value);
				attributeSB.Append(' ');
			}

			return attributeSB.ToString().Trim();
		}
	}

	public abstract class GridRendererBase<T> : IGridRenderer<T> where T : class
	{
		public IEnumerable<T> DataSource { get; private set; }
		public GridColumnBuilder<T> Columns { get; private set; }
		public GridOptions Options { get; private set; }
		public IDictionary HtmlAttributes { get; private set; }
		public ViewContext Context { get; private set; }

		protected virtual TextWriter Writer
		{
			get { return Context.HttpContext.Response.Output; }
		}

		public void Render(IEnumerable<T> dataSource, GridColumnBuilder<T> columns, GridOptions options,
		                   IDictionary htmlAttributes, ViewContext context)
		{
			DataSource = dataSource;
			Columns = columns;
			Options = options;
			HtmlAttributes = htmlAttributes;
			Context = context;

			Options.EmptyMessageText = Options.EmptyMessageText ?? "There is no data available.";

			Render();
		}

		protected virtual void Render()
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

			RenderGridEnd(!headerRendered);
		}

		/// <summary>
		/// Iterates over the items collection, builds up the markup for each row and outputs the results.
		/// </summary>
		protected virtual void RenderItems()
		{
			bool isAlternate = false;
			foreach(var item in DataSource)
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
			if(DataSource == null)
			{
				return false;
			}

			IEnumerator<T> enumerator = DataSource.GetEnumerator();

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