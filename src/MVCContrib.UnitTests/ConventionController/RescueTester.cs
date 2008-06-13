using System;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib;
using MvcContrib.Attributes;
using MvcContrib.UnitTests.ConventionController;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using System.Web;

namespace MvcContrib.UnitTests.ConventionController
{
	[TestFixture]
	public class RescueTester
	{
		private RescueViewEngine _viewEngine;
		private ControllerContext _controllerContext;
		private MockRepository _mocks;
		private Exception _exception;
		private BaseRescueTestController _controller;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_viewEngine = new RescueViewEngine();
			_exception = new Exception();

			SetupController(new RescueTestController());
		}

		private void SetupController(BaseRescueTestController controller)
		{
			_controller = controller;
			_controller.ViewEngine = _viewEngine;
			_controllerContext = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), controller);
			_controller.ControllerContext = _controllerContext;
		}

		[Test]
		public void ViewName_should_return_name_of_view_appended_to_Rescues_directory()
		{
			RescueAttribute rescue = new RescueAttribute("TestRescue");
			Assert.That(rescue.ViewName, Is.EqualTo("Rescues/TestRescue"));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Should_throw_if_ViewName_is_empty_string()
		{
			RescueAttribute rescue = new RescueAttribute(string.Empty);
		}

		[Test]
		public void When_OnActionExecuted_is_invoked_then_the_correct_view_should_be_rendered()
		{
			RescueAttribute rescue = new RescueAttribute("TestRescue");
			Assert.That(rescue.PerformRescue(_exception, _controller), Is.True);
			string expectedRescueView = "Rescues/TestRescue";
			Assert.That(_viewEngine.ViewContext.ViewName, Is.EqualTo(expectedRescueView));
		}

		[Test]
		public void If_controller_is_a_ConventionController_then_OnPreRescue_should_be_invoked()
		{
			RescueAttribute rescue = new RescueAttribute("TestRescue");
			Assert.That(((RescueTestController)_controller).OnPreRescueFired, Is.False);
			Assert.That(rescue.PerformRescue(_exception, _controller), Is.True);
			Assert.That(((RescueTestController)_controller).OnPreRescueFired, Is.True);
		}

		[Test]
		public void If_rescue_exception_type_does_not_match_exception_type_then_nothing_should_be_rendered()
		{
			RescueAttribute rescue = new RescueAttribute("TestRescue");
			_exception = new RescueTestException();
			SetupController(_controller);
			rescue = new RescueAttribute("TestRescue", typeof(InvalidOperationException));
			
			Assert.That(rescue.PerformRescue(_exception, _controller), Is.False);
			Assert.That(_viewEngine.ViewContext, Is.Null);
			Assert.That(((RescueTestController)_controller).OnPreRescueFired, Is.False);
		}

		[Test]
		public void If_rescue_exception_type_matches_exception_type_then_view_should_be_rendered()
		{
			RescueAttribute rescue = new RescueAttribute("TestRescue");

			_exception = new RescueTestException();
			SetupController(_controller);
			rescue = new RescueAttribute("TestRescue", typeof(RescueTestException));
			rescue.PerformRescue(_exception, _controller);
			
			string expectedRescueView = "Rescues/TestRescue";
			Assert.That(_viewEngine.ViewContext.ViewName, Is.EqualTo(expectedRescueView));

		}

		[Test]
		public void When_exception_is_thrown_by_an_action_then_it_should_be_handled_by_action_level_rescue()
		{
			_controller.InvokeActionPublic("ThrowMethodError");
			string expectedRescueView = "Rescues/TestMethodRescue";
			Assert.That(_viewEngine.ViewContext.ViewName, Is.EqualTo(expectedRescueView));
		}

		[Test]
		public void When_exception_is_thrown_by_an_action_then_it_should_be_handled_by_controller_level_rescue()
		{
			_controller.InvokeActionPublic("ThrowError");
			string expectedRescueView = "Rescues/TestRescue";
			Assert.That(_viewEngine.ViewContext.ViewName, Is.EqualTo(expectedRescueView));
		}

		[Test]
		public void When_exception_is_thrown_by_a_filter_then_it_should_be_handled()
		{
			_controller.InvokeActionPublic("ThrowFilter");
			string expectedRescueView = "Rescues/TestRescue";
			Assert.That(_viewEngine.ViewContext.ViewName, Is.EqualTo(expectedRescueView));
			Assert.IsFalse(((RescueTestController)_controller).ActionExecuted);
		}

		[Test]
		[Ignore]
		public void ThreadAbortException_should_be_ignored()
		{
			ThreadAbortException exception =
				(ThreadAbortException)typeof(ThreadAbortException).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0].Invoke(null);

			var rescue = new RescueAttribute("TestRescue");
			Assert.IsTrue(rescue.PerformRescue(exception, _controller));
			Assert.IsNull(_viewEngine.ViewContext);
		}
	}

	public class RescueViewEngine : IViewEngine
	{
		public ViewContext ViewContext { get; set; }

		public void RenderView(ViewContext viewContext)
		{
			ViewContext = viewContext;
		}
	}

	public class ThrowFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			throw new Exception();
		}
	}

	[Rescue("TestRescue")]
	internal class RescueTestController : BaseRescueTestController
	{
		public bool OnPreRescueFired;
		public bool ActionExecuted;

		public ActionResult ThrowError()
		{
			throw new NotImplementedException();
		}

		[Rescue("TestMethodRescue")]
		public ActionResult ThrowMethodError()
		{
			throw new NotImplementedException();
		}

		[ThrowFilter]
		public ActionResult ThrowFilter()
		{
			ActionExecuted = true;
			return new EmptyResult();
		}

		public override void OnPreRescue(Exception exception)
		{
			OnPreRescueFired = true;
			base.OnPreRescue(exception);
		}
	}

	[Rescue("Test", typeof(RescueTestException))]
	internal class RescueTestControllerWithExceptionTypes : BaseRescueTestController
	{
		public void ThrowError()
		{
			throw new RescueTestException();
		}

		[Rescue("Test", typeof(RescueTestException))]
		public void ThrowMethodError()
		{
			throw new RescueTestException();
		}

		[Rescue("Test", typeof(RescueTestException))]
		public void DontDoThis()
		{
			throw new NotImplementedException();
		}
	}

	internal abstract class BaseRescueTestController : MvcContrib.ConventionController
	{
		public string ViewRendered
		{
			get;
			private set;
		}

		public void InvokeActionPublic(string actionName)
		{
			ControllerContext.RouteData.Values.Add("action", actionName);
			Execute(ControllerContext);
		}
	}

	internal class RescueTestException : Exception
	{
	}
}
