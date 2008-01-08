using System;
using System.Web.Mvc;
using Castle.Windsor;

namespace MvcContrib.ControllerFactories
{
	public class WindsorControllerFactory : IControllerFactory
	{
		IController IControllerFactory.CreateController(RequestContext context, Type controllerType)
		{
			return CreateControllerInternal(context, controllerType);
		}

		protected virtual IController CreateControllerInternal(RequestContext context, Type controllerType)
		{
			IWindsorContainer container = GetContainer(context);

			return (IController)container.Resolve(controllerType);
		}

		protected virtual IWindsorContainer GetContainer(RequestContext context)
		{
			if( context == null )
			{
				throw new ArgumentNullException("context");
			}

			IContainerAccessor containerAccessor = context.HttpContext.ApplicationInstance as IContainerAccessor;
			if (containerAccessor == null)
			{
				throw new InvalidOperationException(
					"You must extend the HttpApplication in your web project and implement the IContainerAccessor to properly expose your container instance");
			}

			IWindsorContainer container = containerAccessor.Container;
			if (container == null)
			{
				throw new InvalidOperationException("The container seems to be unavailable in your HttpApplication subclass");
			}

			return container;
		}
	}
}