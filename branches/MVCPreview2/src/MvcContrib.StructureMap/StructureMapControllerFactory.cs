using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace MvcContrib.ControllerFactories
{
	public class StructureMapControllerFactory : IControllerFactory
	{
	    public IController CreateController(RequestContext context, string controllerName)
	    {
            
			controllerName = controllerName + "Controller";
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
	        Type type = null;
            foreach(Assembly assembly in assemblies)
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if(t.Name.Equals(controllerName))
                    {
                        type = t;
                        break;
                    }
                }
            }
	        return ObjectFactory.GetInstance(type) as IController;
	    	//return ObjectFactory.GetNamedInstance<IController>(controllerName);
	    }

	    public void DisposeController(IController controller)
	    {
	        //throw new NotImplementedException();
	    }
	}
}
