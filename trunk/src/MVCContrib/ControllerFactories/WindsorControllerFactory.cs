using System;
using System.Web.Mvc;
using Castle.Windsor;

namespace MVCContrib.ControllerFactories
{
	public class WindsorControllerFactory : IControllerFactory
	{
		IController IControllerFactory.CreateController(RequestContext context, Type controllerType)
		{
			IWindsorContainer container = ObtainContainer(context);
			IController controller = GetController<IController>(container, controllerType);
			return controller;
		}

		protected virtual IWindsorContainer ObtainContainer(RequestContext context)
		{
			if(context == null)
			{
				throw new ArgumentNullException("context");
			}

			IContainerAccessor containerAccessor = context.HttpContext.ApplicationInstance as IContainerAccessor;
			if(containerAccessor == null)
			{
				throw new InvalidOperationException(
					"You must extend the HttpApplication in your web project and implement the IContainerAccessor to properly expose your container instance");
			}

			IWindsorContainer container = containerAccessor.Container;
			if(container == null)
			{
				throw new InvalidOperationException("The container seems to be unavailable in your HttpApplication subclass");
			}

			return container;
		}

		protected virtual T GetController<T>(IWindsorContainer container, Type controllerType) where T : IController
		{
			if(container == null)
			{
				throw new ArgumentNullException("container");
			}
			if(controllerType == null)
			{
				throw new ArgumentNullException("controllerType");
			}

			return (T)container.Resolve(controllerType);
		}
	}
}