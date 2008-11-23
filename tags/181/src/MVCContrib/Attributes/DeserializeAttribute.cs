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

		public override ModelBinderResult BindModel(ModelBindingContext bindingContext)
		{
			NameValueCollection store = GetStore(bindingContext);
			return new ModelBinderResult(_deserializer.Deserialize(store, Prefix, bindingContext.ModelType));
		}
	}
}