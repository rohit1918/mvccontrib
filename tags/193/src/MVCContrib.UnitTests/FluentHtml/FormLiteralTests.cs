using NUnit.Framework;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.FluentHtml.Tests.Helpers;

namespace MvcContrib.FluentHtml.Tests
{
	[TestFixture]
	public class FormLiteralTests
	{
		[Test]
		public void form_literal_renders_correct_hidden_element()
		{
			var html = new FormLiteral("foo.Bar").Value("foo bar").Value(123).ToString();
			
			var element = html.ShouldRenderHtmlDocument().ChildNodes[1]
				.ShouldBeNamed(HtmlTag.Input);
			element.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Hidden);
			element.ShouldHaveAttribute(HtmlAttribute.Name).WithValue("foo.Bar");
			element.ShouldHaveAttribute(HtmlAttribute.Value).WithValue("123");
		}
	}
}
