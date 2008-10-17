using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Filters;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using MvcContrib.TestHelper;
using System.Collections.Generic;
namespace MvcContrib.UnitTests.ConventionController
{
	[TestFixture]
	public class RescueTester
	{
		private RescueViewEngine _viewEngine;
		private ControllerContext _controllerContext;
		private MockRepository _mocks;
		private Exception _exception;
		private Controller _controller;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_viewEngine = new RescueViewEngine();
			_exception = new Exception();

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(_viewEngine);

			SetupController(new RescueTestController());
			
		}

		[TearDown]
		public void Teardown()
		{
			ViewEngines.Engines.Clear();
		}

		private void SetupController(Controller controller)
		{
			_controller = controller;
			var routeData = new RouteData();
			routeData.Values.Add("controller", "test");
			routeData.Values.Add("action", "foo");
			_controllerContext = new ControllerContext(_mocks.DynamicHttpContextBase(), routeData, controller);
			_controller.ControllerContext = _controllerContext;
		}

		[Test]
		public void ViewName_should_return_name_of_view_appended_to_Rescues_directory()
		{
			var rescue = new RescueAttribute("TestRescue");
			Assert.That(rescue.ViewName, Is.EqualTo("Rescues/TestRescue"));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Should_throw_if_ViewName_is_empty_string()
		{
			var rescue = new RescueAttribute(string.Empty);
		}

		[Test]
		public void When_OnActionExecuted_is_invoked_then_the_correct_view_should_be_rendered()
		{
			var rescue = new RescueAttribute("TestRescue");

			var context = new ExceptionContext(_controllerContext, _exception);
			rescue.OnException(context);
            Assert.That(context.ExceptionHandled);
			string expectedRescueView = "Rescues/TestRescue";

			context.Result.AssertViewRendered().ForView(expectedRescueView);
		}

		[Test]
		public void If_rescue_exception_type_does_not_match_exception_type_then_nothing_should_be_rendered()
		{
			_exception = new RescueTestException();
			SetupController(_controller);
			var rescue = new RescueAttribute("TestRescue", typeof(InvalidOperationException));
			var context = new ExceptionContext(_controllerContext, _exception);
            rescue.OnException(context);

			Assert.That(context.ExceptionHandled, Is.False);
			Assert.That(((RescueTestController)_controller).OnExceptionFired, Is.False);
		}

		[Test]
		public void If_rescue_exception_type_matches_exception_type_then_view_should_be_rendered()
		{
			_exception = new RescueTestException();
			SetupController(_controller);
			var rescue = new RescueAttribute("TestRescue", typeof(RescueTestException));
			var context = new ExceptionContext(_controllerContext, _exception);
            rescue.OnException(context);
			
			string expectedRescueView = "Rescues/TestRescue";

			context.Result.AssertViewRendered().ForView(expectedRescueView);
		}

		[Test]
		public void ThreadAbortException_should_be_ignored()
		{
			var constructors = typeof(ThreadAbortException).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
			//Find the constructor with 0 args
			var exception = (ThreadAbortException)constructors.Where(c => c.GetParameters().Length == 0).First().Invoke(null);

			var rescue = new RescueAttribute("TestRescue");
			var context = new ExceptionContext(_controllerContext, exception);
			rescue.OnException(context);

            Assert.IsTrue(context.ExceptionHandled);
			Assert.That(context.Result, Is.InstanceOfType(typeof(EmptyResult)));
		}

		[Test]
		public void When_PerformRescue_is_invoked_with_matching_view_it_should_be_rendered()
		{
			var rescue = new RescueAttribute("TestRescue");
			_viewEngine.CustomViews.Add("Rescues/RescueTestException");

			var context = new ExceptionContext(_controllerContext, new RescueTestException());

			rescue.OnException(context);
			
			Assert.That(context.ExceptionHandled);
			context.Result.AssertViewRendered().ForView("Rescues/RescueTestException");
		}

		[Test]
		public void When_PerformRescue_is_invoked_with_matching_view_and_AutoLocate_off_it_should_not_be_rendered() 
		{
			_viewEngine.CustomViews.Add("Rescues/RescueTestException");
			var rescue = new RescueAttribute("TestRescue") {AutoLocate = false};

			var context = new ExceptionContext(_controllerContext, new RescueTestException());
			rescue.OnException(context);
			Assert.That(context.ExceptionHandled);
			context.Result.AssertViewRendered().ForView("Rescues/TestRescue");
		}

		[Test]
		public void When_PerformRescue_exact_exception_executed_first() 
		{
			var rescue = new RescueAttribute("TestRescue");

			_viewEngine.CustomViews.Add("Rescues/InheritedRescueTestException");
			_viewEngine.CustomViews.Add("Rescues/RescueTestException");

			var context = new ExceptionContext(_controllerContext, new InheritedRescueTestException());
			rescue.OnException(context);

			Assert.That(context.ExceptionHandled);
			context.Result.AssertViewRendered().ForView("Rescues/InheritedRescueTestException");

			context = new ExceptionContext(_controllerContext, new RescueTestException());
			rescue.OnException(context);

			Assert.That(context.ExceptionHandled);
			context.Result.AssertViewRendered().ForView("Rescues/RescueTestException");

			rescue = new RescueAttribute("TestRescue", typeof(RescueTestException));

			_viewEngine.CustomViews.Clear();
			_viewEngine.CustomViews.Add("Rescues/RescueTestException");
			
			context = new ExceptionContext(_controllerContext, new InheritedRescueTestException());
			rescue.OnException(context);

			Assert.That(context.ExceptionHandled);
			context.Result.AssertViewRendered().ForView("Rescues/RescueTestException");

		}

		private class RescueViewEngine : IViewEngine 
		{

			public List<string> CustomViews = new List<string>();

			public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName) 
			{
				return null;
			}

			public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName) 
			{
				if(CustomViews.Contains(viewName))
				{
					return new ViewEngineResult(MockRepository.GenerateStub<IView>(),this);
				}
				return new ViewEngineResult(new List<string>());
			}

		    public void ReleaseView(ControllerContext controllerContext, IView view)
		    {
		        throw new System.NotImplementedException();
		    }
		}

		[Rescue("TestRescue")]
		private class RescueTestController : Controller 
		{
			public bool OnExceptionFired;

			protected override void OnException(ExceptionContext filterContext)
			{
				OnExceptionFired = true;
			}
		}


		private class RescueTestException : Exception 
		{
		}

		private class InheritedRescueTestException : RescueTestException 
		{
		}
	}

	
}
