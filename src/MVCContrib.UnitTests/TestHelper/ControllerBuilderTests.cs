using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using MvcContrib.TestHelper;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;


//Note: these tests confirm that the TestControllerBuilder works properly
//for examples on how to use the TestControllerBuilder and other TestHelper classes,
//look in the \src\Samples\MvcContrib.TestHelper solution
namespace MvcContrib.TestHelper.Test
{
	[TestFixture]
	public class ControllerBuilderTests
	{
		[Test]
		public void CanSpecifyFormVariables()
		{
			TestControllerBuilder builder = new TestControllerBuilder();
			builder.Form["Variable"] = "Value";
			var controller = new TestHelperController();
			builder.InitializeController(controller);
			Assert.AreEqual("Value", controller.HttpContext.Request.Form["Variable"]);
		}

		[Test]
		public void CanSpecifyRouteData()
		{
			TestControllerBuilder builder = new TestControllerBuilder();
			RouteData rd = new RouteData();
			rd.Values["Variable"] = "Value";
			builder.RouteData = rd;

			var controller = new TestHelperController();
			builder.InitializeController(controller);
			Assert.AreEqual("Value", controller.RouteData.Values["Variable"]);
		}

		[Test]
		public void CanSpecifyQueryString()
		{
			TestControllerBuilder handler = new TestControllerBuilder();
			handler.QueryString["Variable"] = "Value";
			var testController = new TestHelperController();
			handler.InitializeController(testController);
			Assert.AreEqual("Value", testController.Request.QueryString["Variable"]);
		}

		[Test]
		public void When_params_is_invoked_it_should_return_a_combination_of_form_and_querystring()
		{
			var builder = new TestControllerBuilder();
			builder.QueryString["foo"] = "bar";
			builder.Form["baz"] = "blah";
			var testController = new TestHelperController();
			builder.InitializeController(testController);
			Assert.That(testController.Request.Params["foo"], Is.EqualTo("bar"));
			Assert.That(testController.Request.Params["baz"], Is.EqualTo("blah"));
		}
	}
}
