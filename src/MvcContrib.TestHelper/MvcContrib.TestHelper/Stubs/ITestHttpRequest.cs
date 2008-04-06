using System.Collections.Specialized;
using System.Web;

namespace MvcContrib.TestHelper.Stubs
{
	/// <summary>Read / Write interface to help build up and stub an <see cref="HttpRequestBase"/> class.</summary>
	public interface ITestHttpRequest
	{
		string HttpMethod { get; set; }

		string Url { get; set; }

		ITestHttpContext HttpContext { get; set; }

		NameValueCollection Form { get; set;  }

		HttpRequestBase ToHttpRequest();

		HttpContextBase ToHttpContext();
	}
}