using System.Web.Mvc;

namespace MvcContrib.UI.Html
{
	public interface IDataBinder
	{
		object ExtractValue(string target, ViewContext context);
	}
}