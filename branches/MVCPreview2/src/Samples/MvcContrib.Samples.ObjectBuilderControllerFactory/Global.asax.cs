using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ObjectBuilder;
using MvcContrib.Samples.ObjectBuilderControllerFactory.Controllers;
using MvcContrib.Samples.ObjectBuilderControllerFactory.Models;

namespace MvcContrib.Samples.ObjectBuilderControllerFactory
{
	public class Global : HttpApplication, IDependencyContainerAccessor
	{
		private static DependencyContainer _container;

        public static IDependencyContainer Container
		{
			get { return _container; }
		}

				IDependencyContainer IDependencyContainerAccessor.Container
		{
			get { return Container; }
		}

		protected void Application_Start(object sender, EventArgs e)
		{
			InitializeContainer();
			AddRoutes();
		}

		/// <summary>
		/// Instantiate the container and add all Controllers that derive from 
        /// ObjectBuilderController to the container.  Also associate the Controller 
        /// with the ObjectBuilderContainer ControllerFactory.
		/// </summary>
        protected virtual void InitializeContainer()
		{
			if (_container == null)
			{
				_container = new DependencyContainer();
				_container.RegisterTypeMapping(typeof(IService), typeof(Service));
				ControllerBuilder.Current.SetControllerFactory(typeof(ControllerFactories.ObjectBuilderControllerFactory));

                Type[] assemblyTypes = typeof(HomeController).Assembly.GetTypes();
				foreach (Type type in assemblyTypes)
				{
					if(typeof(IController).IsAssignableFrom(type) )
					{
						_container.RegisterTypeMapping(type, type);
					}
				}
			}
		}

		protected virtual void AddRoutes()
		{
			RouteTable.Routes.Add(new Route("{controller}.mvc/{action}/{id}", new MvcRouteHandler())
			{
				Defaults = new RouteValueDictionary(new { action = "Index", id = "" }),
			});

			RouteTable.Routes.Add(new Route("Default.aspx", new MvcRouteHandler())
			{
				Defaults = new RouteValueDictionary(new { controller = "Home", action = "Index", id = "" }),
			});
		}
	}
}