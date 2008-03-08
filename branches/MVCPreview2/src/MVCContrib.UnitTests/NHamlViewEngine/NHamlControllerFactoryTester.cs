using System.Web.Mvc;
using MvcContrib.ControllerFactories;
using MvcContrib.ViewFactories;
using NUnit.Framework;
using System.Web.Routing;
using Rhino.Mocks;
using MvcContrib.TestHelper;

namespace MvcContrib.UnitTests.NHamlViewEngine
{
	[TestFixture, Category("NHamlViewEngine")]
	public class NHamlControllerFactoryTester
	{
		[Test, Ignore]
		public void ControllerFactory_Sets_Controller_ViewFactory_To_NHamlViewFactory()
		{
			NHamlControllerFactory controllerFactory = new NHamlControllerFactory();
			ConventionController controller =
				(ConventionController)((IControllerFactory)controllerFactory).CreateController(null, "Convention");

			Assert.IsNotNull(controller.ViewEngine);
			Assert.IsAssignableFrom(typeof(NHamlViewFactory), controller.ViewEngine);
		}
	}
}