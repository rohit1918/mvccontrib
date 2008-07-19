using MvcContrib.UnitTests.ConventionController;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class ControllerExtensionsTester
	{
		[Test]
		public void RedirectToAction_should_redirect_correctly()
		{
			var redirectToRouteResult = new TestController().RedirectToAction(c => c.BasicAction(1));

			Assert.That(redirectToRouteResult.Values["Controller"], Is.EqualTo("Test"));
			Assert.That(redirectToRouteResult.Values["Action"], Is.EqualTo("BasicAction"));
			Assert.That(redirectToRouteResult.Values["Id"], Is.EqualTo(1));
		}
	}
}