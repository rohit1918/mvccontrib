using System.Collections.Generic;
using System.Linq;
using MvcContrib.UI.Grid;
using NUnit.Framework;

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
		public void Should_enumerate()
		{
			_sections.RowStart("foo");
			_sections.Count().ShouldEqual(1);
		}

		[Test]
		public void Should_get_by_key()
		{
			_sections.RowStart("foo");
			AsGridSections[GridSection.RowStart].ShouldBe<GridSection<Person>>();
		}

		[Test]
		public void Should_not_throw_when_obtaining_by_key()
		{
			AsGridSections[GridSection.RowStart].ShouldBeNull();
		}

		[Test]
		public void Should_contain_section()
		{
			_sections.RowStart("foo");
			var section = AsGridSections.Single().Value;
			AsGridSections.Contains(new KeyValuePair<GridSection, GridSection<Person>>(GridSection.RowStart, section)).ShouldBeTrue();
		}

		[Test]
		public void Should_count_sections()
		{
			_sections.RowStart("foo");
			AsGridSections.Count().ShouldEqual(1);
		}


		private IGridSections<Person> AsGridSections
		{
			get { return _sections; }
		}
	}
}