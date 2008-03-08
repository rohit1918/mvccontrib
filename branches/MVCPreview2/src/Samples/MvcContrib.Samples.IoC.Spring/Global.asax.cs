using System;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ControllerFactories;
using MvcContrib.Services;
using MvcContrib.Spring;
using Spring.Core.IO;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Xml;

namespace MvcContrib.Samples.IoC
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Note: Change Url= to Url="[controller].mvc/[action]/[id]" to enable 
            //       automatic support on IIS6 
            ConfigureIoC();
            AddRoutes();
        }

        private void ConfigureIoC()
        {
            IResource input = new FileSystemResource(Server.MapPath("objects.xml"));
            IObjectFactory factory = new XmlObjectFactory(input);
            DependencyResolver.InitializeWith(new SpringDependencyResolver(factory));

            ControllerBuilder.Current.SetControllerFactory(typeof(IoCControllerFactory));
        }

        private void AddRoutes()
        {
            throw new NotImplementedException();
        //    RouteTable.Routes.Add(new Route
        //    {
        //        Url = "[controller]/[action]/[id]",
        //        Defaults = new
        //        {
        //            action = "Index",
        //            id = (string)null
        //        }
        //    ,
        //        RouteHandler = typeof(MvcRouteHandler)
        //    }
        //)
        //    ;

        //    RouteTable.Routes.Add(new Route
        //    {
        //        Url = "Default.aspx",
        //        Defaults = new
        //        {
        //            controller = "Home",
        //            action = "Index",
        //            id = (string)null
        //        }
        //    ,
        //        RouteHandler = typeof(MvcRouteHandler)
        //    }
        //)
        //    ;
        }
    }
}
