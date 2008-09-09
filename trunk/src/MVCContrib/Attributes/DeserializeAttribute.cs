using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcContrib.Attributes
{
	[Serializable]
	public class DeserializeAttribute : AbstractParameterBinderAttribute
	{
		private readonly NameValueDeserializer _deserializer = new NameValueDeserializer();

		public DeserializeAttribute() : base(null)
		{
		}

		public DeserializeAttribute(string prefix)
			: base(prefix)
		{
		}

		public DeserializeAttribute(string prefix, RequestStore requestStore)
			: base(prefix, requestStore)
		{
		}

		public override object GetValue(ControllerContext controllerContext, string modelName, Type modelType, ModelStateDictionary modelState)
		{
			NameValueCollection store = null;

			switch (RequestStore) {
				case RequestStore.Params:
					store = controllerContext.HttpContext.Request.Params;
					break;
				case RequestStore.Form:
					store = controllerContext.HttpContext.Request.Form;
					break;
				case RequestStore.QueryString:
					store = controllerContext.HttpContext.Request.QueryString;
					break;
				case RequestStore.Cookies:
					store = CreateStoreFromCookies(controllerContext.HttpContext.Request.Cookies);
					break;
				case RequestStore.ServerVariables:
					store = controllerContext.HttpContext.Request.ServerVariables;
					break;
				case RequestStore.RouteData:
					store = CreateStoreFromRouteData(controllerContext.RouteData);
					break;
				case RequestStore.All:
					store = CreateStoreFromAll(controllerContext.HttpContext.Request.Params, controllerContext.RouteData);
					break;
			}

			return _deserializer.Deserialize(store, Prefix, modelType);
		}

		public virtual NameValueCollection CreateStoreFromCookies(HttpCookieCollection cookies)
		{
			var cookieCount = cookies.Count;
			var store = new NameValueCollection(cookieCount);
			for(var i = 0; i<cookieCount; i++ )
			{
				var cookie = cookies.Get(i);
				store.Add(cookie.Name, cookie.Value);
			}
			return store;
		}

		public virtual NameValueCollection CreateStoreFromRouteData(RouteData routeData)
		{
			var valueCount = routeData.Values.Count;
			var store = new NameValueCollection(valueCount);
			foreach( var kvp in routeData.Values )
			{
				var value = string.Empty;

				object oValue = kvp.Value;
				if (oValue != null) value = oValue.ToString();

				store.Add(kvp.Key, value);
			}
			return store;
		}

		public virtual NameValueCollection CreateStoreFromAll(NameValueCollection parms, RouteData routeData)
		{
			var store = new NameValueCollection(parms);
			var routeDataStore = CreateStoreFromRouteData(routeData);
			store.Add( routeDataStore );
			return store;
		}
	}
}
