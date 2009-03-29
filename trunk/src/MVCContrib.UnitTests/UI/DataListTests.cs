using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MvcContrib.UI.DataList;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.DataList
{
	[TestFixture]
	public class DataListTests
	{
		private IList<string> _datasource;
		private HttpContextBase _context;

		[SetUp]
		public void Setup()
		{
			_datasource = new List<string> {"test1", "test2"};
			_context = MvcMockHelpers.DynamicHttpContextBase();
		}

		private TextWriter Writer
		{
			get { return _context.Response.Output; }
		}

		[Test]
		public void Should_render_blank_table_when_DataSource_has_no_items()
		{
			//This might not be the expected behavior so

			var dl = new DataList<string>(new List<string>(), 3, RepeatDirection.Horizontal, _context.Response.Output);
			dl.CellTemplate(x => { Writer.Write(x.ToUpper()); }).ToString();

			Assert.AreEqual("<table><tr><td></td></tr></table>", Writer.ToString());
		}

		[Test]
		public void The_default_is_Vertical()
		{
			var dl = new DataList<string>(_datasource, _context.Response.Output);
			Assert.AreEqual(RepeatDirection.Vertical, dl.RepeatDirection);
		}

		[Test]
		public void The_default_is_0_columns()
		{
			var dl = new DataList<string>(_datasource, _context.Response.Output);
			Assert.AreEqual(0, dl.TimesToRepeatColumns);
		}

		[Test]
		public void When_TimesToRepeatColumns_is_0__it_should_render_all_items_in_the_repeat_direction()
		{
			var dl = new DataList<string>(_datasource, _context.Response.Output)
				.RepeatOn().Rows;
			dl.CellTemplate(x => { Writer.Write(x); }).ToString();

			Assert.AreEqual("<table><tr><td>test1</td></tr><tr><td>test2</td></tr></table>", Writer.ToString());
		}

		[Test]
		public void Dictionary_on_table_should_sticks()
		{
			var dl = new DataList<string>(_datasource, _context.Response.Output, new Hash(id => "foo", @class => "bar"));
			dl.CellTemplate(x => { Writer.Write(x); }).ToString();

			Assert.AreEqual("<table id=\"foo\" class=\"bar\"><tr><td>test1</td><td>test2</td></tr></table>", Writer.ToString());
		}

		[Test]
		public void Dictionary_on_table_should_sticks_even_with_empty_DataSource()
		{
			var dl = new DataList<string>(new List<string>(), _context.Response.Output, new Hash(id => "foo", @class => "bar"));
			dl.CellTemplate(x => { Writer.Write(x); }).ToString();

			Assert.AreEqual("<table id=\"foo\" class=\"bar\"><tr><td></td></tr></table>", Writer.ToString());
		}

		[Test]
		public void Can_render_content_horizontaly()
		{
			var dl = new DataList<string>(_datasource, 3, RepeatDirection.Horizontal, _context.Response.Output);
			dl.CellTemplate(x => { Writer.Write(x); }).ToString();

			Assert.AreEqual("<table><tr><td>test1</td><td>test2</td><td></td></tr></table>", Writer.ToString());
		}

		[Test]
		public void Can_render_content_verticaly()
		{
			var dl = new DataList<string>(_datasource, 3, RepeatDirection.Vertical, _context.Response.Output);
			dl.CellTemplate(x => { Writer.Write(x); }).ToString();

			Assert.AreEqual("<table><tr><td>test1</td></tr><tr><td>test2</td></tr><tr><td></td></tr></table>", Writer.ToString());
		}

		[Test]
		public void Should_set_blank_items_with_NoItemTemplate()
		{
			var dl = new DataList<string>(_datasource, 3, RepeatDirection.Horizontal, _context.Response.Output);
			dl.NoItemTemplate(() => { Writer.Write("No Data."); })
				.CellTemplate(x => { Writer.Write(x); }).ToString();

			Assert.AreEqual("<table><tr><td>test1</td><td>test2</td><td>No Data.</td></tr></table>", Writer.ToString());
		}

		[Test]
		public void When_DataSource_is_empty_then_EmptyDateSourceTemplate_should_be_rendered_in_first_cell()
		{
			var dl = new DataList<string>(new List<string>(), _context.Response.Output);
			dl.EmptyDateSourceTemplate(() => { Writer.Write("There is no data available."); }).ToString();

			Assert.AreEqual("<table><tr><td>There is no data available.</td></tr></table>", Writer.ToString());
		}

		[Test]
		public void When_setting_CellAttributes_the_sttributes_it_Sticks()
		{
			var dl = new DataList<string>(_datasource, 3, RepeatDirection.Vertical, _context.Response.Output);
			dl.CellTemplate(x => { Writer.Write(x.ToUpper()); }).CellAttributes(id => "foo", @class => "bar").ToString();

			Assert.AreEqual(
				"<table><tr><td id=\"foo\" class=\"bar\">TEST1</td></tr><tr><td id=\"foo\" class=\"bar\">TEST2</td></tr><tr><td id=\"foo\" class=\"bar\"></td></tr></table>",
				Writer.ToString());
		}

		[Test]
		public void Should_only_render_items_that_match_CellCondition()
		{
			var dl = new DataList<string>(_datasource, _context.Response.Output);
			dl.CellTemplate(x => { Writer.Write(x); }).CellCondition(x => x == "test1").ToString();

			Assert.AreEqual("<table><tr><td>test1</td></tr></table>", Writer.ToString());
		}

		[Test]
		public void Large_lists_render_correctly_Horizontaly()
		{
			var dl = new DataList<string>(
				new List<string> {"1", "2", "3", "4", "5", "6", "7", "8", "9"},
				3, RepeatDirection.Horizontal, _context.Response.Output);
			dl.CellTemplate(x => { Writer.Write(x); }).ToString();

			Assert.AreEqual(
				"<table><tr><td>1</td><td>2</td><td>3</td></tr><tr><td>4</td><td>5</td><td>6</td></tr><tr><td>7</td><td>8</td><td>9</td></tr></table>",
				Writer.ToString());
		}

		[Test]
		public void Large_lists_render_correctly_Verticaly()
		{
			var dl = new DataList<string>(
				new List<string> {"1", "2", "3", "4", "5", "6", "7", "8", "9"},
				3, RepeatDirection.Vertical, _context.Response.Output);
			dl.CellTemplate(x => { Writer.Write(x); }).ToString();

			Assert.AreEqual(
				"<table><tr><td>1</td><td>4</td><td>7</td></tr><tr><td>2</td><td>5</td><td>8</td></tr><tr><td>3</td><td>6</td><td>9</td></tr></table>",
				Writer.ToString());
		}

		[Test]
		public void Can_set_the_amount_of_columns_fluently_with_direction()
		{
			new DataList<string>(_datasource, Writer)
				.ItHas(3).Rows.CellTemplate(x => { Writer.Write(x); }).ToString();

			Assert.AreEqual("<table><tr><td>test1</td></tr><tr><td>test2</td></tr><tr><td></td></tr></table>", Writer.ToString());
		}

		[Test]
		public void Can_set_direction_without_setting_columns_fluently()
		{
			var dl = new DataList<string>(_datasource, Writer)
				.RepeatOn().Rows.CellTemplate(x => { Writer.Write(x); }).ToString();

			Assert.AreEqual("<table><tr><td>test1</td></tr><tr><td>test2</td></tr></table>", Writer.ToString());
		}

		[Test]
		public void When_calling_RepeatOn_with_Rows_RepeatDirection_is_Vertical()
		{
			var dl = new DataList<string>(_datasource, Writer)
				.RepeatOn().Rows;

			Assert.AreEqual(RepeatDirection.Vertical, dl.RepeatDirection);
		}

		[Test]
		public void When_calling_RepeatOn_with_Columns_RepeatDirection_is_Horizontal()
		{
			var dl = new DataList<string>(_datasource, Writer)
				.RepeatOn().Columns;

			Assert.AreEqual(RepeatDirection.Horizontal, dl.RepeatDirection);
		}

		[Test]
		public void When_calling_ItHas_with_Columns_RepeatDirection_is_Horizontal()
		{
			var dl = new DataList<string>(_datasource, Writer)
				.ItHas(5).Columns;

			Assert.AreEqual(RepeatDirection.Horizontal, dl.RepeatDirection);
		}

		[Test]
		public void When_calling_ItHas_with_Rows_RepeatDirection_is_Vertical()
		{
			var dl = new DataList<string>(_datasource, Writer)
				.ItHas(5).Rows;

			Assert.AreEqual(RepeatDirection.Vertical, dl.RepeatDirection);
		}

		[Test]
		public void When_calling_ItHas_amount_should_set_TimesToRepeatColumns()
		{
			var dl = new DataList<string>(_datasource, Writer)
				.ItHas(5).Columns;

			Assert.AreEqual(5, dl.TimesToRepeatColumns);
		}


		//This isn't part of the tests and can be deleted
		private void SampleUsage()
		{
			//You don't using Writer.Write() in your View you just add some html markup.

			var helper = new HtmlHelper(null, null);
			helper.DataList(_datasource)
				.ItHas(3).Rows
				.CellTemplate(x => { Writer.Write(x.ToLower()); })
				.CellCondition(x => x == "test1")
				.EmptyDateSourceTemplate(() => { Writer.Write("There is no data available."); })
				.NoItemTemplate(() => { Writer.Write("No Data."); });


			var dl = new DataList<string>(_datasource, 3, RepeatDirection.Vertical, Writer)
				.ItHas(3).Rows
				.CellTemplate(x => { Writer.Write(x.ToLower()); })
				.CellCondition(x => x == "test1")
				.EmptyDateSourceTemplate(() => { Writer.Write("There is no data available."); })
				.NoItemTemplate(() => { Writer.Write("No Data."); }).ToString();

			string s = Writer.ToString();
		}
	}
}