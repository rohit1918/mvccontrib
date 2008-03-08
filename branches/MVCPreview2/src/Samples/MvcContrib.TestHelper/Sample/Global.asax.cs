using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
            RouteTable.Routes.Add(
                new Route("{controller}/{action}/{id}",
                new RouteValueDictionary( new {action = "Index", id = (string) null}),
                new MvcRouteHandler()));
            
        

            //RouteTable.Routes.Add(new Route
            //{
            //    Url = "Default.aspx",
            //    Defaults = new
            //    {
            //        controller = "Stars",
            //        action = "List",
            //        id = (string) null
            //    }
            //,
            //    RouteHandler = typeof (MvcRouteHandler)
            //}
        //)
            ;
        }
    }
}