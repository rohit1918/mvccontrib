using System;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.UI.Grid;
using NUnit.Framework;
using MvcContrib.UI.Grid.ActionSyntax;
namespace MvcContrib.UnitTests.UI.Grid
{
	[TestFixture]
	public class GridSectionsTester
	{
		private GridSections<Person> _sections;

		[SetUp]
		public void Setup()
		{
			_sections = new GridSections<Person>();
		}


		[Test]
		public void Should_get_by_key()
		{
			_sections.RowStart("foo");
			_sections[GridSection.RowStart].ShouldBe<GridSection<Person>>();
		}

		[Test]
		public void Should_not_throw_when_obtaining_by_key()
		{
			_sections[GridSection.RowStart].ShouldBeNull();
		}


		[Test]
		public void Should_not_throw_when_obtaining_by_other_key()
		{
			_sections[GridSection.RowEnd].ShouldBeNull();
		}

		[Test]
		public void Should_set_first_item()
		{
			_sections.RowStart("foo");
			_sections[GridSection.RowStart].ShouldNotBeNull();
		}

		[Test]
		public void Should_set_second_item()
		{
			_sections.RowEnd("foo");
			_sections[GridSection.RowEnd].ShouldNotBeNull();
		}

		[Test]
		public void Should_set_action_block()
		{
			_sections.RowStart(p => Should_set_action_block());
			_sections[GridSection.RowStart].ShouldNotBeNull();
		}

		[Test]
		public void Should_set_action_bool_block()
		{
			_sections.RowStart((p, b) => Should_set_action_bool_block());
			_sections[GridSection.RowStart].ShouldNotBeNull();
		}

		[Test]
		public void Should_set_end_action_block()
		{
			_sections.RowEnd(p => Should_set_action_bool_block());
			_sections[GridSection.RowEnd].ShouldNotBeNull();
		}

		[Test,ExpectedException(typeof(ArgumentException))]
		public void Should_throw_when_unknown_grid_section_set()
		{
			_sections[(GridSection)3] = new GridSection<Person>((x, y) => { });
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void Should_throw_when_unknown_grid_section_read()
		{
			var x = _sections[(GridSection)2];
		}


	}
}
