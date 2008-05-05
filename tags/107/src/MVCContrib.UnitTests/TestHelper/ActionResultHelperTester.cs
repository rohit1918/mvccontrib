using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using MvcContrib.TestHelper;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.TestHelper
{
	[TestFixture]
	public class ActionResultHelperTester
	{
		[Test]
		public void Should_convert()
		{
			ActionResult result = new EmptyResult();
			EmptyResult converted = result.AssertResultIs<EmptyResult>();
			Assert.IsNotNull(converted);
		}

		[Test, ExpectedException(typeof(ActionResultAssertionException), ExpectedMessage = "Expected result to be of type EmptyResult. It is actually of type HttpRedirectResult.")]
		public void Should_throw_when_conversiontype_is_incorrect()
		{
			ActionResult result = new HttpRedirectResult("http://mvccontrib.org");
			EmptyResult converted = result.AssertResultIs<EmptyResult>();
		}

		[Test]
		public void Should_convert_to_RenderViewResult()
		{
			ActionResult result = new RenderViewResult();
			RenderViewResult converted = result.AssertViewRendered();
			Assert.IsNotNull(converted);
        }

		[Test]
		public void Should_convert_to_ActionRedirectResult()
		{
			ActionResult result = new ActionRedirectResult(new RouteValueDictionary());
			ActionRedirectResult converted = result.AssertActionRedirect();
			Assert.IsNotNull(converted);
		}

		[Test]
		public void Should_convert_to_HttpRedirectResult()
		{
			ActionResult result = new HttpRedirectResult("http://mvccontrib.org");
			HttpRedirectResult converted = result.AssertHttpRedirect();
			Assert.IsNotNull(converted);
		}

		[Test]
		public void WithParameter_should_return_source_result()
		{
			var result = new ActionRedirectResult(new RouteValueDictionary(new { foo = "bar" }));
			var final = result.WithParameter("foo", "bar");
			Assert.That(final, Is.EqualTo(result));
		}

		[Test, ExpectedException(typeof(ActionResultAssertionException), ExpectedMessage = "Could not find a parameter named 'foo' in the result's Values collection.")]
		public void WithParameter_should_throw_if_key_not_in_dictionary()
		{
			var result = new ActionRedirectResult(new RouteValueDictionary());
			result.WithParameter("foo", "bar");
		}

		[Test, ExpectedException(typeof(ActionResultAssertionException), ExpectedMessage = "When looking for a parameter named 'foo', expected 'bar' but was 'baz'.")]
		public void WithParameter_should_throw_if_values_are_different()
		{
			var result = new ActionRedirectResult(new RouteValueDictionary(new { foo = "baz" }));
			result.WithParameter("foo", "bar");
		}

		[Test]
		public void ForView_should_return_source_result()
		{
			var result = new RenderViewResult { ViewName = "Index" };
			var final = result.ForView("Index");
			Assert.That(final, Is.EqualTo(result));
		}

		[Test, ExpectedException(typeof(ActionResultAssertionException), ExpectedMessage = "Expected view name 'Index', actual was 'About'")]
		public void ForView_should_throw_if_view_names_do_not_match()
		{
			var result = new RenderViewResult {ViewName = "About"};
			result.ForView("Index");
		}

		[Test]
		public void ForUrl_should_return_source_Result()
		{
			var result = new HttpRedirectResult("http://mvccontrib.org");
			var final = result.ToUrl("http://mvccontrib.org");
			Assert.That(final, Is.EqualTo(result));
		}

		[Test, ExpectedException(typeof(ActionResultAssertionException), ExpectedMessage = "Expected redirect to 'http://mvccontrib.org', actual was 'http://www.asp.net'")]
		public void ForUrl_should_throw_if_urls_do_not_match()
		{
			var result = new HttpRedirectResult("http://www.asp.net");
			result.ToUrl("http://mvccontrib.org");
		}

		[Test]
		public void Should_chain()
		{
			ActionResult result = new ActionRedirectResult(new RouteValueDictionary(new {controller = "Home", action = "Index", id = 1}));
			var final = result.AssertActionRedirect().ToController("Home").ToAction("Index").WithParameter("id", 1);
			Assert.That(final, Is.EqualTo(result));
		}

		[Test]
		public void ToAction_should_support_strongly_typed_controller_and_action()
		{
				ActionResult result = new ActionRedirectResult(new RouteValueDictionary(new { controller = "PageHandler", action = "About" }));
				var final = result.AssertActionRedirect().ToAction<PageHandler>(c => c.About());
				Assert.That(final, Is.EqualTo(result));
		}

		[Test]
		public void ToAction_with_strongly_typed_controller_can_ignore_the_controller_suffix()
		{
			ActionResult result = new ActionRedirectResult(new RouteValueDictionary(new { controller = "Fake", action = "About" }));
			var final = result.AssertActionRedirect().ToAction<FakeController>(c => c.About());
			Assert.That(final, Is.EqualTo(result));
		}

		class PageHandler : Controller
		{
			public ActionResult About()
			{
				return null;
			}
		}

		class FakeController : Controller
		{
			public ActionResult About()
			{
				return null;
			}
		}

	}
}
