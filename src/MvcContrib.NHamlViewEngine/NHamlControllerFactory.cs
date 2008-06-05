using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.NHamlViewEngine;

namespace MvcContrib.ControllerFactories
{
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class NHamlControllerFactory : DefaultControllerFactory
	{
		protected override IController CreateController(RequestContext requestContext, string controllerName)
		{
			var controller = base.CreateController(requestContext, controllerName);
			var c = controller as Controller;
			if(c != null) c.ViewEngine = new NHamlViewFactory();
			return controller;
		}
	}
}