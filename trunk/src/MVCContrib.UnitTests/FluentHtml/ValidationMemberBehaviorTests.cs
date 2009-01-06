using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
    [TestFixture]
    public class ValidationMemberBehaviorTests
    {
        [Test]
        public void element_for_member_with_no_error_renders_with_no_class()
        {
            var stateDictionary = new ModelStateDictionary();
            Expression<Func<FakeModel, object>> expression = x => x.Title;
            var target = new ValidationMemberBehavior(stateDictionary);
            var textbox = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), 
                                      new List<IMemberBehavior> { target });
            var element = textbox.ToString().ShouldHaveHtmlNode("Title");
            element.ShouldNotHaveAttribute("class");
        }

        [Test]
        public void element_for_member_with_error_renders_with_default_error_class()
        {
            var stateDictionary = new ModelStateDictionary();
            stateDictionary.AddModelError("Title", "Something bad happened");
            var target = new ValidationMemberBehavior(stateDictionary);
            Expression<Func<FakeModel, object>> expression = x => x.Title;
            var textbox = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(),
                                      new List<IMemberBehavior> { target });
            var element = textbox.ToString().ShouldHaveHtmlNode("Title");
            element.ShouldHaveAttribute("class").WithValue("input-validation-error");
        }

        [Test]
        public void element_for_member_with_error_renders_with_specified_error_class_and_specified_other_class()
        {
            var stateDictionary = new ModelStateDictionary();
            stateDictionary.AddModelError("Title", "Something bad happened");
            var target = new ValidationMemberBehavior(stateDictionary, "my-error-class");
            Expression<Func<FakeModel, object>> expression = x => x.Title;
            var textbox = new TextBox(expression.GetNameFor(), expression.GetMemberExpression(),
                                      new List<IMemberBehavior> { target }).Class("another-class");
            var element = textbox.ToString().ShouldHaveHtmlNode("Title");
            element.ShouldHaveAttribute("class").WithValue("another-class my-error-class");
        }
    }
}