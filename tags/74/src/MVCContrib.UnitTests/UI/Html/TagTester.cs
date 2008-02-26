using MvcContrib;
using MvcContrib.UI;
using MvcContrib.UI.Tags;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.UI.Html
{
	/*
	 * Tags are really just property bags. as such there will be three sets of tests per tag that will reach 100% coverage
	 * Creation tests (one test per Init method)
	 * Property Stick tests (one test for all propertys on a tag)
	 * Tag Type validation (one test to make sure the Tag property is correctly set)
	 * On Inputs the Tag Type test will also test for Type Attribute.
	 */
	[TestFixture]
	public class TagTester
	{
		[TestFixture]
		public class Link_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				Link element = new Link();
				Assert.That(element.Tag, Is.EqualTo("a"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				Link element = new Link();
				element.Href = "http://foo";
				Assert.That(element.Href, Is.EqualTo("http://foo"));
				element.Target = "_main";
				Assert.That(element.Target, Is.EqualTo("_main"));
			}

			[Test]
			public void When_Creating_element_With_Href_It_Sticks()
			{
				Link element = new Link("http://foo");
				Assert.That(element.Href, Is.EqualTo("http://foo"));
			}

			[Test]
			public void When_Creating_element_With_Href_and_Dictionary_It_Sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				Link element = new Link("http://foo", hash);
				Assert.That(element.Href, Is.EqualTo("http://foo"));
				Assert.That(element.Tag == "a");
				Assert.That(element.Attributes.Count == 4);
				Assert.That(element["Key1"] == "Val1");
			}
		}

		[TestFixture]
		public class Image_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				Image img = new Image();
				Assert.That(img.Tag, Is.EqualTo("img"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				Image img = new Image();
				img.Src = "http://foo.gif";
				Assert.That(img.Src, Is.EqualTo("http://foo.gif"));
				img.Alt = "A Penguin";
				Assert.That(img.Alt, Is.EqualTo("A Penguin"));
				Assert.That(img.Height == 0);
				img.Height = 10;
				Assert.That(img.Height, Is.EqualTo(10));
				Assert.That(img.Width == 0);
				img.Width = 99;
				Assert.That(img.Width, Is.EqualTo(99));
			}

			[Test]
			public void Int_Properties_Work_Right()
			{
				Image img = new Image();
				Assert.That(img.Height == 0);
				Assert.That(img.Width == 0);
				Assert.That(img["width"] == null);
				Assert.That(img["height"] == null);

				img["width"] = "A bad value";
				Assert.That(img.Width == 0);
				img["height"] = "A bad value";
				Assert.That(img.Height == 0);

				img.Height = 10;
				img.Width = 10;
				Assert.That(img.Height == 10);
				Assert.That(img.Width == 10);
				Assert.That(img["width"] == "10");
				Assert.That(img["height"] == "10");

				img.Height = -1;
				img.Width = 0;
				Assert.That(img.Height == 0);
				Assert.That(img.Width == 0);
				Assert.That(img["width"] == null);
				Assert.That(img["height"] == null);
			}

			[Test]
			public void When_Creating_Image_With_Src_It_Sticks()
			{
				Image img = new Image("http://foo.gif");
				Assert.That(img.Src, Is.EqualTo("http://foo.gif"));
			}
			
			[Test]
			public void When_Creating_Image_With_Src_And_Dictionary_It_Sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				Image element = new Image("http://foo.gif", hash);
				Assert.That(element.Src, Is.EqualTo("http://foo.gif"));
				Assert.That(element.Tag == "img");
				Assert.That(element.Attributes.Count == 4);
				Assert.That(element["Key1"] == "Val1");
			}
		}

		[TestFixture]
		public class Input_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				Input element = new Input("button",Hash.Empty);
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Type, Is.EqualTo("button"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				Input element = new Input("button", Hash.Empty);
				element.Value = "Value Sticks";
				Assert.That(element.Value.ToString() == "Value Sticks");
				element.Name = "Name Sticks";
				Assert.That(element.Name == "Name Sticks");
				element.OnBlur = "OnBlur Sticks";
				Assert.That(element.OnBlur == "OnBlur Sticks");
				element.OnFocus = "OnFocus Sticks";
				Assert.That(element.OnFocus == "OnFocus Sticks");
				element.OnSelect = "OnSelect Sticks";
				Assert.That(element.OnSelect == "OnSelect Sticks");
				element.OnChange = "OnChange Sticks";
				Assert.That(element.OnChange == "OnChange Sticks");
				element.Value = null;
				Assert.That(element.Value == null);
			}

			[Test]
			public void Bool_Properties_Work_Right()
			{
				Input element = new Input("button", Hash.Empty);
				Assert.That(element.Disabled == false);
				Assert.That(element.ReadOnly == false);
				Assert.That(element["disabled"] == null);
				Assert.That(element["readonly"] == null);
				element.Disabled = true;
				element.ReadOnly = true;
				Assert.That(element.Disabled);
				Assert.That(element.ReadOnly);
				Assert.That(element["disabled"] == "disabled");
				Assert.That(element["readonly"] == "readonly");

				element["disabled"] = "manual edit bad";
				element["readonly"] = "manual edit bad";

				Assert.That(element.Disabled == false);
				Assert.That(element.ReadOnly == false);

				element.Disabled = false;
				element.ReadOnly = false;
				Assert.That(element.Disabled == false);
				Assert.That(element.ReadOnly == false);
				Assert.That(element["disabled"] == null);
				Assert.That(element["readonly"] == null);
			}

			[Test]
			public void When_Creating_element_With_Dictionary_Stics()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				Input element = new Input("button", hash); 
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Type, Is.EqualTo("button"));
				Assert.That(element.Attributes.Count == 4);
				Assert.That(element["Key1"] == "Val1");
			}

			[Test]
			public void When_label_is_in_attributes_it_should_be_removed()
			{
				CheckBoxField field = new CheckBoxField(new Hash(label => "Foo"));
				Assert.That(field.Label, Is.EqualTo("Foo"));
				Assert.That(field.Attributes.ContainsKey("label"), Is.False);
			}

			[Test]
			public void When_label_specified_manually_it_should_stick()
			{
				CheckBoxField field = new CheckBoxField();
				field.Label = "Foo";
				Assert.That(field.Label, Is.EqualTo("Foo"));
			}

			[Test]
			public void ToString_should_render_label()
			{
				string expected = "<input id=\"foo\" type=\"checkbox\"/><label for=\"foo\">Bar</label>";
				CheckBoxField field = new CheckBoxField(new Hash(id => "foo", label => "Bar"));
				string html = field.ToString();
				Assert.That(html, Is.EqualTo(expected));
			}

			[Test]
			public void ToString_should_not_render_label_with_no_id()
			{
				string expected = "<input type=\"checkbox\"/>";
				CheckBoxField field = new CheckBoxField(new Hash(label => "Bar"));
				string html = field.ToString();
				Assert.That(html, Is.EqualTo(expected));
			}
		}

		[TestFixture]
		public class TextBox_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				TextBox element = new TextBox();
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Type, Is.EqualTo("text"));
			}

			[Test]
			public void Int_Properties_Work_Right()
			{
				TextBox element = new TextBox();
				Assert.That(element.MaxLength == 0);
				Assert.That(element.Size == 0);
				Assert.That(element["maxlength"] == null);
				Assert.That(element["size"] == null);

				element["maxlength"] = "A bad value";
				Assert.That(element.MaxLength == 0);
				element["size"] = "A bad value";
				Assert.That(element.Size == 0);

				element.Size = 10;
				element.MaxLength = 10;
				Assert.That(element.Size == 10);
				Assert.That(element.MaxLength == 10);
				Assert.That(element["maxlength"] == "10");
				Assert.That(element["size"] == "10");

				element.MaxLength = -1;
				element.Size = 0;
				Assert.That(element.Size == 0);
				Assert.That(element.MaxLength == 0);
				Assert.That(element["maxlength"] == null);
				Assert.That(element["size"] == null);
			}


			[Test]
			public void When_Creating_element_With_Dictionary_Sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				TextBox element = new TextBox(hash);
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Type, Is.EqualTo("text"));
				Assert.That(element.Attributes.Count == 4);
				Assert.That(element["Key1"] == "Val1");
			}
		}
		
		[TestFixture]
		public class TextArea_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				TextArea element = new TextArea();
				Assert.That(element.Tag, Is.EqualTo("textarea"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				TextArea element = new TextArea();
				element.Value = "Value&Sticks";
				Assert.That(element.Value.ToString() == "Value&Sticks");
				Assert.That(element.InnerText == "Value&Sticks");
				Assert.That(element.ToString(), Is.EqualTo("<textarea >Value&amp;Sticks</textarea>"));
				element.Value = null;
				Assert.That(element.Value.ToString() == "");
				Assert.That(element.InnerText == "");
				element.Name = "Name Sticks";
				Assert.That(element.Name == "Name Sticks");
				element.OnBlur = "OnBlur Sticks";
				Assert.That(element.OnBlur == "OnBlur Sticks");
				element.OnFocus = "OnFocus Sticks";
				Assert.That(element.OnFocus == "OnFocus Sticks");
				element.OnSelect = "OnSelect Sticks";
				Assert.That(element.OnSelect == "OnSelect Sticks");
				element.OnChange = "OnChange Sticks";
				Assert.That(element.OnChange == "OnChange Sticks");
			}

			[Test]
			public void Bool_Properties_Work_Right()
			{
				TextArea element = new TextArea();
				Assert.That(element.Disabled == false);
				Assert.That(element.ReadOnly == false);
				Assert.That(element["disabled"] == null);
				Assert.That(element["readonly"] == null);
				element.Disabled = true;
				element.ReadOnly = true;
				Assert.That(element.Disabled);
				Assert.That(element.ReadOnly);
				Assert.That(element["disabled"] == "disabled");
				Assert.That(element["readonly"] == "readonly");

				element["disabled"] = "manual edit bad";
				element["readonly"] = "manual edit bad";

				Assert.That(element.Disabled == false);
				Assert.That(element.ReadOnly == false);

				element.Disabled = false;
				element.ReadOnly = false;
				Assert.That(element.Disabled == false);
				Assert.That(element.ReadOnly == false);
				Assert.That(element["disabled"] == null);
				Assert.That(element["readonly"] == null);
			}

			[Test]
			public void Int_Properties_Work_Right()
			{
				TextArea element = new TextArea();
				Assert.That(element.Rows == 0);
				Assert.That(element.Cols == 0);
				Assert.That(element["rows"] == null);
				Assert.That(element["cols"] == null);

				element["rows"] = "A bad value";
				Assert.That(element.Rows == 0);
				element["cols"] = "A bad value";
				Assert.That(element.Cols == 0);

				element.Cols = 10;
				element.Rows = 10;
				Assert.That(element.Cols == 10);
				Assert.That(element.Rows == 10);
				Assert.That(element["rows"] == "10");
				Assert.That(element["cols"] == "10");

				element.Rows = -1;
				element.Cols = 0;
				Assert.That(element.Cols == 0);
				Assert.That(element.Rows == 0);
				Assert.That(element["rows"] == null);
				Assert.That(element["cols"] == null);
			}

			[Test]
			public void When_Creating_element_With_Dictionary_Sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				TextArea element = new TextArea(hash);
				Assert.That(element.Tag, Is.EqualTo("textarea"));
				Assert.That(element.Attributes.Count == 3);
				Assert.That(element["Key1"] == "Val1");
			}
		}

		[TestFixture]
		public class InputButton_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				InputButton element = new InputButton();
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Type, Is.EqualTo("button"));
			}

			[Test]
			public void When_Creating_element_With_Dictionary_Sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				InputButton element = new InputButton(hash);
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Attributes.Count == 4);
				Assert.That(element["Key1"] == "Val1");
			}
		}
		
		[TestFixture]
		public class InputImage_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				InputImage element = new InputImage();
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Type, Is.EqualTo("image"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				InputImage element = new InputImage();
				element.Src = "Src Sticks";
				Assert.That(element.Src == "Src Sticks");
				element.Alt = "Alt Sticks";
				Assert.That(element.Alt == "Alt Sticks");
			}

			[Test]
			public void When_Creating_element_With_Src_It_Sticks()
			{
				InputImage element = new InputImage("http://image.foo.jpg");
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Type, Is.EqualTo("image"));
				Assert.That(element.Src == "http://image.foo.jpg");
			}

			[Test]
			public void When_Creating_element_With_Dictionary_Sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				InputImage element = new InputImage(hash);
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Attributes.Count == 4);
				Assert.That(element["Key1"] == "Val1");
			}
			[Test]
			public void When_Creating_element_With_Src_And_Dictionary_Sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				InputImage element = new InputImage("http://image.foo.jpg",hash);
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Attributes.Count == 5);
				Assert.That(element["Key1"] == "Val1");
				Assert.That(element.Src == "http://image.foo.jpg");
			}
		}
		
		[TestFixture]
		public class HiddenField_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				HiddenField element = new HiddenField();
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Type, Is.EqualTo("hidden"));
			}

			[Test]
			public void When_Creating_element_With_Dictionary_Sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				HiddenField element = new HiddenField(hash);
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Attributes.Count == 4);
				Assert.That(element["Key1"] == "Val1");
			}
		}
		
		[TestFixture]
		public class Form_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				Form element = new Form();
				Assert.That(element.Tag, Is.EqualTo("form"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				Form element = new Form();
				element.Action = "action sticks";
				Assert.That(element.Action, Is.EqualTo("action sticks"));
				element.OnSubmit = "OnSubmit sticks";
				Assert.That(element.OnSubmit, Is.EqualTo("OnSubmit sticks"));
				element.OnReset = "OnReset sticks";
				Assert.That(element.OnReset, Is.EqualTo("OnReset sticks"));
				Assert.That(element.Method, Is.EqualTo(Form.FORM_METHOD.GET));
				Assert.That(element["method"], Is.EqualTo(null));
				element.Method = Form.FORM_METHOD.POST;
				Assert.That(element.Method, Is.EqualTo(Form.FORM_METHOD.POST));
				Assert.That(element["method"], Is.EqualTo("post"));
				element.Method = Form.FORM_METHOD.GET;
				Assert.That(element.Method, Is.EqualTo(Form.FORM_METHOD.GET));
				Assert.That(element["method"], Is.EqualTo("get"));

				Assert.That(element.IsMultiPart == false);
				Assert.That(element["enctype"] == null);
				element.IsMultiPart = true;

				Assert.That(element.IsMultiPart == true);
				Assert.That(element["enctype"] == "multipart/form-data");

				element.IsMultiPart = false;
				Assert.That(element.IsMultiPart == false);
				Assert.That(element["enctype"] == null);
			}



			[Test]
			public void When_Creating_element_With_Action_Sticks()
			{
				Form element = new Form("http://aURL");
				Assert.That(element.Tag, Is.EqualTo("form"));
				Assert.That(element.Action == "http://aURL");
			}

			[Test]
			public void When_Creating_element_With_Action_And_Method_Sticks()
			{
				Form element = new Form("http://aURL", Form.FORM_METHOD.POST);
				Assert.That(element.Tag, Is.EqualTo("form"));
				Assert.That(element.Action == "http://aURL");
				Assert.That(element.Method == Form.FORM_METHOD.POST);
			}

			[Test]
			public void When_Creating_element_With_Action_Method_And_Dictionary_Sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				Form element = new Form("http://aURL", Form.FORM_METHOD.GET,hash);
				Assert.That(element.Tag, Is.EqualTo("form"));
				Assert.That(element.Attributes.Count == 5);
				Assert.That(element["Key1"] == "Val1");
			}
		}

		[TestFixture]
		public class CheckBoxField_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				CheckBoxField element = new CheckBoxField();
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Type, Is.EqualTo("checkbox"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				CheckBoxField element = new CheckBoxField();
				Assert.That(element.Checked == null);
				element.Checked = true;
				Assert.That(element.Checked == true);
				Assert.That(element["checked"] == "checked");
				element.Checked = null;
				Assert.That(element.Checked == null);

			}

			[Test]
			public void When_Creating_element_With_Dictionary_Sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				CheckBoxField element = new CheckBoxField(hash);
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Attributes.Count == 4);
				Assert.That(element["Key1"] == "Val1");
			}
		}
		
		[TestFixture]
		public class RadioField_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				RadioField element = new RadioField();
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Type, Is.EqualTo("radio"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				RadioField element = new RadioField();
				Assert.That(element.Checked == null);
				element.Checked = true;
				Assert.That(element.Checked == true);
				Assert.That(element["checked"] == "checked");
				element.Checked = null;
				Assert.That(element.Checked == null);

			}

			[Test]
			public void When_Creating_element_With_Dictionary_Sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				RadioField element = new RadioField(hash);
				Assert.That(element.Tag, Is.EqualTo("input"));
				Assert.That(element.Attributes.Count == 4);
				Assert.That(element["Key1"] == "Val1");
			}
		}
		
		
		[TestFixture]
		public class Option_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				Option element = new Option();
				Assert.That(element.Tag, Is.EqualTo("option"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				Option element = new Option();
				element.Value = "Value Sticks";
				Assert.That(element.Value == "Value Sticks");
			}

			[Test]
			public void Bool_Properties_Work_Right()
			{
				Option element = new Option();
				Assert.That(element.Disabled == false);
				Assert.That(element.Selected == false);
				Assert.That(element["disabled"] == null);
				Assert.That(element["selected"] == null);
				element.Disabled = true;
				element.Selected = true;
				Assert.That(element.Disabled);
				Assert.That(element.Selected);
				Assert.That(element["disabled"] == "disabled");
				Assert.That(element["selected"] == "selected");

				element["disabled"] = "manual edit bad";
				element["selected"] = "manual edit bad";

				Assert.That(element.Disabled == false);
				Assert.That(element.Selected == false);

				element.Disabled = false;
				element.Selected = false;
				Assert.That(element.Disabled == false);
				Assert.That(element.Selected == false);
				Assert.That(element["disabled"] == null);
				Assert.That(element["selected"] == null);
			}

			[Test]
			public void When_Creating_element_With_Href_It_Sticks()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				Option element = new Option(hash);
				Assert.That(element.Tag, Is.EqualTo("option"));
				Assert.That(element.Attributes.Count == 3);
				Assert.That(element["Key1"] == "Val1");
			}
		}

		[TestFixture]
		public class Select_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				Select element = new Select();
				Assert.That(element.Tag, Is.EqualTo("select"));
			}

			[Test]
			public void Create_Select_With_Options()
			{
				Select element = new Select();
				Option opt = new Option();
				opt.InnerText = "text";
				opt.Value = "value";
				element.Options.Add(opt);
				element.AddOption("value2", "text2");
				element.FirstOptionValue = "FirstValue";
				element.FirstOption = "FirstText";
				element.SelectedValue = "value2";
				Assert.That(element.ToString(), Is.EqualTo("<select ><option value=\"FirstValue\">FirstText</option><option value=\"value\">text</option><option value=\"value2\" selected=\"selected\">text2</option></select>"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				Select element = new Select();
				element.OnBlur = "OnBlur Sticks";
				Assert.That(element.OnBlur == "OnBlur Sticks");
				element.OnChange = "OnChange Sticks";
				Assert.That(element.OnChange == "OnChange Sticks");
				element.OnFocus = "OnFocus Sticks";
				Assert.That(element.OnFocus == "OnFocus Sticks");
				element.Name = "Name Sticks";
				Assert.That(element.Name == "Name Sticks");
				element.TextField = "TextField Sticks";
				Assert.That(element.TextField == "TextField Sticks");
				element.ValueField = "ValueField Sticks";
				Assert.That(element.ValueField == "ValueField Sticks");
			}

			[Test]
			public void Int_Properties_Work_Right()
			{
				Select element = new Select();
				Assert.That(element.Size == 0);
				Assert.That(element["maxlength"] == null);
				
				element["size"] = "A bad value";
				Assert.That(element.Size == 0);

				element.Size = 10;
				Assert.That(element.Size == 10);
				Assert.That(element["size"] == "10");

				element.Size = 0;
				Assert.That(element.Size == 0);
				Assert.That(element["size"] == null);
			}

			[Test]
			public void Bool_Properties_Work_Right()
			{
				Select element = new Select();
				Assert.That(element.Disabled == false);
				Assert.That(element.Multiple == false);
				Assert.That(element["disabled"] == null);
				Assert.That(element["multiple"] == null);
				element.Disabled = true;
				element.Multiple = true;
				Assert.That(element.Disabled);
				Assert.That(element.Multiple);
				Assert.That(element["disabled"] == "disabled");
				Assert.That(element["multiple"] == "multiple");

				element["disabled"] = "manual edit bad";
				element["multiple"] = "manual edit bad";

				Assert.That(element.Disabled == false);
				Assert.That(element.Multiple == false);

				element.Disabled = false;
				element.Multiple = false;
				Assert.That(element.Disabled == false);
				Assert.That(element.Multiple == false);
				Assert.That(element["disabled"] == null);
				Assert.That(element["multiple"] == null);
			}

		}
	}
}