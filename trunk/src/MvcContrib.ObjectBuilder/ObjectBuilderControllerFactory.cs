using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcContrib.ObjectBuilder
{
    public class ObjectBuilderControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext context, string controllerName)
        {
            Type type = base.GetControllerType(controllerName);

            if(type == null)
            {
                throw new InvalidOperationException(string.Format("Could not find a controller with the name {0}", controllerName));
            }

            IDependencyContainer container = GetContainer(context);
            return (IController)container.Get(type);
        }

        protected virtual IDependencyContainer GetContainer(RequestContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var dependencyContainerAccessor = context.HttpContext.ApplicationInstance as IDependencyContainerAccessor;
            if (dependencyContainerAccessor == null)
            {
                throw new InvalidOperationException(
                    "You must extend the HttpApplication in your web project and implement the IContainerAccessor to properly expose your container instance");
            }

            IDependencyContainer container = dependencyContainerAccessor.Container;
            if (container == null)
            {
                throw new InvalidOperationException("The container seems to be unavailable in your HttpApplication subclass");
            }

            return container;
        }
    }
}
