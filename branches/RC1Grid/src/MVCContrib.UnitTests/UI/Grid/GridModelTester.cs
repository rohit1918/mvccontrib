using MvcContrib.UI.Grid;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.Grid
{
	[TestFixture]
	public class GridModelTester
	{
		private GridModel<Person> _model;

		[SetUp]
		public void Setup()
		{
			_model = new GridModel<Person>();
		}

		[Test]
		public void Should_define_columns_using_ColumnFor()
		{
			_model.ColumnFor(x => x.Name);
			AsGridModel.Columns.Count.ShouldEqual(1);	
		}

		[Test]
		public void Should_define_empty_text()
		{
			_model.Empty("Foo");
			AsGridModel.EmptyText.ShouldEqual("Foo");
		}

		private IGridModel<Person> AsGridModel
		{
			get { return _model; }
		}
	}
}