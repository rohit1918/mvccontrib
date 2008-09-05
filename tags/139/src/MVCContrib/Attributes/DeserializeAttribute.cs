using System;
using System.Collections.Specialized;
using System.Web.Mvc;

namespace MvcContrib.Attributes
{
	[Serializable]
	public class DeserializeAttribute : AbstractParameterBinderAttribute
	{
		private readonly NameValueDeserializer _deserializer = new NameValueDeserializer();

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
					store = new NameValueCollection(controllerContext.HttpContext.Request.Form) { controllerContext.HttpContext.Request.QueryString };
					break;
				case RequestStore.Form:
					store = controllerContext.HttpContext.Request.Form;
					break;
				case RequestStore.QueryString:
					store = controllerContext.HttpContext.Request.QueryString;
					break;
			}

			return _deserializer.Deserialize(store, Prefix, modelType);

		}
	}
}
