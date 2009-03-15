using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.Samples.UI.Models;

namespace MvcContrib.Samples.UI.Controllers
{
	public class FluentHtmlController : Controller
	{
		public ViewResult Index()
		{
			return View(new FluentHtmlViewData 
			{
				Person = new Person() { Gender = "M", Name = "Jeremy", Roles = new List<int> { 1, 2 } },
				Genders = new Dictionary<string, string> { { "M", "Male" }, { "F", "Female" } },
				Roles = new List<Role> { new Role(0, "Administrator"), new Role(1, "Developer"), new Role(2, "User")  }
			});
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ViewResult Index(Person person)
		{
			return View("ViewPerson", person);
		}
	}

	public class FluentHtmlViewData
	{
		public Person Person;
		public IDictionary<string, string> Genders;
		public IEnumerable<Role> Roles;
	}

	
}