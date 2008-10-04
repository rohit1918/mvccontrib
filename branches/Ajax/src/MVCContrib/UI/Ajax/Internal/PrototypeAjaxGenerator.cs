using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MvcContrib.UI.Ajax.JavaScriptElements;

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

		public override IJavaScriptElement MakeAjaxOptionsFrameworkSpecific(AjaxOptions options)
		{
			return new PrototypeAjaxOptionsWrapper(options);
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