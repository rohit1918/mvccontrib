using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Core;
using Castle.Windsor;
using MvcContrib.ControllerFactories;

namespace MvcContrib.Samples.NVelocityViewFactory
{
	public class Global : HttpApplication, IContainerAccessor
	{
		private static WindsorContainer _container;

		public static IWindsorContainer Container
		{
			get { return _container; }
		}

		IWindsorContainer IContainerAccessor.Container
		{
			get { return Container; }
		}

		protected void Application_Start(object sender, EventArgs e)
		{
			InitializeWindsor();
			AddRoutes();
		}

		/// <summary>
		/// Instantiate the container and add all Controllers that derive from 
		/// WindsorController to the container.  Also associate the Controller 
		/// with the WindsorContainer ControllerFactory.
		/// </summary>
		protected virtual void InitializeWindsor()
		{
			if (_container == null)
			{
				_container = new WindsorContainer();

				// Add our singleton NVelocityViewFactory
				_container.AddComponent("ViewFactory", typeof(IViewEngine), typeof(Castle.NVelocityViewFactory));

				Type[] assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
				
				foreach (Type type in assemblyTypes)
				{
					if (typeof(IController).IsAssignableFrom(type))
					{
						_container.AddComponentWithLifestyle(type.Name, type, LifestyleType.Transient);
						ControllerBuilder.Current.SetControllerFactory( typeof(WindsorControllerFactory));
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
