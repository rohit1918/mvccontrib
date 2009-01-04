using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Ninject;
using MvcContrib.Samples.NinjectControllerFactory.Modules;

namespace MvcContrib.Samples.NinjectControllerFactory
{
    public class GlobalApplication : HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Note: Change the URL to "{controller}.mvc/{action}/{id}" to enable
            //       automatic support on IIS6 and IIS7 classic mode

            routes.Add(new Route("{controller}/{action}/{id}", new MvcRouteHandler())
                           {
                               Defaults = new RouteValueDictionary(new {action = "Index", id = ""}),
                           });

            routes.Add(new Route("Default.aspx", new MvcRouteHandler())
                           {
                               Defaults = new RouteValueDictionary(new {controller = "Home", action = "Index", id = ""}),
                           });
        }

        public void InitializeNinject()
        {
            NinjectKernel.Initialize(new ControllerModule(), new DomainModule());
#pragma warning disable 618,612
            ControllerBuilder.Current.SetControllerFactory(typeof (Ninject.NinjectControllerFactory));
#pragma warning restore 618,612
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            InitializeNinject();
            RegisterRoutes(RouteTable.Routes);
        }
    }
}