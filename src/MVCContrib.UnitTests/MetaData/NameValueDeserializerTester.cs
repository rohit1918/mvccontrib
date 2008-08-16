using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using MvcContrib.Attributes;
using NUnit.Framework;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class NameValueDeserializerTester
	{
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void NullPrefixThrows()
		{
			var collection = new NameValueCollection();
			collection["junk"] = "stuff";

			var nvd = new NameValueDeserializer();

			object notGonnaDoIt = nvd.Deserialize(collection, null, typeof(Customer));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyPrefixThrows()
		{
			var collection = new NameValueCollection();
			collection["junk"] = "stuff";

			var nvd = new NameValueDeserializer();

			object notGonnaDoIt = nvd.Deserialize(collection, string.Empty, typeof(Customer));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullTargetTypeThrows()
		{
			var collection = new NameValueCollection();
			collection["junk"] = "stuff";

			var nvd = new NameValueDeserializer();

			object notGonnaDoIt = nvd.Deserialize(collection, "junk", null);
		}

		[Test]
		public void ListPropertyIsSkippedIfNotInitializedAndReadOnly()
		{
			var collection = new NameValueCollection();
			collection["list.ReadonlyIds[0]"] = "10";
			collection["list.ReadonlyIds[1]"] = "20";

			var nvd = new NameValueDeserializer();

			var list = nvd.Deserialize<GenericListClass>(collection, "list");

			Assert.IsNotNull(list);
			Assert.IsNull(list.ReadonlyIds);
		}

		[Test]
		public void ErrorsSettingPropertiesAreIgnored()
		{
			var collection = new NameValueCollection();
			collection["emp.Age"] = "-1";

			var nvd = new NameValueDeserializer();

			var emp = nvd.Deserialize<Employee>(collection, "emp");

			Assert.IsNotNull(emp);
			Assert.AreEqual(0, emp.Age);
		}

		[Test]
		public void ComplexPropertyIsSkippedIfNotInitializedAndReadOnly()
		{
			var collection = new NameValueCollection();
			collection["emp.BatPhone.Number"] = "800-DRK-KNGT";

			var nvd = new NameValueDeserializer();

			var emp = nvd.Deserialize<Employee>(collection, "emp");

			Assert.IsNotNull(emp);
			Assert.IsNull(emp.BatPhone);
		}

		[Test]
		public void DeserializeSimpleObject()
		{
			var collection = new NameValueCollection();
			collection["cust.Id"] = "10";

			var nvd = new NameValueDeserializer();

			var cust = nvd.Deserialize<Customer>(collection, "cust");

			Assert.IsNotNull(cust);
			Assert.AreEqual(10, cust.Id);
		}

		[Test]
		public void DeserializeSimpleArray()
		{
			var collection = new NameValueCollection();
			collection["array.Ids[0]"] = "10";
			collection["array.Ids[1]"] = "20";

			var nvd = new NameValueDeserializer();

			var array = nvd.Deserialize<ArrayClass>(collection, "array");

			Assert.IsNotNull(array);
			Assert.AreEqual(2, array.Ids.Length);
			Assert.AreEqual(10, array.Ids[0]);
			Assert.AreEqual(20, array.Ids[1]);
		}

		[Test]
		public void DeserializeSimpleArrayFromMultiple()
		{
			var collection = new NameValueCollection
			{
				{"array.ids", "10"}, 
				{"array.ids", "20"}
			};

			var nvd = new NameValueDeserializer();

			var array = nvd.Deserialize<ArrayClass>(collection, "array");

			Assert.IsNotNull(array);
			Assert.AreEqual(2, array.Ids.Length);
			Assert.AreEqual(10, array.Ids[0]);
			Assert.AreEqual(20, array.Ids[1]);
		}

		[Test]
		public void DeserializePrimitiveArray()
		{
			var collection = new NameValueCollection();
			collection["ids[0]"] = "10";
			collection["ids[1]"] = "20";

			var nvd = new NameValueDeserializer();

			var array = (int[])nvd.Deserialize(collection, "ids", typeof(int[]));

			Assert.IsNotNull(array);
			Assert.AreEqual(2, array.Length);
			Assert.AreEqual(10, array[0]);
			Assert.AreEqual(20, array[1]);
		}

		[Test]
		public void DeserializePrimitiveArrayFromMultiple()
		{
			var collection = new NameValueCollection
				{
					{"ids", "10"}, 
					{"ids", "20"}
				};

			var nvd = new NameValueDeserializer();

			var array = (int[])nvd.Deserialize(collection, "ids", typeof(int[]));

			Assert.IsNotNull(array);
			Assert.AreEqual(2, array.Length);
			Assert.AreEqual(10, array[0]);
			Assert.AreEqual(20, array[1]);
		}

		[Test]
		public void DeserializePrimitiveGenericList()
		{
			var collection = new NameValueCollection();
			collection["ids[0]"] = "10";
			collection["ids[1]"] = "20";

			var nvd = new NameValueDeserializer();

			var list = nvd.Deserialize<List<int>>(collection, "ids");

			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(10, list[0]);
			Assert.AreEqual(20, list[1]);
		}

		[Test]
		public void DeserializePrimitiveGenericListFromMultiple()
		{
			var collection = new NameValueCollection
				{
					{"ids", "10"}, 
					{"ids", "20"}
				};

			var nvd = new NameValueDeserializer();

			var list = nvd.Deserialize<List<int>>(collection, "ids");

			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(10, list[0]);
			Assert.AreEqual(20, list[1]);
		}

		[Test]
		public void DeserializeEnumGenericListFromMultiple()
		{
			var collection = new NameValueCollection {{"testEnum", "0"}, {"testEnum", "2"}};

			var nvd = new NameValueDeserializer();

			var list = nvd.Deserialize<List<TestEnum>>(collection, "testEnum");

			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(TestEnum.One, list[0]);
			Assert.AreEqual(TestEnum.Three, list[1]);
		}

		[Test]
		public void DeserializeSimpleGenericList()
		{
			var collection = new NameValueCollection();
			collection["list.Ids[0]"] = "10";
			collection["list.Ids[1]"] = "20";

			var nvd = new NameValueDeserializer();

			var list = nvd.Deserialize<GenericListClass>(collection, "list");

			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Ids.Count);
			Assert.AreEqual(10, list.Ids[0]);
			Assert.AreEqual(20, list.Ids[1]);
		}

		[Test]
		public void DeserializeSimpleGenericListFromMultiple()
		{
			var collection = new NameValueCollection
				{
					{"list.Ids", "10"}, 
					{"list.Ids", "20"}
				};

			var nvd = new NameValueDeserializer();

			var list = nvd.Deserialize<GenericListClass>(collection, "list");

			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Ids.Count);
			Assert.AreEqual(10, list.Ids[0]);
			Assert.AreEqual(20, list.Ids[1]);
		}

		[Test]
		public void DeserializeComplexGenericList()
		{
			var collection = new NameValueCollection();
			collection["emp.OtherPhones[0].Number"] = "800-555-1212";
			collection["emp.OtherPhones[1].Number"] = "800-867-5309";
			collection["emp.OtherPhones[1].AreaCodes[0]"] = "800";
			collection["emp.OtherPhones[1].AreaCodes[1]"] = "877";

			var nvd = new NameValueDeserializer();

			var emp = nvd.Deserialize<Employee>(collection, "emp");

			Assert.IsNotNull(emp);
			Assert.AreEqual(2, emp.OtherPhones.Count);
			Assert.AreEqual("800-555-1212", emp.OtherPhones[0].Number);
			Assert.AreEqual("800-867-5309", emp.OtherPhones[1].Number);
			Assert.AreEqual(2, emp.OtherPhones[1].AreaCodes.Count);
			Assert.AreEqual("800", emp.OtherPhones[1].AreaCodes[0]);
			Assert.AreEqual("877", emp.OtherPhones[1].AreaCodes[1]);
		}


		[Test]
		public void DeserializeWithEmptyArray()
		{
			var collection = new NameValueCollection();
			collection["array.Name"] = "test";

			var nvd = new NameValueDeserializer();

			var array = nvd.Deserialize<ArrayClass>(collection, "array");

			Assert.IsNotNull(array);
			Assert.AreEqual(0, array.Ids.Length);
		}

		[Test]
		public void DeserializeComplexObject()
		{
			var collection = new NameValueCollection();
			collection["emp.Id"] = "20";
			collection["emp.Phone.Number"] = "800-555-1212";

			var nvd = new NameValueDeserializer();

			var emp = nvd.Deserialize<Employee>(collection, "emp");

			Assert.IsNotNull(emp);
			Assert.AreEqual(20, emp.Id);
			Assert.AreEqual("800-555-1212", emp.Phone.Number);
		}

		[Test]
		public void EmptyValuesUseDefaultOfType()
		{
			var collection = new NameValueCollection();
			collection["cust.Id"] = "";

			var nvd = new NameValueDeserializer();

			var cust = nvd.Deserialize<Customer>(collection, "cust");

			Assert.IsNotNull(cust);
			Assert.AreEqual(0, cust.Id);
		}

		[Test]
		public void NoMatchingValuesReturnsNewedObject()
		{
			var collection = new NameValueCollection();
			collection["cust.Id"] = "10";

			var nvd = new NameValueDeserializer();

			var cust = nvd.Deserialize<Customer>(collection, "mash");

			Assert.IsNotNull(cust);
		}

		[Test]
		public void DeserializeTrueBool()
		{
			var collection = new NameValueCollection();
			collection["bool.myBool"] = "true,false";

			var nvd = new NameValueDeserializer();

			var boolClass = nvd.Deserialize<BoolClass>(collection, "bool");

			Assert.AreEqual(true, boolClass.MyBool);
		}

		[Test]
		public void DeserializeFalseBool()
		{
			var collection = new NameValueCollection();
			collection["bool.myBool"] = "false";

			var nvd = new NameValueDeserializer();

			var boolClass = nvd.Deserialize<BoolClass>(collection, "bool");

			Assert.AreEqual(false, boolClass.MyBool);
		}

		[Test]
		public void EmptyCollectionReturnsNull()
		{
			var nvd = new NameValueDeserializer();

			var cust = nvd.Deserialize<Customer>(null, "cust");

			Assert.IsNull(cust);
		}

		[Test]
		public void ForCompleteness()
		{
			var attribute = new DeserializeAttribute("test");

			Assert.AreEqual("test", attribute.Prefix);
		}

		[Test]
		public void NoRequestForPropertyShouldNotInstantiateProperty()
		{
			var collection = new NameValueCollection();
			collection["emp.Id"] = "20";
			collection["emp.Phone.Number"] = "800-555-1212";

			var deserializer = new NameValueDeserializer();
			var emp = deserializer.Deserialize<Employee>(collection, "emp");

			Assert.IsNotNull(emp, "Employee should not be null.");
			Assert.IsNotNull(emp.Phone, "Employee phone should not be null.");
			Assert.IsNull(emp.BatPhone, "Employee OtherPhones should be null because it was not in request parameters.");
		}

		[Test]
		public void ShouldNotThrowWithNullValues()
		{
			var collection = new NameValueCollection { { null, null } };
			var deserializer = new NameValueDeserializer();

			deserializer.Deserialize(collection, "cust", typeof(Customer));
		}

		private class Customer
		{
			public int Id
			{
				get;
				set;
			}

			public Employee Employee { get; set; }
		}

		private class ArrayClass
		{
			public string Name
			{
				get;
				set;
			}

			public int[] Ids
			{
				get;
				set;
			}
		}

		private class BoolClass
		{
			public bool MyBool { get; set; }
		}

		private class GenericListClass
		{
			public string Name
			{
				get;
				set;
			}

			public IList<int> Ids
			{
				get;
				set;
			}

			private IList<int> _readonlyIds = null;
			public IList<int> ReadonlyIds
			{
				get { return _readonlyIds; }
			}
		}

		private class Employee
		{
			private readonly Phone _phone = new Phone();
			private readonly List<Phone> _otherPhones = new List<Phone>();

			public int Id
			{
				get;
				set;
			}

			public Phone Phone
			{
				get { return _phone; }
			}

			private Phone _batPhone = null;
			public Phone BatPhone
			{
				get { return _batPhone; }
			}

			public IList<Phone> OtherPhones
			{
				get { return _otherPhones; }
			}

			public Customer Customer { get; set; }

			private int _age;
			public int Age
			{
				get { return _age; }
				set
				{
					if (value < 0) throw new ArgumentException("Age must be greater than 0");
					_age = value;
				}
			}
		}

		public class Phone
		{
			private readonly List<string> _areaCodes = new List<string>();

			public string Number
			{
				get;
				set;
			}

			public IList<string> AreaCodes
			{
				get { return _areaCodes; }
			}
		}

		public enum TestEnum
		{
			One,
			Two,
			Three
		}
	}
}
