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
			_sections.RowStart("foo");
			AsDictionary.ContainsKey(GridSection.RowStart).ShouldBeTrue();
		}

		[Test]
		public void Should_add_section_for_row_end()
		{
			_sections.RowEnd("foo");
			AsDictionary.ContainsKey(GridSection.RowEnd).ShouldBeTrue();
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
			AsDictionary[GridSection.RowStart].ShouldBe<GridSection<Person>>();
		}

		[Test]
		public void Should_not_throw_when_obtaining_by_key()
		{
			AsDictionary[GridSection.RowStart].ShouldBeNull();
		}

		[Test]
		public void Should_add_section()
		{
			AsDictionary.Add(new KeyValuePair<GridSection, GridSection<Person>>(GridSection.RowStart, new GridSection<Person>("foo")));
			_sections.Count().ShouldEqual(1);
		}

		[Test]
		public void Should_clear()
		{
			AsDictionary.Add(new KeyValuePair<GridSection, GridSection<Person>>(GridSection.RowStart, new GridSection<Person>("foo")));
			AsDictionary.Clear();
			_sections.Count().ShouldEqual(0);
		}

		[Test]
		public void Should_contain_section()
		{
			_sections.RowStart("foo");
			var section = AsDictionary.Single().Value;
			AsDictionary.Contains(new KeyValuePair<GridSection, GridSection<Person>>(GridSection.RowStart, section)).ShouldBeTrue();
		}

		[Test]
		public void Should_copy_to_array()
		{
			var array = new KeyValuePair<GridSection, GridSection<Person>>[1];
			_sections.RowStart("foo");
			AsDictionary.CopyTo(array, 0);
			array[0].Value.ShouldBeTheSameAs(_sections.Single().Value);
		}

		[Test]
		public void Should_remove_section()
		{
			_sections.RowStart("foo");
			AsDictionary.Remove(new KeyValuePair<GridSection, GridSection<Person>>(GridSection.RowStart, AsDictionary.Single().Value));
			_sections.Count().ShouldEqual(0);
		}

		[Test]
		public void Should_count_sections()
		{
			_sections.RowStart("foo");
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
			_sections.RowStart("foo");
			AsDictionary.ContainsKey(GridSection.RowStart).ShouldBeTrue();
		}

		private IDictionary<GridSection, GridSection<Person>> AsDictionary
		{
			get { return _sections; }
		}
	}
}