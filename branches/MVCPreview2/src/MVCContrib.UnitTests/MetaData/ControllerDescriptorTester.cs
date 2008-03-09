using System;
using System.Web.Mvc;
using MvcContrib;
using MvcContrib.Attributes;
using MvcContrib.MetaData;
using NUnit.Framework;

namespace MvcContrib.UnitTests.MetaData
{
	[TestFixture]
	public class ControllerDescriptorTester
	{
		[Test]
		public void CanCreateMetaData()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			MetaDataTestController controller = new MetaDataTestController();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(controller);

			Assert.IsNotNull(metaData);
			Assert.AreEqual(typeof(MetaDataTestController), metaData.ControllerType);
			Assert.AreEqual(2, metaData.GetActions("simpleaction").Count);
			Assert.IsFalse(metaData.GetActions("InvalidAction")[0].Parameters[0].IsValid);
		}

		[Test]
		public void CanCreateMetaDataByType()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));

			Assert.IsNotNull(metaData);
			Assert.AreEqual(typeof(MetaDataTestController), metaData.ControllerType);
			Assert.AreEqual(2, metaData.GetActions("simpleaction").Count);
			Assert.IsFalse(metaData.GetActions("InvalidAction")[0].Parameters[0].IsValid);
		}

		[Test]
		public void DoesParseControllerAndActionRescues()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			MetaDataTestController controller = new MetaDataTestController();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(controller);

			Assert.AreEqual(1, metaData.GetAction("BasicAction").Rescues.Count);
			Assert.AreEqual(2, metaData.GetAction("ComplexAction").Rescues.Count);
		}

		[Test]
		public void OutAndRefParametersAreInvalid()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			MetaDataTestController controller = new MetaDataTestController();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(controller);

			Assert.IsFalse(metaData.GetActions("InvalidAction")[0].Parameters[0].IsValid);
			Assert.IsFalse(metaData.GetActions("InvalidAction")[0].Parameters[1].IsValid);
			Assert.IsNull(metaData.GetActions("InvalidAction")[0].Parameters[0].ParameterBinder);
			Assert.IsNull(metaData.GetActions("InvalidAction")[0].Parameters[1].ParameterBinder);
		}

		[Test]
		public void BinderShouldDefaultToSimpleParameterBinder()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));

			ActionParameterMetaData parameter = metaData.GetActions("SimpleAction")[0].Parameters[0];

			Assert.IsInstanceOfType(typeof(SimpleParameterBinder), parameter.ParameterBinder);
		}

		[Test]
		public void ReturnBinderReturnNullIfNoAttributeIsUsed()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));

			ActionMetaData action = metaData.GetActions("ActionReturningValueWithOutBinder")[0];

			Assert.AreEqual(action.ReturnBinderDescriptor,null);
		}

		[Test]
		public void ReturnBinderReturnCorrectType()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));

			ActionMetaData action = metaData.GetActions("ActionWithReturnBinder")[0];

			Assert.IsInstanceOfType(typeof(XMLReturnBinder),action.ReturnBinderDescriptor.ReturnTypeBinder);
			Assert.That(action.ReturnBinderDescriptor.ReturnType == typeof(int[]));
		}

		[Test]
		public void BindShouldReturnNullIfBinderIsNull()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();
			ControllerMetaData metaData = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));

			Assert.IsNull(metaData.GetActions("InvalidAction")[0].Parameters[0].Bind(null));
		}

		[Test]
		public void NonExistentActionShouldReturnNull()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();
			MetaDataTestController controller = new MetaDataTestController();
			ControllerMetaData metaData = controllerDescriptor.GetMetaData(controller);

			Assert.IsNull(metaData.GetAction("Doesn't Exist"));
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
		public void HiddenActionShouldReturnNull()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();
			ControllerMetaData metaData = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));

			Assert.IsNull(metaData.GetAction("HiddenAction"));
		}

		[Test]
		public void ShouldFindDefaultAction()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();
			ControllerMetaData metaData = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));

			Assert.AreEqual("CatchAllAction", metaData.DefaultAction.Name);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void MultipleDefaultActionsThrow()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();
			controllerDescriptor.GetMetaData(typeof(TestControllerWithMultipleDefaultActions));
		}

		[Test]
		public void CachedDescriptorReturnsCachedCopy()
		{
			CountControllerDescriptor inner = new CountControllerDescriptor();
			CachedControllerDescriptor descriptor = new CachedControllerDescriptor(inner);
			ControllerMetaData metaData = descriptor.GetMetaData(new MetaDataTestController());
			ControllerMetaData metaDataAgain = descriptor.GetMetaData(new MetaDataTestController());

			Assert.AreEqual(1, inner.Calls);
		}

		[Test]
		public void Action_with_NonActionAttribute_is_not_valid()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();
			var meta = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));
			Assert.IsNull(meta.GetAction("NonAction"));
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
			return GetMetaData(typeof(MetaDataTestController));
		}

		public ControllerMetaData GetMetaData(Type controllerType)
		{
			Calls++;
			return new ControllerMetaData(controllerType);
		}
	}

	[Rescue("Test")]
	internal class MetaDataTestController : ConventionController
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

		[NonAction]
		public void NonAction()
		{
			
		}

		[DefaultAction]
		public void CatchAllAction()
		{
		}

		[return: XMLReturnBinder]
		public int[] ActionWithReturnBinder()
		{
			return new int[] {2,1,4,5};
		}

		public int[] ActionReturningValueWithOutBinder()
		{
			return new int[]{2,3,1,5};
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

	internal class TestControllerWithMultipleDefaultActions : ConventionController
	{
		[DefaultAction]
		public void Action1()
		{
		}

		[DefaultAction]
		public void Action2()
		{
		}
	}
}
