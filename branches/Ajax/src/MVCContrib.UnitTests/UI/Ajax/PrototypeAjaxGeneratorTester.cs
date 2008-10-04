using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MvcContrib.UI.Ajax.Internal;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using AjaxOptions=MvcContrib.UI.Ajax.AjaxOptions;

namespace MvcContrib.UnitTests.UI.Ajax
{
	[TestFixture]
	public class PrototypeAjaxGeneratorTester
	{
		private PrototypeAjaxGenerator _generator;
		private MockRepository _mocks;
		private ViewContext _context;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_context = _mocks.DynamicViewContext("Index");
			_generator = new PrototypeAjaxGenerator(new AjaxHelper(_context));
			_mocks.ReplayAll();
		}

		[Test]
		public void IsMvcAjaxRequest_should_return_true_for_an_ajax_request()
		{
			_context.HttpContext.Request.Headers["X-Requested-With"] = "XMLHttpRequest";
			Assert.That(_generator.IsMvcAjaxRequest());
		}

		[Test]
		public void CreateLink_should_create_link()
		{
			string expected =
				"<a class=\"foo\" href=\"www.mvccontrib.org\" onclick=\"new Ajax.Request('www.mvccontrib.org', {});\">MvcContrib</a>";
			string actual = _generator.CreateLink("MvcContrib", "www.mvccontrib.org",
			                                      new AjaxOptions {Url = "www.mvccontrib.org"}, new Hash(@class => "foo"));

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Should_convert_ajax_options_to_string_with_confirm_message()
		{
			var options = new AjaxOptions {Confirm = "Are you sure?", Url = "http://mvccontrib.org"};
			string expected = "if(confirm('Are you sure?')) { new Ajax.Request('http://mvccontrib.org', {}); }";
			string actual = _generator.CreateAjaxScriptForLink(null, options);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Should_convert_ajax_options_to_string_with_http_method()
		{
			var options = new AjaxOptions {Url = "http://mvccontrib.org", HttpMethod = "GET"};
			string expected = "new Ajax.Request('http://mvccontrib.org', {method:'GET'});";
			string actual = _generator.CreateAjaxScriptForLink(null, options);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Should_convert_ajax_options_to_string_with_element_to_update()
		{
			var options = new AjaxOptions {Url = "http://mvccontrib.org", UpdateTargetId = "foo"};
			string expected = "new Ajax.Updater('foo', 'http://mvccontrib.org', {});";
			string actual = _generator.CreateAjaxScriptForLink(null, options);

			Assert.That(actual, Is.EqualTo(expected));
		}


		[Test]
		public void Should_convert_ajax_options_to_string_with_element_to_update_and_InsertionMode()
		{
			var options = new AjaxOptions
			              	{InsertionMode = InsertionMode.InsertAfter, UpdateTargetId = "foo", Url = "http://mvccontrib.org"};
			string expected = "new Ajax.Updater('foo', 'http://mvccontrib.org', {insertion:Insertion.After});";
			string actual = _generator.CreateAjaxScriptForLink(null, options);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Should_convert_ajax_options_to_string_with_loading_element_id()
		{
			var options = new AjaxOptions {Url = "http://mvccontrib.org", LoadingElementId = "foo"};
			string expected =
				"new Ajax.Request('http://mvccontrib.org', {onCreate:function() { $('foo').show(); }, onComplete:function() { $('foo').hide(); }});";
			string actual = _generator.CreateAjaxScriptForLink(null, options);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Should_convert_ajax_option_to_string_with_callbacks()
		{
			var options = new AjaxOptions
			              	{
			              		Url = "http://mvccontrib.org",
			              		OnBegin = "begin",
			              		OnComplete = "complete",
			              		OnFailure = "failure",
			              		OnSuccess = "success"
			              	};

			string expected =
				"new Ajax.Request('http://mvccontrib.org', {onCreate:begin, onComplete:complete, onSuccess:success, onFailure:failure});";
			string actual = _generator.CreateAjaxScriptForLink(null, options);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void When_an_oncomplete_is_specified_then_loadingelementid_is_ignored()
		{
			var options = new AjaxOptions {Url = "http://mvccontrib.org", LoadingElementId = "foo", OnComplete = "complete"};
			string expected = "new Ajax.Request('http://mvccontrib.org', {onComplete:complete});";
			string actual = _generator.CreateAjaxScriptForLink(null, options);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void When_an_onbegin_is_specified_then_loadingelementid_is_ignored()
		{
			var options = new AjaxOptions {Url = "http://mvccontrib.org", LoadingElementId = "foo", OnBegin = "begin"};
			string expected = "new Ajax.Request('http://mvccontrib.org', {onCreate:begin});";
			string actual = _generator.CreateAjaxScriptForLink(null, options);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void When_no_url_is_specified_then_the_url_should_default_to_the_href()
		{
			string expected = "new Ajax.Request('http://mvccontrib.org', {});";
			string actual = _generator.CreateAjaxScriptForLink("http://mvccontrib.org", new AjaxOptions());
			Assert.That(actual, Is.EqualTo(expected));
		}
	}
}