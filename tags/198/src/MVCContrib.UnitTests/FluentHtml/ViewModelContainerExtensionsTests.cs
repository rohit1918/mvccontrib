using System;
using MvcContrib.FluentHtml;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class ViewModelContainerExtensionsTests
	{
		private FakeViewModelContainer target;
		private FakeModel fake;

		[SetUp]
		public void SetUp()
		{
			target = new FakeViewModelContainer();
			fake = new FakeModel
			{
				Title = "Test Title",
				Date = DateTime.Now,
				Done = true,
				Id = 123,
				Person = new FakeChildModel
				{
					FirstName = "Mick",
					LastName = "Jagger"
				},
				Numbers = new [] {1, 3}
			};
			target.ViewModel = fake;
		}

		[Test]
		public void can_get_textbox_with_value_from_simple_property()
		{
			var element = target.TextBox(x => x.Title);
			element.ValueAttributeShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_textbox_with_value_from_complex_property()
		{
			var element = target.TextBox(x => x.Person.FirstName);
			element.ValueAttributeShouldEqual(fake.Person.FirstName);
		}

		[Test]
		public void can_get_checkbox_with_checked()
		{
			var element = target.CheckBox(x => x.Done);
			element.AttributeShouldEqual(HtmlAttribute.Checked, HtmlAttribute.Checked);
		}

		[Test]
		public void can_get_literal_with_value()
		{
			var element = target.Literal(x => x.Title);
			element.InnerTextShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_form_literal_with_value()
		{
			var element = target.FormLiteral(x => fake.Title);
			element.InnerTextShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_hidden_with_value()
		{
			var element = target.Hidden(x => fake.Title);
			element.ValueAttributeShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_select()
		{
			var element = target.Select(x => x.Id);
			element.SelectedValues.ShouldCount(1);
			var enumerator = element.SelectedValues.GetEnumerator();
			enumerator.MoveNext();
			enumerator.Current.ShouldEqual(fake.Id);
		}

		[Test]
		public void can_get_multi_select()
		{
			var element = target.MultiSelect(x => x.Numbers);
			element.SelectedValues.ShouldCount(2);
			var enumerator = element.SelectedValues.GetEnumerator();
			enumerator.MoveNext();
			enumerator.Current.ShouldEqual(fake.Numbers[0]);
			enumerator.MoveNext();
			enumerator.Current.ShouldEqual(fake.Numbers[1]);
		}

		[Test]
		public void can_get_password_with_value()
		{
			var element = target.Password(x => fake.Title);
			element.ValueAttributeShouldEqual(fake.Title);
		}

		[Test]
		public void can_get_text_area_with_value()
		{
			var element = target.TextArea(x => fake.Title);
			element.InnerTextShouldEqual(fake.Title);
		}
	}
}
