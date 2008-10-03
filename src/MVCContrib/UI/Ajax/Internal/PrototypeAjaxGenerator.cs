using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

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

		public override string AjaxOptionsToString(AjaxOptions options)
		{
			var builder = new StringBuilder();

			if(!string.IsNullOrEmpty(options.Confirm))
			{
				builder.AppendFormat("if(confirm('{0}')) {{ ", options.Confirm);
			}

			if(string.IsNullOrEmpty(options.UpdateTargetId))
			{
				builder.AppendFormat("new Ajax.Request('{0}', {{", options.Url);				
			}
			else
			{
				builder.AppendFormat("new Ajax.Updater('{0}', '{1}', {{", options.UpdateTargetId, options.Url);

				if(options.InsertionMode == InsertionMode.InsertAfter)
				{
					builder.Append("insertion:Insertion.After, ");
				}
				else if(options.InsertionMode == InsertionMode.InsertBefore)
				{
					builder.Append("insertion:Insertion.Before, ");
				}
			}

			if(!string.IsNullOrEmpty(options.LoadingElementId) && string.IsNullOrEmpty(options.OnBegin) && string.IsNullOrEmpty(options.OnComplete))
			{
				builder.AppendFormat("onCreate:function() {{ $('{0}').show(); }}, onComplete:function() {{ $('{0}').hide(); }}, ", options.LoadingElementId);
			}

			if(!string.IsNullOrEmpty(options.OnBegin))
			{
				builder.AppendFormat("onCreate:{0}, ", options.OnBegin);
			}

			if(!string.IsNullOrEmpty(options.OnComplete))
			{
				builder.AppendFormat("onComplete:{0}, ", options.OnComplete);
			}

			if(!string.IsNullOrEmpty(options.OnSuccess))
			{
				builder.AppendFormat("onSuccess:{0}, ", options.OnSuccess);
			}

			if(!string.IsNullOrEmpty(options.OnFailure))
			{
				builder.AppendFormat("onFailure:{0}, ", options.OnFailure);				
			}

			if(!string.IsNullOrEmpty(options.HttpMethod))
			{
				builder.AppendFormat("method:'{0}', ", options.HttpMethod);
			}

			if(builder[builder.Length - 2] == ',')
			{
				builder.Remove(builder.Length - 2, 2);
			}

			builder.Append("});");

			if(!string.IsNullOrEmpty(options.Confirm))
			{
				builder.Append(" }");
			}

			return builder.ToString();
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