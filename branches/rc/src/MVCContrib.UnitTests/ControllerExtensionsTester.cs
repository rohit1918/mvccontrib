using MvcContrib.UnitTests.ConventionController;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class ControllerExtensionsTester
	{
		[Test]
		public void RedirectToAction_should_redirect_correctly_on_same_controller()
		{
			var redirectToRouteResult = new AnotherTestController().RedirectToAction(c => c.SomeAction(1));

			Assert.That(redirectToRouteResult.RouteValues["Controller"], Is.EqualTo("AnotherTest"));
			Assert.That(redirectToRouteResult.RouteValues["Action"], Is.EqualTo("SomeAction"));
			Assert.That(redirectToRouteResult.RouteValues["Id"], Is.EqualTo(1));
		}

		[Test]
		public void RedirectToAction_should_redirect_correctly_on_another_controller()
		{
			var redirectToRouteResult = new AnotherTestController().RedirectToAction<TestController>(c => c.BasicAction(2));

			Assert.That(redirectToRouteResult.RouteValues["Controller"], Is.EqualTo("Test"));
			Assert.That(redirectToRouteResult.RouteValues["Action"], Is.EqualTo("BasicAction"));
			Assert.That(redirectToRouteResult.RouteValues["Id"], Is.EqualTo(2));
		}
	}
}