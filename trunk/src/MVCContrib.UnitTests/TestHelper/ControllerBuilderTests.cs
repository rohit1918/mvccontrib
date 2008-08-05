using System.Security.Principal;
using System.Web.Routing;
using MvcContrib.Interfaces;
using MvcContrib.Services;
using MvcContrib.TestHelper;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

//Note: these tests confirm that the TestControllerBuilder works properly
//for examples on how to use the TestControllerBuilder and other TestHelper classes,
//look in the \src\Samples\MvcContrib.TestHelper solution

namespace MvcContrib.UnitTests.TestHelper
{
	[TestFixture]
	public class ControllerBuilderTests
	{
		[Test]
		public void CanSpecifyFormVariables()
		{
			var builder = new TestControllerBuilder();
			builder.Form["Variable"] = "Value";
			var controller = new TestHelperController();
			builder.InitializeController(controller);
			Assert.AreEqual("Value", controller.HttpContext.Request.Form["Variable"]);
		}

		[Test]
		public void CanSpecifyRouteData()
		{
			var builder = new TestControllerBuilder();
			var rd = new RouteData();
			rd.Values["Variable"] = "Value";
			builder.RouteData = rd;

			var controller = new TestHelperController();
			builder.InitializeController(controller);
			Assert.AreEqual("Value", controller.RouteData.Values["Variable"]);
		}

		[Test]
		public void CanSpecifyQueryString()
		{
			var handler = new TestControllerBuilder();
			handler.QueryString["Variable"] = "Value";
			var testController = new TestHelperController();
			handler.InitializeController(testController);
			Assert.AreEqual("Value", testController.Request.QueryString["Variable"]);
		}

		[Test]
		public void CanCreateControllerWithNoArgs()
		{
			var handler = new TestControllerBuilder();
			handler.QueryString["Variable"] = "Value";
			var testController = handler.CreateController<TestHelperController>();
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

		[Test]
		public void CanCreateControllerWithArgs()
		{
			var handler = new TestControllerBuilder();
			handler.QueryString["Variable"] = "Value";
			var testController = handler.CreateController<TestHelperWithArgsController>(new TestService());
			Assert.AreEqual("Value", testController.Request.QueryString["Variable"]);
			Assert.AreEqual("Moo", testController.ReturnMooFromService());
		}

		[Test]
		public void CanCreateControllerWithIoCArgs()
		{
			var mocks = new MockRepository();
			using(mocks.Record())
			{
				var resolver = mocks.DynamicMock<IDependencyResolver>();
				Expect.Call(resolver.GetImplementationOf(typeof(TestHelperWithArgsController))).Return(
					new TestHelperWithArgsController(new TestService()));
				DependencyResolver.InitializeWith(resolver);
			}
			using(mocks.Playback())
			{
				var handler = new TestControllerBuilder();
				handler.QueryString["Variable"] = "Value";
				var testController = handler.CreateIoCController<TestHelperWithArgsController>();
				Assert.AreEqual("Value", testController.Request.QueryString["Variable"]);
				Assert.AreEqual("Moo", testController.ReturnMooFromService());
			}
		}

		[Test]
		public void UserShouldBeMocked()
		{
			var mocks = new MockRepository();
			var user = mocks.DynamicMock<IPrincipal>();
            
			var builder = new TestControllerBuilder();
			var controller = builder.CreateController<TestHelperController>();
			controller.ControllerContext.HttpContext.User = user;

			Assert.AreSame(user, controller.User);
		}
	}
}