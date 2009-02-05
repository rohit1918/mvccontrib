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
		private List<Person> people;
		private Grid<Person> grid;
		private TextWriter writer;

		[SetUp]
		public void Setup()
		{
			writer = new StringWriter();
			people = new List<Person>();
			grid = new Grid<Person>(people, writer);
		}

		[Test]
		public void Should_use_custom_renderer()
		{
			var mockRenderer = MockRepository.GenerateMock<IGridRenderer>();
			mockRenderer.Expect(x => x.Render(null, null)).IgnoreArguments().Do(new Action<IGridModel, TextWriter>((g, w) => w.Write("foo")));
			grid.RenderUsing(mockRenderer).ToString();
			writer.ToString().ShouldEqual("foo");
		}

		[Test]
		public void Should_specify_custom_model()
		{
			var mockModel = MockRepository.GenerateStub<IGridModel>();
			var mockRenderer = MockRepository.GenerateMock<IGridRenderer>();
			grid.WithModel(mockModel).RenderUsing(mockRenderer).ToString();
			mockRenderer.AssertWasCalled(x => x.Render(mockModel, writer));
		}
	}
}