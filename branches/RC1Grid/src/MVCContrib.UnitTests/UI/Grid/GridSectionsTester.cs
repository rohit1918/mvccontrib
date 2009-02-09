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
		public void Should_add_section_for_row_start()
		{
			var row = _sections.RowStart();
			row.ShouldBeTheSameAs(AsDictionary[GridSection.RowStart]);
		}

		[Test]
		public void Should_enumerate()
		{
			_sections.RowStart();
			_sections.Count().ShouldEqual(1);
		}

		[Test]
		public void Should_get_by_key()
		{
			var section = _sections.RowStart();
			AsDictionary[GridSection.RowStart].ShouldBeTheSameAs(section);
		}

		[Test]
		public void Should_not_throw_when_obtaining_by_key()
		{
			AsDictionary[GridSection.RowStart].ShouldBeNull();
		}

		private IGridSections<Person> AsDictionary
		{
			get { return _sections; }
		}
	}
}