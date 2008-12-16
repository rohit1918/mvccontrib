using System;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;
using MvcContrib.FluentHtml.Html;
using MvcContrib.FluentHtml.Tests.Helpers;
using MvcContrib.FluentHtml.Tests.Fakes;
using NUnit.Framework;

namespace MvcContrib.FluentHtml.Tests
{
	[TestFixture]
	public class CheckboxTests
	{
		[Test]
		public void basic_checkbox_renders_with_corect_tag_and_type()
		{
			VerifyHtml(new CheckBox("Done").ToString());
		}

		[Test]
		public void model_password_renders_with_id_and_name()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Done;
			VerifyHtml(new CheckBox(expression.GetNameFor(), expression.GetMemberExpression(), null).ToString());
		}

		private void VerifyHtml(string html)
		{
			var doc = html.ShouldRenderHtmlDocument();

			var chechbox = doc.ChildNodes[0].ShouldBeNamed(HtmlTag.Input);
			chechbox.ShouldHaveAttributesCount(4);
			chechbox.ShouldHaveAttribute(HtmlAttribute.Id).WithValue("Done");
			chechbox.ShouldHaveAttribute(HtmlAttribute.Name).WithValue("Done");
			chechbox.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Checkbox);
			chechbox.ShouldHaveAttribute(HtmlAttribute.Value).WithValue("true");

			var hidden = doc.ChildNodes[1].ShouldBeNamed(HtmlTag.Input);
			hidden.ShouldHaveAttributesCount(4);
			hidden.ShouldHaveAttribute(HtmlAttribute.Id).WithValue("Done_Hidden");
			hidden.ShouldHaveAttribute(HtmlAttribute.Name).WithValue("Done");
			hidden.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Hidden);
			hidden.ShouldHaveAttribute(HtmlAttribute.Value).WithValue("false");
		}

		[Test]
		public void checkbox_with_label_after_and_class_renders_label_after_with_class()
		{
			var label = new CheckBox("x").LabelAfter("Check Me", "label").ToString()
				.ShouldRenderHtmlDocument().ChildNodes[1];
			label.ShouldHaveAttribute(HtmlAttribute.Class).WithValue("label");
		}
	}
}
