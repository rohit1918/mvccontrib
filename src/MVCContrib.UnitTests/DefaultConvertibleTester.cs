using System;
using System.Globalization;
using NUnit.Framework;

namespace MVCContrib.UnitTests
{
	[TestFixture]
	public class DefaultConvertibleTester
	{
		[Test]
		public void CanConvertBool()
		{
			DefaultConvertible convertible = new DefaultConvertible("true");
			bool value = convertible.ToBoolean(CultureInfo.InvariantCulture);

			Assert.IsTrue(value);
		}

		[Test]
		public void CanConvertChar()
		{
			DefaultConvertible convertible = new DefaultConvertible("t");
			char value = convertible.ToChar(CultureInfo.InvariantCulture);

			Assert.AreEqual('t', value);
		}

		[Test]
		public void CanConvertSByte()
		{
			DefaultConvertible convertible = new DefaultConvertible("1");
			sbyte value = convertible.ToSByte(CultureInfo.InvariantCulture);

			Assert.AreEqual(1, value);
		}

		[Test]
		public void CanConvertByte()
		{
			DefaultConvertible convertible = new DefaultConvertible("1");
			byte value = convertible.ToByte(CultureInfo.InvariantCulture);

			Assert.AreEqual(1, value);
		}

		[Test]
		public void CanConvertShort()
		{
			DefaultConvertible convertible = new DefaultConvertible("1");
			short value = convertible.ToInt16(CultureInfo.InvariantCulture);

			Assert.AreEqual(1, value);
		}

		[Test]
		public void CanConvertUShort()
		{
			DefaultConvertible convertible = new DefaultConvertible("1");
			ushort value = convertible.ToUInt16(CultureInfo.InvariantCulture);

			Assert.AreEqual(1, value);
		}

		[Test]
		public void CanConvertInt()
		{
			DefaultConvertible convertible = new DefaultConvertible("1");
			int value = convertible.ToInt32(CultureInfo.InvariantCulture);

			Assert.AreEqual(1, value);
		}

		[Test]
		public void CanConvertUInt()
		{
			DefaultConvertible convertible = new DefaultConvertible("1");
			uint value = convertible.ToUInt32(CultureInfo.InvariantCulture);

			Assert.AreEqual(1, value);
		}

		private enum WeAre
		{
			Lovin,
			Every,
			Minute,
			Of,
			It
		}

		[Test]
		public void CanConvertEnum()
		{
			DefaultConvertible convertible = new DefaultConvertible("Minute");
			WeAre value = (WeAre)convertible.ToEnum(typeof(WeAre));

			Assert.AreEqual(WeAre.Minute, value);
		}

		[Test]
		public void EnumDefaultsToDefault()
		{
			DefaultConvertible convertible = new DefaultConvertible("BowWowWowWow");
			WeAre value = (WeAre)convertible.ToEnum(typeof(WeAre));

			Assert.AreEqual(WeAre.Lovin, value);
		}

		[Test]
		public void CanConvertGuid()
		{
			Guid actual = Guid.NewGuid();
			DefaultConvertible convertible = new DefaultConvertible(actual.ToString());
			Guid value = convertible.ToGuid();

			Assert.AreEqual(actual, value);
		}

		[Test]
		public void NeverThrows()
		{
			DefaultConvertible convertible = new DefaultConvertible(null);

			convertible.ToBoolean(CultureInfo.InvariantCulture);
			convertible.ToByte(CultureInfo.InvariantCulture);
			convertible.ToDateTime(CultureInfo.InvariantCulture);
			convertible.ToDecimal(CultureInfo.InvariantCulture);
			convertible.ToDouble(CultureInfo.InvariantCulture);
			convertible.ToEnum(null);
			convertible.ToGuid();
			convertible.ToInt16(CultureInfo.InvariantCulture);
			convertible.ToInt32(CultureInfo.InvariantCulture);
			convertible.ToInt64(CultureInfo.InvariantCulture);
			convertible.ToSByte(CultureInfo.InvariantCulture);
			convertible.ToSingle(CultureInfo.InvariantCulture);
			convertible.ToString(CultureInfo.InvariantCulture);
			convertible.ToUInt16(CultureInfo.InvariantCulture);
			convertible.ToUInt32(CultureInfo.InvariantCulture);
			convertible.ToUInt64(CultureInfo.InvariantCulture);
		}
	}
}
