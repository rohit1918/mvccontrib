using System.Web.Mvc;
using MvcContrib;

namespace Website.Controllers.SubControllers
{
	public class FormSubmitController : SubController
	{
		[AcceptVerbs("POST")]
		public ViewResult FormSubmit(string sometextbox)
		{
			ViewData["sometextbox"] = sometextbox;
			return View("posted");
		}

		[AcceptVerbs("GET")]
		public ViewResult FormSubmit()
		{
			return View();
		}
	}
}