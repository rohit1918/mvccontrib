using System;
using System.Collections.Specialized;
using System.Web.Mvc;

namespace MvcContrib.Attributes
{
	[Serializable]
	public class DeserializeAttribute : AbstractParameterBinderAttribute
	{
		private readonly NameValueDeserializer _deserializer = new NameValueDeserializer();

		public DeserializeAttribute()
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
			NameValueCollection store = GetStore(controllerContext);
			return _deserializer.Deserialize(store, Prefix, modelType);
		}
	}
}
