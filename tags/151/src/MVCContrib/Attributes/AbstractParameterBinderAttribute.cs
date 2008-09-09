using System;
using System.Web.Mvc;
using MvcContrib.MetaData;

namespace MvcContrib.Attributes
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public abstract class AbstractParameterBinderAttribute : CustomModelBinderAttribute, IModelBinder
	{
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

		public override IModelBinder GetBinder() {
			return this;
		}

		public abstract object GetValue(ControllerContext controllerContext, string modelName, Type modelType, ModelStateDictionary modelState);
	}

	public enum RequestStore
	{
		QueryString = 1,
		Form = 2,
		Cookies = 3,
		ServerVariables = 4,
		Params = 5,
		RouteData = 6,
		All = 7
	}
}
