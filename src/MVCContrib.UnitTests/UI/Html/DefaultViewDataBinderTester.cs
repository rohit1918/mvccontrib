using System.Collections;
using MvcContrib.UI.Html;
using NUnit.Framework.SyntaxHelpers;
using System;
using NUnit.Framework;
using System.Web.Mvc;
using System.Collections.Generic;

namespace MvcContrib.UnitTests.UI.Html
{

	[TestFixture]
	public class DefaultDataBinderTester
	{
		public class DataBinderTesterBase : BaseViewTester
		{
			protected DefaultDataBinder _binder;

			[SetUp]
			protected override void Setup()
			{
				base.Setup();
				_binder = new DefaultDataBinder();
			}

			protected void AddToViewData(string key, object item)
			{
				((IDictionary)_viewContext.ViewData)[key] = item;
			}
		}

		[TestFixture]
		public class When_ExtractValue_Is_Invoked : DataBinderTesterBase
		{
			[Test]
			public void It_should_obtain_the_value_from_ViewData()
			{
				AddToViewData("test", "Value");
				object instance = _binder.ExtractValue("test", _viewContext);
				Assert.That(instance, Is.EqualTo("Value"));
			}

			[Test]
			public void It_should_obtain_the_value_from_typed_ViewData()
			{
				var viewContext = new ViewContext(
						_viewContext.HttpContext, 
						_viewContext.RouteData, 
						_viewContext.Controller, 
						_viewContext.ViewName, 
						_viewContext.MasterName, 
						new ViewDataDictionary(new Person("Jeremy")), 
						_viewContext.TempData);


				object instance = _binder.ExtractValue("Name", viewContext);
				Assert.That(instance, Is.EqualTo("Jeremy"));
			}

			[Test]
			public void It_should_obtain_nested_property_from_typed_viewdata()
			{
				var p = new Person();
				p.NestedPerson = new Person("Jeremy");

				var viewContext = new ViewContext(
						_viewContext.HttpContext,
						_viewContext.RouteData,
						_viewContext.Controller,
						_viewContext.ViewName,
						_viewContext.MasterName,
						new ViewDataDictionary(p), 
						_viewContext.TempData);

				object instance = _binder.ExtractValue("NestedPerson.Name", viewContext);
				Assert.That(instance, Is.EqualTo("Jeremy"));
			}

			[Test]
			public void It_should_obtain_the_value_from_TempData_if_not_in_ViewData()
			{
				_viewContext.TempData["test"] = "Value";
				object instance = _binder.ExtractValue("test", _viewContext);
				Assert.That(instance, Is.EqualTo("Value"));
			}

			[Test]
			public void It_should_obtain_property_value_for_complex_object()
			{
				AddToViewData("person", new Person("Jeremy"));

				object instance = _binder.ExtractValue("person.Name", _viewContext);
				Assert.That(instance, Is.EqualTo("Jeremy"));
			}

			[Test]
			public void It_should_obtain_field_value_for_complex_object()
			{
				var p = new Person("Jeremy");
				p.Country = "UK";

				AddToViewData("person", p);
				object instance = _binder.ExtractValue("person.Country", _viewContext);
				Assert.That(instance, Is.EqualTo("UK"));
			}

			[Test]
			public void It_should_obtain_item_from_a_collection()
			{
				var values = new List<string>(new[] { "Foo", "Bar", "FooBar" });
				AddToViewData("values", values);

				object instance = _binder.ExtractValue("values[1]", _viewContext);
				Assert.That(instance, Is.EqualTo("Bar"));
			}

			[Test]
			public void It_should_obtain_item_from_a_generic_IList_that_does_not_implement_IList()
			{
				var values = new CustomList<string>();
				values.Add("Foo");
				values.Add("Bar");

				AddToViewData("values", values);

				object instance = _binder.ExtractValue("values[1]", _viewContext);
				Assert.That(instance, Is.EqualTo("Bar"));
			}

			[Test] 
			public void It_should_return_null_from_an_empty_generic_IList_that_does_not_implement_IList()
			{
				var values = new CustomList<string>();
				AddToViewData("values", values);

				object instance = _binder.ExtractValue("values[1]", _viewContext);
				Assert.That(instance, Is.Null);
			}

			[Test]
			public void It_should_obtain_property_from_collection_of_complex_objects()
			{
				var people = new List<Person>();
				people.Add(new Person("Jeremy"));
				people.Add(new Person("Josh"));

				AddToViewData("people", people);

				object instance = _binder.ExtractValue("people[1].Name", _viewContext);
				Assert.That(instance, Is.EqualTo("Josh"));
			}

			[Test]
			public void It_should_obtain_nested_collection_properties()
			{
				var p = new Person();
				p.Languages.Add("C#");
				p.Languages.Add("VB");

				AddToViewData("person", p);

				object instance = _binder.ExtractValue("person.Languages[0]", _viewContext);
				Assert.That(instance, Is.EqualTo("C#"));
			}

			[Test]
			public void It_should_obtain_nested_properties()
			{
				var person = new Person();
				person.NestedPerson = new Person("Jeremy");
				AddToViewData("person", person);

				object instance = _binder.ExtractValue("person.NestedPerson.Name", _viewContext);
				Assert.That(instance, Is.EqualTo("Jeremy"));
			}

