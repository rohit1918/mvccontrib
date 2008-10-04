using System.Web.Mvc.Ajax;
using MvcContrib.UI.Ajax.Internal;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.UI.Ajax
{
	[TestFixture]
	public class PrototypAjaxOptionsTester
	{
		private string OptionsToString(AjaxOptions options)
		{
			return new PrototypeAjaxOptionsWrapper(options).ToJavaScript();
		}

		[Test]
		public void Should_convert_ajax_options_to_string_with_confirm_message()
		{
			var options = new AjaxOptions {Confirm = "Are you sure?", Url = "http://mvccontrib.org"};
			string expected = "if(confirm('Are you sure?')) { new Ajax.Request('http://mvccontrib.org', {}); }";
			string actual = OptionsToString(options);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Should_convert_ajax_options_to_string_with_http_method()
		{
			var options = new AjaxOptions {Url = "http://mvccontrib.org", HttpMethod = "GET"};
			string expected = "new Ajax.Request('http://mvccontrib.org', {method:'GET'});";
			string actual = OptionsToString(options);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Should_convert_ajax_options_to_string_with_element_to_update()
		{
			var options = new AjaxOptions {Url = "http://mvccontrib.org", UpdateTargetId = "foo"};
			string expected = "new Ajax.Updater('foo', 'http://mvccontrib.org', {});";
			string actual = OptionsToString(options);

			Assert.That(actual, Is.EqualTo(expected));
		}


		[Test]
		public void Should_convert_ajax_options_to_string_with_element_to_update_and_InsertionMode()
		{
			var options = new AjaxOptions
			              	{InsertionMode = InsertionMode.InsertAfter, UpdateTargetId = "foo", Url = "http://mvccontrib.org"};
			string expected = "new Ajax.Updater('foo', 'http://mvccontrib.org', {insertion:Insertion.After});";
			string actual = OptionsToString(options);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Should_convert_ajax_options_to_string_with_loading_element_id()
		{
			var options = new AjaxOptions {Url = "http://mvccontrib.org", LoadingElementId = "foo"};
			string expected =
				"new Ajax.Request('http://mvccontrib.org', {onCreate:function() { $('foo').show(); }, onComplete:function() { $('foo').hide(); }});";
			string actual = OptionsToString(options);

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
			string actual = OptionsToString(options);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void When_an_oncomplete_is_specified_then_loadingelementid_is_ignored()
		{
			var options = new AjaxOptions {Url = "http://mvccontrib.org", LoadingElementId = "foo", OnComplete = "complete"};
			string expected = "new Ajax.Request('http://mvccontrib.org', {onComplete:complete});";
			string actual = OptionsToString(options);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void When_an_onbegin_is_specified_then_loadingelementid_is_ignored()
		{
			var options = new AjaxOptions {Url = "http://mvccontrib.org", LoadingElementId = "foo", OnBegin = "begin"};
			string expected = "new Ajax.Request('http://mvccontrib.org', {onCreate:begin});";
			string actual = OptionsToString(options);

			Assert.That(actual, Is.EqualTo(expected));
		}
	}
}