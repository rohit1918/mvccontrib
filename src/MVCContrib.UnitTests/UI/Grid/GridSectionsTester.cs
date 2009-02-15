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

		[Test]
		public void Should_add_section()
		{
			AsDictionary.Add(new KeyValuePair<GridSection, GridSection<Person>>(GridSection.RowStart, new GridSection<Person>()));
			_sections.Count().ShouldEqual(1);
		}

		[Test]
		public void Should_clear()
		{
			AsDictionary.Add(new KeyValuePair<GridSection, GridSection<Person>>(GridSection.RowStart, new GridSection<Person>()));
			AsDictionary.Clear();
			_sections.Count().ShouldEqual(0);
		}

		[Test]
		public void Should_contain_section()
		{
			var section = _sections.RowStart() as GridSection<Person>;
			AsDictionary.Contains(new KeyValuePair<GridSection, GridSection<Person>>(GridSection.RowStart, section)).ShouldBeTrue();
		}

		[Test]
		public void Should_copy_to_array()
		{
			var array = new KeyValuePair<GridSection, GridSection<Person>>[1];
			var col = _sections.RowStart();
			AsDictionary.CopyTo(array, 0);
			array[0].Value.ShouldBeTheSameAs(col);
		}

		[Test]
		public void Should_remove_section()
		{
			var column = (GridSection<Person>)_sections.RowStart();
			AsDictionary.Remove(new KeyValuePair<GridSection, GridSection<Person>>(GridSection.RowStart, column));
			_sections.Count().ShouldEqual(0);
		}

		[Test]
		public void Should_count_sections()
		{
			_sections.RowStart();
			AsDictionary.Count.ShouldEqual(1);
		}

		[Test]
		public void Should_not_be_readonly()
		{
			AsDictionary.IsReadOnly.ShouldBeFalse();
		}

		[Test]
		public void Should_contain_key()
		{
			_sections.RowStart();
			AsDictionary.ContainsKey(GridSection.RowStart).ShouldBeTrue();
		}

		private IDictionary<GridSection, GridSection<Person>> AsDictionary
		{
			get { return _sections; }
		}
	}
}