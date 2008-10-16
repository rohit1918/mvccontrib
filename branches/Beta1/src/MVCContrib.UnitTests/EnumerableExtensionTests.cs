using System.Collections.Generic;
using MvcContrib.EnumerableExtensions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.Linq;
namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class EnumerableExtensionTests
	{
		[Test]
		public void Should_create_select_list_with_value_and_text_field()
		{
			var list = People.ToSelectList(x => x.Id, x => x.Name);
			Assert.That(list.DataTextField, Is.EqualTo("Name"));
			Assert.That(list.DataValueField, Is.EqualTo("Id"));
		}

		[Test]
		public void Should_create_select_list_with_selected_value()
		{
			var list = People.ToSelectList(x => x.Id, x => x.Name, 3);
			Assert.That(list.SelectedValue, Is.EqualTo(3));
		}

		[Test]
		public void Should_create_select_list_with_selected_value_using_selector()
		{
			var list = People.ToSelectList(x => x.Id, x => x.Name, x => x.Name == "Jeremy");
			Assert.That(list.SelectedValues.Cast<Person>().Single().Id, Is.EqualTo(3));
		}

		private IEnumerable<Person> People
		{
			get
			{
				yield return new Person { Id = 1, Name = "Jeffrey" };
				yield return new Person { Id = 2, Name = "Eric" };
				yield return new Person { Id = 3, Name = "Jeremy" };
			}
		}


		private class Person
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}
	}
}