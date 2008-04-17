using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using MvcContrib.UnitTests.ComponentControllerFactories;
using NUnit.Framework;
using Rhino.Mocks;
using MvcContrib.UI.Html;

namespace MvcContrib.UnitTests.UI.Html
{
	[TestFixture]
	public class ComponentControllerExtensionsTester
	{
		[SetUp]
		public void SetUp()
		{
			builder = new TestControllerBuilder();
			controller = builder.CreateController<TestHelperController>();
			htmlHelper = new HtmlHelper(new ViewContext(controller.ControllerContext, "someview", "", null, null));
		}

		private TestControllerBuilder builder;
		private TestHelperController controller;
		private HtmlHelper htmlHelper;

		[Test]
		public void RenderedHtmlReturnsCorrectValue()
		{
			var returnValue = htmlHelper.RenderComponent2<SampleComponentController>(x => x.ShowMeTheNumbers(2, 5));
			Assert.AreEqual("sample", returnValue);
		}
		[Test]
		public void SampleComponentCallsCorrectFunction()
		{
			int a = 0;
			Action<SampleComponentController> action = (z) => a = 8;
			htmlHelper.RenderComponent2<SampleComponentController>(action);
			Assert.AreEqual(8,a);

		}

	}
}
