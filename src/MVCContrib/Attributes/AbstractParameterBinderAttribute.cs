﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using MvcContrib.MetaData;
using System.Web;
using System.Web.Routing;

namespace MvcContrib.Attributes
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public abstract class AbstractParameterBinderAttribute : CustomModelBinderAttribute, IModelBinder
	{
		public AbstractParameterBinderAttribute()
			: this(null)
		{
		}

		public AbstractParameterBinderAttribute(string prefix)
			: this(prefix, RequestStore.All)
		{
		}

		public AbstractParameterBinderAttribute(string prefix, RequestStore requestStore)
		{
			_prefix = prefix;
			_requestStore = requestStore;
		}

		private readonly string _prefix;
		public string Prefix
		{
			get { return _prefix; }
		}

		private readonly RequestStore _requestStore;
		public RequestStore RequestStore
		{
			get { return _requestStore; }
		}

		public override IModelBinder GetBinder()
		{
			return this;
		}

		public virtual NameValueCollection GetStore(ControllerContext controllerContext)
		{
			NameValueCollection store = null;

			switch (RequestStore)
			{
				case RequestStore.QueryString:
					store = controllerContext.HttpContext.Request.QueryString;
					break;
				case RequestStore.Form:
					store = controllerContext.HttpContext.Request.Form;
					break;
				case RequestStore.Cookies:
					store = CreateStoreFromCookies(controllerContext.HttpContext.Request.Cookies);
					break;
				case RequestStore.ServerVariables:
					store = controllerContext.HttpContext.Request.ServerVariables;
					break;
				case RequestStore.Params:
					store = controllerContext.HttpContext.Request.Params;
					break;
				case RequestStore.TempData:
					store = CreateStoreFromDictionary(controllerContext.Controller.TempData);
					break;
				case RequestStore.RouteData:
					store = CreateStoreFromDictionary(controllerContext.RouteData.Values);
					break;
				case RequestStore.All:
					store = CreateStoreFromAll(controllerContext.HttpContext.Request.Params, controllerContext.Controller.TempData, controllerContext.RouteData);
					break;
			}

			return store;
		}

		public virtual NameValueCollection CreateStoreFromCookies(HttpCookieCollection cookies)
		{
			var cookieCount = cookies.Count;
			var store = new NameValueCollection(cookieCount);
			for (var i = 0; i < cookieCount; i++)
			{
				var cookie = cookies.Get(i);
				store.Add(cookie.Name, cookie.Value);
			}
			return store;
		}

		public virtual NameValueCollection CreateStoreFromDictionary(IDictionary<string, object> dict)
		{
			if (dict == null) return new NameValueCollection();

			var valueCount = dict.Count;
			var store = new NameValueCollection(valueCount);
			foreach (var kvp in dict)
			{
				var value = string.Empty;

				object oValue = kvp.Value;
				if (oValue != null) value = oValue.ToString();

				store.Add(kvp.Key, value);
			}
			return store;
		}

		public virtual NameValueCollection CreateStoreFromAll(NameValueCollection parms, TempDataDictionary tempData, RouteData routeData)
		{
			var store = new NameValueCollection(parms);
			var tempDataStore = CreateStoreFromDictionary(tempData);
			var routeDataStore = CreateStoreFromDictionary(routeData.Values);
			store.Add(tempDataStore);
			store.Add(routeDataStore);
			return store;
		}

		public abstract object GetValue(ControllerContext controllerContext, string modelName, Type modelType, ModelStateDictionary modelState);
	    public ModelBinderResult BindModel(ModelBindingContext bindingContext)
	    {
	        throw new System.NotImplementedException();
	    }
	}

	public enum RequestStore
	{
		///<summary>
		/// Sets paramter values from Request.QueryString
		///</summary>
		QueryString = 1,
		///<summary>
		/// Sets parameter values from Request.Form
		///</summary>
		Form = 2,

		Cookies = 3,

		ServerVariables = 4,
		///<summary>
		/// Sets parameter values from all of the above locations in the above order
		/// If a parameter value is found in more than one location, they will be returned in comma separated form in the above order
		/// e.g. QueryString = 4, Form = 6 "4,6"
		///</summary>
		Params = 5,
		///<summary>
		/// Sets parameter values from Controller.TempData only
		///</summary>
		TempData = 6,

		RouteData = 7,
		///<summary>
		/// Sets parameter values from all fo the above locations in the above order
		/// If a parameter value is found in more than one location, they will be returned in comma separated form in the above order
		/// e.g. QueryString = 4, Form = 6, TempData = 2, RouteData = 1 "4,6,3,2"
		///</summary>
		All = 8
	}
}