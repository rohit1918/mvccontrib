using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Filters;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.Filters
{
	[TestFixture]
	public class ModelStateToTempDataAttributeTests
	{
		private TestingController controller;
		private ModelStateToTempDataAttribute attr;
		private ActionExecutedContext context;

		[SetUp]
		public void Setup()
		{
			controller = new TestingController().SetupControllerContext();
			attr = new ModelStateToTempDataAttribute();
			context = new ActionExecutedContext(controller.ControllerContext, MockRepository.GenerateStub<ActionDescriptor>(),false, null);
		}

		private void SetupModelState(ModelStateDictionary modelData)
		{
			modelData.AddModelError("foo", "bar");
			modelData.AddModelError("foo", "baz");
			modelData.AddModelError("bar", new Exception("blah"));
		}

		private TempDataDictionary TempData
		{
			get { return controller.TempData; }
		}

		private ModelStateDictionary ModelState
		{
			get { return controller.ViewData.ModelState; }
		}

		[Test]
		public void When_a_redirect_occurs_modelstate_should_be_copied_to_tempdata()
		{
			SetupModelState(ModelState);
			context.Result = new RedirectToRouteResult(new RouteValueDictionary());

			attr.OnActionExecuted(context);

			var fromTempData =(ModelStateDictionary)TempData[ModelStateToTempDataAttribute.TempDataKey];

			Assert.That(fromTempData.Count, Is.EqualTo(2));
			Assert.That(fromTempData["foo"].Errors.Count(), Is.EqualTo(2));
			Assert.That(fromTempData["foo"].Errors.First().ErrorMessage, Is.EqualTo("bar"));
			Assert.That(fromTempData["bar"].Errors.First().Exception.Message, Is.EqualTo("blah"));
		}

		[Test]
		public void When_a_viewresult_is_returned_and_nothing_is_in_tempdata_then_nothing_should_be_copied_to_modelstate()
		{
			context.Result = new ViewResult();
			attr.OnActionExecuted(context);
			Assert.IsFalse(TempData.ContainsKey(ModelStateToTempDataAttribute.TempDataKey));
			Assert.That(ModelState.IsValid);
		}

		[Test]
		public void When_a_viewresult_is_returned_the_modelstate_should_be_copied_from_tempdata_to_viewdata()
		{
			var tempDataModelState = new ModelStateDictionary();
			SetupModelState(tempDataModelState);
			TempData[ModelStateToTempDataAttribute.TempDataKey] = tempDataModelState;

			context.Result = new ViewResult();

			attr.OnActionExecuted(context);

			Assert.That(ModelState.Count, Is.EqualTo(2));
			Assert.That(ModelState["foo"].Errors.Count, Is.EqualTo(2));
			Assert.That(ModelState["foo"].Errors[0].ErrorMessage, Is.EqualTo("bar"));
			Assert.That(ModelState["bar"].Errors[0].Exception.Message, Is.EqualTo("blah"));
		}

		[Test]
		public void Should_be_serializable()
		{
			SetupModelState(ModelState);
			context.Result = new RedirectToRouteResult(new RouteValueDictionary());
			attr.OnActionExecuted(context);
			var fromTempData =(ModelStateDictionary)TempData[ModelStateToTempDataAttribute.TempDataKey];

			using(var stream = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(stream, fromTempData);
			}
		}
	}
}