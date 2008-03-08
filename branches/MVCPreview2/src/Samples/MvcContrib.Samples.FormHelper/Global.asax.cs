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
            throw new NotImplementedException();
            //RouteTable.Routes.Add(new Route
            //{
            //    Url = "[controller].mvc/[action]",
            //    Defaults = new { action = "Index", id = (string)null },
            //    RouteHandler = typeof(MvcRouteHandler)
            //});

            //RouteTable.Routes.Add(new Route
            //{
            //    Url = "Default.aspx",
            //    Defaults = new { controller = "Home", action = "Index", id = (string)null },
            //    RouteHandler = typeof(MvcRouteHandler)
            //});
		}

	}
}
