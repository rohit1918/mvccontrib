using System.Web.Mvc;
using MvcContrib.UI.Html;
using MvcContrib.UI.Tags;
using NUnit.Framework;
using MvcContrib.UnitTests;
using MvcContrib;
using NUnit.Framework.SyntaxHelpers;
using System.Collections;
using System.Collections.Generic;
namespace MVCContrib.UnitTests.UI.Html
{
	[TestFixture]
	public class FormHelperTester
	{
		public class BaseFormHelperTester : BaseViewTester
		{
			protected FormHelper _helper;

			[SetUp]
			protected override void Setup()
			{
				base.Setup();
				_helper = new FormHelper(_viewContext);
			}

			protected List<Person> BuildPeople()
			{
				List<Person> people = new List<Person>();
				people.Add(new Person("Jeremy", 1));
				people.Add(new Person("Josh", 2));

				return people;
			}
		}

		[TestFixture]
		public class When_FormHelper_is_instantiated : BaseFormHelperTester
		{
			[Test]
			public void ViewContext_should_be_set()
			{
				Assert.That(_helper.ViewContext, Is.EqualTo(_viewContext));
			}

			[Test]
			public void Binder_should_be_DefaultDataBinder()
			{
				Assert.That(_helper.Binder, Is.InstanceOfType(typeof(DefaultDataBinder)));
			}
		}

		[TestFixture]
		public class When_Binder_Is_Null : BaseFormHelperTester
		{
			[Test]
			public void ObtainFromViewData_should_return_null()
			{
				((IDictionary)_viewContext.ViewData).Add("foo", "Bar");
				_helper.Binder = null;
				string html = _helper.TextField("foo");
				string expected = "<input type=\"text\" name=\"foo\" id=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}
		}

		[TestFixture]
		public class When_FormHelperExtensions_Is_Used : BaseViewTester
		{
			[SetUp]
			protected override void Setup()
			{
				base.Setup();
			}

			[Test]
			public void Then_a_FormHelper_should_be_created()
			{
				HtmlHelper helper = new HtmlHelper(_viewContext);
				IFormHelper formHelper = FormHelperExtensions.Form(helper);
				Assert.IsNotNull(formHelper);
			}
		}

		[TestFixture]
		public class When_TextField_Is_Invoked : BaseFormHelperTester
		{

