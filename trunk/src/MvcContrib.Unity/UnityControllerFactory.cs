using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using MvcContrib.Unity;
using DefaultControllerFactory=MvcContrib.ControllerFactories.DefaultControllerFactory;

namespace MvcContrib.Unity
{
    public class UnityControllerFactory : DefaultControllerFactory
    {
        protected override IController CreateController(RequestContext context, string controllerName)
        {
            Type type = GetControllerType(controllerName);

            if (type == null)
            {
                throw new InvalidOperationException(string.Format("Could not find a controller with the name {0}", controllerName));
            }

            IUnityContainer container = GetContainer(context);
            return (IController)container.Resolve(type);
        }

        protected virtual IUnityContainer GetContainer(RequestContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            IUnityContainerAccessor unityContainerAccessor = context.HttpContext.ApplicationInstance as IUnityContainerAccessor;
            if (unityContainerAccessor == null)
            {
                throw new InvalidOperationException(
                    "You must extend the HttpApplication in your web project and implement the IContainerAccessor to properly expose your container instance");
            }

            IUnityContainer container = unityContainerAccessor.Container;
            if (container == null)
            {
                throw new InvalidOperationException("The container seems to be unavailable in your HttpApplication subclass");
            }

            return container;
        }
    }
}