using System.Web;

namespace MvcContrib.TestHelper.Stubs
{
	public class MvcHttpContextStub : HttpContextBase, ITestHttpContext
	{
		public MvcHttpContextStub()
		{
			Tester.Request = new MvcHttpRequestStub(this);
		}

		public override HttpRequestBase Request
		{
			get { return Tester.Request.ToHttpRequest(); }
		}

		public virtual ITestHttpContext Tester
		{
			get { return this; }
		}

		public virtual HttpContextBase ToHttpContext()
		{
			return this;
		}

		ITestHttpRequest ITestHttpContext.Request { get; set; }
	}
}