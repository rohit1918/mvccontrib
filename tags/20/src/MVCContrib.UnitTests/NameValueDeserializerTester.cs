using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using MvcContrib.Attributes;
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
			NameValueCollection collection = new NameValueCollection();
			collection["junk"] = "stuff";

			NameValueDeserializer nvd = new NameValueDeserializer();

			object notGonnaDoIt = nvd.Deserialize(collection, null, typeof(Customer));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyPrefixThrows()
		{
			NameValueCollection collection = new NameValueCollection();
			collection["junk"] = "stuff";

			NameValueDeserializer nvd = new NameValueDeserializer();

			object notGonnaDoIt = nvd.Deserialize(collection, string.Empty, typeof(Customer));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullTargetTypeThrows()
		{
			NameValueCollection collection = new NameValueCollection();
			collection["junk"] = "stuff";

			NameValueDeserializer nvd = new NameValueDeserializer();

			object notGonnaDoIt = nvd.Deserialize(collection, "junk", null);
		}

		[Test]
		public void ListPropertyIsSkippedIfNotInitializedAndReadOnly()
		{
			NameValueCollection collection = new NameValueCollection();
			collection["list.ReadonlyIds[0]"] = "10";
			collection["list.ReadonlyIds[1]"] = "20";

			NameValueDeserializer nvd = new NameValueDeserializer();

			GenericListClass list = nvd.Deserialize<GenericListClass>(collection, "list");

			Assert.IsNotNull(list);
			Assert.IsNull(list.ReadonlyIds);
		}

		[Test]
		public void ErrorsSettingPropertiesAreIgnored()
		{
			NameValueCollection collection = new NameValueCollection();
			collection["emp.Age"] = "-1";

			NameValueDeserializer nvd = new NameValueDeserializer();

			Employee emp = nvd.Deserialize<Employee>(collection, "emp");

			Assert.IsNotNull(emp);
			Assert.AreEqual(0, emp.Age);
		}

		[Test]
		public void ComplexPropertyIsSkippedIfNotInitializedAndReadOnly()
		{
			NameValueCollection collection = new NameValueCollection();
			collection["emp.BatPhone.Number"] = "800-DRK-KNGT";

			NameValueDeserializer nvd = new NameValueDeserializer();

			Employee emp = nvd.Deserialize<Employee>(collection, "emp");

			Assert.IsNotNull(emp);
			Assert.IsNull(emp.BatPhone);
		}

		[Test]
		public void DeserializeSimpleObject()
		{
			NameValueCollection collection = new NameValueCollection();
			collection["cust.Id"] = "10";

			NameValueDeserializer nvd = new NameValueDeserializer();

			Customer cust = nvd.Deserialize<Customer>(collection, "cust");

			Assert.IsNotNull(cust);
			Assert.AreEqual(10, cust.Id);
		}

		[Test]
		public void DeserializeSimpleArray()
		{
			NameValueCollection collection = new NameValueCollection();
			collection["array.Ids[0]"] = "10";
			collection["array.Ids[1]"] = "20";

			NameValueDeserializer nvd = new NameValueDeserializer();

			ArrayClass array = nvd.Deserialize<ArrayClass>(collection, "array");

			Assert.IsNotNull(array);
			Assert.AreEqual(2, array.Ids.Length);
			Assert.AreEqual(10, array.Ids[0]);
			Assert.AreEqual(20, array.Ids[1]);
		}

		[Test]
		public void DeserializePrimitiveArray()
		{
			NameValueCollection collection = new NameValueCollection();
			collection["ids[0]"] = "10";
			collection["ids[1]"] = "20";

			NameValueDeserializer nvd = new NameValueDeserializer();

			int[] array = (int[])nvd.Deserialize(collection, "ids", typeof(int[]));

			Assert.IsNotNull(array);
			Assert.AreEqual(2, array.Length);
			Assert.AreEqual(10, array[0]);
			Assert.AreEqual(20, array[1]);
		}

		[Test]
		public void DeserializePrimitiveGenericList()
		{
			NameValueCollection collection = new NameValueCollection();
			collection["ids[0]"] = "10";
			collection["ids[1]"] = "20";

			NameValueDeserializer nvd = new NameValueDeserializer();

			List<int> list = nvd.Deserialize<List<int>>(collection, "ids");

			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(10, list[0]);
			Assert.AreEqual(20, list[1]);
		}

		[Test]
		public void DeserializeSimpleGenericList()
		{
			NameValueCollection collection = new NameValueCollection();
			collection["list.Ids[0]"] = "10";
			collection["list.Ids[1]"] = "20";

			NameValueDeserializer nvd = new NameValueDeserializer();

			GenericListClass list = nvd.Deserialize<GenericListClass>(collection, "list");

			Assert.IsNotNull(list);
			Assert.AreEqual(2, list.Ids.Count);
			Assert.AreEqual(10, list.Ids[0]);
			Assert.AreEqual(20, list.Ids[1]);
		}

		[Test]
		public void DeserializeComplexGenericList()
		{
			NameValueCollection collection = new NameValueCollection();
			collection["emp.OtherPhones[0].Number"] = "800-555-1212";
			collection["emp.OtherPhones[1].Number"] = "800-867-5309";
			collection["emp.OtherPhones[1].AreaCodes[0]"] = "800";
			collection["emp.OtherPhones[1].AreaCodes[1]"] = "877";

			NameValueDeserializer nvd = new NameValueDeserializer();

			Employee emp = nvd.Deserialize<Employee>(collection, "emp");

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
			NameValueCollection collection = new NameValueCollection();
			collection["array.Name"] = "test";

			NameValueDeserializer nvd = new NameValueDeserializer();

			ArrayClass array = nvd.Deserialize<ArrayClass>(collection, "array");

			Assert.IsNotNull(array);
			Assert.AreEqual(0, array.Ids.Length);
		}

		[Test]
		public void DeserializeComplexObject()
		{
			NameValueCollection collection = new NameValueCollection();
			collection["emp.Id"] = "20";
			collection["emp.Phone.Number"] = "800-555-1212";

			NameValueDeserializer nvd = new NameValueDeserializer();

			Employee emp = nvd.Deserialize<Employee>(collection, "emp");

			Assert.IsNotNull(emp);
			Assert.AreEqual(20, emp.Id);
			Assert.AreEqual("800-555-1212", emp.Phone.Number);
		}

		[Test]
		public void EmptyValuesUseDefaultOfType()
		{
			NameValueCollection collection = new NameValueCollection();
			collection["cust.Id"] = "";

			NameValueDeserializer nvd = new NameValueDeserializer();

			Customer cust = nvd.Deserialize<Customer>(collection, "cust");

			Assert.IsNotNull(cust);
			Assert.AreEqual(0, cust.Id);
		}

		[Test]
		public void NoMatchingValuesReturnsNewedObject()
		{
			NameValueCollection collection = new NameValueCollection();
			collection["cust.Id"] = "10";

			NameValueDeserializer nvd = new NameValueDeserializer();

			Customer cust = nvd.Deserialize<Customer>(collection, "mash");

			Assert.IsNotNull(cust);
		}

		[Test]
		public void EmptyCollectionReturnsNull()
		{
			NameValueDeserializer nvd = new NameValueDeserializer();

			Customer cust = nvd.Deserialize<Customer>(null, "cust");

			Assert.IsNull(cust);
		}

		[Test]
		public void ForCompleteness()
		{
			DeserializeAttribute attribute = new DeserializeAttribute("test");

			Assert.AreEqual("test", attribute.Prefix);
		}

		private class Customer
		{
			public int Id
			{
				get;
				set;
			}
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

			private IList<int> _readonlyIds;
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

			private Phone _batPhone;
			public Phone BatPhone
			{
				get { return _batPhone; }
			}

			public IList<Phone> OtherPhones
			{
				get { return _otherPhones; }
			}

			private int _age;
			public int Age
			{
				get { return _age; }
				set
				{
					if (value < 0 ) throw new ArgumentException("Age must be greater than 0");
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
	}
}