using System.Collections.Generic;
using MvcContrib;
using MvcContrib.UI.Tags;
using NUnit.Framework.SyntaxHelpers;
using System.Linq;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.Html
{
	[TestFixture]
	public class InputElementListTester
	{
		[TestFixture]
		public class When_Elements_Are_Added
		{
			[Test]
			public void Then_ToString_should_produce_correct_html()
			{
				CheckBoxField field1 = new CheckBoxField(new Hash(name => "foo", value => "bar"));
				CheckBoxField field2 = new CheckBoxField(new Hash(name => "x", value => "y"));
				string expected = field1.ToString() + field2.ToString();

				InputElementList<CheckBoxField> list = new InputElementList<CheckBoxField>(Hash.Empty);
				list.Add(field1);
				list.Add(field2);
				string html = list.ToString();

				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void Enumerating_should_return_checkboxes()
			{
				CheckBoxField field1 = new CheckBoxField(new Hash(name => "foo", value => "bar"));
				CheckBoxField field2 = new CheckBoxField(new Hash(name => "x", value => "y"));

				InputElementList<CheckBoxField> list = new InputElementList<CheckBoxField>(Hash.Empty);
				list.Add(field1);
				list.Add(field2);

				List<CheckBoxField> boxes = new List<CheckBoxField>();
				
				foreach(CheckBoxField field in list)
				{
					boxes.Add(field);
				}

				Assert.That(boxes.Count, Is.EqualTo(2));
				Assert.That(boxes[0], Is.EqualTo(field1));
				Assert.That(boxes[1], Is.EqualTo(field2));
			}
		}

		[TestFixture]
		public class When_Custom_Attributes_are_specified
		{
			[Test]
			public void They_should_be_applied_to_all_checkboxes()
			{
				InputElementList<CheckBoxField> list = new InputElementList<CheckBoxField>(new Hash(@class => "foo"));

				list.Add(new CheckBoxField());
				list.Add(new CheckBoxField());

				foreach(CheckBoxField field in list)
				{
					Assert.That(field.Class, Is.EqualTo("foo"));
				}
			}

			[Test]
			public void They_should_not_overwrite_existing_attributes()
			{
				InputElementList<CheckBoxField> list = new InputElementList<CheckBoxField>(new Hash(@class => "foo"));
				list.Add(new CheckBoxField(new Hash(@class => "bar")));
				Assert.That(list.First().Class, Is.EqualTo("bar"));
			}
		}

		[TestFixture]
		public class When_ToFormattedString_is_invoked
		{
			[Test]
			public void Then_the_format_should_be_applied_to_each_checkbox()
			{
				InputElementList<CheckBoxField> list = new InputElementList<CheckBoxField>(Hash.Empty);
				list.Add(new CheckBoxField(new Hash(name => "Test1")));
				list.Add(new CheckBoxField(new Hash(name => "Test2")));

				string expected = "<input name=\"Test1\" type=\"checkbox\"/><br /><input name=\"Test2\" type=\"checkbox\"/><br />";
				string output = list.ToFormattedString("{0}<br />");
				Assert.That(output, Is.EqualTo(expected));
			}
		}
	}
}