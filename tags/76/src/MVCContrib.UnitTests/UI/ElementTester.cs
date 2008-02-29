using System.Collections;
using System.Collections.Generic;
using MvcContrib.UI;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.UI
{
	[TestFixture]
	public class ElementTester
	{
		[TestFixture]
		public class With_All_Properties
		{
			[Test]
			public void When_Tag_Has_No_Value_Exception_Is_Thrown()
			{
				Element element = new Element();
				element.Tag = "";
				try
				{
					string val = element.ToString();
				}
				catch (System.Exception e)
				{
					Assert.That(e.Message, Is.EqualTo("tag must contain a value"));
				}
			}

			[Test]
			public void When_Created_Without_A_Tag_Then_The_Tag_Is_A_Div()
			{
				Element el = new Element();
				Assert.That(el.Tag, Is.EqualTo("div"));
			}

			[Test]
			public void Use_Full_Close_Tag_Is_False()
			{
				Element el = new Element("ul");
				Assert.That(el.UseFullCloseTag, Is.False);
			}

			[Test]
			public void When_Created_With_A_Tag_Then_That_Tag_Is_Got()
			{
				Element el = new Element("ul");
				Assert.That(el.Tag, Is.EqualTo("ul"));
			}

			[Test]
			public void When_Tag_Is_Set_Then_Tag_Is_Got()
			{
				Element el = new Element();
				el.Tag = "goose";
				Assert.That(el.Tag, Is.EqualTo("goose"));
			}

			[Test]
			public void When_Id_Is_Set_Then_Id_Is_Got()
			{
				Element el = new Element();
				Assert.That(el.Id, Is.Null);
				el.Id = "goose";
				Assert.That(el.Id, Is.EqualTo("goose"));
			}

			[Test]
			public void When_Class_Is_Set_Then_Class_Is_Got()
			{
				Element el = new Element();
				Assert.That(el.Class, Is.Null);
				el.Class = "goose";
				Assert.That(el.Class, Is.EqualTo("goose"));
			}

			[Test]
			public void When_OnClick_Is_Set_Then_OnClick_Is_Got()
			{
                ScriptableElement el = new ScriptableElement();
				Assert.That(el.OnClick, Is.Null);
				el.OnClick = "goose";
				Assert.That(el.OnClick, Is.EqualTo("goose"));
			}

			[Test]
			public void When_InnerHtml_Is_Set_Then_InnerHtml_Is_Got()
			{
				Element el = new Element();
				Assert.That(el.InnerText, Is.EqualTo(string.Empty));
				el.InnerText = "goose";
				Assert.That(el.InnerText, Is.EqualTo("goose"));
			}

			[Test]
			public void When_Using_This_Id_Is_Set_And_This_Id_Is_Got_Then_This_Id_Equals_Id_Property()
			{
				Element el = new Element();
				Assert.That(el.Id, Is.Null);
				el["id"] = "goose";
				Assert.That(el["id"], Is.EqualTo("goose"));
				Assert.That(el.Id, Is.EqualTo("goose"));
			}

			[Test]
			public void When_Id_And_Class_Are_Set_Then_Can_Enumerate_Over_Id_And_Class()
			{
				Element el = new Element();
				el.Id = "goose";
				el.Class = "chicken";
				bool sawId = false;
				bool sawClass = false;
				foreach (DictionaryEntry attribute in el)
				{
					if (attribute.Key.Equals("id") && attribute.Value.Equals("goose"))
					{
						sawId = true;
					}
					if (attribute.Key.Equals("class") && attribute.Value.Equals("chicken"))
					{
						sawClass = true;
					}
				}
				Assert.That(sawId, Is.True);
				Assert.That(sawClass, Is.True);
			}

			[Test]
			public void When_Id_And_Class_Are_Set_Then_Can_Enumerate_Over_Id_And_Class_NonGeneric()
			{
				Element el = new Element();
				el.Id = "goose";
				el.Class = "chicken";
				bool sawId = false;
				bool sawClass = false;
				foreach (object val in (IEnumerable)el)
				{
					DictionaryEntry attribute = (DictionaryEntry) val;
					if (attribute.Key.Equals("id") && attribute.Value.Equals("goose"))
					{
						sawId = true;
					}
					if (attribute.Key.Equals("class") && attribute.Value.Equals("chicken"))
					{
						sawClass = true;
					}
				}
				Assert.That(sawId, Is.True);
				Assert.That(sawClass, Is.True);
			}

			[Test]
			public void When_Getting_ToString_Then_Tag_Is_Generated()
			{
				Element el = new Element();
				Assert.That(el.ToString(), Text.Contains("<div />"));
			}

			[Test]
			public void When_Getting_ToString_With_Id_Then_Id_Attribute_Is_Generated()
			{
				Element el = new Element();
				el.Id = "goose";
				Assert.That(el.ToString(), Text.Contains("id=\"goose\""));
			}

            [Test]
            public void When_Getting_ToString_With_A_Empty_Attribute_Then_It_Is_Not_Generated()
            {
                Element el = new Element();
                el.Id = "goose";
                el["checked"] = "";
                Assert.That(el.ToString(), Text.DoesNotContain("checked"));
            }

			[Test]
			public void When_Set_Selector_Then_Get_Selector()
			{
				Element element = new Element();
				element.Selector = "#foo";
				Assert.That(element.Selector.ToString(), Is.EqualTo("#foo"));
			}

			[Test]
			public void When_Set_Id_Then_Get_Selector()
			{
				Element element = new Element();
				element.Id = "foo";
				Assert.That(element.Selector.ToString(), Is.EqualTo("#foo"));
			}

			[Test]
			public void When_Selector_Is_Null_Get_ID()
			{
				Element element = new Element();
				element.Id = "foo";
				element.Selector = null;
				Assert.That(element.Selector.ToString(), Is.EqualTo("foo"));
			}

			[Test]
			public void When_SelfClosing_Then_SelfCloses()
			{
				Element element = new Element();
				Assert.That(element.ToString(), Is.EqualTo("<div />"));
			}

			[Test]
			public void When_Not_SelfClosing_Then_TagHasEnd()
			{
				MvcContrib.UI.Tags.Script script = new MvcContrib.UI.Tags.Script();
				Assert.That(script.ToString(), Is.EqualTo("<script type=\"text/javascript\"></script>"));
			}
			
			[Test]
			public void Tag_Will_Close_With_Full_Tag_If_There_Is_InnerText()
			{
				Element element = new Element();
				element.InnerText = "This is Text";
				Assert.That(element.ToString(), Is.EqualTo("<div >This is Text</div>"));
			}

			[Test]
			public void Tag_With_Escaped_Flag_Has_Escaped_Content_When_Rendered()
			{
				Element element = new Element();
				element.InnerText = "This is Text";
				element.EscapeInnerText = true;
				Assert.That(element.ToString(), Is.EqualTo("<div >/*<![CDATA[*/\r\nThis is Text\r\n//]]></div>"));
			}

			[Test]
			public void Tag_With_Four_Or_More_Attributes_Renders_Correctly()
			{
				Element element = new Element();
				element.InnerText = "This is Text";
				element.Id = "MyID";
				element.Class = "MyClass";
				element["Style"] = "MyStyle";
				element["onclick"] = "MyOnClick";
				element["Gizmodo"] = "A cool Website";
				Assert.That(element.ToString(), Is.EqualTo("<div id=\"MyID\" class=\"MyClass\" Style=\"MyStyle\" onclick=\"MyOnClick\" Gizmodo=\"A cool Website\">This is Text</div>"));
			}

		}
	}
}