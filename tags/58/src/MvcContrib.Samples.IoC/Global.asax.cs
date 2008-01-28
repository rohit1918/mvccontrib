using System;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using MvcContrib.ControllerFactories;
using MvcContrib.Samples.IoC.Controllers;
using MvcContrib.Services;
using MvcContrib.UnitTests.ControllerFactories;
using StructureMap;

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
            StructureMapConfiguration.UseDefaultStructureMapConfigFile = false;
            StructureMapConfiguration.BuildInstancesOf<HomeController>().TheDefaultIsConcreteType<HomeController>();
            DependencyResolver.InitializeWith(new StructureMapDependencyResolver());
            ControllerBuilder.Current.SetDefaultControllerFactory(typeof(IoCControllerFactory));
        }

        private void AddRoutes()
        {
            RouteTable.Routes.Add(new Route
            {
                Url = "[controller]/[action]/[id]",
                Defaults = new
                {
                    action = "Index",
                    id = (string)null
                }
            ,
                RouteHandler = typeof(MvcRouteHandler)
            }
        )
            ;

            RouteTable.Routes.Add(new Route
            {
                Url = "Default.aspx",
                Defaults = new
                {
                    controller = "Home",
                    action = "Index",
                    id = (string)null
                }
            ,
                RouteHandler = typeof(MvcRouteHandler)
            }
        )
            ;
        }
    }
}