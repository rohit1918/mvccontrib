using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;
using MvcContrib.FluentHtml.Html;
using MvcContrib.FluentHtml.Tests.Helpers;
using MvcContrib.FluentHtml.Tests.Fakes;

namespace MvcContrib.FluentHtml.Tests
{
	[TestFixture]
	public class TextAreaTests
	{
		[Test]
		public void textarea_renders_with_correct_tag_id_and_name()
		{
			var element = new TextArea("foo.Bar").ToString()
				.ShouldHaveHtmlNode("foo_Bar")
				.ShouldBeNamed(HtmlTag.TextArea);

			element.ShouldHaveAttribute(HtmlAttribute.Name).WithValue("foo.Bar");
		}

		[Test]
		public void model_textbox_renders_with_correct_tag_id_and_name()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Person.FirstName;
			var element = new TextArea(expression.GetNameFor(), expression.GetMemberExpression(), null).ToString()
				.ShouldHaveHtmlNode("Person_FirstName")
				.ShouldBeNamed(HtmlTag.TextArea);

			element.ShouldHaveAttribute(HtmlAttribute.Name).WithValue("Person.FirstName");
		}

		[Test]
		public void textarea_renders_with_correct_inner_text()
		{
			new TextArea("foo.Bar").Value("foo bar").ToString()
				.ShouldHaveHtmlNode("foo_Bar")
				.ShouldHaveInnerTextEqual("foo bar");
		}

		[Test]
		public void textarea_renders_with_formatted_inner_text()
		{
			new TextArea("foo.Bar").Value(1234.5).Format("$#,##0.00").ToString()
				.ShouldHaveHtmlNode("foo_Bar")
				.ShouldHaveInnerTextEqual("$1,234.50");
		}

		[Test]
		public void textarea_renders_with_correct_inner_text_from_enumerable_value_with_formatting()
		{
			var items = new List<decimal> { 1234.5m, 234, 345.666m };
			new TextArea("foo.Bar").Value(items).Format("$#,##0.00").ToString()
				.ShouldHaveHtmlNode("foo_Bar")
				.ShouldHaveInnerTextEqual("$1,234.50\r\n$234.00\r\n$345.67");
		}

		[Test]
		public void textarea_rows_renders_rows()
		{
			new TextArea("x").Rows(9).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Rows).WithValue("9");
		}

		[Test]
		public void textarea_columns_renders_columns()
		{
			new TextArea("x").Columns(44).ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldHaveAttribute(HtmlAttribute.Cols).WithValue("44");
		}
	}
}
