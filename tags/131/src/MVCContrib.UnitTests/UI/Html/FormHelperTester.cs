using System.Web.Mvc;
using MvcContrib.UI.Html;
using MvcContrib.UI.Tags;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.Collections;
using System.Collections.Generic;
using Rhino.Mocks;
using MvcContrib.Interfaces;
using System.Linq;
using MvcContrib.Services;
namespace MvcContrib.UnitTests.UI.Html
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
				_helper = new FormHelper();
				_helper.ViewContext = _viewContext;
			}

			protected List<Person> BuildPeople()
			{
				var people = new List<Person>();
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
		public class When_GetInstance_Is_Invoked : BaseViewTester
		{
			[SetUp]
			public void SetUp()
			{
				//Ensure there isn't a dependencyresolver hanging around from a previous test...
				DependencyResolver.InitializeWith(null);
				base.Setup();
			}

			[TearDown]
			public void TearDown()
			{
				DependencyResolver.InitializeWith(null);
			}

			[Test]
			public void The_FormHelper_in_HttpContext_Items_should_be_returned()
			{
				var helper = new FormHelper();
				_viewContext.HttpContext.Items.Add(FormHelper.CACHE_KEY, helper);
				Assert.That(FormHelper.GetInstance(_viewContext), Is.EqualTo(helper));
			}

			[Test]
			public void A_new_FormHelper_should_be_created_and_cached_in_HttpContext_items_and_ViewContext_should_be_set()
			{
				IFormHelper helper = FormHelper.GetInstance(_viewContext);
				Assert.That(helper, Is.Not.Null);
				Assert.That(_viewContext.HttpContext.Items[FormHelper.CACHE_KEY], Is.EqualTo(helper));
				Assert.That(helper.ViewContext, Is.EqualTo(_viewContext));
			}

			[Test]
			public void Then_the_formhelper_should_be_created_using_the_dependencyresolver()
			{
				var helper = new FormHelper();
				using (mocks.Record())
				{
					var resolver = mocks.DynamicMock<IDependencyResolver>();
					DependencyResolver.InitializeWith(resolver);
					Expect.Call(resolver.GetImplementationOf<IFormHelper>()).Return(helper);
				}
				using (mocks.Playback())
				{
					IFormHelper instance = FormHelper.GetInstance(_viewContext);
					Assert.That(instance, Is.EqualTo(helper));
				}
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
				//Ensure there isn't a dependencyresolver hanging around from a previous test...
				DependencyResolver.InitializeWith(null);
			}

			[Test]
			public void Then_a_FormHelper_should_be_created()
			{
				var helper = new HtmlHelper(_viewContext, new ViewPage());
				IFormHelper formHelper = HtmlHelperExtensions.Form(helper);
				Assert.IsNotNull(formHelper);
			}
		}

		[TestFixture]
		public class When_TextField_Is_Invoked : BaseFormHelperTester
		{

			[Test]
			public void With_strongly_typed_options_the_correct_html_is_generated()
			{
				var textField = new TextBox();
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
		public class When_PasswordField_Is_Invoked : BaseFormHelperTester
		{

			[Test]
			public void With_strongly_typed_options_the_correct_html_is_generated()
			{
				var textField = new Password();
				textField.Name = "foo";
				textField.Class = "bar";
				textField.Value = "A Value";

				string html = _helper.PasswordField(textField);
				string expected = "<input type=\"password\" name=\"foo\" class=\"bar\" value=\"A Value\" id=\"foo\"/>";

				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_Name_only_the_correct_html_is_generated()
			{
				string html = _helper.PasswordField("foo");
				string expected = "<input type=\"password\" name=\"foo\" id=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_dictionary_attributes_the_correct_html_is_generated()
			{
				string html = _helper.PasswordField("foo", new Hash(@class => "bar"));
				string expected = "<input class=\"bar\" type=\"password\" name=\"foo\" id=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void Id_should_not_be_overwritten_if_explicitly_specified()
			{
				string html = _helper.PasswordField("foo", new Hash(id => "bar"));
				string expected = "<input id=\"bar\" type=\"password\" name=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void And_the_name_matches_an_item_in_the_ViewData_it_should_be_databound()
			{
				((IDictionary)_viewContext.ViewData).Add("foo", "Bar");
				string html = _helper.PasswordField("foo");
				string expected = "<input type=\"password\" name=\"foo\" id=\"foo\" value=\"Bar\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}
		}


		[TestFixture]
		public class When_HiddenField_Is_Invoked : BaseFormHelperTester
		{
			[Test]
			public void With_strongly_typed_options_then_correct_html_should_be_generated()
			{
				var hiddenField = new HiddenField();
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
				var textArea = new TextArea();
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
				string expected = "<textarea name=\"foo\" id=\"foo\"></textarea>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_dictionary_attributes_the_correct_html_is_generated()
			{
				string html = _helper.TextArea("foo", new Hash(@class => "bar"));
				string expected = "<textarea class=\"bar\" name=\"foo\" id=\"foo\"></textarea>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void Id_should_not_be_overwritten_if_explicitly_specified()
			{
				string html = _helper.TextArea("foo", new Hash(id => "bar"));
				string expected = "<textarea id=\"bar\" name=\"foo\"></textarea>";
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
				var button = new SubmitButton();
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
		public class When_ImageButton_Is_Invoked : BaseFormHelperTester
		{
			[Test]
			public void With_strongly_typed_options_then_correct_html_should_be_generated()
			{
				var button = new InputImage();
				button.Name = "foo";
				button.Src = "foo.gif";
				button.Class = "bar";
				button.Alt = "A Value";

				string html = _helper.ImageButton(button);
				string expected = "<input type=\"image\" name=\"foo\" src=\"foo.gif\" class=\"bar\" alt=\"A Value\"/>";

				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_value_only_the_correct_html_is_generated()
			{
				string html = _helper.ImageButton("foo.gif", "A Value");
				string expected = "<input type=\"image\" src=\"foo.gif\" alt=\"A Value\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_dictionary_attributes_the_correct_html_is_generated()
			{
				string html = _helper.ImageButton("foo.gif", "A Value", new Hash(@class => "bar"));
				string expected = "<input class=\"bar\" type=\"image\" src=\"foo.gif\" alt=\"A Value\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}
		}

		[TestFixture]
		public class When_Select_Is_Invoked : BaseFormHelperTester
		{
			[Test]
			public void With_strongly_typed_options_then_correct_html_should_be_generated()
			{
				var select = new Select();
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
				((IDictionary)_viewContext.ViewData).Add("persons", BuildPeople().Select(p => p.Id).ToArray());
				string html = _helper.Select("persons", BuildPeople(), "Name", "Id");
				string expected = "<select name=\"persons\" id=\"persons\"><option value=\"1\" selected=\"selected\">Jeremy</option><option value=\"2\" selected=\"selected\">Josh</option></select>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void And_the_name_matches_an_item_in_the_ViewData_it_should_be_databound_ids_only()
			{
				((IDictionary)_viewContext.ViewData).Add("persons", new[] { 1, 2 });
				string html = _helper.Select("persons", BuildPeople(), "Name", "Id");
				string expected = "<select name=\"persons\" id=\"persons\"><option value=\"1\" selected=\"selected\">Jeremy</option><option value=\"2\" selected=\"selected\">Josh</option></select>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void And_multiple_selectedvalues_are_specified_then_the_correct_items_should_be_selected()
			{
				string html = _helper.Select("persons", BuildPeople(), "Name", "Id", new Hash(selectedValue => new[] {1 , 2 }));
				string expected = "<select name=\"persons\" id=\"persons\"><option value=\"1\" selected=\"selected\">Jeremy</option><option value=\"2\" selected=\"selected\">Josh</option></select>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_generic_enum_parameter_and_the_name_matches_an_item_in_the_ViewData_it_should_be_databound()
			{
				((IDictionary)_viewContext.ViewData).Add("enumArray", new[] { TestEnum.One, TestEnum.Two });
				var html = _helper.Select<TestEnum>("enumArray");
				var expected = "<select name=\"enumArray\" id=\"enumArray\"><option value=\"0\" selected=\"selected\">One</option><option value=\"1\" selected=\"selected\">Two</option></select>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_generic_enum_parameter_and_the_name_matches_property_on_complex_object_in_the_ViewData_it_should_be_databound()
			{
				((IDictionary)_viewContext.ViewData).Add("person", new Person("Jeremy", 1) {Test = TestEnum.Two});
				var html = _helper.Select<TestEnum>("person.Test");
				var expected = "<select name=\"person.Test\" id=\"person-Test\"><option value=\"0\">One</option><option value=\"1\" selected=\"selected\">Two</option></select>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_SelectedValue_in_dictionary_then_correct_value_should_be_selected()
			{
				string html = _helper.Select("foo", BuildPeople(), "Name", "Id", new Hash(SelectedValue => "2"));
				string expected = "<select name=\"foo\" id=\"foo\"><option value=\"1\">Jeremy</option><option value=\"2\" selected=\"selected\">Josh</option></select>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_generic_enum_parameter_the_correct_html_is_generated()
			{
				var html = _helper.Select<TestEnum>("foo");
				var expected =
					"<select name=\"foo\" id=\"foo\"><option value=\"0\">One</option><option value=\"1\">Two</option></select>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_generic_enum_parameter__and_firstoption_and_selected_the_correct_html_is_generated()
			{
				var html = _helper.Select<TestEnum>("foo", new Hash(firstOption => "test", firstOptionValue => "", SelectedValue => TestEnum.Two));
				var expected =
					"<select name=\"foo\" id=\"foo\"><option value=\"\">test</option><option value=\"0\">One</option><option value=\"1\" selected=\"selected\">Two</option></select>";
				Assert.That(html, Is.EqualTo(expected));
			}
		}

		[TestFixture]
		public class When_CheckBoxField_Is_Invoked : BaseFormHelperTester
		{
			[Test]
			public void With_strongly_typed_options_then_correct_html_should_be_generated()
			{
				var checkBox = new CheckBoxField();
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

			[Test]
			public void With_mixed_case_label_attribute_then_lowercase_label_should_be_generated()
			{
				string html = _helper.CheckBoxField("foo", new Hash(LAbel => "bar"));
				string expected = "<input name=\"foo\" type=\"checkbox\" id=\"foo\" value=\"true\"/><label for=\"foo\">bar</label><input type=\"hidden\" value=\"false\" id=\"fooH\" name=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}
		}

		[TestFixture]
		public class When_RadioField_Is_Invoked : BaseFormHelperTester
		{
			[Test]
			public void With_strongly_typed_options_the_correct_html_is_generated()
			{
				var radioField = new RadioField();
				radioField.Id = "foo";
				radioField.Name = "foo";
				radioField.Class = "bar";
				radioField.Value = "A Value";

				string html = _helper.RadioField(radioField);
				string expected = "<input type=\"radio\" id=\"foo\" name=\"foo\" class=\"bar\" value=\"A Value\"/>";

				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void And_value_is_not_null_then_the_autogenerated_id_should_end_with_value()
			{
				var field = new RadioField();
				field.Value = "bar";
				field.Name = "foo";

				string html = _helper.RadioField(field);
				string expected = "<input type=\"radio\" value=\"bar\" name=\"foo\" id=\"foo-bar\"/>";

				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void And_value_contains_a_space_then_the_autogenerated_id_should_remove_the_space()
			{
				var field = new RadioField();
				field.Value = "A value";
				field.Name = "foo";

				string html = _helper.RadioField(field);
				string expected = "<input type=\"radio\" value=\"A value\" name=\"foo\" id=\"foo-Avalue\"/>";

				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_Name_only_the_correct_html_is_generated()
			{
				string html = _helper.RadioField("foo", null);
				string expected = "<input type=\"radio\" name=\"foo\" id=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void With_dictionary_attributes_the_correct_html_is_generated()
			{
				string html = _helper.RadioField("foo", null, new Hash(@class => "bar"));
				string expected = "<input class=\"bar\" type=\"radio\" name=\"foo\" id=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void Id_should_not_be_overwritten_if_explicitly_specified()
			{
				string html = _helper.RadioField("foo", null, new Hash(id => "bar"));
				string expected = "<input id=\"bar\" type=\"radio\" name=\"foo\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void And_its_value_matches_the_viewdata_value_then_checked_should_be_set()
			{
				((IDictionary)_viewContext.ViewData).Add("foo", "bar");
				string html = _helper.RadioField("foo", "bar");
				string expected = "<input type=\"radio\" name=\"foo\" value=\"bar\" checked=\"checked\" id=\"foo-bar\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void And_its_value_does_not_match_viewdata_value_then_checked_should_not_be_set()
			{
				((IDictionary)_viewContext.ViewData).Add("foo", "foo");
				string html = _helper.RadioField("foo", "bar");
				string expected = "<input type=\"radio\" name=\"foo\" value=\"bar\" id=\"foo-bar\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void And_its_value_is_not_present_in_the_viewdata_then_checked_should_not_be_set()
			{
				string html = _helper.RadioField("foo", "bar");
				string expected = "<input type=\"radio\" name=\"foo\" value=\"bar\" id=\"foo-bar\"/>";
				Assert.That(html, Is.EqualTo(expected));
			}
		}

		[TestFixture]
		public class When_CheckBoxList_is_invoked : BaseFormHelperTester
		{
			[Test]
			public void With_strongly_typed_options_then_correct_html_should_be_generated()
			{
				var list = new CheckBoxList();
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
				var list = new RadioList();
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

			[TestFixture]
			public class When_For_is_invoked : BaseFormHelperTester
			{
				[Test]
				public void Form_tag_should_be_written_to_output_stream()
				{
					string expected = "<form method=\"post\" action=\"/home/index\"></form>";
					_helper.For<object>(new object(), "/home/index", delegate { });
					Assert.That(_output.ToString(), Is.EqualTo(expected));
				}

				[Test]
				public void With_method_of_POST_in_the_attributes_the_correct_html_should_be_generated()
				{
					string expected = "<form method=\"post\" action=\"/home/index\"></form>";
					_helper.For<object>(new object(), "/home/index", new Hash(method => "post"), delegate { });
					Assert.That(_output.ToString(), Is.EqualTo(expected));
				}

				[Test]
				public void With_method_of_GET_in_the_attributes_the_correct_html_should_be_generated()
				{
					string expected = "<form method=\"get\" action=\"/home/index\"></form>";
					_helper.For<object>(new object(), "/home/index", new Hash(method => "get"), delegate { });
					Assert.That(_output.ToString(), Is.EqualTo(expected));
				}

				[Test]
				public void With_string_key_and_item_is_in_viewdata_then_item_should_be_extracted_from_viewdata()
				{
					var p = new Person("Jeremy", 1);
					((IDictionary)_viewContext.ViewData).Add("person", p);
					_helper.For<Person>("person", "/home/index", Hash.Empty, f =>
					{
						Assert.That(f.Item.Name, Is.EqualTo("Jeremy"));
					});
				}

				[Test]
				public void With_string_key_and_item_is_not_in_viewdata_then_form_item_should_be_null()
				{
					_helper.For<Person>("person", "/home/index", Hash.Empty, f =>
					{
						Assert.That(f.Item, Is.Null);
					});
				}
			}
		}

		
		#region Person test object
		public class Person
		{
			public Person(string name, int id)
			{
				Name = name;
				Id = id;
			}

			public string Name { get; set; }

			public int Id { get; set; }

			public TestEnum Test { get; set; }
		}
		#endregion

		public enum TestEnum
		{
			One,
			Two
		}
	}
}
