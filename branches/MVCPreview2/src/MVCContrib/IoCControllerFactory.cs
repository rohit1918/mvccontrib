using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Interfaces;
using MvcContrib.Services;

namespace MvcContrib.ControllerFactories
{
    public class IoCControllerFactory : DefaultControllerFactory
    {
        private IDependencyResolver resolver = null;

        public IoCControllerFactory(IDependencyResolver resolver)
        {
            if(resolver==null)
                throw new ArgumentNullException("resolver");
            this.resolver = resolver;
        }

        public IoCControllerFactory()
        {
            
        }

        protected override Type GetControllerType(string controllerName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type type = null;
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    foreach(Type t in assembly.GetTypes())
                    {
                        if(t.Name.Equals(controllerName))
                        {
                            type = t;
                            break;
                        }
                    }
                }catch
                {
                    ;
                }
            }
            return type;
        }

        protected override IController CreateController(RequestContext context, string controllerName)
        {
			//TODO: Don't rely on DefaultControllerFactory's GetControllerType
            controllerName += "Controller";

        	Type controllerType = GetControllerType(controllerName);

			if (controllerType != null)
			{
				if (resolver != null)
				{
					return resolver.GetImplementationOf<IController>(controllerType);
				}
				else
				{

					return DependencyResolver.GetImplementationOf<IController>(controllerType);
				}
			}
			else
				throw new Exception(string.Format("Could not find a type for the controller name '{0}'", controllerName));
        }

        protected override void DisposeController(IController controller)
        {
            //TODO: Release properly.
        }
    }
}
