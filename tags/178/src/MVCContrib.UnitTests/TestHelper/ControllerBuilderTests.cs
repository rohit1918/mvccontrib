using System;
using System.Security.Principal;
using System.Web.Caching;
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
		private TestControllerBuilder builder;

		[SetUp]
		public void Setup()
		{
			builder = new TestControllerBuilder();
		}

		[Test]
		public void CanSpecifyFormVariables()
		{
			builder.Form["Variable"] = "Value";
			var controller = new TestHelperController();
			builder.InitializeController(controller);
			Assert.AreEqual("Value", controller.HttpContext.Request.Form["Variable"]);
		}

		[Test]
		public void CanSpecifyRouteData()
		{
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
			builder.QueryString["Variable"] = "Value";
			var testController = new TestHelperController();
			builder.InitializeController(testController);
			Assert.AreEqual("Value", testController.Request.QueryString["Variable"]);
		}

		[Test]
		public void CanSpecifyAppRelativeCurrentExecutionFilePath()
		{
			builder.AppRelativeCurrentExecutionFilePath = "someUrl";
			var testController = new TestHelperController();
			builder.InitializeController(testController);
			Assert.AreEqual("someUrl", testController.Request.AppRelativeCurrentExecutionFilePath);
		}

		[Test]
		public void CanSpecifyApplicationPath()
		{
			builder.ApplicationPath = "someUrl";
			var testController = new TestHelperController();
			builder.InitializeController(testController);
			Assert.AreEqual("someUrl", testController.Request.ApplicationPath);
		}

		[Test]
		public void CanSpecifyPathInfol()
		{
			builder.PathInfo = "someUrl";
			var testController = new TestHelperController();
			builder.InitializeController(testController);
			Assert.AreEqual("someUrl", testController.Request.PathInfo);
		}

		[Test]
		public void CanSpecifyRawUrl()
		{
			builder.RawUrl = "someUrl";
			var testController = new TestHelperController();
			builder.InitializeController(testController);
			Assert.AreEqual("someUrl", testController.Request.RawUrl);
		}

		[Test]
		public void CanCreateControllerWithNoArgs()
		{
			builder.QueryString["Variable"] = "Value";
			var testController = builder.CreateController<TestHelperController>();
			Assert.AreEqual("Value", testController.Request.QueryString["Variable"]);
		}

		[Test]
		public void When_params_is_invoked_it_should_return_a_combination_of_form_and_querystring()
		{
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
			builder.QueryString["Variable"] = "Value";
			var testController = builder.CreateController<TestHelperWithArgsController>(new TestService());
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
				builder.QueryString["Variable"] = "Value";
				var testController = builder.CreateIoCController<TestHelperWithArgsController>();
				Assert.AreEqual("Value", testController.Request.QueryString["Variable"]);
				Assert.AreEqual("Moo", testController.ReturnMooFromService());
			}
		}

		[Test]
		public void UserShouldBeMocked()
		{
			var mocks = new MockRepository();
			var user = mocks.DynamicMock<IPrincipal>();

			var controller = builder.CreateController<TestHelperController>();
			controller.ControllerContext.HttpContext.User = user;

			Assert.AreSame(user, controller.User);
		}

		[Test]
		public void CacheIsAvailable()
		{
			var builder = new TestControllerBuilder();

			Assert.IsNotNull(builder.HttpContext.Cache);

			var controller = new TestHelperController();
			builder.InitializeController(controller);

			Assert.IsNotNull(controller.HttpContext.Cache);

			string testKey = "TestKey";
			string testValue = "TestValue";

			controller.HttpContext.Cache.Add(testKey,
			                                 testValue,
			                                 null,
			                                 DateTime.Now.AddSeconds(1),
			                                 Cache.NoSlidingExpiration,
			                                 CacheItemPriority.Normal,
			                                 null);

			Assert.AreEqual(testValue,
			                controller.HttpContext.Cache[testKey]);
		}
	}
}