using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;
using MvcContrib.FluentHtml.Expressions;
using MvcContrib.FluentHtml.Tests.Fakes;
using MvcContrib.FluentHtml.Tests.Helpers;

namespace MvcContrib.FluentHtml.Tests
{
	[TestFixture]
	public class MemberExpressionExtensionTests
	{
		private FakeModel model;

		[SetUp]
		public void SetUp()
		{
			model = new FakeModel
			{
				Title = "Test Title",
				Date = DateTime.Now,
				Done = true,
				Id = 123,
				Person = new FakeChildModel { FirstName = "Mick", LastName = "Jagger" },
				Numbers = new [] {1, 3},
				Customers = new List<FakeChildModel>
				{
					new FakeChildModel { FirstName = "John", LastName = "Wyane" },
					new FakeChildModel { FirstName = "Marlin", LastName = "Brando" }
				}
			};
		}

		[Test]
		public void can_get_name_for_simple_string_property()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Title;
			var name = expression.GetNameFor();
			name.ShouldEqual("Title");
		}

		[Test]
		public void can_get_name_for_simple_decimal_property()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Price;
			var name = expression.GetNameFor();
			name.ShouldEqual("Price");
		}

		[Test]
		public void can_get_name_for_compound_string_property()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Person.FirstName;
			var name = expression.GetNameFor();
			name.ShouldEqual("Person.FirstName");
		}

		[Test]
		public void can_get_name_for_instance_of_simple_collection_property()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Numbers[0];
			var name = expression.GetNameFor();
			name.ShouldEqual("Numbers[0]");
		}

		[Test]
		public void can_get_name_for_instance_of_simple_collection_property_using_a_local_variable_as_indexer()
		{
			var i = 33;
			Expression<Func<FakeModel, object>> expression = x => x.Numbers[i];
			var name = expression.GetNameFor();
			name.ShouldEqual("Numbers[33]");
		}

		[Test]
		public void can_get_name_for_instance_of_complex_collection_property()
		{
			Expression<Func<FakeModel, object>> expression = x => x.Customers[0].FirstName;
			var name = expression.GetNameFor();
			name.ShouldEqual("Customers[0].FirstName");
		}

		[Test]
		public void can_get_name_for_instance_of_nested_complex_collection_property()
		{
			var i = 0;
			Expression<Func<FakeModel, object>> expression = x => x.FakeModelList[1].Customers[i].Balance;
			var name = expression.GetNameFor();
			name.ShouldEqual("FakeModelList[1].Customers[0].Balance");
		}

		[Test]
		public void can_get_name_for_property_of_instance_of_collection()
		{
			Expression<Func<IList<FakeModel>, object>> expression = x => x[999].Title;
			var name = expression.GetNameFor();
			name.ShouldEqual("[999].Title");
		}

		[Test]
		public void can_get_name_for_property_of_instance_of_array()
		{
			var i = 888;
			Expression<Func<FakeModel, object>> expression = x => x.FakeModelArray[999].FakeModelArray[i].Person.FirstName;
			var name = expression.GetNameFor();
			name.ShouldEqual("FakeModelArray[999].FakeModelArray[888].Person.FirstName");
		}
	}
}
