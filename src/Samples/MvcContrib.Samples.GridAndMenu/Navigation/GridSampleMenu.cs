using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.Samples.GridAndMenu.Controllers;
using MvcContrib.UI.MenuBuilder;

namespace MvcContrib.Samples.GridAndMenu.Navigation
{
	public class GridSampleMenu
	{
		public static MenuItem MainMenu()
		{
			Menu.DefaultIconDirectory = "/Content/";
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
						Menu.Items("Some Direct Links Items",
							Menu.Link("Big Blue", "http://microsoft.com"),
							Menu.Link("Google", "http://google.com")
						),

						Menu.Secure<AccountController>(p => p.LogOff()),
						Menu.Secure<AccountController>(p => p.MagicLogOn())
					).SetListClass("jd_menu");
		}
	}
}