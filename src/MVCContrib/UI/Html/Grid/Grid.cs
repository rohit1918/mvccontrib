using System.Linq;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

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

		protected override void RenderRowStart()
		{
			RenderText("<tr>");
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

		protected override void RenderGridEnd()
		{
			RenderText("</table>");
			//TODO: Implement pagination here using IPagination and Pagination<T> 
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