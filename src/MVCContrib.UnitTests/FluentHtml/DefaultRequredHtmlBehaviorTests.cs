using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;

namespace MvcContrib.UnitTests.FluentHtml
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
