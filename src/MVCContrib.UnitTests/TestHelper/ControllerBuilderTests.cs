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
	}
}
