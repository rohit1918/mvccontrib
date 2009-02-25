using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using MvcContrib.UI.Grid;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI.Grid
{
	[TestFixture]
	public class GridTester
	{
		private List<Person> _people;
		private Grid<Person> _grid;
		private TextWriter _writer;
		private IGridModel<Person> _model;
		private ViewContext _context;

		[SetUp]
		public void Setup()
		{
			_writer = new StringWriter();
			_people = new List<Person>();
			_model = new GridModel<Person>();
			_context = new ViewContext();
			_grid = new Grid<Person>(_people, _writer, _context);
			_grid.WithModel(_model);
		}

		[Test]
		public void Should_use_custom_renderer()
		{
			var mockRenderer = MockRepository.GenerateMock<IGridRenderer<Person>>();
			mockRenderer.Expect(x => x.Render(null, null, null, null)).IgnoreArguments().Do(new Action<IGridModel<Person>, IEnumerable<Person>, TextWriter, ViewContext>((g, d, w, c) => w.Write("foo")));
			_grid.RenderUsing(mockRenderer).ToString();
			_writer.ToString().ShouldEqual("foo");
		}

		[Test]
		public void Should_specify_custom_model()
		{
			var mockModel = MockRepository.GenerateStub<IGridModel<Person>>();
			mockModel.Stub(x => x.Sections).Return(new GridSections<Person>());
			var mockRenderer = MockRepository.GenerateMock<IGridRenderer<Person>>();
			_grid.WithModel(mockModel).RenderUsing(mockRenderer).Render();
			mockRenderer.AssertWasCalled(x => x.Render(mockModel, _people, _writer, _context));
		}

		[Test]
		public void Columns_should_be_stored()
		{
			var model = new GridModel<Person>();
			_grid.WithModel(model).Columns(col => col.For(x => x.Name));
			((IGridModel<Person>)model).Columns.Count.ShouldEqual(1);
		}

		[Test]
		public void RowStart_section_should_be_stored_when_rendered()
		{
			var model = new GridModel<Person>();
			_grid.WithModel(model).RowStart("foo");
			_grid.ToString();
			((IGridModel<Person>)model).Sections[GridSection.RowStart].ShouldNotBeNull();
		}

		[Test]
		public void RowEnd_section_should_be_stored_when_rendered()
		{
			var model = new GridModel<Person>();
			_grid.WithModel(model).RowEnd("foo");
			_grid.ToString();
			((IGridModel<Person>)model).Sections[GridSection.RowEnd].ShouldNotBeNull();
		}

		[Test]
		public void RowStart_action_should_be_stored_when_rendered()
		{
			var model = new GridModel<Person>();
			_grid.WithModel(model).RowStart((p) => { RowStart_action_should_be_stored_when_rendered(); });
			_grid.ToString();
			((IGridModel<Person>)model).Sections[GridSection.RowStart].ShouldNotBeNull();
		}

		[Test]
		public void RowStart_action_bool_should_be_stored_when_rendered()
		{
			var model = new GridModel<Person>();
			_grid.WithModel(model).RowStart((p, x) => { RowStart_action_bool_should_be_stored_when_rendered(); });
			_grid.ToString();
			((IGridModel<Person>)model).Sections[GridSection.RowStart].ShouldNotBeNull();
		}

		[Test]
		public void RowEnd_actoion_should_be_stored_when_rendered()
		{
			var model = new GridModel<Person>();
			_grid.WithModel(model).RowEnd((p) => { RowEnd_actoion_should_be_stored_when_rendered(); });
			_grid.ToString();
			((IGridModel<Person>)model).Sections[GridSection.RowEnd].ShouldNotBeNull();
		}


		[Test]
		public void Empty_text_should_be_stored()
		{
			_grid.Empty("Foo");
			_model.EmptyText.ShouldEqual("Foo");
		}

		[Test]
		public void Custom_attributes_should_be_stored()
		{
			var attrs = new Dictionary<string, object> { { "foo", "bar" } };
			_grid.Attributes(attrs);
			_model.Attributes["foo"].ShouldEqual("bar");
		}

		[Test]
		public void Custom_attributes_should_be_stored_using_lambdas()
		{
			_grid.Attributes(foo => "bar");
			_model.Attributes["foo"].ShouldEqual("bar");
		}


		[Test]
		public void Renders_to_provided_renderer_by_default()
		{
			_grid.Empty("Foo");
			_grid.Render();
			_writer.ToString().ShouldEqual("<table class=\"grid\"><tr><td>Foo</td></tr></table>");
		}


	}
}