using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.MetaData;
using MvcContrib.UnitTests.ConventionController;
using NUnit.Framework;
using Rhino.Mocks;
using System.Web;

namespace MvcContrib.UnitTests.ConventionController
{
	[TestFixture]
	public class ConventionControllerTester
	{
		private TestController _controller;
		private MockRepository _mocks;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			_controller = new TestController();
			_controller.ControllerDescriptor = new ControllerDescriptor();
		}

		[Test]
		public void ControllerDescriptorDefaultsToCached()
		{
			TestController controller = new TestController();
			Assert.IsNotNull(controller.ControllerDescriptor);
			Assert.AreEqual(typeof(CachedControllerDescriptor), controller.ControllerDescriptor.GetType());
		}

/*		[Test]
		public void OnErrorShouldNotThrowIfInnerExceptionIsNull()
		{
			TestController controller = new TestController();
			controller.InvokeOnErrorWithoutInnerException();
			Assert.IsTrue(controller.OnErrorWasCalled);
		}

		[Test]
		public void ShouldNotBeRescuedWhenThreadAbortExceptionThrownDueToRedirect()
		{
			Expect.Call(delegate { _controller.Response.Redirect(null); });
			_mocks.Replay(_controller.Response);
			_controller.DoInvokeAction("WithRedirect");
			_mocks.Verify(_controller.Response);

			Assert.IsTrue(_controller.OnErrorWasCalled);
			Assert.IsTrue(_controller.OnErrorResult.Value);
		}*/
	}
}