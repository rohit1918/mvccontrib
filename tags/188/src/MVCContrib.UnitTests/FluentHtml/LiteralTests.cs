using NUnit.Framework;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.FluentHtml.Tests.Helpers;

namespace MvcContrib.FluentHtml.Tests
{
	[TestFixture]
	public class LiteralTests
	{
		[Test]
		public void literal_renders_with_correct_tag()
		{
			var html = new Literal().ToString();
			html.ShouldRenderHtmlDocument().ChildNodes[0]
				.ShouldBeNamed(HtmlTag.Span);
		}

		[Test]
		public void literal_renders_with_correct_inner_text()
		{
			var html = new Literal().Value("foo bar").ToString();
			html.ShouldRenderHtmlDocument().ChildNodes[0]
				.ShouldHaveInnerTextEqual("foo bar");
		}

		[Test]
		public void literal_renders_with_inner_text_formatted()
		{
			var html = new Literal().Value(1234.5m).Format("$#,##0.00").ToString();
			html.ShouldRenderHtmlDocument().ChildNodes[0]
				.ShouldHaveInnerTextEqual("$1,234.50");
		}
	}
}
