using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Renders a grid as an HTML table.
	/// </summary>
	public class HtmlTableGridRenderer<T> : GridRenderer<T> where T: class 
	{
		private const string DefaultCssClass = "grid";

		public HtmlTableGridRenderer(ViewEngineCollection engines) : base(engines)
		{
			
		}
		public HtmlTableGridRenderer() {}

		protected override void RenderHeaderCellEnd()
		{
			RenderText("</th>");
		}

		protected override void RenderHeaderCellStart(GridColumn<T> column)
		{
			string attrs = BuildHtmlAttributes(column.HeaderAttributes);
			if(attrs.Length > 0)
				attrs = " " + attrs;

			RenderText(string.Format("<th{0}>", attrs));
		}

		protected override void RenderRowStart(GridRowViewData<T> rowData)
		{
			if(rowData.IsAlternate)
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

		protected override void RenderStartCell(GridColumn<T> column, GridRowViewData<T> rowData)
		{
			string attrs = BuildHtmlAttributes(column.Attributes(rowData));
			if (attrs.Length > 0)
				attrs = " " + attrs;

			RenderText(string.Format("<td{0}>", attrs));
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
			if(! GridModel.Attributes.ContainsKey("class"))
			{
				GridModel.Attributes["class"] = DefaultCssClass;
			}

			string attrs = BuildHtmlAttributes(GridModel.Attributes);

			if(attrs.Length > 0)
				attrs = " " + attrs;

			RenderText(string.Format("<table{0}>", attrs));
		}

		protected override void RenderGridEnd(bool isEmpty)
		{
			RenderText("</table>");
		}

		protected override void RenderEmpty()
		{
			RenderText("<tr><td>" + GridModel.EmptyText + "</td></tr>");
		}

		/// <summary>
		/// Converts the specified attributes dictionary of key-value pairs into a string of HTML attributes. 
		/// </summary>
		/// <returns></returns>
		private static string BuildHtmlAttributes(IDictionary<string, object> attributes)
		{
			if(attributes == null || attributes.Count == 0)
			{
				return string.Empty;
			}

			const string attributeFormat = "{0}=\"{1}\"";

			return string.Join(" ",
                   attributes.Select(pair => string.Format(attributeFormat, pair.Key, pair.Value)).ToArray()
			);
		}
	}
}