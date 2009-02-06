using System.Collections.Generic;
using HtmlAgilityPack;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using HtmlAttribute=MvcContrib.FluentHtml.Html.HtmlAttribute;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class RadioSetTests
	{
		[Test]
		public void radio_set_renders_in_a_div()
		{
			var html = new RadioSet("foo.Bar").ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar");
			element.ShouldBeNamed(HtmlTag.Div).ShouldHaveChildNodesCount(0);
		}

		[Test]
		public void can_generate_radio_set_from_model_choices()
		{
			var items = new List<FakeModel> 
			{ 
				new FakeModel { Price = 1, Title = "One" },
				new FakeModel { Price = 2, Title = "Two" },
			};
			var html = new RadioSet("foo.Bar").Options(items, x => x.Price, x => x.Title).ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar");
			var options = element.ShouldHaveChildNodesCount(2);

			VerifyOption("foo.Bar", items[0].Price, items[0].Title, options[0]);
			VerifyOption("foo.Bar", items[1].Price, items[1].Title, options[1]);
		}

		[Test]
		public void can_generate_radio_set_from_enum()
		{
			var html = new RadioSet("foo.Bar").Options<FakeEnum>().ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar");
			var options = element.ShouldHaveChildNodesCount(4);

			VerifyOption("foo.Bar", (int)FakeEnum.Zero, FakeEnum.Zero, options[0]);
			VerifyOption("foo.Bar", (int)FakeEnum.One, FakeEnum.One, options[1]);
			VerifyOption("foo.Bar", (int)FakeEnum.Two, FakeEnum.Two, options[2]);
			VerifyOption("foo.Bar", (int)FakeEnum.Three, FakeEnum.Three, options[3]);
		}

		[Test]
		public void radio_renders_selected_item_as_checked()
		{
			var html = new RadioSet("foo.Bar").Options<FakeEnum>().Selected(FakeEnum.Zero).ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar");
			var firstOption = element.ShouldBeNamed(HtmlTag.Div).ShouldHaveChildNodesCount(4)[0];
			var firstOptionRadioInput = firstOption.ShouldHaveChildNodesCount(2)[0];
			firstOptionRadioInput.ShouldHaveAttribute(HtmlAttribute.Checked).WithValue(HtmlAttribute.Checked);
		}

		private void VerifyOption(string name, object value, object text, HtmlNode option)
		{
			option.ShouldBeNamed(HtmlTag.Label);
			var optionNodes = option.ShouldHaveChildNodesCount(2);

			var radioInput = optionNodes[0];
			var labelText = optionNodes[1];

			radioInput.ShouldBeNamed(HtmlTag.Input);
			radioInput.ShouldHaveAttribute(HtmlAttribute.Name).WithValue(name);
			radioInput.ShouldHaveAttribute(HtmlAttribute.Value).WithValue(value.ToString());
			labelText.ShouldHaveInnerTextEqual(text.ToString());
		}
	}
}