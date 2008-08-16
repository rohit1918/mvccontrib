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

		public override object Bind(Type targetType, string paramName, ControllerContext context)
		{
			NameValueCollection store = null;

			switch(RequestStore)
			{
				case RequestStore.Params:
					store = new NameValueCollection(context.HttpContext.Request.Form) {context.HttpContext.Request.QueryString};
					break;
				case RequestStore.Form:
					store = context.HttpContext.Request.Form;
					break;
				case RequestStore.QueryString:
					store = context.HttpContext.Request.QueryString;
					break;
			}

			return _deserializer.Deserialize(store, Prefix, targetType);
		}
	}
}
