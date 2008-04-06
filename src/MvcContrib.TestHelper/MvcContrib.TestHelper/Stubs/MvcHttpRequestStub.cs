using System;
using System.Collections.Specialized;
using System.Security.Policy;
using System.Web;

namespace MvcContrib.TestHelper.Stubs
{
	/// <remarks>Abstract Base Classes SUCK!  This is the dumbest workaround implementation I have ever seen.</remarks>
	public class MvcHttpRequestStub : HttpRequestBase, ITestHttpRequest
	{
		public MvcHttpRequestStub() : this(new MvcHttpContextStub())
		{
		}

		public MvcHttpRequestStub(ITestHttpContext httpContext)
		{
			Tester.HttpContext = httpContext;
			Tester.Form = new NameValueCollection();
		}

		public override string ApplicationPath
		{
			get { return ""; }
		}

		public override Uri Url
		{
			get { return new Uri(Tester.Url); }
		}

		public override string HttpMethod
		{
			get { return Tester.HttpMethod; }
		}

		public override NameValueCollection Form
		{
			get { return Tester.Form; }
		}

		protected virtual ITestHttpRequest Tester
		{
			get { return this; }
		}

		public virtual HttpRequestBase ToHttpRequest()
		{
			return this;
		}

		public virtual HttpContextBase ToHttpContext()
		{
			return Tester.HttpContext.ToHttpContext();
		}

		string ITestHttpRequest.HttpMethod { get; set; }

		string ITestHttpRequest.Url { get; set; }

		ITestHttpContext ITestHttpRequest.HttpContext { get; set; }

		NameValueCollection ITestHttpRequest.Form { get; set; }
	}
}