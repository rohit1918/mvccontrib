using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcContrib;
using MvcContrib.UI;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MVCContrib.UnitTests.UI
{
	[TestFixture]
	public class ScriptableElementTester
	{
		[TestFixture]
		public class With_All_Properties
		{
			[Test]
			public void When_Set_UseInlineScripts_Then_Get_UseInlineScripts()
			{
				ScriptableElement element = new ScriptableElement();
				element.UseInlineScripts = true;
				Assert.That(element.UseInlineScripts, Is.EqualTo(true));
			}
			[Test]
			public void All_Properties_Stick_When_Set()
			{
				ScriptableElement element = new ScriptableElement();
				element.OnClick = "OnClick Is Sticky";
				Assert.That(element.OnClick, Is.EqualTo("OnClick Is Sticky"));
				element.OnDblClick = "OnDblClick Is Sticky";
				Assert.That(element.OnDblClick, Is.EqualTo("OnDblClick Is Sticky"));
				element.OnKeyDown = "OnKeyDown Is Sticky";
				Assert.That(element.OnKeyDown, Is.EqualTo("OnKeyDown Is Sticky"));
				element.OnKeyPress = "OnKeyPress Is Sticky";
				Assert.That(element.OnKeyPress, Is.EqualTo("OnKeyPress Is Sticky"));
				element.OnKeyUp = "OnKeyUp Is Sticky";
				Assert.That(element.OnKeyUp, Is.EqualTo("OnKeyUp Is Sticky"));
				element.OnMouseDown = "OnMouseDown Is Sticky";
				Assert.That(element.OnMouseDown, Is.EqualTo("OnMouseDown Is Sticky"));
				element.OnMouseMove = "OnMouseMove Is Sticky";
				Assert.That(element.OnMouseMove, Is.EqualTo("OnMouseMove Is Sticky"));
				element.OnMouseOut = "OnMouseOut Is Sticky";
				Assert.That(element.OnMouseOut, Is.EqualTo("OnMouseOut Is Sticky"));
				element.OnMouseOver = "OnMouseOver Is Sticky";
				Assert.That(element.OnMouseOver, Is.EqualTo("OnMouseOver Is Sticky"));
				element.OnMouseUp = "OnMouseUp Is Sticky";
				Assert.That(element.OnMouseUp, Is.EqualTo("OnMouseUp Is Sticky"));
			}

			[Test]
			public void Init_With_Tag_Name_Uses_Tag()
			{
				ScriptableElement element = new ScriptableElement("div");
				Assert.That(element.Tag == "div");
			}

			[Test]
			public void Init_With_Dictionary_Uses_Dictionary()
			{
				Hash hash = new Hash();
				hash.Add("Key1", "Val1");
				hash.Add("Key2", "Val2");
				hash.Add("Key3", "Val3");
				ScriptableElement element = new ScriptableElement("div", hash);
				Assert.That(element.Tag == "div");
				Assert.That(element.Attributes.Count == 3);
				Assert.That(element["Key1"] == "Val1");
			}
		}
	}
}
