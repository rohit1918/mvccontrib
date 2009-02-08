using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.UI.Grid;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI.Grid
{
	[TestFixture]
	public class GridRendererTester
	{
		private List<Person> _people;
		private GridModel<Person> _model;
		private IViewEngine _viewEngine;

		[SetUp]
		public void Setup()
		{
			_model = new GridModel<Person>();
			_people = new List<Person> {new Person {Id = 1, Name = "Jeremy", DateOfBirth = new DateTime(1987, 4, 19)}};
			_viewEngine = MockRepository.GenerateMock<IViewEngine>();

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(_viewEngine);
		}

		private IGridColumn<Person> ColumnFor(Expression<Func<Person, object>> expression)
		{
			return _model.Column.For(expression);
		}

		[TearDown]
		public void Teardown()
		{
			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new WebFormViewEngine());
		}

		[Test]
		public void Should_render_empty_table()
		{
			string expected = "<table class=\"grid\"><tr><td>There is no data available.</td></tr></table>";
			RenderGrid(null).ShouldEqual(expected);
		}

		[Test]
		public void Should_render_empty_table_when_collection_is_empty()
		{
			_people.Clear();
			string expected = "<table class=\"grid\"><tr><td>There is no data available.</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_empty_table_with_custom_message()
		{
			_people.Clear();
			string expected = "<table class=\"grid\"><tr><td>Test</td></tr></table>";
			_model.Empty("Test");
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Custom_html_attrs()
		{
			_people.Clear();
			string expected = "<table class=\"sortable grid\"><tr><td>There is no data available.</td></tr></table>";
			_model.Attributes(@class => "sortable grid");
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render()
		{
			ColumnFor(x => x.Name);
			ColumnFor(x => x.Id);
			string expected =
				"<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_with_custom_Header_format()
		{
			ColumnFor(x => x.Name).Header("<td>TEST</td>");
			ColumnFor(x => x.Id);
			string expected = "<table class=\"grid\"><thead><tr><td>TEST</td><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_with_custom_header_partial()
		{
			SetupViewEngine("testPartial", "<td>TEST</td>");
			ColumnFor(x => x.Name).HeaderPartial("testPartial");
			ColumnFor(x => x.Id);
			string expected = "<table class=\"grid\"><thead><tr><td>TEST</td><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr></table>";
			RenderGrid().ShouldEqual(expected);

		}


		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void Should_throw_when_view_cannot_be_found()
		{
			_viewEngine.Expect(x => x.FindPartialView(Arg<ControllerContext>.Is.Anything, Arg<string>.Is.Equal("foo"), Arg<bool>.Is.Anything))
				.IgnoreArguments()
				.Return(new ViewEngineResult(new[] { "foo", "bar" }))
				.Repeat.Any();

			ColumnFor(x => x.Name).HeaderPartial("foo");
			RenderGrid();
		}

		[Test]
		public void Header_should_be_split_pascal_case()
		{
			ColumnFor(x => x.DateOfBirth).Format("{0:dd}");
			string expected =
				"<table class=\"grid\"><thead><tr><th>Date Of Birth</th></tr></thead><tr class=\"gridrow\"><td>19</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}


		[Test]
		public void With_format()
		{
			ColumnFor(x => x.DateOfBirth).Format("{0:ddd}");
			var dayString = string.Format("{0:ddd}", _people[0].DateOfBirth);
			string expected = "<table class=\"grid\"><thead><tr><th>Date Of Birth</th></tr></thead><tr class=\"gridrow\"><td>" +
			                  dayString + "</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Complicated_column()
		{
			ColumnFor(x => x.Id + "-" + x.Name).Named("Test");
			string expected =
				"<table class=\"grid\"><thead><tr><th>Test</th></tr></thead><tr class=\"gridrow\"><td>1-Jeremy</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Column_heading_should_be_empty()
		{
			ColumnFor(x => x.Id + "-" + x.Name);
			string expected =
				"<table class=\"grid\"><thead><tr><th></th></tr></thead><tr class=\"gridrow\"><td>1-Jeremy</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Column_should_be_rendered_using_custom_partial()
		{
			SetupViewEngine("Foo", (v, w) => {
				var model = ((Person)v.ViewData.Model);
				w.Write("<td>" + model.Name + "_TEST</td>");
			});

			ColumnFor(x => x.Name).Partial("Foo");
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy_TEST</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Custom_column_should_use_partial_with_same_name_as_column()
		{
			SetupViewEngine("Name", "<td>Foo</td>");
			_model.Column.For("Name");
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Foo</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Custom_column_with_custom_partial()
		{
			SetupViewEngine("Foo", "<td>Foo</td>");
			_model.Column.For("Name").Partial("Foo");
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Foo</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}


		[Test]
		public void With_cell_condition()
		{
			ColumnFor(x => x.Name);
			ColumnFor(x => x.Id).CellCondition(x => false);
			string expected =
				"<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td></td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void With_col_condition()
		{
			ColumnFor(x => x.Name);
			ColumnFor(x => x.Id).Visible(false);
			string expected =
				"<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void When_a_custom_renderer_is_specified_then_column_condition_should_still_be_checked()
		{
			/*_helper.Grid<Person>("people", column => column.For("Custom").Do(x => Writer.Write("<td>Foo</td>")).ColumnCondition(() => false));
			string expected = "<table class=\"grid\"><thead><tr></tr></thead><tr class=\"gridrow\"></tr></table>";
			RenderGrid().ShouldEqual(expected);*/
			Assert.Fail();
		}

		[Test]
		public void Should_render_with_strongly_typed_data_and_custom_sections()
		{
			/*	_helper.Grid(new List<Person> { new Person { Id = 1 } }, column => column.For(p => p.Id), sections => sections.RowStart(p => Writer.Write("<tr foo=\"bar\">")));
			string expected = "<table class=\"grid\"><thead><tr><th>Id</th></tr></thead><tr foo=\"bar\"><td>1</td></tr></table>";
			RenderGrid().ShouldEqual(expected);*/
			Assert.Fail();
		}


		[Test]
		public void Should_render_custom_row_end()
		{
			/*	_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id); }, sections => sections.RowEnd(person => Writer.Write("</tr>TEST")));
				string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr>TEST</table>";
				RenderGrid().ShouldEqual(expected);
			 */
			Assert.Fail();
		}

		[Test]
		public void Should_render_custom_row_start()
		{
			/*_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id); }, sections => sections.RowStart(p => Writer.Write("<tr class=\"row\">")));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"row\"><td>Jeremy</td><td>1</td></tr></table>";
			RenderGrid().ShouldEqual(expected);*/
			Assert.Fail();
		}

		[Test]
		public void Alternating_rows_should_have_correct_css_class()
		{
			_people.Add(new Person {Name = "Person 2"});
			_people.Add(new Person {Name = "Person 3"});
			ColumnFor(x => x.Name);
			string expected =
				"<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td></tr><tr class=\"gridrow_alternate\"><td>Person 2</td></tr><tr class=\"gridrow\"><td>Person 3</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}

		[Test]
		public void Should_render_custom_row_start_with_alternate_row()
		{
			/*	_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); }, sections => sections.RowStart((p, isAlternate) => Writer.Write("<tr class=\"row " + (isAlternate ? "gridrow_alternate" : "gridrow") + "\">")));
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"row gridrow\"><td>Jeremy</td></tr><tr class=\"row gridrow_alternate\"><td>Person 2</td></tr><tr class=\"row gridrow\"><td>Person 3</td></tr></table>";
			RenderGrid().ShouldEqual(expected);*/
			Assert.Fail();
		}

		[Test]
		public void Should_render_header_attributes()
		{
			ColumnFor(x => x.Name).HeaderAttributes(style => "width:100%");
			string expected = "<table class=\"grid\"><thead><tr><th style=\"width:100%\">Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td></tr></table>";
			RenderGrid().ShouldEqual(expected);
		}
		
		[Test]
		public void Should_render_header_attributes_when_rendering_custom_row_start()
		{
			/*ColumnFor(x => x.Name).HeaderAttributes(style => "width:100%");
			_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			//_helper.Grid<Person>("people", column => column.For(p => p.Name).HeaderAttributes(new Hash(style => "width:100%")), sections => sections.RowStart((p, isAlternate) => Writer.Write("<tr class=\"row " + (isAlternate ? "gridrow_alternate" : "gridrow") + "\">")));
			string expected = "<table class=\"grid\"><thead><tr><th style=\"width:100%\">Name</th></tr></thead><tr class=\"row gridrow\"><td>Jeremy</td></tr><tr class=\"row gridrow_alternate\"><td>Person 2</td></tr><tr class=\"row gridrow\"><td>Person 3</td></tr></table>";
			RenderGrid().ShouldEqual(expected);*/
			Assert.Fail();
		}

		private string RenderGrid()
		{
			return RenderGrid(_people);
		}

		private string RenderGrid(IEnumerable<Person> dataSource)
		{
			var renderer = new HtmlTableGridRenderer<Person>();
			var writer = new StringWriter();
			renderer.Render(_model, dataSource, writer, new ViewContext() { View = MockRepository.GenerateStub<IView>(), TempData = new TempDataDictionary()});
			return writer.ToString();
		}

		private void SetupViewEngine(string viewName, string viewContents)
		{
			SetupViewEngine(viewName, (v, w) => w.Write(viewContents));
		}

		private void SetupViewEngine(string viewName, Action<ViewContext, TextWriter> action)
		{
			var view = MockRepository.GenerateMock<IView>();
			_viewEngine.Expect(x => x.FindPartialView(Arg<ControllerContext>.Is.Anything, Arg<string>.Is.Equal(viewName), Arg<bool>.Is.Anything)).Return(new ViewEngineResult(view, _viewEngine));

			view.Expect(x => x.Render(null, null)).IgnoreArguments()
				.Do(action);
		}
	}
}