using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UI.Html.Grid.Legacy;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using System.Collections.Generic;
using MvcContrib.UI.Html.Grid;
using MvcContrib.Pagination;

namespace MvcContrib.UnitTests.UI.Html.Grid
{
	[TestFixture]
	public class HtmlGridTester
	{
		private class Person
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public DateTime DateOfBirth { get; set; }
		}

		private MockRepository _mocks;
		private ViewContext _context;
		private List<Person> _people;
		private GridColumnBuilder<Person> _columnBuilder;

		[SetUp]
		public void Setup()
		{
			_columnBuilder = new GridColumnBuilder<Person>();
			_mocks = new MockRepository();
			_context = new ViewContext(_mocks.DynamicHttpContextBase(), new RouteData(), MockRepository.GenerateStub<ControllerBase>(), MockRepository.GenerateStub<IView>(), new ViewDataDictionary(),new TempDataDictionary() );
			SetupResult.For(_context.HttpContext.Request.FilePath).Return("Test.mvc");
			_people = new List<Person>
			          	{
			          		new Person { Id = 1, Name = "Jeremy", DateOfBirth = new DateTime(1987, 4, 19)}
			          	};
			_mocks.ReplayAll();

		}

		private GridColumnBuilder<Person> Column
		{
			get { return _columnBuilder; }
		}

		private TextWriter Writer
		{
			get { return _context.HttpContext.Response.Output; }
		}

		private void RenderGrid()
		{
			RenderGrid(Hash.Empty);
		}
		
		private void RenderGrid(IDictionary htmlAttributes)
		{
			var grid = new Grid<Person>();
			grid.Render(_people, _columnBuilder, new GridOptions().LoadFromDictionary(htmlAttributes),  htmlAttributes, _context);
		}


