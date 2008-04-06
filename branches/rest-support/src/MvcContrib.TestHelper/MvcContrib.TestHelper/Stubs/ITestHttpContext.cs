using System.Web;

namespace MvcContrib.TestHelper.Stubs
{
	public interface ITestHttpContext
	{
		HttpContextBase ToHttpContext();

		ITestHttpRequest Request { get; set; }
	}
}