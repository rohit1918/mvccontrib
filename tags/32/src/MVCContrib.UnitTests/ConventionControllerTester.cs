using System;
using System.Reflection;
using System.Threading;
using MvcContrib;
using MvcContrib.Attributes;
using MvcContrib.MetaData;
using NUnit.Framework;

namespace MVCContrib.UnitTests
{
	[TestFixture]
	public class ConventionControllerTester
	{
		private TestController _controller;

		[SetUp]
		public void SetUp()
		{
			_controller = new TestController();
			_controller.ControllerDescriptor = new ControllerDescriptor();
		}

		[Test]
		[ExpectedException(typeof(TargetInvocationException))]
		public void BadActionThrows()
		{
			_controller.DoInvokeAction("BadAction");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyActionThrows()
		{
			_controller.DoInvokeAction(string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void NullActionThrows()
		{
			_controller.DoInvokeAction(null);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void OverloadedActionsThrow()
		{
			_controller.DoInvokeAction("SimpleAction");
		}

		[Test]
		public void UnknownActionReturnsFalse()
		{
			Assert.IsFalse(_controller.DoInvokeAction("Unknown"));
		}

		[Test]
		public void ValidActionReturnsTrue()
		{
			Assert.IsTrue(_controller.DoInvokeAction("ComplexAction"));
		}

		[Test]
		public void ControllerDescriptorDefaultsToCached()
		{
			TestController controller = new TestController();
			Assert.IsNotNull(controller.ControllerDescriptor);
			Assert.AreEqual(typeof(CachedControllerDescriptor), controller.ControllerDescriptor.GetType());
		}

		class TestController : ConventionController
		{
			public void BasicAction(int id)
			{
			}

			public void SimpleAction(string param1)
			{
			}

			public void SimpleAction(string param1, int param2)
			{
			}

			public void ComplexAction([Deserialize("ids")] int[] ids)
			{
			}

			public void BadAction()
			{
				throw new AbandonedMutexException();
			}

			protected override bool OnError(ActionMetaData action, Exception exception)
			{
				if( action.Name.Equals("BadAction", StringComparison.OrdinalIgnoreCase) )
				{
					return false;
				}

				return true;
			}

			public bool DoInvokeAction(string action)
			{
				return InvokeAction(action);
			}

			public void DoInvokeActionMethod(ActionMetaData action)
			{
				InvokeActionMethod(action);
			}
		}
	}
}
