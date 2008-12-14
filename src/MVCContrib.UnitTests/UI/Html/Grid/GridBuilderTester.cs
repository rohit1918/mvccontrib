using System;
using System.Collections;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UI.Html.Grid;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using System.Collections.Generic;
namespace MvcContrib.UnitTests.UI.Html
{
	[TestFixture]
	public class GridBuilderTester
	{
		private ViewContext _context;
		private GridBuilder<Person> _builder;
		private TextWriter _writer;

		[SetUp]
		public void Setup()
		{
			_context = new ViewContext(MvcMockHelpers.DynamicHttpContextBase(), new RouteData(), MockRepository.GenerateStub<ControllerBase>(), MockRepository.GenerateStub<IView>(), new ViewDataDictionary(), new TempDataDictionary());
			_builder = new GridBuilder<Person>(_context);
			_writer = _context.HttpContext.Response.Output;
		}

		[Test]
		public void GridExtension_should_create_instance_of_gridbuilder()
		{
			var helper = new HtmlHelper(_context, MockRepository.GenerateStub<IViewDataContainer>());
			var grid = helper.Grid<Person>();

			Assert.That(grid, Is.InstanceOfType(typeof(GridBuilder<Person>)));
		}

		[Test]
		public void Should_extract_datasource_from_viewdata()
		{
			var people = new List<Person>();
			_context.ViewData.Add("people", people);

			_builder.FromViewData("people");
			Assert.That(_builder.DataSource, Is.SameAs(people));
		}

		[Test]
		public void Should_use_explicit_datasource()
		{
			var people = new List<Person>();
			_builder.WithData(people);
			Assert.That(_builder.DataSource, Is.SameAs(people));
		}

		[Test]
		public void Should_render_to_output_stream()
		{
			_builder.Render();
			string expected = "<table class=\"grid\"><tr><td>There is no data available.</td></tr></table>";

			Assert.That(_writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void ToString_should_return_null_and_render()
		{
			string toStringResult = _builder.ToString();
			Assert.IsNull(toStringResult);
			Assert.IsNotEmpty(_writer.ToString());
		}

		[Test]
		public void Should_allow_attribute_to_be_specified()
		{
			_builder.Attribute("id", "foo");
			Assert.That(_builder.HtmlAttributes["id"], Is.EqualTo("foo"));
		}

		[Test]
		public void Should_set_css_class()
		{
			_builder.Class("foo");
			Assert.That(_builder.Classes.Contains("foo"));
		}

		[Test]
		public void Should_override_custom_css_class_when_rendered()
		{
			_builder.Class("foo");
			_builder.Render();
			Assert.That(_writer.ToString().StartsWith("<table class=\"foo\">"));
		}

		[Test]
		public void Should_supply_multiple_css_classes_when_rendered()
		{
			_builder.Class("foo");
			_builder.Class("bar");
			_builder.Render();

			Assert.That(_writer.ToString().StartsWith("<table class=\"foo bar\">"));
		}

		[Test]
		public void Should_define_style()
		{
			_builder.Style("width", "100%");
			Assert.That(_builder.Styles["width"], Is.EqualTo("100%"));
		}

		[Test]
		public void Should_render_style()
		{
			_builder.Style("width", "100%");
			_builder.Render();
			Assert.That(_writer.ToString().StartsWith("<table style=\"width:100%\" class=\"grid\">"));
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void Should_throw_when_columns_are_null()
		{
			_builder.Columns(null);
		}

		[Test]
		public void Should_define_columns()
		{
			_builder.WithData(new[] {new Person {Name = "Jeremy"}})
					.Columns(column => {
						column.For(x => x.Name);
					});

			string expected = "<table class=\"grid\"><thead><tr><th>Name</th></tr></thead><tr class=\"gridrow\"><td>Jeremy</td></tr></table>";


			_builder.Render();
			Assert.That(_writer.ToString(), Is.EqualTo(expected)); ;
		}

		[Test]
		public void Empty_should_set_EmptyMessageText_on_the_grid_options()
		{
			_builder.Empty("It is empty");
			Assert.That(_builder.GridOptions.EmptyMessageText, Is.EqualTo("It is empty"));
		}

		[Test]
		public void Custom_grid_options_should_be_passed_to_the_underlying_grid()
		{
			_builder.GridOptions.EmptyMessageText = "It is empty";
			_builder.Render();
			string expected = "<table class=\"grid\"><tr><td>It is empty</td></tr></table>";
			Assert.That(_writer.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void Should_use_custom_renderer()
		{
			var renderer = new CustomRenderer();

			_builder.RenderUsing(renderer);
			_builder.Render();

			Assert.IsTrue(renderer.Rendered);
		}

		[Test]
		public void Should_render_partial_for_custom_renderer()
		{
			ViewEngines.Engines.Clear();
			var engine = MockRepository.GenerateMock<IViewEngine>();
			var view = MockRepository.GenerateMock<IView>();
			view.Expect(x=>x.Render(null,null)).IgnoreArguments().Do(new Action<ViewContext, TextWriter>((c,w) => w.Write("Foo")));
            engine.Expect(x => x.FindPartialView(null, null)).IgnoreArguments().Do(new Func<ControllerContext, string, ViewEngineResult>((c,s) => new ViewEngineResult(view, engine)));
			ViewEngines.Engines.Add(engine);

			var columnBuilder = new GridColumnBuilder<Person>();
			columnBuilder.For("test").RenderPartial("MyPartial");

			columnBuilder[0].CustomRenderer(null, _writer, _context);
			Assert.That(_writer.ToString(), Is.EqualTo("Foo"));

			ViewEngines.Engines.Clear();
		}

		private class CustomRenderer : IGridRenderer<Person>
		{
			public bool Rendered;
			public void Render(IEnumerable<Person> dataSource, GridColumnBuilder<Person> columns, GridOptions options, IDictionary htmlAttributes, ViewContext context)
			{
				Rendered = true;
			}
		}

		private class Person
		{
			public string Name { get; set; }
		}
	}
}