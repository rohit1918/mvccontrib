using System;
using System.Collections.Generic;
using System.IO;
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

		[SetUp]
		public void Setup()
		{
			_writer = new StringWriter();
			_people = new List<Person>();
			_model = new GridModel<Person>();
			_grid = new Grid<Person>(_people, _writer);
			_grid.WithModel(_model);
		}

		[Test]
		public void Should_use_custom_renderer()
		{
			var mockRenderer = MockRepository.GenerateMock<IGridRenderer<Person>>();
			mockRenderer.Expect(x => x.Render(null, null, null)).IgnoreArguments().Do(new Action<IGridModel<Person>, IEnumerable<Person>, TextWriter>((g, d, w) => w.Write("foo")));
			_grid.RenderUsing(mockRenderer).ToString();
			_writer.ToString().ShouldEqual("foo");
		}

		[Test]
		public void Should_specify_custom_model()
		{
			var mockModel = MockRepository.GenerateStub<IGridModel<Person>>();
			var mockRenderer = MockRepository.GenerateMock<IGridRenderer<Person>>();
			_grid.WithModel(mockModel).RenderUsing(mockRenderer).ToString();
			mockRenderer.AssertWasCalled(x => x.Render(mockModel, _people, _writer));
		}

		[Test]
		public void Columns_should_be_stored()
		{
			var columns = new List<GridColumn<Person>>();
			var mockModel = MockRepository.GenerateStub<IGridModel<Person>>();
			mockModel.Expect(x => x.Columns).Return(columns);

			_grid.WithModel(mockModel).Columns(col => col.For(x => x.Name));
			columns.Count.ShouldEqual(1);
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
			var attrs = new Dictionary<string, object> { { "foo", "bar"  } };
			_grid.Attributes(attrs);
			_model.Attributes["foo"].ShouldEqual("bar");
		}

		[Test]
		public void Custom_attributes_should_be_stored_using_lambdas()
		{
			_grid.Attributes(foo => "bar");
			_model.Attributes["foo"].ShouldEqual("bar");
		}
	}
}