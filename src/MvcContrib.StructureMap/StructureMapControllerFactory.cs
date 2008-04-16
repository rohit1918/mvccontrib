using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;
using DefaultControllerFactory=MvcContrib.ControllerFactories.DefaultControllerFactory;

namespace MvcContrib.StructureMap
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