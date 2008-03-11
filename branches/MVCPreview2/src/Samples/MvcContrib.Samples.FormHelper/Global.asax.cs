using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;

namespace MvcContrib.Samples.FormHelper
{
	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{
			RouteTable.Routes.Add(new Route("{controller}.mvc/{action}/{id}", new MvcRouteHandler())
									{
										Defaults = new RouteValueDictionary(new { action = "index", id = "" })
									});

			RouteTable.Routes.Add(new Route("Default.aspx", new MvcRouteHandler())
			{
				Defaults = new RouteValueDictionary(new { controller = "Home", action = "index", id = "" })
			});

		}

	}
}
