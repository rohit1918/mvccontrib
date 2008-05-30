using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using System.Collections.Generic;
using MvcContrib.UI.Html.Grid;
using MvcContrib.Pagination;
namespace MvcContrib.UnitTests.UI.Html
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
		private HttpContextBase _context;
		private HtmlHelper _helper;
		private ViewContext _viewContext;
		private List<Person> _people;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_context = _mocks.DynamicHttpContextBase();
			SetupResult.For(_context.Request.FilePath).Return("Test.mvc");
			_viewContext = new ViewContext(_context, new RouteData(), _mocks.DynamicMock<IController>(), "index", "", new ViewDataDictionary(), null);
			_helper = new HtmlHelper(_viewContext, new ViewPage());
			_people = new List<Person>
			              	{
			              		new Person() { Id = 1, Name = "Jeremy", DateOfBirth = new DateTime(1987, 4, 19)}
			              	};
			AddToViewData("people", _people);
			_mocks.ReplayAll();

		}
		private TextWriter Writer
		{
			get { return _context.Response.Output; }
		}


		private void AddToViewData(string key, Object value)
		{
			_viewContext.ViewData.Add(key, value);
		}

		[Test]
		public void Should_render_empty_table()
		{
			string expected = "<table class=\"grid\"><tr><td>There is no data available.</td></tr></table>";
			Grid<Person> grid = new Grid<Person>(null, null, null, Writer, null);
			grid.Render();

			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_empty_table_when_collection_is_empty()
		{
			_people.Clear();
			string expected = "<table class=\"grid\"><tr><td>There is no data available.</td></tr></table>";
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id); });

			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_empty_table_with_custom_message()
		{
			string expected = "<table class=\"grid\"><tr><td>Test</td></tr></table>";
			_helper.Grid<Person>((string)null, new Hash(empty => "Test"), column => { column.For(p => p.Name); column.For(p => p.Id); });
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Custom_html_attrs()
		{
			string expected = "<table class=\"sortable grid\"><tr><td>There is no data available.</td></tr></table>";
			Grid<Person> grid = new Grid<Person>(null, null, new Hash(@class => "sortable grid"), Writer, null);
			grid.Render();

			Assert.That(Writer.ToString(), Is.EqualTo(expected));

		}

		[Test]
		public void Should_render()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id); });
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr><td>Jeremy</td><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_with_custom_Header_section()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.Name).Header(() => Writer.Write("<td>TEST</td>")); column.For(p => p.Id); });
			string expected = "<table class=\"grid\"><thead><tr><td>TEST</td><th>Id</th></tr></thead><tr><td>Jeremy</td><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));

		}

		[Test]
		public void Header_should_be_split_pascal_case()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.DateOfBirth).Formatted("{0:dd}"); });

			string expected = "<table class=\"grid\"><thead><tr><th>Date Of Birth</th></tr></thead><tr><td>19</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}


		[Test]
		public void With_format()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.DateOfBirth).Formatted("{0:ddd}"); });

			string expected = "<table class=\"grid\"><thead><tr><th>Date Of Birth</th></tr></thead><tr><td>Sun</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Complicated_column()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.Id.ToString() + "-" + p.Name, "Test"); });
			string expected = "<table class=\"grid\"><thead><tr><th>Test</th></tr></thead><tr><td>1-Jeremy</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Column_heading_should_be_empty()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.Id.ToString() + "-" + p.Name); });
			string expected = "<table class=\"grid\"><thead><tr><th></th></tr></thead><tr><td>1-Jeremy</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Custom_item_section()
		{
			_helper.Grid<Person>("people", column => { column.For("Name").Do(s => Writer.Write("<td>Test</td>")); });
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr><td>Test</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void With_anonymous_type()
		{
			AddToViewData("test", new ArrayList { new { Name = "Testing" } });
			_helper.Grid<object>("test", column => { column.For("Name"); });
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr><td>Testing</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void With_cell_condition()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id).CellCondition(p => false); });
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr><td>Jeremy</td><td></td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void With_col_condition()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id).ColumnCondition(() => false); });
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr><td>Jeremy</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));

		}

		[Test]
		public void Should_encode()
		{
			AddToViewData("people2", new List<Person> { new Person { Name = "Jeremy&" } });
			_helper.Grid<Person>("people2", column => { column.For(p => p.Name); });
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr><td>Jeremy&amp;</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_not_encode()
		{
			AddToViewData("people2", new List<Person> { new Person { Name = "Jeremy&" } });
			_helper.Grid<Person>("people2", column => { column.For(p => p.Name).DoNotEncode(); });
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr><td>Jeremy&</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_with_strongly_typed_data()
		{
			_helper.Grid<Person>(new List<Person> { new Person { Id = 1 } }, column => { column.For(p => p.Id); });
			string expected = "<table class=\"grid\"><thead><tr><th>Id</th></tr></thead><tr><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected)); 
		}

		[Test]
		public void Should_render_custom_row_end()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id); column.RowEnd(person =>
			                                                                                                        	{
			                                                                                                        		Writer.Write("</tr>TEST");	
			                                                                                                        	}); });
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr><td>Jeremy</td><td>1</td></tr>TEST</table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_custom_row_start()
		{
			_helper.Grid<Person>("people", column => { column.For(p => p.Name); column.For(p => p.Id); column.RowStart(p =>
			                                                                                                          	{
			                                                                                                          		Writer.Write("<tr class=\"row\">");
			                                                                                                          	}); });
			string expected = "<table class=\"grid\"><thead><tr><th>Name</th><th>Id</th></tr></thead><tr class=\"row\"><td>Jeremy</td><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_with_pagination_last_and_next()
		{
			_people.Add(new Person() { Name = "Person2" });
			_people.Add(new Person() { Name = "Person 3" });
			AddToViewData("pagedPeople", _people.AsPagination(1, 2));
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Showing 1 - 2 of 3 </span><span class='paginationRight'>first | prev | <a href=\"Test.mvc?page=2\">next</a> | <a href=\"Test.mvc?page=2\">last</a></span></div>";
			_helper.Grid<Person>("pagedPeople", column => column.For(p => p.Name));
			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_with_pagination_first_and_previous()
		{
			_people.Add(new Person() { Name = "Person2" });
			_people.Add(new Person() { Name = "Person 3" });
			AddToViewData("pagedPeople", _people.AsPagination(2, 2));
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Showing 3 - 3 of 3 </span><span class='paginationRight'><a href=\"Test.mvc?page=1\">first</a> | <a href=\"Test.mvc?page=1\">prev</a> | next | last</span></div>";
			_helper.Grid<Person>("pagedPeople", column => column.For(p => p.Name));
			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_pagination_with_querystring()
		{
			_people.Add(new Person() { Name = "Person2" });
			_people.Add(new Person() { Name = "Person 3" });
			_context.Request.QueryString.Add("a", "b");
			AddToViewData("pagedPeople", _people.AsPagination(2, 2));
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Showing 3 - 3 of 3 </span><span class='paginationRight'><a href=\"Test.mvc?page=1&amp;a=b\">first</a> | <a href=\"Test.mvc?page=1&amp;a=b\">prev</a> | next | last</span></div>";
			_helper.Grid<Person>("pagedPeople", column => column.For(p => p.Name));
			Assert.That(Writer.ToString().EndsWith(expected));
		}

		[Test]
		public void Should_render_pagination_with_different_message_if_pagesize_is_1()
		{
			_people.Add(new Person() { Name = "Person2" });
			_people.Add(new Person() { Name = "Person 3" });
			AddToViewData("pagedPeople", _people.AsPagination(1, 1));
			string expected = "</table><div class='pagination'><span class='paginationLeft'>Showing 1 of 3 </span><span class='paginationRight'>first | prev | <a href=\"Test.mvc?page=2\">next</a> | <a href=\"Test.mvc?page=3\">last</a></span></div>";
			_helper.Grid<Person>("pagedPeople", column => column.For(p => p.Name));
			Assert.That(Writer.ToString().EndsWith(expected));

		}
	}
}
