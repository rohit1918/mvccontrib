using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class ValidationMemberBehaviorTests
	{
		ModelStateDictionary stateDictionary;
		private Expression<Func<FakeModel, object>> expression;
		private TextBox textbox;
		private ValidationMemberBehavior target;

		[SetUp]
		public void SetUp()
		{
			stateDictionary = new ModelStateDictionary();
		}

		[Test]
		public void element_for_member_with_no_error_renders_with_no_class()
		{
			target = new ValidationMemberBehavior(() => stateDictionary);
			expression = x => x.Price;
			textbox = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(),
				new List<IMemberBehavior> { target });
			var element = textbox.ToString().ShouldHaveHtmlNode("Price");
			element.ShouldNotHaveAttribute(HtmlAttribute.Class);
		}

		[Test]
		public void element_for_member_with_error_renders_with_default_error_class()
		{
			stateDictionary.AddModelError("Price", "Something bad happened");
			target = new ValidationMemberBehavior(() => stateDictionary);
			expression = x => x.Price;
			textbox = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(),
				new List<IMemberBehavior> { target });
			var element = textbox.ToString().ShouldHaveHtmlNode("Price");
			element.ShouldHaveAttribute(HtmlAttribute.Class).WithValue("input-validation-error");
		}

		[Test]
		public void element_for_member_with_error_renders_with_specified_error_class_and_specified_other_class()
		{
			stateDictionary.AddModelError("Price", "Something bad happened");
			target = new ValidationMemberBehavior(() => stateDictionary, "my-error-class");
			expression = x => x.Price;
			textbox = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(),
				new List<IMemberBehavior> { target }).Class("another-class");
			var element = textbox.ToString().ShouldHaveHtmlNode("Price");
			element.ShouldHaveAttribute(HtmlAttribute.Class).Value
				.ShouldContain("another-class")
				.ShouldContain("my-error-class");
		}

		[Test]
		public void element_with_error_renders_with_attempted_value()
		{
			stateDictionary.AddModelError("Price", "Something bad happened");
		    stateDictionary["Price"].Value = new ValueProviderResult("bad value", "bad value", CultureInfo.CurrentCulture);
			target = new ValidationMemberBehavior(() => stateDictionary);
			expression = x => x.Price;
			textbox = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(),
				new List<IMemberBehavior> { target });
			var element = textbox.ToString().ShouldHaveHtmlNode("Price");
			element.ShouldHaveAttribute(HtmlAttribute.Value).WithValue("bad value");
		}
	}
}