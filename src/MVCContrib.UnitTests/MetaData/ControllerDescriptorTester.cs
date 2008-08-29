using System;
using System.Web.Mvc;
using MvcContrib.Attributes;
using MvcContrib.Castle;
using MvcContrib.Filters;
using MvcContrib.MetaData;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
namespace MvcContrib.UnitTests.MetaData
{
	[TestFixture]
	public class ControllerDescriptorTester
	{
		[Test]
		public void CanCreateMetaData()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			var controller = new MetaDataTestController();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(controller);

			Assert.IsNotNull(metaData);
			Assert.AreEqual(typeof(MetaDataTestController), metaData.ControllerType);
			Assert.AreEqual(2, metaData.GetActions("simpleaction").Count);
		}

		[Test]
		public void CanCreateMetaDataByType()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));

			Assert.IsNotNull(metaData);
			Assert.AreEqual(typeof(MetaDataTestController), metaData.ControllerType);
			Assert.AreEqual(2, metaData.GetActions("simpleaction").Count);
		}

		[Test]
		public void MethodWithCustomReturnTypeAndNoReturnBinderIsNotAnAction()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();

			ControllerMetaData metaData = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));

			Assert.IsNull(metaData.GetAction("ActionReturningValueWithOutBinder"));
		}

		[Test]
		public void NonExistentActionShouldReturnNull()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();
			var controller = new MetaDataTestController();
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
			var inner = new CountControllerDescriptor();
			var descriptor = new CachedControllerDescriptor(inner);
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
			var descriptor = new CachedControllerDescriptor(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CachedDescriptorThrowsOnNullController()
		{
			var descriptor = new CachedControllerDescriptor();
			descriptor.GetMetaData((IController)null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CachedDescriptorThrowsOnNullType()
		{
			var descriptor = new CachedControllerDescriptor();
			descriptor.GetMetaData((Type)null);
		}

		[Test]
		public void Methods_that_return_void_should_be_recognised_as_actions()
		{
			IControllerDescriptor controllerDescriptor = new ControllerDescriptor();
			ControllerMetaData metaData = controllerDescriptor.GetMetaData(typeof(MetaDataTestController));
			Assert.That(metaData.GetAction("VoidAction"), Is.Not.Null);
		}

		[Test]
		public void Methods_that_return_objects_should_be_recognised_as_actions()
		{
			var descriptor = new ControllerDescriptor();
			var action = descriptor.GetMetaData(typeof(MetaDataTestController)).GetAction("ContentAction");

			Assert.That(action, Is.Not.Null);
		}

		[Test]
		public void Methods_on_controller_should_not_be_recognised_as_actions()
		{
			var descriptor = new ControllerDescriptor();
			var action = descriptor.GetMetaData(typeof(MetaDataTestController)).GetAction("Dispose");
			Assert.That(action, Is.Null);
		}

		[Test]
		public void Filters_should_return_filters_for_controller_and_action()
		{
			var filters = new ControllerDescriptor().GetMetaData(typeof(MetaDataTestController)).GetAction("BasicAction").Filters;
			Assert.That(filters.ExceptionFilters.Count, Is.EqualTo(1));
			Assert.That(filters.ActionFilters.Count, Is.EqualTo(2));
		}

		[Test]
		public void Filters_should_be_ordered_correctly()
		{
			var filters = new ControllerDescriptor().GetMetaData(typeof(MetaDataTestController)).GetAction("BasicAction").Filters;
			Assert.That(filters.ActionFilters[0], Is.InstanceOfType(typeof(PostOnlyAttribute)));
			Assert.That(filters.ActionFilters[1], Is.InstanceOfType(typeof(OutputCacheAttribute)));
		}
	}

	internal class CountControllerDescriptor : IControllerDescriptor
	{
		public int Calls;

		public CountControllerDescriptor()
		{
			Calls = 0;
		}

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

	[/*Rescue("Test"),*/ PostOnly]
	internal class MetaDataTestController : Controller
	{
		public void VoidAction()
		{
			
		}

		[OutputCache]
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

		public string ContentAction()
		{
			return string.Empty;
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

//		[Rescue("Test")]
		public ActionResult ComplexAction([Deserialize("complex")] object complex)
		{
			return new EmptyResult();
		}

		public ActionResult InvalidAction(out string test, ref string test2)
		{
			test = "test";
			return new EmptyResult();
		}
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
