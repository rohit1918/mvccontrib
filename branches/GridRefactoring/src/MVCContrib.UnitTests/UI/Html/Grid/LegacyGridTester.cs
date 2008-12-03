using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UI.Html.Grid.Legacy;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI.Html.Grid
{
	[TestFixture]
	public class LegacyGridTester
	{
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
			var view = _mocks.DynamicMock<IView>();
			_viewContext = new ViewContext(_context, new RouteData(), _mocks.DynamicMock<ControllerBase>(), view,
			                               new ViewDataDictionary(), null);
			_helper = new HtmlHelper(_viewContext, new ViewPage());
			_people = new List<Person> 
			          	{
			          		new Person {Id = 1, Name = "Jeremy", DateOfBirth = new DateTime(1987, 4, 19)}
			          	};
			AddToViewData("people", _people);
			_mocks.ReplayAll();
		}

		private void AddToViewData(string key, Object value)
		{
			_viewContext.ViewData.Add(key, value);
		}

		private TextWriter Writer
		{
			get { return _context.Response.Output; }
		}

		[Test]
		public void Should_obtain_custom_empty_message_from_html_attributes_dictionary()
		{
			string expected = "<table class=\"grid\"><tr><td>Test</td></tr></table>";
			_helper.Grid<Person>((string)null, new Hash(empty => "Test"), column => { column.For(p => p.Name); column.For(p => p.Id); });
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_with_strongly_typed_data()
		{
			_helper.Grid(new List<Person> {new Person {Id = 1}}, column => column.For(p => p.Id));
			string expected = "<table class=\"grid\"><thead><tr><th>Id</th></tr></thead><tr class=\"gridrow\"><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_with_strongly_typed_data_and_custom_sections()
		{
			_helper.Grid(new List<Person> { new Person { Id = 1 } }, column => column.For(p => p.Id), sections => sections.RowStart(p => Writer.Write("<tr foo=\"bar\">")));
			string expected = "<table class=\"grid\"><thead><tr><th>Id</th></tr></thead><tr foo=\"bar\"><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_render_with_strongly_typed_data_and_custom_attributes() 
		{
			_helper.Grid(new List<Person> { new Person { Id = 1 } }, new Hash(style => "width: 100%"), column => column.For(p => p.Id));
			string expected = "<table style=\"width: 100%\" class=\"grid\"><thead><tr><th>Id</th></tr></thead><tr class=\"gridrow\"><td>1</td></tr></table>";
			Assert.That(Writer.ToString(), Is.EqualTo(expected));
		}

		private class Person
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public DateTime DateOfBirth { get; set; }
		}
	}
}