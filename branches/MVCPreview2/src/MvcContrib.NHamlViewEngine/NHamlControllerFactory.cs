using System;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ViewFactories;

namespace MvcContrib.ControllerFactories
{
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class NHamlControllerFactory : DefaultControllerFactory
	{
		protected override IController CreateController(RequestContext requestContext, string controllerName)
		{
			IController controller = base.CreateController(requestContext, controllerName);
			Controller c = controller as Controller;
			c.ViewEngine = new NHamlViewFactory();
			return controller;
		}
	}
}