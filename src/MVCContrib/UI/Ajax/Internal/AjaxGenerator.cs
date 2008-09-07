using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcContrib.UI.Ajax.Internal
{
	/// <summary>
	/// Base class for Ajax functionality
	/// </summary>
	public abstract class AjaxGenerator
	{
		protected AjaxHelper AjaxHelper { get; set; }

		protected AjaxGenerator(AjaxHelper ajaxHelper)
		{
			AjaxHelper = ajaxHelper;
		}

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

		public virtual string ActionLink(string linkText, string actionName, string controllerName, RouteValueDictionary values, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
		{
			throw new NotImplementedException();
		}

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

		public abstract IDisposable Form(string actionName, string controllerName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes);

		public abstract bool IsMvcAjaxRequest();

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

		public abstract string RouteLink(string linkText, string routeName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes);
	}
}