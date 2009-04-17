using System.Web;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.UnitTests.FluentHtml
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
			const decimal item = 1234.5m;
			var expected = string.Format("{0:$#,##0.00}", item);
			var html = new Literal().Value(item).Format("$#,##0.00").ToString();
			html.ShouldRenderHtmlDocument().ChildNodes[0]
				.ShouldHaveInnerTextEqual(expected);
		}

        [Test]
        public void literal_html_encodes_its_value()
        {
            const string value = "<div>Foo</div>";
            var html = new Literal().Value(value).ToString();
            html.ShouldRenderHtmlDocument().ChildNodes[0]
                .ShouldHaveInnerTextEqual(HttpUtility.HtmlEncode(value));
        }
	}
}
