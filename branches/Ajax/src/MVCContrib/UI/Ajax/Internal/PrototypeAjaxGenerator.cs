using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MvcContrib.UI.Ajax.Internal
{
	/// <summary>
	/// AjaxGenerator implementation for the Prototype JS library.
	/// </summary>
	public class PrototypeAjaxGenerator : AjaxGenerator
	{
		/// <summary>
		/// Creates a new instance of the PrototypeAjaxGenerator class
		/// </summary>
		/// <param name="ajaxHelper">An instance of the AjaxHelper being used</param>
		public PrototypeAjaxGenerator(AjaxHelper ajaxHelper) : base(ajaxHelper)
		{
		}

		/// <summary>
		/// Creates a hyperlink to the specified URL using Ajax.
		/// </summary>
		/// <param name="linkText">The link text</param>
		/// <param name="targetUrl">The url to link to</param>
		/// <param name="ajaxOptions">AJAX options</param>
		/// <param name="htmlAttributes">Additional HTML attributes</param>
		/// <returns>A string containing the hyperlink</returns>
		public override string CreateLink(string linkText, string targetUrl, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
		{
			var tag = new TagBuilder("a") { InnerHtml = HttpUtility.HtmlEncode(linkText) };
			tag.MergeAttributes(htmlAttributes);
			tag.MergeAttribute("href", targetUrl);
			//tag.MergeAttribute("onclick", "foo");
			throw new NotImplementedException();
//			return tag.ToString();
		}

		public override TagBuilder CreateFormTag(string url, AjaxOptions options, IDictionary<string, object> htmlAttributes)
		{
			throw new System.NotImplementedException();
		}

		public override bool IsMvcAjaxRequest()
		{
			return AjaxHelper.ViewContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
		}
	}
}