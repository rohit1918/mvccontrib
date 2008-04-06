using System;
using System.Web;
using System.Web.Routing;
using MvcContrib.Rest.Routing.Ext;
using MvcContrib.TestHelper.Stubs;

namespace MvcContrib.TestHelper.Context.Builders
{
	public class MvcRequestBuilder<TReturnBuilder> where TReturnBuilder : MvcRequestBuilder<TReturnBuilder>
	{
		protected readonly ITestHttpRequest _requestStub = new MvcHttpContextStub().Tester.Request;


		public virtual TReturnBuilder Post
		{
			get
			{
				_requestStub.HttpMethod = "POST";
				return (TReturnBuilder)this;
			}
		}

		public virtual TReturnBuilder Get
		{
			get
			{
				_requestStub.HttpMethod = "GET";
				return (TReturnBuilder)this;
			}
		}

		public virtual TReturnBuilder Put
		{
			get
			{
				_requestStub.HttpMethod = "PUT";
				return (TReturnBuilder)this;
			}
		}

		public virtual TReturnBuilder Delete
		{
			get
			{
				_requestStub.HttpMethod = "DELETE";
				return (TReturnBuilder)this;
			}
		}

		public virtual HttpRequestBase ToRequest()
		{
			return _requestStub.ToHttpRequest();
		}

		public virtual HttpContextBase ToHttpContext()
		{
			return _requestStub.ToHttpContext();
		}

		public virtual RequestContext ToRequestContext()
		{
			return new RequestContext(ToHttpContext(), new RouteData());
		}

		public static implicit operator RequestContext(MvcRequestBuilder<TReturnBuilder> builder)
		{
			return builder.ToRequestContext();
		}

		public virtual TReturnBuilder WithFormValues(params Func<object, object>[] values)
		{
			_requestStub.Form.Merge(new Hash(values));
			return (TReturnBuilder)this;
		}

		public virtual TReturnBuilder To(string url)
		{
			_requestStub.Url = "http://localhost/" + url;
			return (TReturnBuilder)this;
		}
	}
}