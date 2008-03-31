using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;
using MvcContrib.Castle;
using NUnit.Framework;

namespace MvcContrib.UnitTests.ViewFactories
{
	[TestFixture, NUnit.Framework.Category("NVelocityViewEngine")]
	public class DescriptableDictionaryTester
	{
		[Test]
		public void Creates_One_Property_For_Each_Dictionary_Entry()
		{
			Hashtable hash = new Hashtable();
			hash["Prop1"] = 1;
			hash["Prop2"] = "a";

			DescriptableDictionary dict = new DescriptableDictionary(hash);

			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(dict);
			Assert.AreEqual(2, properties.Count);
			Assert.AreEqual(1, properties[0].GetValue(dict));
			Assert.AreEqual("a", properties[1].GetValue(dict));
		}

		[Test]
		public void ForCoverage()
		{
			Hashtable hash = new Hashtable();
			hash["Prop1"] = 1;
			DescriptableDictionary dict = new DescriptableDictionary(hash);
			PropertyDescriptor property = TypeDescriptor.GetProperties(dict)[0];

			property.CanResetValue(null);
			object o = property.ComponentType;
			o = property.IsReadOnly;
			o = property.PropertyType;
			property.ResetValue(null);
			property.SetValue(null, null);
			property.ShouldSerializeValue(null);

			dict = new DescriptableDictionary(new SerializationInfo(typeof(Hashtable), new FormatterConverter()), new StreamingContext());
			dict.GetAttributes();
			dict.GetClassName();
			dict.GetComponentName();
			dict.GetConverter();
			dict.GetDefaultEvent();
			dict.GetDefaultProperty();
			dict.GetEditor(typeof(string));
			dict.GetEvents();
			dict.GetEvents(new Attribute[] {});
			dict.GetPropertyOwner(null);
		}
	}
}