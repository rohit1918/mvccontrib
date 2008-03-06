using System;
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
	    	return ObjectFactory.GetNamedInstance<IController>(controllerName);
	    }

	    public void DisposeController(IController controller)
	    {
	        //throw new NotImplementedException();
	    }
	}
}
