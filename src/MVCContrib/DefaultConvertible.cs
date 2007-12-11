using System;
using System.ComponentModel;
using System.Globalization;

namespace MVCContrib
{
	public class DefaultConvertible : IConvertible
	{
		private static readonly Type[] ConvertTypes = new Type[]
			{
				typeof(bool),
				typeof(char),
				typeof(sbyte),
				typeof(byte),
				typeof(short),
				typeof(ushort),
				typeof(int),
				typeof(uint),
				typeof(long),
				typeof(ulong),
				typeof(float),
				typeof(double),
				typeof(decimal),
				typeof(DateTime),
				typeof(string),
				typeof(Enum),
				typeof(Guid)
			};

		private readonly string _value;

		public DefaultConvertible(string value)
		{
			_value = value;
		}

		public TypeCode GetTypeCode()
		{
			return _value.GetTypeCode();
		}

		public bool ToBoolean(IFormatProvider provider)
		{
			bool oValue;
			if(bool.TryParse(_value, out oValue))
			{
				return oValue;
			}

			return new bool();
		}

		public char ToChar(IFormatProvider provider)
		{
			if(_value == null)
			{
				return new char();
			}

			return _value[0];
		}

		public sbyte ToSByte(IFormatProvider provider)
		{
			sbyte oValue;
			if(sbyte.TryParse(_value, NumberStyles.Any, provider, out oValue))
			{
				return oValue;
			}

			return new sbyte();
		}

		public byte ToByte(IFormatProvider provider)
		{
			byte oValue;
			if(byte.TryParse(_value, NumberStyles.Any, provider, out oValue))
			{
				return oValue;
			}

			return new byte();
		}

		public short ToInt16(IFormatProvider provider)
		{
			short oValue;
			if(short.TryParse(_value, NumberStyles.Any, provider, out oValue))
			{
				return oValue;
			}

			return new short();
		}

		public ushort ToUInt16(IFormatProvider provider)
		{
			ushort oValue;
			if(ushort.TryParse(_value, NumberStyles.Any, provider, out oValue))
			{
				return oValue;
			}

			return new ushort();
		}

		public int ToInt32(IFormatProvider provider)
		{
			int oValue;
			if(int.TryParse(_value, NumberStyles.Any, provider, out oValue))
			{
				return oValue;
			}

			return new int();
		}

		public uint ToUInt32(IFormatProvider provider)
		{
			uint oValue;
			if(uint.TryParse(_value, NumberStyles.Any, provider, out oValue))
			{
				return oValue;
			}

			return new uint();
		}

		public long ToInt64(IFormatProvider provider)
		{
			long oValue;
			if(long.TryParse(_value, NumberStyles.Any, provider, out oValue))
			{
				return oValue;
			}

			return new long();
		}

		public ulong ToUInt64(IFormatProvider provider)
		{
			ulong oValue;
			if(ulong.TryParse(_value, NumberStyles.Any, provider, out oValue))
			{
				return oValue;
			}

			return new ulong();
		}

		public float ToSingle(IFormatProvider provider)
		{
			float oValue;
			if(float.TryParse(_value, NumberStyles.Any, provider, out oValue))
			{
				return oValue;
			}

			return new float();
		}

		public double ToDouble(IFormatProvider provider)
		{
			double oValue;
			if(double.TryParse(_value, NumberStyles.Any, provider, out oValue))
			{
				return oValue;
			}

			return new double();
		}

		public decimal ToDecimal(IFormatProvider provider)
		{
			decimal oValue;
			if(decimal.TryParse(_value, NumberStyles.Any, provider, out oValue))
			{
				return oValue;
			}

			return new decimal();
		}

		public DateTime ToDateTime(IFormatProvider provider)
		{
			DateTime oValue;
			if(DateTime.TryParse(_value, provider, DateTimeStyles.None, out oValue))
			{
				return oValue;
			}

			return new DateTime();
		}

		public string ToString(IFormatProvider provider)
		{
			return _value;
		}

		public object ToEnum(Type conversionType)
		{
			if( conversionType == null || !conversionType.IsEnum)
			{
				return null;
			}

			if(_value == null)
			{
				return Enum.ToObject(conversionType, 0);
			}

			try
			{
				return Enum.Parse(conversionType, _value, true);
			}
			catch(ArgumentException)
			{
				return Enum.ToObject(conversionType, 0);
			}
		}

		public Guid ToGuid()
		{
			if(_value == null)
			{
				return Guid.Empty;
			}

			try
			{
				return new Guid(_value);
			}
			catch(FormatException)
			{
			}
			catch(OverflowException)
			{
			}

			return Guid.Empty;
		}

		public object WithTypeConverter(Type conversionType)
		{
			if(_value == null) return null;

			TypeConverter typeConverter = TypeDescriptor.GetConverter(conversionType);
			if (typeConverter == null || !typeConverter.CanConvertFrom(ConvertTypes[14]))
			{
				return null;
			}

			return typeConverter.ConvertFromString(_value);
		}

		public virtual object ToType(Type conversionType, IFormatProvider provider)
		{
			if(conversionType == ConvertTypes[0])
			{
				return ToBoolean(provider);
			}
			if(conversionType == ConvertTypes[1])
			{
				return ToChar(provider);
			}
			if(conversionType == ConvertTypes[2])
			{
				return ToSByte(provider);
			}
			if(conversionType == ConvertTypes[3])
			{
				return ToByte(provider);
			}
			if(conversionType == ConvertTypes[4])
			{
				return ToInt16(provider);
			}
			if(conversionType == ConvertTypes[5])
			{
				return ToUInt16(provider);
			}
			if(conversionType == ConvertTypes[6])
			{
				return ToInt32(provider);
			}
			if(conversionType == ConvertTypes[7])
			{
				return ToUInt32(provider);
			}
			if(conversionType == ConvertTypes[8])
			{
				return ToInt64(provider);
			}
			if(conversionType == ConvertTypes[9])
			{
				return ToUInt64(provider);
			}
			if(conversionType == ConvertTypes[10])
			{
				return ToSingle(provider);
			}
			if(conversionType == ConvertTypes[11])
			{
				return ToDouble(provider);
			}
			if(conversionType == ConvertTypes[12])
			{
				return ToDecimal(provider);
			}
			if(conversionType == ConvertTypes[13])
			{
				return ToDateTime(provider);
			}
			if(conversionType == ConvertTypes[14])
			{
				return ToString(provider);
			}
			if(conversionType == ConvertTypes[16])
			{
				return ToEnum(conversionType);
			}
			if(conversionType == ConvertTypes[17])
			{
				return ToGuid();
			}

			return WithTypeConverter(conversionType);
		}
	}
}
