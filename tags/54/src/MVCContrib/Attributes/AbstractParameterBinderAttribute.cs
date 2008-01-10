using System;
using System.Web.Mvc;
using MvcContrib.MetaData;

namespace MvcContrib.Attributes
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public abstract class AbstractParameterBinderAttribute : Attribute, IParameterBinder
	{
		public AbstractParameterBinderAttribute(string prefix)
			: this(prefix, RequestStore.Params)
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

		public abstract object Bind(Type targetType, string paramName, ControllerContext context);
	}

	public enum RequestStore
	{
		QueryString = 1,
		Form = 2,
		Params = 3
	}
}
