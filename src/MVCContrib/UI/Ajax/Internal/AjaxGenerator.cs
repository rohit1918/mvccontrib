using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UI.Ajax.JavaScriptElements;
namespace MvcContrib.UI.Ajax.Internal
{
	using AjaxOptions = System.Web.Mvc.Ajax.AjaxOptions;

	/// <summary>
	/// Base class for Ajax functionality
	/// </summary>
	public abstract class AjaxGenerator
	{
		protected AjaxHelper AjaxHelper { get; set; }
		protected UrlHelper UrlHelper { get; private set; }

		protected AjaxGenerator(AjaxHelper ajaxHelper)
		{
			AjaxHelper = ajaxHelper;
			UrlHelper = new UrlHelper(ajaxHelper.ViewContext);
		}

		#region ActionLink

		public string ActionLink(string linkText, string actionName, AjaxOptions ajaxOptions)
		{
			return ActionLink(linkText, actionName, (string)null, ajaxOptions);
		}

		public string ActionLink(string linkText, string actionName, object values, AjaxOptions ajaxOptions)
		{
			return ActionLink(linkText, actionName, null, values, ajaxOptions);
		}

		public string ActionLink(string linkText, string actionName, string controllerName, AjaxOptions ajaxOptions)
		{
			return ActionLink(linkText, actionName, controllerName, null, ajaxOptions, null);
		}

		public string ActionLink(string linkText, string actionName, RouteValueDictionary values, AjaxOptions ajaxOptions)
		{
			return ActionLink(linkText, actionName, null, values, ajaxOptions);
		}

		public string ActionLink(string linkText, string actionName, object values, AjaxOptions ajaxOptions, object htmlAttributes)
		{
			return ActionLink(linkText, actionName, null, values, ajaxOptions, htmlAttributes);
		}

		public string ActionLink(string linkText, string actionName, string controllerName, object values, AjaxOptions ajaxOptions)
		{
			return ActionLink(linkText, actionName, controllerName, values, ajaxOptions, null);
		}

		public string ActionLink(string linkText, string actionName, string controllerName, RouteValueDictionary values, AjaxOptions ajaxOptions)
		{
			return ActionLink(linkText, actionName, controllerName, values, ajaxOptions, null);
		}

