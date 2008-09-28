using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.UI.Ajax;
using MvcContrib.UI.Ajax.Internal;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI.Ajax
{
	[TestFixture]
	public class PrototypeAjaxGeneratorTester
	{
		private AjaxGenerator generator;
		private MockRepository mocks;
		private ViewContext context;

		[SetUp]
		public void Setup()
		{
			mocks = new MockRepository();
			context = mocks.DynamicViewContext("Index");
			generator = new PrototypeAjaxGenerator(new AjaxHelper(context));
			mocks.ReplayAll();
		}

		[Test]
		public void IsMvcAjaxRequest_should_return_true_for_an_ajax_request()
		{
			context.HttpContext.Request.Headers["X-Requested-With"] = "XMLHttpRequest";
			Assert.That(generator.IsMvcAjaxRequest());
		}

		[Test]
		public void When_CreateLink_is_invoked_then_the_correct_html_should_be_generated_with_options_and_attributes()
		{
//			string expected = "<a class=\"foo\" href=\"www.mvccontrib.org\">MvcContrib</a>";
//			string actual = generator.CreateLink("MvcContrib", "www.mvccontrib.org", new AjaxOptions(), new Hash(@class => "foo"));
		
//			Assert.That(actual, Is.EqualTo(expected));
			Assert.Fail("Implement me");
		}


	}
}