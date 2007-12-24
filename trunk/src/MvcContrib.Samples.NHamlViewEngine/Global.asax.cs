using System;
using System.Web.Mvc;
using MvcContrib.ControllerFactories;

namespace MvcContrib.Samples
{
	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{
			AppDomain.CurrentDomain.SetData("SQLServerCompactEditionUnderWebHosting", true);

			// Note: Change Url= to Url="[controller].mvc/[action]/[id]" to enable 
			//       automatic support on IIS6 

			RouteTable.Routes.Add(new Route
			{
				Url = "[controller]/[action]/[id]",
				Defaults = new { action = "Index", id = (string)null },
				RouteHandler = typeof(MvcRouteHandler)
			});

			RouteTable.Routes.Add(new Route
			{
				Url = "Default.aspx",
				Defaults = new { controller = "Home", action = "Index", id = (string)null },
				RouteHandler = typeof(MvcRouteHandler)
			});

			ControllerBuilder.Current.SetDefaultControllerFactory(typeof(NHamlControllerFactory));
		}
	}
}