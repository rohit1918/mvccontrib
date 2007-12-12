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
			DefaultConvertible convertible = new DefaultConvertible(sbyte.MaxValue.ToString());
			sbyte value = convertible.ToSByte(CultureInfo.InvariantCulture);

			Assert.AreEqual(sbyte.MaxValue, value);
		}

		[Test]
		public void CanConvertByte()
		{
			DefaultConvertible convertible = new DefaultConvertible(byte.MaxValue.ToString());
			byte value = convertible.ToByte(CultureInfo.InvariantCulture);

			Assert.AreEqual(byte.MaxValue, value);
		}

		[Test]
		public void CanConvertShort()
		{
			DefaultConvertible convertible = new DefaultConvertible(short.MaxValue.ToString());
			short value = convertible.ToInt16(CultureInfo.InvariantCulture);

			Assert.AreEqual(short.MaxValue, value);
		}

		[Test]
		public void CanConvertUShort()
		{
			DefaultConvertible convertible = new DefaultConvertible(ushort.MaxValue.ToString());
			ushort value = convertible.ToUInt16(CultureInfo.InvariantCulture);

			Assert.AreEqual(ushort.MaxValue, value);
		}

		[Test]
		public void CanConvertInt()
		{
			DefaultConvertible convertible = new DefaultConvertible(int.MaxValue.ToString());
			int value = convertible.ToInt32(CultureInfo.InvariantCulture);

			Assert.AreEqual(int.MaxValue, value);
		}

		[Test]
		public void CanConvertUInt()
		{
			DefaultConvertible convertible = new DefaultConvertible(uint.MaxValue.ToString());
			uint value = convertible.ToUInt32(CultureInfo.InvariantCulture);

			Assert.AreEqual(uint.MaxValue, value);
		}

		[Test]
		public void CanConvertLong()
		{
			DefaultConvertible convertible = new DefaultConvertible(long.MaxValue.ToString());
			long value = convertible.ToInt64(CultureInfo.InvariantCulture);

			Assert.AreEqual(long.MaxValue, value);
		}

		[Test]
		public void CanConvertULong()
		{
			DefaultConvertible convertible = new DefaultConvertible(ulong.MaxValue.ToString());
			ulong value = convertible.ToUInt64(CultureInfo.InvariantCulture);

			Assert.AreEqual(ulong.MaxValue, value);
		}

		[Test]
		public void CanConvertFloat()
		{
			DefaultConvertible convertible = new DefaultConvertible("1.1");
			float value = convertible.ToSingle(CultureInfo.InvariantCulture);

			Assert.AreEqual(1.1F, value);
		}

		[Test]
		public void CanConvertDouble()
		{
			DefaultConvertible convertible = new DefaultConvertible("1.2");
			double value = convertible.ToDouble(CultureInfo.InvariantCulture);

			Assert.AreEqual(1.2, value);
		}

		[Test]
		public void CanConvertDecimal()
		{
			DefaultConvertible convertible = new DefaultConvertible("1.3");
			decimal value = convertible.ToDecimal(CultureInfo.InvariantCulture);

			Assert.AreEqual(1.3M, value);
		}

		[Test]
		public void CanConvertDateTime()
		{
			DefaultConvertible convertible = new DefaultConvertible("11/05/1605");
			DateTime value = convertible.ToDateTime(CultureInfo.InvariantCulture);
			DateTime actual = new DateTime(1605, 11, 5);

			Assert.AreEqual(actual, value);
		}

		[Test]
		public void ReturnsCorrectTypeCode()
		{
			DefaultConvertible convertible = new DefaultConvertible(string.Empty);
			TypeCode value = convertible.GetTypeCode();

			Assert.AreEqual(TypeCode.String, value);
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
		public void EnumDefaultsToDefaultWhenNull()
		{
			DefaultConvertible convertible = new DefaultConvertible(null);
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
		public void GuidDefaultsToEmpty()
		{
			DefaultConvertible convertible = new DefaultConvertible("GetSome");
			Guid value = convertible.ToGuid();

			Assert.AreEqual(Guid.Empty, value);
		}

		[Test]
		public void GuidDefaultsToEmptyWhenNull()
		{
			DefaultConvertible convertible = new DefaultConvertible(null);
			Guid value = convertible.ToGuid();

			Assert.AreEqual(Guid.Empty, value);
		}

		[Test]
		public void FallsBackToTypeConverter()
		{
			DefaultConvertible convertible = new DefaultConvertible("1.02:03:04.005");
			TimeSpan value = (TimeSpan)convertible.ToType(typeof(TimeSpan), CultureInfo.InvariantCulture);
			TimeSpan actual = new TimeSpan(1, 2, 3, 4, 5);

			Assert.AreEqual(actual, value);
		}

		[Test]
		public void NeverThrows()
		{
			DefaultConvertible convertible = new DefaultConvertible(null);

			convertible.ToChar(CultureInfo.InvariantCulture);
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
