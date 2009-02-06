using System.Collections.Generic;
using System.Linq;
using MvcContrib.UI.Grid;
using NUnit.Framework;

namespace MvcContrib.UnitTests.UI.Grid
{
	[TestFixture]
	public class ColumnBuilderTester
	{
		private ColumnBuilder<Person> builder;

		[SetUp]
		public void Setup()
		{
			builder = new ColumnBuilder<Person>();
		}

		[Test]
		public void Should_define_columns()
		{
			builder.For(x => x.Name);
			builder.For(x => x.DateOfBirth);
			builder.Count().ShouldEqual(2);
		}


		[Test]
		public void Should_infer_column_name_from_lambda()
		{
			builder.For(x => x.Name);
			builder.Single().Name.ShouldEqual("Name");
		}

		[Test]
		public void Should_build_column_with_name()
		{
			builder.For(x => x.Name).Named("foo");
			builder.Single().Name.ShouldEqual("foo");
		}

		[Test]
		public void Name_should_be_null_if_no_name_specified_and_not_MemberExpression()
		{
			builder.For(x => 1);
			builder.Single().Name.ShouldBeNull();
		}

		[Test]
		public void Name_should_be_split_pascal_case()
		{
			builder.For(x => x.DateOfBirth);
			builder.Single().Name.ShouldEqual("Date Of Birth");
		}

		[Test]
		public void Name_should_not_be_split_if_DoNotSplit_specified()
		{
			builder.For(x => x.DateOfBirth).DoNotSplit();
			builder.Single().Name.ShouldEqual("DateOfBirth");
		}

		[Test]
		public void Should_obtain_value()
		{
			builder.For(x => x.Name);
			builder.Single().GetValue(new Person { Name = "Jeremy" }).ShouldEqual("Jeremy");
		}

		[Test]
		public void Should_format_item()
		{
			builder.For(x => x.Name).Format("{0}_TEST");
			builder.Single().GetValue(new Person{Name="Jeremy"}).ShouldEqual("Jeremy_TEST");
		}

		[Test]
		public void Should_not_return_value_when_CellCondition_returns_false()
		{
			builder.For(x => x.Name).CellCondition(x => false);
			builder.Single().GetValue(new Person(){Name = "Jeremy"}).ShouldBeNull();
		}

		[Test]
		public void Column_should_not_be_visible()
		{
			builder.For(x => x.Name).Visible(false);
			builder.Single().Visible.ShouldBeFalse();
		}

		[Test]
		public void Should_html_encode_output()
		{
			builder.For(x => x.Name);
			builder.Single().GetValue(new Person{Name = "<script>"}).ShouldEqual("&lt;script&gt;");
		}

		[Test]
		public void Should_not_html_encode_output()
		{
			builder.For(x => x.Name).DoNotEncode();
			builder.Single().GetValue(new Person{Name = "<script>"}).ShouldEqual("<script>");
		}

		[Test]
		public void Should_specify_header_attributes()
		{
			var attrs = new Dictionary<string, object> { { "foo", "bar" } };
			builder.For(x => x.Name).HeaderAttributes(attrs);
			builder.Single().HeaderAttributes["foo"].ShouldEqual("bar");
		}

		[Test]
		public void Should_specify_header_attributes_using_lambdas()
		{
			builder.For(x => x.Name).HeaderAttributes(foo => "bar");
			builder.Single().HeaderAttributes["foo"].ShouldEqual("bar");
		}

		[Test]
		public void Should_create_custom_column()
		{
			builder.For("Name");
			builder.Single().Name.ShouldEqual("Name");
		}
	}
}