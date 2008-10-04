using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MvcContrib.UI.Ajax.JavaScriptElements;
namespace MvcContrib.UI.Ajax.Internal
{
	using AjaxOptions = System.Web.Mvc.Ajax.AjaxOptions;

	/// <summary>
	/// AjaxGenerator implementation for the Prototype JS library.
	/// </summary>
	public class PrototypeAjaxGenerator : AjaxGenerator
	{
		private const string _confirmFormat = "if(confirm('{0}')) {{ {1} }}";

		/// <summary>
		/// Creates a new instance of the PrototypeAjaxGenerator class
		/// </summary>
		/// <param name="ajaxHelper">An instance of the AjaxHelper being used</param>
		public PrototypeAjaxGenerator(AjaxHelper ajaxHelper) : base(ajaxHelper)
		{
		}

		public override string CreateLink(string linkText, string targetUrl, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
		{
			var tag = new TagBuilder("a") {InnerHtml = HttpUtility.HtmlEncode(linkText)};
			tag.MergeAttributes(htmlAttributes);
			tag.MergeAttribute("href", targetUrl);
			tag.MergeAttribute("onclick", CreateAjaxScriptForLink(targetUrl, ajaxOptions));
			return tag.ToString();
		}

		public override TagBuilder CreateFormTag(string url, AjaxOptions options, IDictionary<string, object> htmlAttributes)
		{
			throw new NotImplementedException();
		}

		public override bool IsMvcAjaxRequest()
		{
			return AjaxHelper.ViewContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
		}

		/// <summary>
		/// Creates the script needed to ajaxify a hyperlink.
		/// </summary>
		/// <returns>A string that can be used on the onClick event for a hyperlink</returns>
		public string CreateAjaxScriptForLink(string url, AjaxOptions options)
		{
			if(string.IsNullOrEmpty(options.Url))
			{
				options.Url = url;
			}

			FunctionCall function;

			bool useUpdater = !string.IsNullOrEmpty(options.UpdateTargetId);

			if(useUpdater)
			{
				function = new FunctionCall("new Ajax.Updater", options.UpdateTargetId, options.Url, AjaxOptionsToJavaScript(options));
			}
			else
			{
				function = new FunctionCall("new Ajax.Request", options.Url, AjaxOptionsToJavaScript(options));
			}

			return WrapInConfirmIfSpecified(options, function);
		}

		private string WrapInConfirmIfSpecified(AjaxOptions options, IJavaScriptElement function)
		{
			if(!string.IsNullOrEmpty(options.Confirm))
			{
				return string.Format(_confirmFormat, options.Confirm, function.ToJavaScript());				
			}
			return function.ToJavaScript();
		}

		protected IJavaScriptElement AjaxOptionsToJavaScript(AjaxOptions options)
		{
			var optionsDict = new JavaScriptDictionary();

			//The LoadingElementId property should be ignored if onCreate/onComplete are explicitly specified. 
			if(!string.IsNullOrEmpty(options.LoadingElementId)
			   && string.IsNullOrEmpty(options.OnBegin)
			   && string.IsNullOrEmpty(options.OnComplete))
			{
				optionsDict["onCreate"] = new AnonymousFunction(string.Format("$('{0}').show();", options.LoadingElementId));
				optionsDict["onComplete"] = new AnonymousFunction(string.Format("$('{0}').hide();", options.LoadingElementId));
			}
			else
			{
				optionsDict["onCreate"] = new JavaScriptLiteral(options.OnBegin);
				optionsDict["onComplete"] = new JavaScriptLiteral(options.OnComplete);
			}

			optionsDict["onSuccess"] = new JavaScriptLiteral(options.OnSuccess);
			optionsDict["onFailure"] = new JavaScriptLiteral(options.OnFailure);
			optionsDict["method"] = options.HttpMethod;

			if(options.InsertionMode == InsertionMode.InsertAfter)
			{
				optionsDict["insertion"] = new JavaScriptLiteral("Insertion.After");
			}
			else if(options.InsertionMode == InsertionMode.InsertBefore)
			{
				optionsDict["insertion"] = new JavaScriptLiteral("Insertion.Before");
			}

			return optionsDict;
		}
	}
}