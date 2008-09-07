using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using MvcContrib.Pagination;

namespace MvcContrib.UI.Html.Grid
{
	/// <summary>
	/// Implementation of the grid for rendering HTML in a gridview style.
	/// </summary>
	/// <typeparam name="T">Type of object to be rendered in each row.</typeparam>
	public class Grid<T> : GridBase<T> where T : class
	{
		private const string Default_Css_Class = "grid";
		private const string Empty_Text_Key = "empty";

		/// <summary>
		/// Custom HTML attributes.
		/// </summary>
		public IDictionary HtmlAttributes { get; private set; }
		/// <summary>
		/// The HTTP Context.
		/// </summary>
		public HttpContextBase Context { get; set; }

		/// <summary>
		/// Creates a new instance of the <see cref="Grid{T}"/> class using the specified viewDataKey to extract the data source from the viewdata.
		/// </summary>
		/// <param name="viewDataKey">Key to use to extract the </param>
		/// <param name="viewContext">The view context</param>
		/// <param name="columns">Columns</param>
		/// <param name="htmlAttributes">Custom html attributes and options</param>
		/// <param name="writer">Where to write the output</param>
		public Grid(string viewDataKey, ViewContext viewContext, GridColumnBuilder<T> columns, IDictionary htmlAttributes, TextWriter writer) 
			: this(GetDataSourceFromViewData(viewDataKey, viewContext), columns, htmlAttributes, writer, viewContext.HttpContext)
		{
				
		}

		protected static IEnumerable<T> GetDataSourceFromViewData(string key, ViewContext context)
		{
			object items = new DefaultDataBinder().ExtractValue(key, context);
			IEnumerable<T> collection = null;

			if (items != null)
			{
				//First try as IEnumerable of T
				collection = items as IEnumerable<T>;

				//Otherwise try IEnumerable with a cast.
				//TODO: error handling?
				if (collection == null)
				{
					collection = (items as IEnumerable).Cast<T>();
				}
			}

			return collection;
		}


		/// <summary>
		/// Creates a new instance of the <see cref="Grid{T}"/> using the specified data source.
		/// </summary>
		/// <param name="items">Data source</param>
		/// <param name="columns">Columns</param>
		/// <param name="htmlAttributes">Custom attributes and options</param>
		/// <param name="writer">Where to write the output</param>
		/// <param name="context">HTTP Context</param>
		public Grid(IEnumerable<T> items, GridColumnBuilder<T> columns, IDictionary htmlAttributes, TextWriter writer, HttpContextBase context)
			: base(items, columns, writer)
		{
			Context = context;
			HtmlAttributes = htmlAttributes ?? Hash.Empty;

			if (!HtmlAttributes.Contains("class"))
			{
				HtmlAttributes["class"] = Default_Css_Class;
			}

			if(HtmlAttributes.Contains(Empty_Text_Key))
			{
				EmptyMessageText = HtmlAttributes[Empty_Text_Key] as string;
				HtmlAttributes.Remove(Empty_Text_Key);
			}
		}

		protected override void RenderHeaderCellEnd()
		{
			RenderText("</th>");
		}

		protected override void RenderHeaderCellStart()
		{
			RenderText("<th>");
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
			string attrs = BuildHtmlAttributes();
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
				builder.AppendFormat("Showing {0} of {1} ", pagedList.FirstItem, pagedList.TotalItems);
			}
			else
			{
				builder.AppendFormat("Showing {0} - {1} of {2} ", pagedList.FirstItem, pagedList.LastItem, pagedList.TotalItems);
			}
			builder.Append("</span>");
			builder.Append("<span class='paginationRight'>");

			if (pagedList.PageNumber == 1)
			{
				builder.Append("first");
			}
			else
			{
				builder.Append(CreatePageLink(1, "first"));
			}

			builder.Append(" | ");

			if (pagedList.HasPreviousPage)
			{
				builder.Append(CreatePageLink(pagedList.PageNumber - 1, "prev"));
			}
			else
			{
				builder.Append("prev");
			}


			builder.Append(" | ");

			if (pagedList.HasNextPage)
			{
				builder.Append(CreatePageLink(pagedList.PageNumber + 1, "next"));
			}
			else
			{
				builder.Append("next");
			}


			builder.Append(" | ");

			int lastPage = pagedList.TotalPages;

			if (pagedList.PageNumber < lastPage)
			{
				builder.Append(CreatePageLink(lastPage, "last"));
			}
			else
			{
				builder.Append("last");
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
			string queryString = CreateQueryString(Context.Request.QueryString);
			string filePath = Context.Request.FilePath;
			return string.Format("<a href=\"{0}?page={1}{2}\">{3}</a>", filePath, pageNumber, queryString, text);
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
			RenderText("<tr><td>" + EmptyMessageText + "</td></tr>");
		}


		/// <summary>
		/// Converts the HtmlAttributes dictionary of key-value pairs into a string of HTML attributes. 
		/// </summary>
		/// <returns></returns>
		private string BuildHtmlAttributes()
		{
			if (HtmlAttributes.Count == 0)
			{
				return string.Empty;
			}

			var attributes = new StringBuilder();

			foreach (DictionaryEntry entry in HtmlAttributes)
			{
				attributes.AppendFormat("{0}=\"{1}\"", entry.Key, entry.Value);
				attributes.Append(' ');
			}

			return attributes.ToString().Trim();
		}
	}
}
