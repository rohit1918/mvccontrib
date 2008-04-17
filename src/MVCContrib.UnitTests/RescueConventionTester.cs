using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib;
using MvcContrib.Attributes;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using System.Web;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	[Category("RescueConventionTester")]
	public class When_ConventionController_is_decorated_with_Rescue_attribute_and_exception_occurs
	{
		private RescueTestController _rescueTestControllerDecorated;

		[Test]
		public void Then_controller_should_render_the_RescueAttribute_view()
		{
			string expectedRescueView = "Rescues/TestRescue";

			Assert.That(_rescueTestControllerDecorated.ViewRendered, Is.EqualTo(expectedRescueView));
		}

		[Test]
		public void And_the_onPreRescue_method_should_be_invoked()
		{
			Assert.That(_rescueTestControllerDecorated.onPreRescueFired, Is.True);
		}

		[SetUp]
		public void SetUpContext()
		{
			MockRepository mocks = new MockRepository();
			HttpContextBase context = mocks.DynamicMock<HttpContextBase>();
			mocks.Replay(context);
			_rescueTestControllerDecorated = new RescueTestController();
			_rescueTestControllerDecorated.ControllerContext = new ControllerContext(context, new RouteData(), _rescueTestControllerDecorated);
			string publicActionName = "ThrowError";
			_rescueTestControllerDecorated.InvokeActionPublic(publicActionName);
		}
	}

	[TestFixture]
	[Category("RescueConventionTester")]
	public class When_ConventionController_method_is_decorated_with_Rescue_attribute_and_exception_occurs
	{
		[Test]
		public void Then_controller_should_render_method_RescueAttribute_view()
		{
			RescueTestController rescueTestControllerDecorated = new RescueTestController();
			SetControllerContext(rescueTestControllerDecorated);

			string publicActionName = "ThrowMethodError";
			rescueTestControllerDecorated.InvokeActionPublic(publicActionName);

			string expectedRescueView = "Rescues/TestMethodRescue";

			Assert.That(rescueTestControllerDecorated.ViewRendered, Is.EqualTo(expectedRescueView));
		}

		public void SetControllerContext(Controller controller)
		{
			MockRepository mocks = new MockRepository();
			HttpContextBase context = mocks.DynamicMock<HttpContextBase>();
			mocks.Replay(context);
			controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);
		}
	}

	[TestFixture]
	[Category("RescueConventionTester")]
	public class When_ConventionController_method_is_decorated_with_Rescue_attribute_and_an_ExceptionType_is_specified
	{
		[Test]
		public void Then_controller_should_render_method_RescueAttribute_view_for_only_the_ExceptionType_specified()
		{
			RescueTestControllerWithExceptionTypes controller = new RescueTestControllerWithExceptionTypes();
			SetControllerContext(controller);

			string publicActionName = "ThrowError";
			controller.InvokeActionPublic(publicActionName);

			string expectedRescueView = "Rescues/Test";

			Assert.That(controller.ViewRendered, Is.EqualTo(expectedRescueView));
		}

		[Test]
		public void Then_controller_Should_not_rescue_the_action_if_the_incorrect_ExceptionType_is_thrown()
		{
			RescueTestControllerWithExceptionTypes controller = new RescueTestControllerWithExceptionTypes();
			SetControllerContext(controller);

			string publicActionName = "DontDoThis";

			try
			{
				controller.InvokeActionPublic(publicActionName);
			}
			catch(TargetInvocationException exc)
			{
				Assert.IsAssignableFrom(typeof(NotImplementedException), exc.InnerException);
			}
		}

		public void SetControllerContext(Controller controller)
		{
			MockRepository mocks = new MockRepository();
			HttpContextBase context = mocks.DynamicMock<HttpContextBase>();
			mocks.Replay(context);
			controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);
		}
	}

	[Rescue("TestRescue")]
	internal class RescueTestController : BaseRescueTestController
	{
		public bool onPreRescueFired;

		public void ThrowError()
		{
			throw new NotImplementedException();
		}

		[Rescue("TestMethodRescue")]
		public void ThrowMethodError()
		{
			throw new NotImplementedException();
		}

		protected override void OnPreRescue(Exception exception)
		{
			onPreRescueFired = true;
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
		/*protected override void RenderView(string viewName, string masterName, object viewData)
		{
			this.ViewRendered = viewName;
		}*/

		public string ViewRendered
		{
			get;
			private set;
		}

		public bool InvokeActionPublic(string actionName)
		{
			//return base.InvokeAction(actionName);
			throw new NotImplementedException();
		}
	}

	internal class RescueTestException : Exception
	{
	}
}