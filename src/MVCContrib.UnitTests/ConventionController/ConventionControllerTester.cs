using MvcContrib.ActionResults;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.ConventionController
{
	[TestFixture]
	public class ConventionControllerTester
	{
		private TestController _controller;

		[SetUp]
		public void SetUp()
		{
			_controller = new TestController();
		}

		[Test]
		public void RenderXml_should_return_XmlResult_object()
		{
			var result = _controller.XmlResult() as XmlResult;
			Assert.That(result, Is.Not.Null);
			Assert.That(result.ObjectToSerialize, Is.EqualTo("Test 1 2 3"));
		}

		[Test]
		public void Binary_should_return_BinaryResult_object()
		{
			var result = _controller.BinaryResult() as BinaryResult;
			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void Expression_based_redirect_to_action_should_redirect_correctly_to_same_controller()
		{
			var redirectToRouteResult = _controller.RedirectActionOnSameController();

			Assert.That(redirectToRouteResult.Values["Controller"], Is.EqualTo("Test"));
			Assert.That(redirectToRouteResult.Values["Action"], Is.EqualTo("BasicAction"));
			Assert.That(redirectToRouteResult.Values["Id"], Is.EqualTo(1));
		}

		[Test]
		public void Expression_based_redirect_to_action_should_redirect_correctly_to_another_controller()
		{
			var redirectToRouteResult = _controller.RedirectActionOnAnotherController();

			Assert.That(redirectToRouteResult.Values["Controller"], Is.EqualTo("AnotherTest"));
			Assert.That(redirectToRouteResult.Values["Action"], Is.EqualTo("SomeAction"));
			Assert.That(redirectToRouteResult.Values["Id"], Is.EqualTo(2));
		}
	}
}