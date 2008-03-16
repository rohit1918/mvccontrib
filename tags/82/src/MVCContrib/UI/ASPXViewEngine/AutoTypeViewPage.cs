using System.Web.Mvc;

namespace MvcContrib.UI.ASPXViewEngine
{
	public class AutoTypeViewPage<TViewData> : ViewPage<TViewData>
	{
		protected override void SetViewData(object viewData)
		{
			base.SetViewData(AutoTypingHelper.PerformLooseTypecast<TViewData>(viewData));
		}
	}
}
