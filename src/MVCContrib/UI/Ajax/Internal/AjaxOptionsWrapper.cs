using System.Collections.Generic;
using System.Text;

namespace MvcContrib.UI.Ajax.Internal
{
	public abstract class AjaxOptionsWrapper
	{
		protected System.Web.Mvc.Ajax.AjaxOptions _options;

		protected AjaxOptionsWrapper(System.Web.Mvc.Ajax.AjaxOptions options)
		{
			_options = options;
		}
	}
}