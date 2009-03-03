using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class DefaultMaxlengthHtmlBehaviorTests
	{
		private IList<IBehaviorMarker> behaviors;

        [SetUp]
        public void SetUp()
        {
            behaviors = new List<IBehaviorMarker> {new DefaultMaxLengthMemberBehavior()};
        }

	    [Test]
		public void member_with_maxlength_attribute_renders_with_maxlength_attribute()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Title;
			var textbox = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), behaviors);
			var element = textbox.ToString().ShouldHaveHtmlNode("Title");
			element.ShouldHaveAttribute("maxlength").ValueShouldContain("200");
		}

		[Test]
		public void member_without_required_attribute_renders_without_required_class()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Done;
            var textbox = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), behaviors);
			var element = textbox.ToString().ShouldHaveHtmlNode("Done");
			element.ShouldNotHaveAttribute("maxlength");
		}

		[Test]
		public void render_element_without_maxlength_method_renders_without_maxlength()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Done;
            var checkbox = new CheckBox(expression.GetNameFor(), expression.GetMemberExpression(), behaviors);
            var element = checkbox.ToString().ShouldHaveHtmlNode("Done");
			element.ShouldNotHaveAttribute("maxlength");
		}
	}
}