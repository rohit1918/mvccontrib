using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace MvcContrib.ControllerFactories
{
	public class StructureMapControllerFactory : DefaultControllerFactory
	{
	    protected override IController CreateController(RequestContext context, string controllerName)
	    {
	    	Type controllerType = base.GetControllerType(controllerName);
			return ObjectFactory.GetInstance(controllerType) as IController;
	    }
	}
}