			[Test]
			public void With_strongly_typed_options_the_correct_html_is_generated()
			{
				TextBox textField = new TextBox();
				textField.Name = "foo";
				textField.Class = "bar";
				textField.Value = "A Value";

				string html = _helper.TextField(textField);
				string expected = "<input type=\"text\" name=\"foo\" class=\"bar\" value=\"A Value\" id=\"foo\"/>";

				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_Name_only_the_correct_html_is_generated()
			{
				string html = _helper.TextField("foo");
				string expected = "<input type=\"text\" name=\"foo\" id=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_dictionary_attributes_the_correct_html_is_generated()
			{
				string html = _helper.TextField("foo", new Hash(@class => "bar"));
				string expected = "<input class=\"bar\" type=\"text\" name=\"foo\" id=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void Id_should_not_be_overwritten_if_explicitly_specified()
			{
				string html = _helper.TextField("foo", new Hash(id => "bar"));
				string expected = "<input id=\"bar\" type=\"text\" name=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void And_the_name_matches_an_item_in_the_ViewData_it_should_be_databound()
			{
				((IDictionary)_viewContext.ViewData).Add("foo", "Bar");
				string html = _helper.TextField("foo");
				string expected = "<input type=\"text\" name=\"foo\" id=\"foo\" value=\"Bar\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}
		}

		[TestFixture]
		public class When_HiddenField_Is_Invoked : BaseFormHelperTester
		{
			[Test]
			public void With_strongly_typed_options_then_correct_html_should_be_generated()
			{
				HiddenField hiddenField = new HiddenField();
				hiddenField.Name = "foo";
				hiddenField.Class = "bar";
				hiddenField.Value = "A Value";

				string html = _helper.HiddenField(hiddenField);
				string expected = "<input type=\"hidden\" name=\"foo\" class=\"bar\" value=\"A Value\" id=\"foo\"/>";

				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_name_only_the_correct_html_is_generated()
			{
				string html = _helper.HiddenField("foo");
				string expected = "<input type=\"hidden\" name=\"foo\" id=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_dictionary_attributes_the_correct_html_is_generated()
			{
				string html = _helper.HiddenField("foo", new Hash(@class => "bar"));
				string expected = "<input class=\"bar\" type=\"hidden\" name=\"foo\" id=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void Id_should_not_be_overwritten_if_explicitly_specified()
			{
				string html = _helper.HiddenField("foo", new Hash(id => "bar"));
				string expected = "<input id=\"bar\" type=\"hidden\" name=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void And_the_name_matches_an_item_in_the_ViewData_it_should_be_databound()
			{
				((IDictionary)_viewContext.ViewData).Add("foo", "Bar");
				string html = _helper.HiddenField("foo");
				string expected = "<input type=\"hidden\" name=\"foo\" id=\"foo\" value=\"Bar\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}
		}

		[TestFixture]
		public class When_TextArea_Is_Invoked : BaseFormHelperTester
		{
			[Test]
			public void With_strongly_typed_options_then_correct_html_should_be_generated()
			{
				TextArea textArea = new TextArea();
				textArea.Name = "foo";
				textArea.Class = "bar";
				textArea.InnerText = "A Value";

				string html = _helper.TextArea(textArea);
				string expected = "<textarea name=\"foo\" class=\"bar\" id=\"foo\">A Value</textarea>";

				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_name_only_the_correct_html_is_generated()
			{
				string html = _helper.TextArea("foo");
				string expected = "<textarea name=\"foo\" id=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_dictionary_attributes_the_correct_html_is_generated()
			{
				string html = _helper.TextArea("foo", new Hash(@class => "bar"));
				string expected = "<textarea class=\"bar\" name=\"foo\" id=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void Id_should_not_be_overwritten_if_explicitly_specified()
			{
				string html = _helper.TextArea("foo", new Hash(id => "bar"));
				string expected = "<textarea id=\"bar\" name=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void And_the_name_matches_an_item_in_the_ViewData_it_should_be_databound()
			{
				((IDictionary)_viewContext.ViewData).Add("foo", "Bar");
				string html = _helper.TextArea("foo");
				string expected = "<textarea name=\"foo\" id=\"foo\">Bar</textarea>";
				Assert.That(html, Is.EqualTo(expected));
			}
		}

		[TestFixture]
		public class When_Submit_Is_Invoked : BaseFormHelperTester
		{
			[Test]
			public void With_strongly_typed_options_then_correct_html_should_be_generated()
			{
				SubmitButton button = new SubmitButton();
				button.Name = "foo";
				button.Class = "bar";
				button.Value = "A Value";

				string html = _helper.Submit(button);
				string expected = "<input type=\"submit\" name=\"foo\" class=\"bar\" value=\"A Value\"/>";

				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_no_value_then_value_should_default_to_Submit()
			{
				string html = _helper.Submit();
				string expected = "<input type=\"submit\" value=\"Submit\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_value_only_the_correct_html_is_generated()
			{
				string html = _helper.Submit("foo");
				string expected = "<input type=\"submit\" value=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_dictionary_attributes_the_correct_html_is_generated()
			{
				string html = _helper.Submit("foo", new Hash(@class => "bar"));
				string expected = "<input class=\"bar\" type=\"submit\" value=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}
		}

		[TestFixture]
		public class When_Select_Is_Invoked : BaseFormHelperTester
		{
			[Test]
			public void With_strongly_typed_options_then_correct_html_should_be_generated()
			{
				Select select = new Select();
				select.Name = "foo";
				select.Class = "bar";
				select.TextField = "Name";
				select.ValueField = "Id";

				string html = _helper.Select(BuildPeople(), select);
				string expected =
					"<select name=\"foo\" class=\"bar\" id=\"foo\"><option value=\"1\">Jeremy</option><option value=\"2\">Josh</option></select>";

				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_Name_and_datasource_the_correct_html_is_generated()
			{
				string html = _helper.Select("foo", BuildPeople(), "Name", "Id");
				string expected =
					"<select name=\"foo\" id=\"foo\"><option value=\"1\">Jeremy</option><option value=\"2\">Josh</option></select>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_dictionary_attributes_the_correct_html_is_generated()
			{
				string html = _helper.Select("foo", BuildPeople(), "Name", "Id", new Hash(@class => "bar"));
				string expected = "<select class=\"bar\" name=\"foo\" id=\"foo\"><option value=\"1\">Jeremy</option><option value=\"2\">Josh</option></select>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_dictionary_attributes_firstoption_and_firstoptionvalue_should_be_extracted()
			{
				string html = _helper.Select("foo", BuildPeople(), "Name", "Id", new Hash(firstOption => "test", firstOptionValue => 0));
				string expected =
					"<select name=\"foo\" id=\"foo\"><option value=\"0\">test</option><option value=\"1\">Jeremy</option><option value=\"2\">Josh</option></select>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void Id_should_not_be_overwritten_if_explicitly_specified()
			{
				string html = _helper.Select("foo", BuildPeople(), "Name", "Id", new Hash(id => "bar"));
				string expected =
					"<select id=\"bar\" name=\"foo\"><option value=\"1\">Jeremy</option><option value=\"2\">Josh</option></select>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void And_the_name_matches_an_item_in_the_ViewData_it_should_be_databound()
			{
				((IDictionary)_viewContext.ViewData).Add("person", 2);
				string html = _helper.Select("person", BuildPeople(), "Name", "Id");
				string expected = "<select name=\"person\" id=\"person\"><option value=\"1\">Jeremy</option><option value=\"2\" selected=\"selected\">Josh</option></select>";
				Assert.That(html, Is.EqualTo(expected));
			}
			
		}

		[TestFixture]
		public class When_CheckBoxField_Is_Invoked : BaseFormHelperTester
		{
			[Test]
			public void With_strongly_typed_options_then_correct_html_should_be_generated()
			{
				CheckBoxField checkBox = new CheckBoxField();
				checkBox.Name = "foo";
				checkBox.Class = "bar";

				string html = _helper.CheckBoxField(checkBox);
				string expected = "<input type=\"checkbox\" name=\"foo\" class=\"bar\" id=\"foo\" value=\"true\"/><input type=\"hidden\" value=\"false\" id=\"fooH\" name=\"foo\"/>";

				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_Name_only_the_correct_html_is_generated()
			{
				string html = _helper.CheckBoxField("foo");
				string expected = "<input type=\"checkbox\" name=\"foo\" id=\"foo\" value=\"true\"/><input type=\"hidden\" value=\"false\" id=\"fooH\" name=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_dictionary_attributes_the_correct_html_is_generated()
			{
				string html = _helper.CheckBoxField("foo", new Hash(@class => "bar"));
				string expected = "<input class=\"bar\" type=\"checkbox\" name=\"foo\" id=\"foo\" value=\"true\"/><input type=\"hidden\" value=\"false\" id=\"fooH\" name=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void Id_should_not_be_generated_if_explicitly_specified()
			{
				string html = _helper.CheckBoxField("foo", new Hash(id => "bar"));
				string expected = "<input id=\"bar\" type=\"checkbox\" name=\"foo\" value=\"true\"/><input type=\"hidden\" value=\"false\" id=\"barH\" name=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void Then_Checked_should_be_true_if_item_in_viewdata_is_true()
			{
				((IDictionary)_viewContext.ViewData).Add("foo", true);
				string html = _helper.CheckBoxField("foo");
				string expected = "<input type=\"checkbox\" name=\"foo\" id=\"foo\" value=\"true\" checked=\"checked\"/><input type=\"hidden\" value=\"false\" id=\"fooH\" name=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void Then_Checked_should_be_false_if_item_in_viewdata_is_false()
			{
				((IDictionary)_viewContext.ViewData).Add("foo", false);
				string html = _helper.CheckBoxField("foo");
				string expected = "<input type=\"checkbox\" name=\"foo\" id=\"foo\" value=\"true\"/><input type=\"hidden\" value=\"false\" id=\"fooH\" name=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_custom_value_it_should_not_be_overwritten()
			{
				string html = _helper.CheckBoxField("foo", new Hash(value => "bar"));
				string expected = "<input value=\"bar\" type=\"checkbox\" name=\"foo\" id=\"foo\"/><input type=\"hidden\" value=\"false\" id=\"fooH\" name=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}
		}

		[TestFixture]
		public class When_RadioField_Is_Invoked : BaseFormHelperTester
		{
			[Test]
			public void With_strongly_typed_options_the_correct_html_is_generated()
			{
				RadioField radioField = new RadioField();
				radioField.Name = "foo";
				radioField.Class = "bar";
				radioField.Value = "A Value";

				string html = _helper.RadioField(radioField);
				string expected = "<input type=\"radio\" name=\"foo\" class=\"bar\" value=\"A Value\" id=\"foo\"/>";

				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_Name_only_the_correct_html_is_generated()
			{
				string html = _helper.RadioField("foo");
				string expected = "<input type=\"radio\" name=\"foo\" id=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_dictionary_attributes_the_correct_html_is_generated()
			{
				string html = _helper.RadioField("foo", new Hash(@class => "bar"));
				string expected = "<input class=\"bar\" type=\"radio\" name=\"foo\" id=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void Id_should_not_be_overwritten_if_explicitly_specified()
			{
				string html = _helper.RadioField("foo", new Hash(id => "bar"));
				string expected = "<input id=\"bar\" type=\"radio\" name=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void And_the_name_matches_an_item_in_the_ViewData_it_should_be_databound()
			{
				((IDictionary)_viewContext.ViewData).Add("foo", "Bar");
				string html = _helper.RadioField("foo");
				string expected = "<input type=\"radio\" name=\"foo\" id=\"foo\" value=\"Bar\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}
		}

		[TestFixture]
		public class When_CheckBoxList_is_invoked : BaseFormHelperTester
		{
			[Test]
			public void With_strongly_typed_options_then_correct_html_should_be_generated()
			{
				CheckBoxList list = new CheckBoxList();
				list.Name = "foo";
				list.TextField = "Name";
				list.ValueField = "Id";

				
				CheckBoxList output = _helper.CheckBoxList(BuildPeople(), list);
				Assert.That(output, Is.EqualTo(list));

				string expected =
					"<input type=\"checkbox\" name=\"foo\" id=\"foo-0\" value=\"1\"/><label for=\"foo-0\">Jeremy</label><input type=\"checkbox\" name=\"foo\" id=\"foo-1\" value=\"2\"/><label for=\"foo-1\">Josh</label>";

				Assert.That(output.ToString(), Is.EqualTo(expected));
			}

			[Test]
			public void With_name_datasource_textfield_and_valuefield_the_correct_html_should_be_generated()
			{
				CheckBoxList list = _helper.CheckBoxList("foo", BuildPeople(), "Name", "Id");

				string expected =
					"<input type=\"checkbox\" name=\"foo\" id=\"foo-0\" value=\"1\"/><label for=\"foo-0\">Jeremy</label><input type=\"checkbox\" name=\"foo\" id=\"foo-1\" value=\"2\"/><label for=\"foo-1\">Josh</label>";

				Assert.That(list.ToString(), Is.EqualTo(expected));
			}

			[Test]
			public void With_dictionary_of_attributes_the_correct_html_should_be_generated()
			{
				CheckBoxList list = _helper.CheckBoxList("foo", BuildPeople(), "Name", "Id", new Hash(@class => "bar"));
				string expected =
					"<input type=\"checkbox\" name=\"foo\" id=\"foo-0\" value=\"1\" class=\"bar\"/><label for=\"foo-0\">Jeremy</label><input type=\"checkbox\" name=\"foo\" id=\"foo-1\" value=\"2\" class=\"bar\"/><label for=\"foo-1\">Josh</label>";

				Assert.That(list.ToString(), Is.EqualTo(expected));
			}

		}

		[TestFixture]
		public class When_RadioList_is_invoked : BaseFormHelperTester
		{
			[Test]
			public void With_strongly_typed_options_then_correct_html_should_be_generated()
			{
				RadioList list = new RadioList();
				list.Name = "foo";
				list.TextField = "Name";
				list.ValueField = "Id";


				RadioList output = _helper.RadioList(BuildPeople(), list);
				Assert.That(output, Is.EqualTo(list));

				string expected =
					"<input type=\"radio\" name=\"foo\" id=\"foo-0\" value=\"1\"/><label for=\"foo-0\">Jeremy</label><input type=\"radio\" name=\"foo\" id=\"foo-1\" value=\"2\"/><label for=\"foo-1\">Josh</label>";

				Assert.That(output.ToString(), Is.EqualTo(expected));
			}

			[Test]
			public void With_name_datasource_textfield_and_valuefield_the_correct_html_should_be_generated()
			{
				RadioList list = _helper.RadioList("foo", BuildPeople(), "Name", "Id");

				string expected =
					"<input type=\"radio\" name=\"foo\" id=\"foo-0\" value=\"1\"/><label for=\"foo-0\">Jeremy</label><input type=\"radio\" name=\"foo\" id=\"foo-1\" value=\"2\"/><label for=\"foo-1\">Josh</label>";

				Assert.That(list.ToString(), Is.EqualTo(expected));
			}

			[Test]
			public void With_dictionary_of_attributes_the_correct_html_should_be_generated()
			{
				RadioList list = _helper.RadioList("foo", BuildPeople(), "Name", "Id", new Hash(@class => "bar"));
				string expected =
					"<input type=\"radio\" name=\"foo\" id=\"foo-0\" value=\"1\" class=\"bar\"/><label for=\"foo-0\">Jeremy</label><input type=\"radio\" name=\"foo\" id=\"foo-1\" value=\"2\" class=\"bar\"/><label for=\"foo-1\">Josh</label>";

				Assert.That(list.ToString(), Is.EqualTo(expected));
			}

			
		}

		#region Person test object
		public class Person
		{
			private string _name;
			private int _id;


			public Person(string name, int id)
			{
				_name = name;
				_id = id;
			}

			public string Name
			{
				get { return _name; }
				set { _name = value; }
			}

			public int Id
			{
				get { return _id; }
				set { _id = value; }
			}
		}
		#endregion
	}
}