using System;
using System.Web.Mvc;
using MvcContrib;
using MvcContrib.Attributes;
using MvcContrib.MetaData;
using NUnit.Framework;

namespace MVCContrib.UnitTests.MetaData
{
	[TestFixture]
	public class ControllerDescriptorTester
	{
		[Test]
		public void CanCreateMetaData()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			TestController controller = new TestController();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(controller);

			Assert.IsNotNull(metaData);
			Assert.AreEqual(typeof(TestController), metaData.ControllerType);
			Assert.AreEqual(2, metaData.GetActions("simpleaction").Count);
			Assert.IsFalse(metaData.GetActions("InvalidAction")[0].Parameters[0].IsValid);
		}

		[Test]
		public void CanCreateMetaDataByType()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(typeof(TestController));

			Assert.IsNotNull(metaData);
			Assert.AreEqual(typeof(TestController), metaData.ControllerType);
			Assert.AreEqual(2, metaData.GetActions("simpleaction").Count);
			Assert.IsFalse(metaData.GetActions("InvalidAction")[0].Parameters[0].IsValid);
		}

		[Test]
		public void DoesParseControllerAndActionRescues()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			TestController controller = new TestController();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(controller);

			Assert.AreEqual(1, metaData.GetAction("BasicAction").Rescues.Count);
			Assert.AreEqual(2, metaData.GetAction("ComplexAction").Rescues.Count);
		}

		[Test]
		public void OutAndRefParametersAreInvalid()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			TestController controller = new TestController();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(controller);

			Assert.IsFalse(metaData.GetActions("InvalidAction")[0].Parameters[0].IsValid);
			Assert.IsFalse(metaData.GetActions("InvalidAction")[0].Parameters[1].IsValid);
		}


		[Test]
		public void ForCoverage()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			TestController controller = new TestController();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(controller);

			ActionParameterMetaData parameter = metaData.Actions[1].Parameters[0];

			ActionMetaData unknownAction = metaData.GetAction("Doesn't Exist");

			Assert.IsNull(unknownAction);
			Assert.IsNotNull(parameter.ParameterInfo);
			Assert.IsNull(parameter.ParameterBinder);
			Assert.IsNull(parameter.Bind(null));
			Assert.IsTrue(parameter.IsValid);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullControllerThrows()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();
			ControllerMetaData metaData = controllerDescriptor.GetMetaData((IController)null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullTypeThrows()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();
			controllerDescriptor.GetMetaData((Type)null);
		}

		[Test]
		public void CachedDescriptorReturnsCachedCopy()
		{
			CountControllerDescriptor inner = new CountControllerDescriptor();
			CachedControllerDescriptor descriptor = new CachedControllerDescriptor(inner);
			ControllerMetaData metaData = descriptor.GetMetaData(new TestController());
			ControllerMetaData metaDataAgain = descriptor.GetMetaData(new TestController());

			Assert.AreEqual(1, inner.Calls);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CachedDescriptorRequiresInnerDescriptor()
		{
			CachedControllerDescriptor descriptor = new CachedControllerDescriptor(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CachedDescriptorThrowsOnNullController()
		{
			CachedControllerDescriptor descriptor = new CachedControllerDescriptor();
			descriptor.GetMetaData((IController)null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CachedDescriptorThrowsOnNullType()
		{
			CachedControllerDescriptor descriptor = new CachedControllerDescriptor();
			descriptor.GetMetaData((Type)null);
		}
	}

	internal class CountControllerDescriptor : IControllerDescriptor
	{
		public int Calls = 0;

		public ControllerMetaData GetMetaData(IController controller)
		{
			return GetMetaData(typeof(TestController));
		}

		public ControllerMetaData GetMetaData(Type controllerType)
		{
			Calls++;
			return new ControllerMetaData(controllerType);
		}
	}

	[Rescue("Test")]
	internal class TestController : ConventionController
	{
		public void BasicAction()
		{
		}

		public void SimpleAction(string param1)
		{
		}

		public void SimpleAction(string param1, int param2)
		{
		}

		[Rescue("Test")]
		public void ComplexAction([Deserialize("complex")] object complex)
		{
		}

		public void InvalidAction(out string test, ref string test2)
		{
			test = "test";
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
