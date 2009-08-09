using System.Collections.Generic;
using HtmlAgilityPack;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using MvcContrib.UnitTests.FluentHtml.Helpers;
using NUnit.Framework;
using HtmlAttribute = MvcContrib.FluentHtml.Html.HtmlAttribute;

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
			var options = element.ShouldHaveChildNodesCount(4);

			VerifyOption("foo.Bar", items[0].Price, items[0].Title, options[0], options[1]);
			VerifyOption("foo.Bar", items[1].Price, items[1].Title, options[2], options[3]);
		}

		[Test]
		public void can_generate_radio_set_from_enum()
		{
			var html = new RadioSet("foo.Bar").Options<FakeEnum>().ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar");
			var options = element.ShouldHaveChildNodesCount(8);

			VerifyOption("foo.Bar", (int)FakeEnum.Zero, FakeEnum.Zero, options[0], options[1]);
			VerifyOption("foo.Bar", (int)FakeEnum.One, FakeEnum.One, options[2], options[3]);
			VerifyOption("foo.Bar", (int)FakeEnum.Two, FakeEnum.Two, options[4], options[5]);
			VerifyOption("foo.Bar", (int)FakeEnum.Three, FakeEnum.Three, options[6], options[7]);
		}

		[Test]
		public void radio_renders_selected_item_as_checked()
		{
			var html = new RadioSet("foo.Bar").Options<FakeEnum>().Selected(FakeEnum.Zero).ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar");
			var firstRadio = element.ShouldBeNamed(HtmlTag.Div).ShouldHaveChildNodesCount(8)[0];
			firstRadio.ShouldHaveAttribute(HtmlAttribute.Checked).WithValue(HtmlAttribute.Checked);
		}

		[Test]
		public void can_specify_option_format_for_radio_set()
		{
			var html = new RadioSet("foo.Bar").Options<FakeEnum>().ItemFormat("{0}<br/>").ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar");
			var nodes = element.ShouldHaveChildNodesCount(12);
			var brName = "br";
			nodes[2].ShouldBeNamed(brName);
			nodes[5].ShouldBeNamed(brName);
			nodes[8].ShouldBeNamed(brName);
			nodes[11].ShouldBeNamed(brName);
		}

		[Test]
		public void can_specify_item_class_for_radio_set()
		{
			var cssClass = "highClass";
			var html = new RadioSet("foo.Bar").Options<FakeEnum>().ItemClass(cssClass).ToString();
			var element = html.ShouldHaveHtmlNode("foo_Bar");
			var nodes = element.ShouldHaveChildNodesCount(8);
			foreach (var node in nodes)
			{
				node.ShouldHaveAttribute(HtmlAttribute.Class).WithValue(cssClass);
			}
		}

		[Test]
		public void can_use_html_for_radio_set_labels()
		{
			var items = new List<FakeModel> 
			{ 
				new FakeModel { Id = 1, Title = "<h1>One</h1>" },
				new FakeModel { Id = 2, Title = "Two" },
			};

			string html = new RadioSet("foo.Bar").Options(items, x => x.Id, x => x.Title)
				.Label("Header<br />")
				.DoNotHtmlEncodeLabels()
				.Selected(2)
				.ToString();

			html.ShouldContain("Header<br />");
			html.ShouldContain("<h1>One</h1>");
		}

		[Test]
		public void can_render_some_items_disabled_when_bound_to_enum()
		{
			var html = new RadioSet("foo.Bar").Options<FakeEnum>()
				.Disabled(new List<FakeEnum> { FakeEnum.One })
				.ToString();

			var element = html.ShouldHaveHtmlNode("foo_Bar");
			var options = element.ShouldHaveChildNodesCount(8);

			options[0].ShouldNotHaveAttribute(HtmlAttribute.Disabled);
			options[2].ShouldHaveAttribute(HtmlAttribute.Disabled).WithValue(HtmlAttribute.Disabled);
		}

		[Test]
		public void can_render_some_items_disabled()
		{
			var items = new List<FakeModel> 
			{ 
				new FakeModel { Id = 1, Title = "One" },
				new FakeModel { Id = 2, Title = "Two" },
			};

			var itemsDisabled = new List<int> { 1 };

			var html = new RadioSet("foo.Bar").Options(items, x => x.Id, x => x.Title).Selected(2)
				.Disabled(itemsDisabled).ToString();

			var element = html.ShouldHaveHtmlNode("foo_Bar");
			var nodes = element.ShouldHaveChildNodesCount(4);

			nodes[0].ShouldHaveAttribute(HtmlAttribute.Disabled).WithValue(HtmlAttribute.Disabled);
			nodes[2].ShouldNotHaveAttribute(HtmlAttribute.Disabled);
		}

		private void VerifyOption(string name, object value, object text, HtmlNode input, HtmlNode label)
		{
			input.ShouldBeNamed(HtmlTag.Input);
			input.ShouldHaveAttribute(HtmlAttribute.Name).WithValue(name);
			input.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.Radio);
			input.ShouldHaveAttribute(HtmlAttribute.Value).WithValue(value.ToString());

			label.ShouldBeNamed(HtmlTag.Label);
			label.ShouldHaveInnerTextEqual(text.ToString());
		}
	}
}