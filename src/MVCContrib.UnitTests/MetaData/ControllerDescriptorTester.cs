using System;
using System.Web.Mvc;
using MvcContrib;
using MvcContrib.Attributes;
using MvcContrib.Castle;
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
		public void MethodWithCustomReturnTypeAndNoReturnBinderIsNotAnAction()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));

			Assert.IsNull(metaData.GetAction("ActionReturningValueWithOutBinder"));
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
		public void Properties_should_not_be_recognised_as_actions()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();
			var meta = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));
			Assert.IsNull(meta.GetAction("get_Property"));
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

		[Test]
		public void ShouldThrowIfReturnTypeIsNull()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();
			ControllerMetaData metaData = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));
			Assert.IsNull(metaData.GetAction("VoidAction"));
		}

		[Test]
		public void CastleControllerDescriptor_should_use_CastleSimpleBinder_instead_of_SimpleBinder()
		{
			var controllerDescriptor = new CastleControllerDescriptor();
			ControllerMetaData metaData = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));
			ActionParameterMetaData parameter = metaData.GetActions("SimpleAction")[0].Parameters[0];
			Assert.IsInstanceOfType(typeof(CastleSimpleBinder), parameter.ParameterBinder);
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
	internal class MetaDataTestController : Controller
	{
		public void VoidAction()
		{
			
		}

		public ActionResult BasicAction()
		{
			return new EmptyResult();
		}

		public ActionResult SimpleAction(string param1)
		{
			return new EmptyResult();
		}

		public ActionResult SimpleAction(string param1, int param2)
		{
			return new EmptyResult();
		}

		public string Property { get; set; }

		[NonAction]
		public ActionResult NonAction()
		{
			return new EmptyResult();
		}

		[DefaultAction]
		public ActionResult CatchAllAction()
		{
			return new EmptyResult();
		}

		[Rescue("Test")]
		public ActionResult ComplexAction([Deserialize("complex")] object complex)
		{
			return new EmptyResult();
		}

		public ActionResult InvalidAction(out string test, ref string test2)
		{
			test = "test";
			return new EmptyResult();
		}

//		public bool DoInvokeAction(string action)
//		{
//			return InvokeAction(action);
//			throw new NotImplementedException();
//		}

//		public void DoInvokeActionMethod(ActionMetaData action)
//		{
//			InvokeActionMethod(action);
//		}
	}

	internal class TestControllerWithMultipleDefaultActions : Controller
	{
		[DefaultAction]
		public ActionResult Action1()
		{
			return new EmptyResult();
		}

		[DefaultAction]
		public ActionResult Action2()
		{
			return new EmptyResult();
		}
	}
}
