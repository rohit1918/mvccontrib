using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;
using MvcContrib.FluentHtml.Tests.Fakes;
using MvcContrib.FluentHtml.Tests.Helpers;

namespace MvcContrib.FluentHtml.Tests
{
	[TestFixture]
	public class DefaultRequredHtmlBehaviorTests
	{
		[Test]
		public void member_with_required_attribute_renders_with_required_class()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Price;
			var textbox = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), 
				new List<IMemberBehavior> {new DefaultRequiredMemberBehavior()});
			var element = textbox.ToString().ShouldHaveHtmlNode("Price");
			element.ShouldHaveAttribute("class").ValueShouldContain("required");
		}

		[Test]
		public void member_without_required_attribute_renders_without_required_class()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Done;
			var textbox = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), 
				new List<IMemberBehavior> { new DefaultRequiredMemberBehavior() });
			var element = textbox.ToString().ShouldHaveHtmlNode("Done");
			element.ShouldNotHaveAttribute("class");
		}
	}
}
