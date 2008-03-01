using System;
using System.Web;
using System.Web.Mvc;

namespace MvcTestingFramework.Sample
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            SetupRoutes();
        }

        public static void SetupRoutes()
        {
            RouteTable.Routes.Add(new Route
            {
                Url = "[controller]/[action]/[id]",
                Defaults = new
                {
                    action = "Index",
                    id = (string) null
                }
            ,
                RouteHandler = typeof (MvcRouteHandler)
            }
        )
            ;

            RouteTable.Routes.Add(new Route
            {
                Url = "Default.aspx",
                Defaults = new
                {
                    controller = "Stars",
                    action = "List",
                    id = (string) null
                }
            ,
                RouteHandler = typeof (MvcRouteHandler)
            }
        )
            ;
        }
    }
}