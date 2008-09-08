using System.Web.Mvc;
using NUnit.Framework;
using Website.Controllers;

namespace Website.UnitTests.Controllers
{
	/// <summary>
	/// Summary description for HomeControllerTest
	/// </summary>
	[TestFixture]
	public class HomeControllerTest
	{
		[Test]
		public void Index()
		{
			// Setup
			var controller = new HomeController();

			// Execute
			var result = controller.Index(null) as ViewResult;

			// Verify
			ViewDataDictionary viewData = result.ViewData;
			Assert.AreEqual("Home Page", viewData["Title"]);
			Assert.AreEqual("Welcome to ASP.NET MVC!", viewData["Message"]);
		}

		[Test]
		public void About()
		{
			// Setup
			var controller = new HomeController();

			// Execute
			var result = controller.About() as ViewResult;

			// Verify
			ViewDataDictionary viewData = result.ViewData;
			Assert.AreEqual("About Page", viewData["Title"]);
		}
	}
}