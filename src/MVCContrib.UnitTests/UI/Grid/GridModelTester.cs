using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
			_model.Column.For(x => x.Name);
			AsGridModel.Columns.Count.ShouldEqual(1);	
		}

		[Test]
		public void Should_define_sections()
		{
			_model.Sections.RowStart();
			AsGridModel.Sections[GridSection.RowStart].ShouldNotBeNull();
		}

		[Test]
		public void Should_define_empty_text()
		{
			_model.Empty("Foo");
			AsGridModel.EmptyText.ShouldEqual("Foo");
		}

		[Test]
		public void Should_store_attributes()
		{
			_model.Attributes(new Dictionary<string, object>() { {"foo", "bar"} });
			AsGridModel.Attributes["foo"].ShouldEqual("bar");
		}

		[Test]
		public void Should_store_attributes_with_lambdas()
		{
			_model.Attributes(foo=>"bar");
			AsGridModel.Attributes["foo"].ShouldEqual("bar");
		}

		private IGridModel<Person> AsGridModel
		{
			get { return _model; }
		}
	}
}