		[Test]
		public void Should_render_empty_table()
		{
			string expected = "<table class=\"grid\"><tr><td>There is no data available.</td></tr></table>";
			var grid = new Grid<Person>();
			
			grid.Render(null, null, null, null, _context);

			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_empty_table_when_collection_is_empty()
		{
			_people.Clear();
			Column.For(p => p.Name); 
			Column.For(p => p.Id);

			string expected = "<table class=\"grid\"><tr><td>There is no data available.</td></tr></table>";
			RenderGrid();
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Custom_html_attrs()
		{
			_people.Clear();
			string expected = "<table class=\"sortable grid\"><tr><td>There is no data available.</td></tr></table>";
			RenderGrid(new Hash(@class => "sortable grid"));
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render()
		{
			Column.For(p => p.Name);
			Column.For(p => p.Id);
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr></table>";
			RenderGrid();
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_with_custom_Header_section()
		{
			Column.For(p => p.Name).Header(() => Writer.Write("<td>TEST</td>")); Column.For(p => p.Id);
			RenderGrid();
			string expected = "<table class=\"grid\"><thead><tr><td>TEST</td><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));

		}

		[Test]
		public void Header_should_be_split_pascal_case()
		{
			Column.For(p => p.DateOfBirth).Formatted("{0:dd}");
			RenderGrid();
			string expected = "<table class=\"grid\"><thead><tr><th>Date Of Birth</th></tr></thead><tr class=\"gridrow\"><td>19</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void With_format()
		{
			Column.For(p => p.DateOfBirth).Formatted("{0:ddd}");
			RenderGrid();
			var dayString = string.Format("{0:ddd}", _people[0].DateOfBirth);
			string expected = "<table class=\"grid\"><thead><tr><th>Date Of Birth</th></tr></thead><tr class=\"gridrow\"><td>" + dayString + "</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Complicated_column()
		{
			Column.For(p => p.Id + "-" + p.Name, "Test");
			RenderGrid();
			string expected = "<table class=\"grid\"><thead><tr><th>Test</th></tr></thead><tr class=\"gridrow\"><td>1-Jeremy</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Column_heading_should_be_empty()
		{
			Column.For(p => p.Id + "-" + p.Name);
			RenderGrid();
			string expected = "<table class=\"grid\"><thead><tr><th></th></tr></thead><tr class=\"gridrow\"><td>1-Jeremy</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Custom_item_section()
		{
			Column.For("Name").Do(s => Writer.Write("<td>Test</td>"));
			RenderGrid();
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Test</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void With_anonymous_type()
		{
			var column = new GridColumnBuilder<object>();
			column.For("Name");

			var grid = new Grid<object>();
			grid.Render(new object[] {new {Name = "Testing"}}, column, null, null, _context);

			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Testing</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void With_cell_condition()
		{
			Column.For(p => p.Name);
			Column.For(p => p.Id).CellCondition(p => false);
			RenderGrid();
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td></td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void With_col_condition()
		{
			Column.For(p => p.Name);
			Column.For(p => p.Id).ColumnCondition(() => false);
			RenderGrid();
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));

		}

		[Test]
		public void When_a_custom_renderer_is_specified_then_column_condition_should_still_be_checked()
		{
			Column.For("Custom").Do(x => Writer.Write("<td>Foo</td>")).ColumnCondition(() => false);
			RenderGrid();
			string expected = "<table class=\"grid\"><thead><tr></tr></thead><tr class=\"gridrow\"></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_encode()
		{
			_people.Clear();
			_people.Add(new Person() { Name = "Jeremy&"});
			Column.For(p => p.Name);
			RenderGrid();
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy&amp;</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_not_encode()
		{
			_people.Clear();
			_people.Add(new Person{Name = "Jeremy&"});
			Column.For(p => p.Name).DoNotEncode();
			RenderGrid();
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy&</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		
		[Test]
		public void Should_render_custom_row_end()
		{
			Column.For(p => p.Name); 
			Column.For(p => p.Id);
			Column.RowEnd(p => Writer.Write("</tr>TEST"));

			RenderGrid();
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td><td>1</td></tr>TEST</table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_custom_row_start()
		{
			Column.For(p => p.Name); Column.For(p => p.Id);
			Column.RowStart(p => Writer.Write("<tr class=\"row\">"));
			RenderGrid();
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"row\"><td>Jeremy</td><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_with_pagination_last_and_next()
		{
			_people.Add(new Person { Name = "Person2" });
			_people.Add(new Person { Name = "Person 3" });

			var pagedPeople = _people.AsPagination(1, 2);
			var grid = new Grid<Person>();
			grid.Render(pagedPeople, Column, null, Hash.Empty, _context);

			string expected = "</table><div class='pagination'><span class='paginationLeft'>Showing 1 - 2 of 3 </span><span class='paginationRight'>first | prev | <a href=\"Test.mvc?page=2\">next</a> | <a href=\"Test.mvc?page=2\">last</a></span></div>";
			
			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_with_pagination_first_and_previous()
		{
			_people.Add(new Person { Name = "Person2" });
			_people.Add(new Person { Name = "Person 3" });
			var pagedPeople = _people.AsPagination(2, 2);
			var grid = new Grid<Person>();
			Column.For(p => p.Name);
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Showing 3 - 3 of 3 </span><span class='paginationRight'><a href=\"Test.mvc?page=1\">first</a> | <a href=\"Test.mvc?page=1\">prev</a> | next | last</span></div>";

			grid.Render(pagedPeople, Column, null, null, _context);

			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_pagination_with_querystring()
		{
			_people.Add(new Person { Name = "Person2" });
			_people.Add(new Person { Name = "Person 3" });
			_context.HttpContext.Request.QueryString.Add("a", "b");
			var pagedPeople = _people.AsPagination(2, 2);
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Showing 3 - 3 of 3 </span><span class='paginationRight'><a href=\"Test.mvc?page=1&amp;a=b\">first</a> | <a href=\"Test.mvc?page=1&amp;a=b\">prev</a> | next | last</span></div>";
			Column.For(x => x.Name);
			var grid = new Grid<Person>();
			grid.Render(pagedPeople, Column, null,null,_context);
			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_pagination_with_different_message_if_pagesize_is_1()
		{
			_people.Add(new Person { Name = "Person2" });
			_people.Add(new Person { Name = "Person 3" });
			Column.For(p => p.Name);
			var pagedPeople = _people.AsPagination(1, 1);
			var grid = new Grid<Person>();
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Showing 1 of 3 </span><span class='paginationRight'>first | prev | <a href=\"Test.mvc?page=2\">next</a> | <a href=\"Test.mvc?page=3\">last</a></span></div>";
			grid.Render(pagedPeople, Column, null,null, _context);
			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Alternating_rows_should_have_correct_css_class()
		{
			_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			Column.For(p => p.Name);
			RenderGrid();
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td></tr><tr class=\"gridrow_alternate\"><td>Person 2</td></tr><tr class=\"gridrow\"><td>Person 3</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_not_render_pagination_when_datasource_is_empty()
		{
			var people = new List<Person>().AsPagination(1);
			var grid = new Grid<Person>();
			Column.For(x => x.Name);
			grid.Render(people, Column, null,null, _context);
			string expected = "<table class=\"grid\"><tr><td>There is no data available.</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_localized_pagination()
		{
			_people.Add(new Person { Name = "Person2" });
			_people.Add(new Person { Name = "Person 3" });
			var pagedPeople = _people.AsPagination(1, 2);
			var grid = new Grid<Person>();
			grid.Options.PaginationFormat = "Visar {0} - {1} av {2} ";
			grid.Options.PaginationFirst = "första";
			grid.Options.PaginationPrev = "föregående";
			grid.Options.PaginationNext = "nästa";
			grid.Options.PaginationLast = "sista";
			Column.For(p => p.Name);
			grid.Render(pagedPeople, Column,null,null, _context);

			string expected = "</table><div class='pagination'><span class='paginationLeft'>Visar 1 - 2 av 3 </span><span class='paginationRight'>första | föregående | <a href=\"Test.mvc?page=2\">nästa</a> | <a href=\"Test.mvc?page=2\">sista</a></span></div>";
			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_custom_row_start_with_alternate_row()
		{
			_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			Column.For(p => p.Name);
			Column.RowStart((p, isAlternate) => Writer.Write("<tr class=\"row " + (isAlternate ? "gridrow_alternate" : "gridrow") + "\">"));
			RenderGrid();

			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"row gridrow\"><td>Jeremy</td></tr><tr class=\"row gridrow_alternate\"><td>Person 2</td></tr><tr class=\"row gridrow\"><td>Person 3</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}


		[Test]
		public void Should_render_localized_pagination_with_different_message_if_pagesize_is_1()
		{
			_people.Add(new Person { Name = "Person2" });
			_people.Add(new Person { Name = "Person 3" });
			Column.For(x => x.Name);
			var pagedPeople = _people.AsPagination(1, 1);
			var grid = new Grid<Person>();
			grid.Options.PaginationSingleFormat = "Visar {0} av {1} ";
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Visar 1 av 3 </span><span class='paginationRight'>first | prev | <a href=\"Test.mvc?page=2\">next</a> | <a href=\"Test.mvc?page=3\">last</a></span></div>";
			grid.Render(pagedPeople, Column, null,null, _context);

			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_pagination_with_custom_page_name()
		{
			_people.Add(new Person { Name = "Person2" });
			_people.Add(new Person { Name = "Person 3" });
			var pagedPeople = _people.AsPagination(1, 2);
			Column.For(p => p.Name);
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Showing 1 - 2 of 3 </span><span class='paginationRight'>first | prev | <a href=\"Test.mvc?my_page=2\">next</a> | <a href=\"Test.mvc?my_page=2\">last</a></span></div>";
			var grid = new Grid<Person>();
			grid.Options.PageQueryName = "my_page";
			grid.Render(pagedPeople, Column, null,null, _context);
			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_header_attributes()
		{
			_people.Add(new Person { Name = "Person 2" });
			_people.Add(new Person { Name = "Person 3" });
			Column.For(p => p.Name).HeaderAttributes(new Hash(style=>"width:100%"));
			Column.RowStart((p, isAlternate) => Writer.Write("<tr class=\"row " + (isAlternate ? "gridrow_alternate" : "gridrow") + "\">"));

			RenderGrid();
			string expected = "<table class=\"grid\"><thead><tr><th style=\"width:100%\">Name</th></tr></thead><tr class=\"row gridrow\"><td>Jeremy</td></tr><tr class=\"row gridrow_alternate\"><td>Person 2</td></tr><tr class=\"row gridrow\"><td>Person 3</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

	}
}