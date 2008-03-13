using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;

namespace MvcContrib.ControllerFactories
{
	public class WindsorControllerFactory : IControllerFactory
	{
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

	    public IController CreateController(RequestContext context, string controllerName)
	    {
	    	controllerName = controllerName.ToLower() + "controller"; 

	    	IWindsorContainer container = GetContainer(context);
	    	return (IController)container.Resolve(controllerName);
	    }

	    public void DisposeController(IController controller)
	    {
	    	IDisposable disposable = controller as IDisposable;

			if(disposable != null)
			{
				disposable.Dispose();
			}
	    }
	}
}
