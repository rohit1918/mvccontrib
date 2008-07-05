using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ControllerFactories;
using MvcContrib.NHamlViewEngine;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.NHamlViewEngine
{
	[TestFixture, Category("NHamlViewEngine")]
	public class NHamlControllerFactoryTester
	{
		[Test]
		public void ControllerFactory_Sets_Controller_ViewFactory_To_NHamlViewFactory()
		{
			var mocks = new MockRepository();
			var context = new RequestContext(mocks.DynamicHttpContextBase(), new RouteData());

			var controllerFactory = new NHamlControllerFactory();
			var controller =
				(Controller)((IControllerFactory)controllerFactory).CreateController(context, "Convention");

			Assert.IsNotNull(controller.ViewEngine);
			Assert.IsAssignableFrom(typeof(NHamlViewFactory), controller.ViewEngine);
		}
	}
}
