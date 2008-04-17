using System.Web.Mvc;
using MvcContrib.ControllerFactories;
using MvcContrib.NHamlViewEngine;
using NUnit.Framework;
using System.Web.Routing;
using Rhino.Mocks;
using MvcContrib.TestHelper;

namespace MvcContrib.UnitTests.NHamlViewEngine
{
	[TestFixture, Category("NHamlViewEngine")]
	public class NHamlControllerFactoryTester
	{
		[Test]
		public void ControllerFactory_Sets_Controller_ViewFactory_To_NHamlViewFactory()
		{
			MockRepository mocks = new MockRepository();
			RequestContext context = new RequestContext(mocks.DynamicHttpContextBase(), new RouteData());

			NHamlControllerFactory controllerFactory = new NHamlControllerFactory();
			MvcContrib.ConventionController controller =
				(MvcContrib.ConventionController)((IControllerFactory)controllerFactory).CreateController(context, "Convention");

			Assert.IsNotNull(controller.ViewEngine);
			Assert.IsAssignableFrom(typeof(NHamlViewFactory), controller.ViewEngine);
		}
	}
}