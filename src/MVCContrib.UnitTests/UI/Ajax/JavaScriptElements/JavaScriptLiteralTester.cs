using MvcContrib.UI.Ajax.JavaScriptElements;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.UI.Ajax.JavaScriptElements
{
	[TestFixture]
	public class JavaScriptLiteralTester
	{
		[Test]
		public void Should_generate_literal()
		{
			var literal = new JavaScriptLiteral("foo");
			Assert.That(literal.ToJavaScript(), Is.EqualTo("foo"));
		}
	}
}