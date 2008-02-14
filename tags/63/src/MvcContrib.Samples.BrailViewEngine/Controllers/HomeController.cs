using System;
using System.Web.Mvc;

namespace MvcContrib.Samples
{
	public class HomeController : Controller
	{
		[ControllerAction]
		public void Index()
		{
			RenderView("Index", "Site");
		}

		[ControllerAction]
		public void Contact()
		{
			CompanyInfo companyInfo = new CompanyInfo();
			companyInfo.CompanyName = "Your company name here";
			companyInfo.AddressLine1 = "Company address Line 1";
			companyInfo.AddressLine2 = "Company address Line 2";
			companyInfo.City = "City";
			companyInfo.State = "State";
			companyInfo.Zip = "00000";
			companyInfo.Email = "email@yourcompany.com";
			
			RenderView("Contact", "Site", companyInfo);
		}

		[ControllerAction]
		public void About()
		{
			ViewData["now"] = DateTime.Now;

			RenderView("About", "Site");
		}
	}
}