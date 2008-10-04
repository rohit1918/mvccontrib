using MvcContrib.UI.Ajax.JavaScriptElements;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.UI.Ajax.JavaScriptElements
{
	[TestFixture]
	public class FunctionCallTester
	{
		[Test]
		public void Should_generate_function_call()
		{
			string expected = "foo();";
			var function = new FunctionCall("foo");
			Assert.That(function.ToJavaScript(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_generate_function_call_with_arguments()
		{
			string expected = "foo(1, 'two', {three:'four'});";
			var function = new FunctionCall("foo", 1, "two", new JavaScriptDictionary(new Hash(three => "four")));
			Assert.That(function.ToJavaScript(), Is.EqualTo(expected));
		}

	}
}