			[Test, ExpectedException(typeof(Exception), ExpectedMessage = "The specified index '-1' is outside the bounds of the array. Property people")]
			public void It_should_throw_if_index_smaller_than_zero()
			{
				var people = new List<Person>();
				people.Add(new Person("Jeremy"));
				people.Add(new Person("Josh"));

				AddToViewData("people", people);

				_binder.ExtractValue("people[-1].Name", _viewContext);
				Assert.Fail("Should throw an exception as index < 0");
			}

			[Test, ExpectedException(typeof(Exception), ExpectedMessage = "Could not convert (param people[foo]) index to Int32. Value is foo")]
			public void It_should_throw_if_index_is_not_integer()
			{
				var people = new List<Person>();
				people.Add(new Person("Jeremy"));
				people.Add(new Person("Josh"));

				AddToViewData("people", people);

				_binder.ExtractValue("people[foo].Name", _viewContext);
				Assert.Fail("Should throw an exception as index is not a valid int.");
			}

			[Test, ExpectedException(typeof(Exception), ExpectedMessage = "Property 'NonReadable' cannot be read")]
			public void It_should_throw_if_property_is_not_readable()
			{
				var p = new Person();
				p.NonReadable = "Foo";

				AddToViewData("person", p);

				_binder.ExtractValue("person.NonReadable", _viewContext);
				Assert.Fail("Should throw an exception as property is not readable.");
			}

			[Test, ExpectedException(typeof(Exception), ExpectedMessage = "Property 'Item' has indexes, which are not supported. InstanceType.FullName = 'System.Collections.Generic.List`1[[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]'.")]
			public void It_should_throw_for_indexer_properties()
			{
				var values = new List<string>();
				values.Add("Foo");
				values.Add("Bar");

				AddToViewData("values", values);

				_binder.ExtractValue("values.Item", _viewContext);

				Assert.Fail("Should throw as indexer properties are not supported");
			}

			[Test, ExpectedException(typeof(Exception), ExpectedMessage = "The property Name is being accessed as an indexed property but does not seem to implement IList. In fact the type is String")]
			public void It_should_throw_if_indexed_property_is_not_valid_collection()
			{
				AddToViewData("person", new Person("Jeremy"));

				_binder.ExtractValue("person.Name[0]", _viewContext);
				Assert.Fail("Should throw as Name is not a list.");
			}

			[Test]
			public void It_should_return_null_for_an_empty_collection()
			{
				var values = new List<string>();
				AddToViewData("values", values);

				object instance = _binder.ExtractValue("values[0]", _viewContext);
				Assert.That(instance, Is.Null);
			}


			[Test]
			public void It_should_return_null_if_target_is_null()
			{
				_binder.ExtractValue(null, _viewContext);
			}
			
			#region Custom List
			class CustomList<T> : IList<T>
			{
				private List<T> items = new List<T>();

				public int IndexOf(T item)
				{
					return items.IndexOf(item);
				}

				public void Insert(int index, T item)
				{
					items.Insert(index, item);
				}

				public void RemoveAt(int index)
				{
					items.RemoveAt(index);
				}

				public T this[int index]
				{
					get { return items[index]; }
					set { items[index] = value; }
				}

				public void Add(T item)
				{
					items.Add(item);
				}

				public void Clear()
				{
					items.Clear();
				}

				public bool Contains(T item)
				{
					return items.Contains(item);
				}

				public void CopyTo(T[] array, int arrayIndex)
				{
					items.CopyTo(array, arrayIndex);
				}

				public bool Remove(T item)
				{
					return items.Remove(item);
				}

				public int Count
				{
					get { return items.Count; }
				}

				public bool IsReadOnly
				{
					get { return ((IList)items).IsReadOnly; }
				}

				IEnumerator<T> IEnumerable<T>.GetEnumerator()
				{
					return items.GetEnumerator();
				}

				public IEnumerator GetEnumerator()
				{
					return items.GetEnumerator();
				}
			}
			#endregion
		}

		[TestFixture]
		public class When_a_nested_scope_is_used : DataBinderTesterBase
		{
			[Test]
			public void Then_it_should_be_used_to_obtain_root_instance()
			{
				var p = new Person("Jeremy");
				object value;
				using(_binder.NestedBindingScope(p))
				{
					value = _binder.ExtractValue("Name", _viewContext);
				}
				Assert.That(value, Is.EqualTo("Jeremy"));
			}

			[Test]
			public void Then_rootInstance_should_return_to_null_when_scope_is_disposed()
			{
				var p = new Person("Jeremy");
				using (_binder.NestedBindingScope(p))
				{
					_binder.ExtractValue("Name", _viewContext);
				}
				Assert.That(_binder.NestedRootInstance, Is.Null);
			}

			[Test]
			public void Then_RootInstance_should_return_to_previous_root_instance_when_scope_is_disposed()
			{
				var first = new Person();
				var second = new Person();

				using (_binder.NestedBindingScope(first))
				{
					Assert.That(_binder.NestedRootInstance, Is.SameAs(first));
					using(_binder.NestedBindingScope(second))
					{
						Assert.That(_binder.NestedRootInstance, Is.SameAs(second));
					}
					Assert.That(_binder.NestedRootInstance, Is.SameAs(first));
				}
				Assert.That(_binder.NestedRootInstance, Is.Null);
			}
		}

		class Person
		{
			public string Country;
			private string _nonReadable;
			private List<string> _languages = new List<string>();


			public Person NestedPerson { get; set; }

			public Person()
			{
			}

			public Person(string name)
			{
				Name = name;
			}

			public List<string> Languages
			{
				get { return _languages; }
			}

			public string Name { get; set; }

			public string NonReadable
			{
				set { _nonReadable = value; }
			}
		}

	}
}
