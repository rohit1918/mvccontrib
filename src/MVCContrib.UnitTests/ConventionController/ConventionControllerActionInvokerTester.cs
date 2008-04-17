using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.MetaData;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class ConventionControllerActionInvokerTester
	{
		private MockRepository _mocks;
		private ControllerContext _context;
		private TestController _controller;
		private ConventionControllerActionInvoker _invoker;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_controller = new TestController();
			_context = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), _controller);
			_mocks.ReplayAll();
			_invoker = new ConventionControllerActionInvoker(_context);
		}

		private bool InvokeAction(string actionName, ConventionController controller)
		{
			throw new NotImplementedException();
		}

		private bool InvokeAction(string actionName)
		{
			return InvokeAction(actionName, _controller);
		}

		[Test]
		public void When_instantiated_the_Controller_property_should_be_set()
		{
			Assert.That(_invoker.Controller, Is.SameAs(_controller));
		}

		[Test, ExpectedException(typeof(Exception), ExpectedMessage = "The ConventionControllerActionInvoker can only be used with controllers that inherit from ConventionController.")]
		public void When_inistantiated_should_throw_if_controller_is_not_conventioncontroller()
		{
			_context = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), _mocks.DynamicMock<IController>());
			new ConventionControllerActionInvoker(_context);
		}

		[Test]
		public void FindActionMetaData_should_return_null_for_empty_string()
		{
			Assert.IsNull(_invoker.FindActionMetaData(string.Empty));
		}

		[Test]
		public void FindActionMetaData_should_return_null_for_null_action()
		{
			Assert.IsNull(_invoker.FindActionMetaData(null));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void InvokeAction_should_throw_for_empty_action()
		{
			_invoker.InvokeAction(string.Empty, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void InvokeAction_should_throw_for_null_action()
		{
			_invoker.InvokeAction(null, null);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void FindActionMetaData_should_throw_for_overloaded_actions()
		{
			_invoker.FindActionMetaData("SimpleAction");
		}


		[Test]
		public void FindActionMeta_should_return_defaultaction_the_specified_action_does_not_exist()
		{
			var controller = new DefaultActionController
			                 	{
			                 		ControllerDescriptor = new ControllerDescriptor()
			                 	};

			_context = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), controller);
			_invoker = new ConventionControllerActionInvoker(_context);

			var meta = _invoker.FindActionMetaData("Unknown");
			Assert.AreEqual("DefaultAction", meta.Name);
		}

		[Test]
		public void FindActionMeta_should_return_null_for_unknown_action()
		{
			Assert.That(_invoker.FindActionMetaData("Unknown"), Is.Null);
		}

		[Test]
		public void Should_set_selectedaction_on_controller()
		{
			Assert.That(_controller.SelectedAction, Is.Null);
			_invoker.InvokeAction("BasicAction", null);
			Assert.That(_controller.SelectedAction, Is.Not.Null);
			Assert.That(_controller.SelectedAction.Name, Is.EqualTo("BasicAction"));
		}

		[Test]
		public void Invoke_should_return_false_for_invalid_action()
		{
			Assert.That(_invoker.InvokeAction("Unknown", null), Is.False);
		}

		[Test]
		public void Valid_action_Returns_true()
		{
			Assert.IsTrue(_invoker.InvokeAction("ComplexAction", null));
			Assert.IsTrue(_controller.ActionWasCalled);
		}

		/*[Test]
		//[ExpectedException(typeof(TargetInvocationException))]
		//public void BadActionThrows()
		//{
		//	InvokeAction("BadAction");
		//}

		[Test]
		public void ValidActionReturnsTrue()
		{
			Assert.IsTrue(InvokeAction("ComplexAction"));
			Assert.IsTrue(_controller.ActionWasCalled);
		}

		[Test]
		public void HiddenActionReturnsFalse()
		{
			Assert.IsFalse(InvokeAction("HiddenAction"));
		}

		[Test]
		public void ValidActionReturnsFalseWhenOnPreActionReturnsFalse()
		{
			_controller.CancelAction = true;
			InvokeAction("ComplexAction");
			Assert.IsFalse(_controller.ActionWasCalled);
		}

		[Test]
		public void ControllerDescriptorDefaultsToCached()
		{
			TestController controller = new TestController();
			Assert.IsNotNull(controller.ControllerDescriptor);
			Assert.AreEqual(typeof(CachedControllerDescriptor), controller.ControllerDescriptor.GetType());
		}

		[Test]
		public void OnErrorShouldNotThrowIfInnerExceptionIsNull()
		{
			TestController controller = new TestController();
			controller.InvokeOnErrorWithoutInnerException();
			Assert.IsTrue(controller.OnErrorWasCalled);
		}

		[Test]
		public void ShouldNotBeRescuedWhenThreadAbortExceptionThrownDueToRedirect()
		{
			Expect.Call(() => _controller.Response.Redirect(null));
			_mocks.Replay(_controller.Response);
			InvokeAction("WithRedirect");
			_mocks.Verify(_controller.Response);

			Assert.IsTrue(_controller.OnErrorWasCalled);
			Assert.IsTrue(_controller.OnErrorResult.Value);
		}

		[Test]
		public void ReturnBinderShouldBeInvoked()
		{
			InvokeAction("ReturnBinder");
			Assert.IsTrue(_controller.ReturnBinderInvoked);
		}	*/
		
	}
}