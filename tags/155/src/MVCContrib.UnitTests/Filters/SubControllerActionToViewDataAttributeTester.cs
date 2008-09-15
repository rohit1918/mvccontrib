using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Filters;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.Filters
{
	[TestFixture]
	public class SubControllerActionToViewDataAttributeTester
	{
		[Test]
		public void ShouldPushTheActionOfEachSubcontrollerIntoViewdata()
		{
			var c1 = new SubController();
			var c2 = new SubController();

			var attribute = new SubControllerActionToViewDataAttribute();
			var controller = new TestingController();
			ActionExecutingContext context = GetFilterContext(controller);
			context.ActionParameters["c1"] = c1;
			context.ActionParameters["c2"] = c2;

			attribute.OnActionExecuting(context);

			Assert.That(controller.ViewData.Get<Action>("c1"), Is.Not.Null);
			Assert.That(controller.ViewData.Get<Action>("c2"), Is.Not.Null);
		}

		[Test]
		public void ShouldIgnoreActionParametersThatAreNull()
		{
			var attribute = new SubControllerActionToViewDataAttribute();
			var controller = new TestingController();
			ActionExecutingContext context = GetFilterContext(controller);
			context.ActionParameters["c1"] = null;
			context.ActionParameters["c2"] = new SubController();

			attribute.OnActionExecuting(context);

			Assert.That(controller.ViewData.Count, Is.EqualTo(1));
			Assert.That(controller.ViewData.Get<Action>("c2"), Is.Not.Null);
		}


		private static ActionExecutingContext GetFilterContext(ControllerBase controller)
		{
			var mockResponse = MockRepository.GenerateStub<HttpResponseBase>();
			var mockHttpContext = MockRepository.GenerateStub<HttpContextBase>();
			mockHttpContext.Stub(c => c.Response).Return(mockResponse).Repeat.Any();
			mockHttpContext.Stub(c => c.Timestamp).Return(new DateTime(2001, 1, 1)).Repeat.Any();
			var controllerContext = new ControllerContext(mockHttpContext, new RouteData(), controller);
			controller.ControllerContext = controllerContext;
			var actionExecutingContext = new ActionExecutingContext(
				controllerContext, new Dictionary<string, object>());
			return actionExecutingContext;
		}
	}

	internal class TestingController : Controller
	{
	}
}