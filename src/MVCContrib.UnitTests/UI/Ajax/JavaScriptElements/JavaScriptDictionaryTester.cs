using System;
using System.Collections.Generic;
using MvcContrib.UI.Ajax.JavaScriptElements;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.UI.Ajax.JavaScriptElements
{
	[TestFixture]
	public class JavaScriptDictionaryTester
	{
		[Test]
		public void Should_create_object()
		{
			var dict = new JavaScriptDictionary();
			dict.Add("foo", "bar");
			dict.Add("baz", "blah");
			string expected = "{foo:'bar', baz:'blah'}";
			Assert.That(dict.ToJavaScript(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_not_quote_javascript_literal()
		{
			var dict = new JavaScriptDictionary();
			dict.Add("foo", new JavaScriptLiteral("bar"));
			string expected = "{foo:bar}";
			Assert.That(dict.ToJavaScript(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_ignore_empty_entries()
		{
			var dict = new JavaScriptDictionary();
			dict.Add("foo", "bar");
			dict.Add("baz", "");
			string expected = "{foo:'bar'}";
			Assert.That(dict.ToJavaScript(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_use_custom_dictionary()
		{
			var dict = new JavaScriptDictionary(new Hash(foo => "bar", baz => "blah"));
			string expected = "{foo:'bar', baz:'blah'}";

			Assert.That(dict.ToJavaScript(), Is.EqualTo(expected));
		}
	}
}