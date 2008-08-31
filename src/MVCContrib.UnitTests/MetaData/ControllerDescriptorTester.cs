using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.Attributes;
using MvcContrib.Filters;
using MvcContrib.MetaData;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
namespace MvcContrib.UnitTests.MetaData
{
	[TestFixture]
	public class ControllerDescriptorTester
	{
		private ControllerDescriptor _descriptor;

		[SetUp]
		public void Setup()
		{
			_descriptor = new ControllerDescriptor();
		}

		[Test]
		public void IsValidAction_should_return_true_for_valid_action()
		{
			var method = typeof(MetaDataTestController).GetMethod("BasicAction");
			Assert.IsTrue(ControllerDescriptor.IsValidAction(method));
		}

		[Test]
		public void IsValidAction_should_return_false_for_property()
		{
			var getter = typeof(MetaDataTestController).GetMethod("get_Property");
			var setter = typeof(MetaDataTestController).GetMethod("set_Property");
			Assert.IsFalse(ControllerDescriptor.IsValidAction(getter));
			Assert.IsFalse(ControllerDescriptor.IsValidAction(setter));
		}

		[Test]
		public void IsValidAction_Should_return_false_for_method_decorated_with_NonActionAttribute()
		{
			var method = typeof(MetaDataTestController).GetMethod("NonAction");
			Assert.IsFalse(ControllerDescriptor.IsValidAction(method));
		}

		[Test]
		public void IsValidAction_should_Return_false_for_public_method_on_controller()
		{
			var method = typeof(Controller).GetMethod("Dispose");
			Assert.IsFalse(ControllerDescriptor.IsValidAction(method));
		}

		[Test]
		public void IsAliasedMethod_should_return_true_if_method_has_ActionnNameAttribute()
		{
			var method = typeof(MetaDataTestController).GetMethod("NamedAction");
			Assert.IsTrue(ControllerDescriptor.IsAliasedMethod(method));
		}

		[Test]
		public void IsAliasedMethod_should_return_false_if_method_has_no_ActionNameAttribute()
		{
			var method = typeof(MetaDataTestController).GetMethod("BasicAction");
			Assert.IsFalse(ControllerDescriptor.IsAliasedMethod(method));
		}

		[Test]
		public void GetActionMethods_should_find_action_methods()
		{
			var actions = _descriptor.GetMetaData(typeof(MetaDataTestController)).Actions;
			Assert.That(actions.Length, Is.EqualTo(9));
		}

		[Test]
		public void CreateActionMetaData_should_create_action_metadata()
		{
			var metaData = _descriptor.GetMetaData(typeof(MetaDataTestController)).Actions.Single(x => x.Name == "BasicAction");
			var method = typeof(MetaDataTestController).GetMethod("BasicAction");
			Assert.That(metaData, Is.Not.Null);
			Assert.That(metaData.Filters.ExceptionFilters.Count, Is.EqualTo(1));
			Assert.That(metaData.Filters.ActionFilters.Count, Is.EqualTo(2));
			Assert.That(metaData.MethodInfo, Is.EqualTo(method));
		}

		[Test]
		public void CreateActionMetaData_should_create_aliased_metadata()
		{
			var method = typeof(MetaDataTestController).GetMethod("NamedAction");
			var metaData = _descriptor.GetMetaData(typeof(MetaDataTestController)).Actions.Single(x => x.Name == "NamedAction") as AliasedActionMetaData;
			Assert.That(metaData, Is.Not.Null);
			Assert.That(metaData.MethodInfo, Is.EqualTo(method));
			Assert.That(metaData.Aliases[0].Name, Is.EqualTo("Foo"));
		}

		[Test]
		public void Should_Create_metadata()
		{
			var meta = _descriptor.GetMetaData(new MetaDataTestController());
			Assert.That(meta, Is.Not.Null);
			Assert.That(meta.ControllerType, Is.EqualTo(typeof(MetaDataTestController)));
			Assert.That(meta.Actions.Where(x => x.Name.Equals("simpleaction", StringComparison.OrdinalIgnoreCase)).Count(), Is.EqualTo(2));
		}

		[Test]
		public void Should_create_metadata_by_type()
		{
			var meta = _descriptor.GetMetaData(typeof(MetaDataTestController));
			Assert.That(meta, Is.Not.Null);
			Assert.That(meta.ControllerType, Is.EqualTo(typeof(MetaDataTestController)));
			Assert.That(meta.Actions.Where(x => x.Name.Equals("simpleaction", StringComparison.OrdinalIgnoreCase)).Count(), Is.EqualTo(2));
		}

