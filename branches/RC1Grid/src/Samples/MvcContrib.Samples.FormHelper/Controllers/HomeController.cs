using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.Samples.FormHelper.Models;
using MvcContrib.Pagination;
using System.Linq;
namespace MvcContrib.Samples.FormHelper.Controllers
{
	public class HomeController : ConventionController
	{

		public ActionResult Index()
		{
			return View();
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

			return View();
		}

		public ActionResult ValidationHelper()
		{
			return View();
		}

		public ActionResult Grid(int? page)
		{
			var people = new List<Person>();
			for (var i = 0; i < 25; i ++ )
			{
				people.Add(new Person {Id = i, Name = "Person " + i, Gender = i%2 == 0 ? Gender.Male : Gender.Female, RoleId=2 }); 
			}

			ViewData["people"] = people.AsQueryable().AsPagination(page.GetValueOrDefault(1), 10);
			return View();
		}
	}
}