		public string ActionLink(string linkText, string actionName, RouteValueDictionary values, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
		{
			return ActionLink(linkText, actionName, null, values, ajaxOptions, htmlAttributes);
		}

		public string ActionLink(string linkText, string actionName, string controllerName, object values, AjaxOptions ajaxOptions, object htmlAttributes)
		{
			var valuesDictionary = new RouteValueDictionary(values);
			var htmlAttributesDictionary = DictionaryExtensions.AnonymousObjectToCaseSensitiveDictionary(htmlAttributes);

			return ActionLink(linkText, actionName, controllerName, valuesDictionary, ajaxOptions, htmlAttributesDictionary);
		}

		public string ActionLink(string linkText, string actionName, string controllerName, RouteValueDictionary values, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
		{
			if (string.IsNullOrEmpty(linkText)) 
			{
				throw new ArgumentNullException("linkText");
			}
			
			if (string.IsNullOrEmpty(actionName)) 
			{
				throw new ArgumentNullException("actionName");
			}

			if (ajaxOptions == null) 
			{
				throw new ArgumentNullException("ajaxOptions");
			}

			string url = CreateUrl(null, actionName, controllerName, values);
			return CreateLink(linkText, url, ajaxOptions, htmlAttributes ?? new Dictionary<string, object>());
		}

		#endregion

		#region Form

		public IDisposable Form(string actionName, AjaxOptions ajaxOptions)
		{
			return Form(actionName, (string) null, ajaxOptions);
		}

		public IDisposable Form(string actionName, object values, AjaxOptions ajaxOptions)
		{
			return Form(actionName, null, values, ajaxOptions);	
		}

		public IDisposable Form(string actionName, string controllerName, AjaxOptions ajaxOptions)
		{
			return Form(actionName, controllerName, null, ajaxOptions, null);
		}

		public IDisposable Form(string actionName, RouteValueDictionary values, AjaxOptions ajaxOptions)
		{
			return Form(actionName, null, values, ajaxOptions);
		}

		public IDisposable Form(string actionName, object values, AjaxOptions ajaxOptions, object htmlAttributes)
		{
			return Form(actionName, null, values, ajaxOptions, htmlAttributes);
		}

		public IDisposable Form(string actionName, string controllerName, object values, AjaxOptions ajaxOptions)
		{
			return Form(actionName, controllerName, values, ajaxOptions, null);	
		}

		public IDisposable Form(string actionName, string controllerName, RouteValueDictionary values, AjaxOptions ajaxOptions)
		{
			return Form(actionName, controllerName, values, ajaxOptions, null);
		}

		public IDisposable Form(string actionName, RouteValueDictionary values, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
		{
			return Form(actionName, null, values, ajaxOptions, htmlAttributes);
		}

		public IDisposable Form(string actionName, string controllerName, object values, AjaxOptions ajaxOptions, object htmlAttributes)
		{
			var valuesDictionary = new RouteValueDictionary(values);
			Dictionary<string, object> attributesDictionary = DictionaryExtensions.AnonymousObjectToCaseSensitiveDictionary(htmlAttributes);
			return Form(actionName, controllerName, valuesDictionary, ajaxOptions, attributesDictionary);
		}

		public virtual IDisposable Form(string actionName, string controllerName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
		{
			if (string.IsNullOrEmpty(actionName)) 
			{
				throw new ArgumentNullException("actionName");
			}
			
			if (ajaxOptions == null) 
			{
				throw new ArgumentNullException("ajaxOptions");
			}

			string url = CreateUrl(null, actionName, controllerName, valuesDictionary);
			var tagBuilder = CreateFormTag(url, ajaxOptions, htmlAttributes);

			return new DisposableElement(AjaxHelper.ViewContext.HttpContext.Response.Output, tagBuilder);
		}

		#endregion

		#region RouteLink

		public string RouteLink(string linkText, object values, AjaxOptions ajaxOptions)
		{
			return RouteLink(linkText, null, new RouteValueDictionary(values), ajaxOptions, new Dictionary<string, object>());
		}

		public string RouteLink(string linkText, string routeName, AjaxOptions ajaxOptions)
		{
			return RouteLink(linkText, routeName, new RouteValueDictionary(), ajaxOptions, new Dictionary<string, object>());
		}
		
		public string RouteLink(string linkText, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions)
		{
			return RouteLink(linkText, null, valuesDictionary, ajaxOptions, new Dictionary<string, object>());
		}

		public string RouteLink(string linkText, object values, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
		{
			return RouteLink(linkText, null, new RouteValueDictionary(values), ajaxOptions, htmlAttributes);
		}

		public string RouteLink(string linkText, object values, AjaxOptions ajaxOptions, object htmlAttributes)
		{
			var htmlAttributesDictionary = DictionaryExtensions.AnonymousObjectToCaseSensitiveDictionary(htmlAttributes);
			return RouteLink(linkText, null, new RouteValueDictionary(values), ajaxOptions, htmlAttributesDictionary);
		}

		public string RouteLink(string linkText, string routeName, object values, AjaxOptions ajaxOptions)
		{
			return RouteLink(linkText, routeName, new RouteValueDictionary(values), ajaxOptions, new Dictionary<string, object>());
		}

		public string RouteLink(string linkText, string routeName, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
		{
			return RouteLink(linkText, routeName, new RouteValueDictionary(), ajaxOptions, htmlAttributes);
		}

		public string RouteLink(string linkText, string routeName, AjaxOptions ajaxOptions, object htmlAttributes)
		{
			var htmlAttributesDictionary = DictionaryExtensions.AnonymousObjectToCaseSensitiveDictionary(htmlAttributes);
			return RouteLink(linkText, routeName, new RouteValueDictionary(), ajaxOptions, htmlAttributesDictionary);
		}

		public string RouteLink(string linkText, string routeName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions)
		{
			return RouteLink(linkText, routeName, valuesDictionary, ajaxOptions, new Dictionary<string, object>());
		}

		public string RouteLink(string linkText, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
		{
			return RouteLink(linkText, null, valuesDictionary, ajaxOptions, htmlAttributes);
		}

		public string RouteLink(string linkText, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, object htmlAttributes)
		{
			var htmlAttributesDictionary = DictionaryExtensions.AnonymousObjectToCaseSensitiveDictionary(htmlAttributes);
			return RouteLink(linkText, null, valuesDictionary, ajaxOptions, htmlAttributesDictionary);
		}

		public string RouteLink(string linkText, string routeName, object values, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
		{
			return RouteLink(linkText, routeName, new RouteValueDictionary(values), ajaxOptions, htmlAttributes);
		}

		public string RouteLink(string linkText, string routeName, object values, AjaxOptions ajaxOptions, object htmlAttributes)
		{
			var htmlAttributesDictionary = DictionaryExtensions.AnonymousObjectToCaseSensitiveDictionary(htmlAttributes);
			return RouteLink(linkText, routeName, new RouteValueDictionary(values), ajaxOptions, htmlAttributesDictionary);
		}

		public string RouteLink(string linkText, string routeName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, object htmlAttributes) 
		{
			var htmlAttributesDictionary = DictionaryExtensions.AnonymousObjectToCaseSensitiveDictionary(htmlAttributes);
			return RouteLink(linkText, routeName, valuesDictionary, ajaxOptions, htmlAttributesDictionary);
		}

		public string RouteLink(string linkText, string routeName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
		{
			if (string.IsNullOrEmpty(linkText)) 
			{
				throw new ArgumentNullException("linkText");
			}
			
			if (ajaxOptions == null) 
			{
				throw new ArgumentNullException("ajaxOptions");
			}

			string url = CreateUrl(routeName, null, null, valuesDictionary);
			return CreateLink(linkText, url, ajaxOptions, htmlAttributes);
		}

		#endregion

		/// <summary>
		/// Creates a hyperlink to the specified URL using Ajax.
		/// </summary>
		/// <param name="linkText">The link text</param>
		/// <param name="targetUrl">The url to link to</param>
		/// <param name="ajaxOptions">AJAX options</param>
		/// <param name="htmlAttributes">Additional HTML attributes</param>
		/// <returns>A string containing the hyperlink</returns>
		public abstract string CreateLink(string linkText, string targetUrl, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes);
		
		public abstract TagBuilder CreateFormTag(string url, AjaxOptions options, IDictionary<string, object> htmlAttributes);

		public abstract bool IsMvcAjaxRequest();

		protected virtual string CreateUrl(string routeName, string action, string controller, RouteValueDictionary values)
		{
			values = values ?? new RouteValueDictionary();

			if(action != null)
			{
				if(controller != null)
				{
					return UrlHelper.Action(action, controller, values);
				}

				return UrlHelper.Action(action, values);
			}

			if(routeName != null)
			{
				return UrlHelper.RouteUrl(routeName, values);
			}

			return UrlHelper.RouteUrl(values);
		}

	}
}