using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;

namespace MVCContrib.UnitTests
{
	[TestFixture]
	public class NameValueDeserializerTester
	{
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

		    public IList<Phone> OtherPhones
			{
				get { return _otherPhones; }
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
