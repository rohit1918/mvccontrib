using System;
using System.Reflection;
using MvcContrib;
using MvcContrib.Attributes;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MVCContrib.UnitTests
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
			_rescueTestControllerDecorated = new RescueTestController();
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

			string publicActionName = "ThrowMethodError";
			rescueTestControllerDecorated.InvokeActionPublic(publicActionName);

			string expectedRescueView = "Rescues/TestMethodRescue";

			Assert.That(rescueTestControllerDecorated.ViewRendered, Is.EqualTo(expectedRescueView));
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

			string publicActionName = "ThrowError";
			controller.InvokeActionPublic(publicActionName);

			string expectedRescueView = "Rescues/Test";

			Assert.That(controller.ViewRendered, Is.EqualTo(expectedRescueView));
		}

		[Test]
		public void Then_controller_Should_not_rescue_the_action_if_the_incorrect_ExceptionType_is_thrown()
		{
			RescueTestControllerWithExceptionTypes controller = new RescueTestControllerWithExceptionTypes();

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

	internal abstract class BaseRescueTestController : ConventionController
	{
		protected override void RenderView(string viewName, string masterName, object viewData)
		{
			this.ViewRendered = viewName;
		}

		public string ViewRendered
		{
			get;
			private set;
		}

		public bool InvokeActionPublic(string actionName)
		{
			return base.InvokeAction(actionName);
		}
	}

	internal class RescueTestException : Exception
	{
	}
}