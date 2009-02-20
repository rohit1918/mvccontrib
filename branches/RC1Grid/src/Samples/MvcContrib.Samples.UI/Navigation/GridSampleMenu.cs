using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.UI.MenuBuilder;

namespace MvcContrib.Samples.UI.Navigation
{
	using UI.Controllers;

	public class GridSampleMenu
	{
		public static MenuItem MainMenu()
		{
			Menu.DefaultIconDirectory = "/Content/";
			Menu.DefaultDisabledClass = "disabled";
			Menu.DefaultSelectedClass = "selected";
			return Menu.Begin(
						Menu.Items("Home Controller Items",
							Menu.Secure<HomeController>(p => p.Index()),
							Menu.Secure<HomeController>(p => p.About()),
							Menu.Secure<HomeController>(p => p.SecurePage1()),
							Menu.Secure<HomeController>(p => p.SecurePage2())
						),
						Menu.Items("Some Insecure Items",
							Menu.Action<HomeController>(p => p.Index()),
							Menu.Action<HomeController>(p => p.About(),"About", "blarg.gif"),
							Menu.Action<HomeController>(p => p.SecurePage1()),
							Menu.Action<HomeController>(p => p.SecurePage2())
						),
						Menu.Items("Some Disabled Items",
							Menu.Action<HomeController>(p => p.Index(), "Disabled Index").SetDisabled(true).SetShowWhenDisabled(true),
							Menu.Action<HomeController>(p => p.About(), "Disabled About").SetDisabled(true).SetShowWhenDisabled(true)
						),
						Menu.Items("Some Direct Links Items",
							Menu.Link("http://microsoft.com", "Big Blue"),
							Menu.Link("http://google.com", "Google")
						),

						Menu.Secure<AccountController>(p => p.LogOff()),
						Menu.Secure<AccountController>(p => p.MagicLogOn())
					).SetListClass("jd_menu");
		}
	}
}