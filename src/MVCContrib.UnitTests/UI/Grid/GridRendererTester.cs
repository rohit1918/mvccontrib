using System;
using System.Collections.Generic;
using System.IO;
using MvcContrib.UI.Grid;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.Grid
{
	[TestFixture]
	public class GridRendererTester
	{
		private List<Person> _people;
		private GridModel<Person> _model;

		[SetUp]
		public void Setup()
		{
			_model = new GridModel<Person>();
			_people = new List<Person> { new Person { Id = 1, Name = "Jeremy", DateOfBirth = new DateTime(1987, 4, 19) } };
		}


		[Test]
		public void Should_render_empty_table() {
			string expected = "<table class=\"grid\"><tr><td>There is no data available.</td></tr></table>";
			RenderGrid(null).ShouldEqual(expected);
		}

		[Test]
		public void Should_render_empty_table_when_collection_is_empty() {
			_people.Clear();
			string expected = "<table class=\"grid\"><tr><td>There is no data available.</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_empty_table_with_custom_message() {
			_people.Clear();
			string expected = "<table class=\"grid\"><tr><td>Test</td></tr></table>";
			_model.Empty("Test");
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Custom_html_attrs() {
			_people.Clear();
			string expected = "<table class=\"sortable grid\"><tr><td>There is no data available.</td></tr></table>";
			_model.Attributes(@class => "sortable grid");
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render() {
			_model.ColumnFor(x => x.Name);
			_model.ColumnFor(x => x.Id);
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_with_custom_Header_section() {
			/*
			_helper.Grid<Person>("people", column => { column.For(p => p.Name).Header(() => Writer.Write("<td>TEST</td>")); column.For(p => p.Id); });
			string expected = "<table class=\"grid\"><thead><tr><td>TEST</td><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
*/
			Assert.Fail();
		}

		[Test]
		public void Header_should_be_split_pascal_case() {
			_model.ColumnFor(x => x.DateOfBirth).Format("{0:dd}");
			string expected = "<table class=\"grid\"><thead><tr><th>Date Of Birth</th></tr></thead><tr class=\"gridrow\"><td>19</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}


		[Test]
		public void With_format() {
			_model.ColumnFor(x => x.DateOfBirth).Format("{0:ddd}");
			var dayString = string.Format("{0:ddd}", _people[0].DateOfBirth);
			string expected = "<table class=\"grid\"><thead><tr><th>Date Of Birth</th></tr></thead><tr class=\"gridrow\"><td>" + dayString + "</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Complicated_column() {
			_model.ColumnFor(x => x.Id + "-" + x.Name).Named("Test");
			string expected = "<table class=\"grid\"><thead><tr><th>Test</th></tr></thead><tr class=\"gridrow\"><td>1-Jeremy</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Column_heading_should_be_empty() {
			_model.ColumnFor(x => x.Id + "-" + x.Name);
			string expected = "<table class=\"grid\"><thead><tr><th></th></tr></thead><tr class=\"gridrow\"><td>1-Jeremy</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Custom_item_section() {
		/*	_helper.Grid<Person>("people", column => column.For("Name").Do(s => Writer.Write("<td>Test</td>")));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Test</td></tr></table>";
			RenderGrid().ShouldEqual(expected);*/
			Assert.Fail();
		}

		[Test]
		public void With_cell_condition() {
			_model.ColumnFor(x => x.Name);
			_model.ColumnFor(x => x.Id).CellCondition(x => false);
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td></td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void With_col_condition() {
			_model.ColumnFor(x => x.Name);
			_model.ColumnFor(x => x.Id).Visible(false);
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td></tr></table>";
			RenderGrid().ShouldEqual(expected);

		}

		[Test]
		public void When_a_custom_renderer_is_specified_then_column_condition_should_still_be_checked() {
			/*_helper.Grid<Person>("people", column => column.For("Custom").Do(x => Writer.Write("<td>Foo</td>")).ColumnCondition(() => false));
			string expected = "<table class=\"grid\"><thead><tr></tr></thead><tr class=\"gridrow\"></tr></table>";
			RenderGrid().ShouldEqual(expected);*/
			Assert.Fail();
		}

		[Test]
		public void Should_render_with_strongly_typed_data_and_custom_sections() {
		/*	_helper.Grid(new List<Person> { new Person { Id = 1 } }, column => column.For(p => p.Id), sections => sections.RowStart(p => Writer.Write("<tr foo=\"bar\">")));
			string expected = "<table class=\"grid\"><thead><tr><th>Id</th></tr></thead><tr foo=\"bar\"><td>1</td></tr></table>";
			RenderGrid().ShouldEqual(expected);*/
			Assert.Fail();
		}


		[Test]
		public void Should_render_custom_row_end() {
			/*	_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id); }, sections => sections.RowEnd(person => Writer.Write("</tr>TEST")));
				string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr>TEST</table>";
				RenderGrid().ShouldEqual(expected);
			 */
			Assert.Fail();
		}

		[Test]
		public void Should_render_custom_row_start() {
			/*_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id); }, sections => sections.RowStart(p => Writer.Write("<tr class=\"row\">")));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"row\"><td>Jeremy</td><td>1</td></tr></table>";
			RenderGrid().ShouldEqual(expected);*/
			Assert.Fail();
		}

		[Test]
		public void Alternating_rows_should_have_correct_css_class() {
			_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			_model.ColumnFor(x => x.Name);
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td></tr><tr class=\"gridrow_alternate\"><td>Person 2</td></tr><tr class=\"gridrow\"><td>Person 3</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_custom_row_start_with_alternate_row() {
		/*	_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); }, sections => sections.RowStart((p, isAlternate) => Writer.Write("<tr class=\"row " + (isAlternate ? "gridrow_alternate" : "gridrow") + "\">")));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"row gridrow\"><td>Jeremy</td></tr><tr class=\"row gridrow_alternate\"><td>Person 2</td></tr><tr class=\"row gridrow\"><td>Person 3</td></tr></table>";
			RenderGrid().ShouldEqual(expected);*/
			Assert.Fail();
		}

		[Test]
		public void Should_render_header_attributes() {
			/*_model.ColumnFor(x => x.Name).HeaderAttributes(style => "width:100%");
			_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			//_helper.Grid<Person>("people", column => column.For(p => p.Name).HeaderAttributes(new Hash(style => "width:100%")), sections => sections.RowStart((p, isAlternate) => Writer.Write("<tr class=\"row " + (isAlternate ? "gridrow_alternate" : "gridrow") + "\">")));
			string expected = "<table class=\"grid\"><thead><tr><th style=\"width:100%\">Name</th></tr></thead><tr class=\"row gridrow\"><td>Jeremy</td></tr><tr class=\"row gridrow_alternate\"><td>Person 2</td></tr><tr class=\"row gridrow\"><td>Person 3</td></tr></table>";
			RenderGrid().ShouldEqual(expected);*/
			Assert.Fail();
		}
		private string RenderGrid() {
			return RenderGrid(_people);
		}

		private string RenderGrid(IEnumerable<Person> dataSource) {
			var renderer = new HtmlTableGridRenderer<Person>();
			var writer = new StringWriter();
			renderer.Render(_model, dataSource, writer);
			return writer.ToString();
		}
	}
}