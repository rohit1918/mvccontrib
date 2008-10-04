using MvcContrib.UI.Ajax.JavaScriptElements;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.UI.Ajax.JavaScriptElements
{
	[TestFixture]
	public class AnonymousFunctionTester
	{
		[Test]
		public void Should_generate_function_call()
		{
			string expected = "function() { foo(); }";
			var func = new AnonymousFunction("foo();");
			Assert.That(func.ToJavaScript(), Is.EqualTo(expected));
		}
	}
}