using MvcContrib.ControllerFactories;
using MvcContrib.ViewFactories;
using NUnit.Framework;

namespace MvcContrib.UnitTests.NHamlViewEngine
{
	[TestFixture, Category("NHamlViewEngine")]
	public class NHamlControllerFactoryTester
	{
		[Test]
		public void ControllerFactory_Sets_Controller_ViewFactory_To_NHamlViewFactory()
		{
			NHamlControllerFactory controllerFactory = new NHamlControllerFactory();
			ConventionController controller =
				(ConventionController)controllerFactory.CreateController(null, typeof(ConventionController));

			Assert.IsNotNull(controller.ViewFactory);
			Assert.IsAssignableFrom(typeof(NHamlViewFactory), controller.ViewFactory);
		}
	}
}