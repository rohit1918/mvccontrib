using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using MvcContrib.Pagination;
using MvcContrib.UI.Html.Grid.Legacy;

namespace MvcContrib.UI.Html.Grid
{
	/// <summary>
	/// Implementation of the grid for rendering HTML in a gridview style.
	/// </summary>
	/// <typeparam name="T">Type of object to be rendered in each row.</typeparam>
	public class Grid<T> : GridBase<T> where T : class
	{
		private const string Default_Css_Class = "grid";

		protected override void Render() 
		{
			if (!Attributes.Contains("class"))
			{
				Attributes["class"] = Default_Css_Class;
			}
			base.Render();
		}

		protected override void RenderHeaderCellEnd()
		{
			RenderText("</th>");
		}

		protected override void RenderHeaderCellStart(GridColumn<T> column)
		{
			string attrs = BuildHtmlAttributes(column.HeaderAttributes);
			if (attrs.Length > 0)
				attrs = " " + attrs;

			RenderText(string.Format("<th{0}>", attrs));
		}

		protected override void RenderRowStart(bool isAlternate)
		{
			if(isAlternate)
			{
				RenderText("<tr class=\"gridrow_alternate\">");				
			}
			else
			{
				RenderText("<tr class=\"gridrow\">");
			}
		}

		protected override void RenderRowEnd()
		{
			RenderText("</tr>");
		}

		protected override void RenderEndCell()
		{
			RenderText("</td>");
		}

		protected override void RenderStartCell(GridColumn<T> column)
		{
			RenderText("<td>");
		}

		protected override void RenderHeadStart()
		{
			RenderText("<thead><tr>");
		}

		protected override void RenderHeadEnd()
		{
			RenderText("</tr></thead>");
		}

		protected override void RenderGridStart()
		{
			string attrs = BuildHtmlAttributes(this.Attributes);
			if (attrs.Length > 0)
				attrs = " " + attrs;

			RenderText(string.Format("<table{0}>", attrs));
		}

		protected override void RenderGridEnd(bool isEmpty)
		{
			RenderText("</table>");
			
			if(!isEmpty)
			{
				var pagination = Items as IPagination;
				if (pagination != null) {
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
			if (pagedList.PageSize == 1)
			{
				builder.AppendFormat(Options.PaginationSingleFormat, pagedList.FirstItem, pagedList.TotalItems);
			}
			else
			{
				builder.AppendFormat(Options.PaginationFormat, pagedList.FirstItem, pagedList.LastItem, pagedList.TotalItems);
			}
			builder.Append("</span>");
			builder.Append("<span class='paginationRight'>");

			if (pagedList.PageNumber == 1)
			{
				builder.Append(Options.PaginationFirst);
			}
			else
			{
				builder.Append(CreatePageLink(1, Options.PaginationFirst));
			}

			builder.Append(" | ");

			if (pagedList.HasPreviousPage)
			{
				builder.Append(CreatePageLink(pagedList.PageNumber - 1, Options.PaginationPrev));
			}
			else
			{
				builder.Append(Options.PaginationPrev);
			}


			builder.Append(" | ");

			if (pagedList.HasNextPage)
			{
				builder.Append(CreatePageLink(pagedList.PageNumber + 1, Options.PaginationNext));
			}
			else
			{
				builder.Append(Options.PaginationNext);
			}


			builder.Append(" | ");

			int lastPage = pagedList.TotalPages;

			if (pagedList.PageNumber < lastPage)
			{
				builder.Append(CreatePageLink(lastPage, Options.PaginationLast));
			}
			else
			{
				builder.Append(Options.PaginationLast);
			}


			builder.Append(@"</span></div>");


			RenderText(builder.ToString());
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
			return string.Format("<a href=\"{0}?{1}={2}{3}\">{4}</a>", filePath, Options.PageQueryName, pageNumber, queryString, text);
		}

		protected virtual string CreateQueryString(NameValueCollection values)
		{
			var builder = new StringBuilder();

			foreach(string key in values.Keys)
			{
				if(key == "page") //Don't re-add any existing 'page' variable to the querystring - this will be handled in CreatePageLink.
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
			RenderText("<tr><td>" + Options.EmptyMessageText + "</td></tr>");
		}


		/// <summary>
		/// Converts the specified attributes dictionary of key-value pairs into a string of HTML attributes. 
		/// </summary>
		/// <returns></returns>
		private static string BuildHtmlAttributes(IDictionary attributes)
		{
			if (attributes == null || attributes.Count == 0)
			{
				return string.Empty;
			}

			var attributeSB = new StringBuilder();

			foreach (DictionaryEntry entry in attributes)
			{
				attributeSB.AppendFormat("{0}=\"{1}\"", entry.Key, entry.Value);
				attributeSB.Append(' ');
			}

			return attributeSB.ToString().Trim();
		}
	}
}
