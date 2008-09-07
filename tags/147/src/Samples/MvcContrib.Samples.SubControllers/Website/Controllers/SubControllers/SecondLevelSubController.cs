using System.Web.Mvc;
using MvcContrib;

namespace Website.Controllers.SubControllers
{
	public class SecondLevelSubController : SubController
	{
		public ViewResult SecondLevel(LeftController left, RightController right, FormSubmitController formSubmit)
		{
			ViewData["text"] = "I am a second level controller";
			return View();
		}
	}
}