		[Test]
		public void NonExistentActionShouldReturnNull()
		{
			var controller = new MetaDataTestController();
			ControllerMetaData metaData = _descriptor.GetMetaData(controller);

			Assert.IsNull(metaData.GetAction("Doesn't Exist", null));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullControllerThrows()
		{
			ControllerMetaData metaData = _descriptor.GetMetaData((IController)null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullTypeThrows()
		{
			_descriptor.GetMetaData((Type)null);
		}

		[Test]
		public void ShouldFindDefaultAction()
		{
			ControllerMetaData metaData = _descriptor.GetMetaData(typeof(MetaDataTestController));
			Assert.AreEqual("CatchAllAction", metaData.DefaultAction.Name);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void MultipleDefaultActionsThrow()
		{
			_descriptor.GetMetaData(typeof(TestControllerWithMultipleDefaultActions));
		}

		[Test]
		public void CachedDescriptorReturnsCachedCopy()
		{
			var descriptor = new CachedControllerDescriptor();
			var meta = descriptor.GetMetaData(typeof(MetaDataTestController));
			var second = descriptor.GetMetaData(typeof(MetaDataTestController));
			Assert.That(meta, Is.SameAs(second));
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
			ControllerMetaData metaData = _descriptor.GetMetaData(typeof(MetaDataTestController));
			Assert.That(metaData.GetAction("VoidAction", null), Is.Not.Null);
		}

		[Test]
		public void Methods_that_return_objects_should_be_recognised_as_actions()
		{
			var action = _descriptor.GetMetaData(typeof(MetaDataTestController)).GetAction("ContentAction", null);

			Assert.That(action, Is.Not.Null);
		}

		[Test]
		public void Methods_on_controller_should_not_be_recognised_as_actions()
		{
			var action = _descriptor.GetMetaData(typeof(MetaDataTestController)).GetAction("Dispose", null);
			Assert.That(action, Is.Null);
		}

		[Test]
		public void Filters_should_return_filters_for_controller_and_action()
		{
			var filters = _descriptor.GetMetaData(typeof(MetaDataTestController)).GetAction("BasicAction", null).Filters;
			Assert.That(filters.ExceptionFilters.Count, Is.EqualTo(1));
			Assert.That(filters.ActionFilters.Count, Is.EqualTo(2));
		}

		[Test]
		public void Filters_should_be_ordered_correctly()
		{
			var filters = _descriptor.GetMetaData(typeof(MetaDataTestController)).GetAction("BasicAction", null).Filters;
			Assert.That(filters.ActionFilters[0], Is.InstanceOfType(typeof(PostOnlyAttribute)));
			Assert.That(filters.ActionFilters[1], Is.InstanceOfType(typeof(OutputCacheAttribute)));
		}

		[Test]
		public void SelectionAttributes_should_return_action_selection_attributes()
		{
			Assert.Fail("Implement me");			
		}

		[Test]
		public void Action_should_not_be_valid_if_name_does_not_equal_action()
		{
			Assert.Fail("Implement me");			
		}

		[Test]
		public void Action_should_not_be_valid_if_selector_is_not_valid()
		{
			Assert.Fail("Implement me");			
		}

		[Test]
		public void Action_should_be_valid_if_name_is_valid_and_all_selectors_are_valid()
		{
			Assert.Fail("Implement me");			
		}

		[Test]
		public void Action_should_be_valid_if_name_is_valid_and_there_are_no_selectors()
		{
			Assert.Fail("Implement me");			
		}

		[Test]
		public void AliasedAction_should_not_be_valid_if_no_aliases_are_valid()
		{
			Assert.Fail("Implement me");			
		}

		[Test]
		public void AliasedAction_Should_not_be_valid_if_selector_is_not_valid()
		{
			Assert.Fail("Implement me");			
		}

		[Test]
		public void AliasedAction_should_be_valid_if_aliases_are_all_valid_and_selectors_are_valid()
		{
			Assert.Fail("Implement me");			
		}

		[Test]
		public void Aliased_action_should_be_valid_if_aliases_are_all_valid_and_there_are_no_selectors()
		{
			Assert.Fail("Implement me");
		}

		[Test]
		public void GetAction_Should_return_null_when_there_are_no_matches()
		{
			Assert.Fail("Implement me");			
		}

		[Test]
		public void GetAction_Should_return_valid_action()
		{
			Assert.Fail("Implement me");			
		}

		[Test]
		public void GetAction_should_throw_if_there_are_multiple_valid_actions()
		{
			Assert.Fail("Implement me");
		}

	}

	internal class InvalidTestSelectionAttribute : ActionSelectionAttribute
	{
		public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
		{
			return false;
		}
	}

	internal class ValidTestSelectionAttribute : ActionSelectionAttribute
	{
		public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
		{
			return true;
		}
	}

	[Rescue("Test"), PostOnly]
	internal class MetaDataTestController : Controller
	{
		public void VoidAction()
		{
			
		}

		[ActionName("Foo")]
		public ActionResult NamedAction()
		{
			return new EmptyResult();
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
