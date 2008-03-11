using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ControllerFactories;

namespace MvcContrib.Samples.NHamlViewEngine
{
	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{
			AppDomain.CurrentDomain.SetData("SQLServerCompactEditionUnderWebHosting", true);

			// Note: Change Url= to Url="[controller].mvc/[action]/[id]" to enable 
			//       automatic support on IIS6 

			RouteTable.Routes.Add(new Route("{controller}.mvc/{action}/{id}", new MvcRouteHandler())
			{
				Defaults = new RouteValueDictionary(new { action = "index", id = "" })
			});

			RouteTable.Routes.Add(new Route("Default.aspx", new MvcRouteHandler())
			{
				Defaults = new RouteValueDictionary(new { controller = "Home", action = "index", id = "" })
			});

			ControllerBuilder.Current.SetControllerFactory(new NHamlControllerFactory());
		}
	}
}