using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.Samples.FormHelper.Models;
namespace MvcContrib.Samples.FormHelper.Controllers
{
	public class HomeController : ConventionController
	{

		public ActionResult Index()
		{
			return RenderView();
		}

		public ActionResult FormHelper()
		{
			Person person = new Person();
			person.Id = 1;
			person.Name = "Jeremy";
			person.RoleId = 2;
			person.Gender = Gender.Male;
			ViewData["person"] = person;

			List<Role> roles = new List<Role>();
			roles.Add(new Role(1, "Administrator"));
			roles.Add(new Role(2, "Developer"));
			roles.Add(new Role(3, "User"));
			ViewData["roles"] = roles;

			return RenderView();
		}

		public ActionResult ValidationHelper()
		{
			return RenderView();
		}
	}
}