using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace MVCContrib
{
	public class NameValueDeserializer
	{
		protected virtual IConvertible GetConvertible(string sValue)
		{
			return new DefaultConvertible(sValue);
		}

		public T Deserialize<T>(NameValueCollection collection, string prefix) where T : new()
		{
			return (T)Deserialize(collection, prefix, typeof(T));
		}

		public object Deserialize(NameValueCollection collection, string prefix, Type targetType)
		{
			if(collection == null || collection.Count == 0) return null;

			if(string.IsNullOrEmpty("prefix")) throw new ArgumentException("prefix is requried");

			if(targetType == null) throw new ArgumentNullException("targetType");

			if(targetType.IsArray)
			{
				Type elementType = targetType.GetElementType();
				ArrayList arrayInstance = DeserializeArrayList(collection, prefix, elementType);
				return arrayInstance.ToArray(elementType);
			}
			else if(IsGenericList(targetType))
			{
				IList genericListInstance = CreateGenericListInstance(targetType);
				DeserializeGenericList(collection, prefix, targetType, ref genericListInstance);
				return genericListInstance;
			}
			else
			{
				object instance = null;
				Deserialize(collection, prefix, targetType, ref instance);
				return instance;
			}
		}

		protected virtual void Deserialize(NameValueCollection collection, string prefix, Type targetType, ref object instance)
		{
			if(instance == null)
			{
				instance = CreateInstance(targetType);
			}

			PropertyInfo[] properties = GetProperties(targetType);

			foreach(PropertyInfo property in properties)
			{
				string name = string.Concat(prefix, ".", property.Name);
				Type propertyType = property.PropertyType;

				if(IsSimpleProperty(propertyType))
				{
					string sValue = collection.Get(name);
					if(sValue != null)
					{
						SetValue(instance, property, GetConvertible(sValue));
					}
				}
				else if(propertyType.IsArray)
				{
					Type elementType = propertyType.GetElementType();

					ArrayList arrayInstance = DeserializeArrayList(collection, name, elementType);

					SetValue(instance, property, arrayInstance.ToArray(elementType));
				}
				else if(IsGenericList(propertyType))
				{
					IList genericListProperty = GetGenericListProperty(instance, property);

					if(genericListProperty == null) continue;

					DeserializeGenericList(collection, name, propertyType, ref genericListProperty);
				}
				else
				{
					object complexProperty = GetComplexProperty(instance, property);

					if(complexProperty == null) continue;

					Deserialize(collection, name, propertyType, ref complexProperty);
				}
			}
		}

		protected virtual void DeserializeGenericList(NameValueCollection collection, string prefix, Type targetType,
		                                              ref IList instance)
		{
			Type elementType = targetType.GetGenericArguments()[0];

			ArrayList arrayInstance = DeserializeArrayList(collection, prefix, elementType);

			foreach(object inst in arrayInstance)
			{
				instance.Add(inst);
			}
		}

		protected virtual IList GetGenericListProperty(object instance, PropertyInfo property)
		{
			// If property is already initialized (via object's constructor) use that
			// Otherwise attempt to new it
			IList genericListProperty = property.GetValue(instance, null) as IList;
			if(genericListProperty == null)
			{
				genericListProperty = CreateGenericListInstance(property.PropertyType);
				if(!SetValue(instance, property, genericListProperty))
				{
					return null;
				}
			}
			return genericListProperty;
		}

		protected virtual IList CreateGenericListInstance(Type targetType)
		{
			Type elementType = targetType.GetGenericArguments()[0];

			Type desiredListType = targetType.IsInterface
			                       	? typeof(List<>).MakeGenericType(elementType)
			                       	: targetType;

			return CreateInstance(desiredListType) as IList;
		}

		protected virtual bool IsGenericList(Type instanceType)
		{
			if(!instanceType.IsGenericType)
			{
				return false;
			}

			if(typeof(IList).IsAssignableFrom(instanceType))
			{
				return true;
			}

			Type[] genericArgs = instanceType.GetGenericArguments();

			if(genericArgs.Length == 0)
			{
				return false;
			}

			Type listType = typeof(IList<>).MakeGenericType(genericArgs[0]);

			return listType.IsAssignableFrom(instanceType);
		}

		protected virtual ArrayList DeserializeArrayList(NameValueCollection collection, string prefix, Type elementType)
		{
			string[] arrayPrefixes = GetArrayPrefixes(collection, prefix);

			ArrayList arrayInstance = new ArrayList(arrayPrefixes.Length);

			foreach(string arrayPrefix in arrayPrefixes)
			{
				object inst = null;

				if(IsSimpleProperty(elementType))
				{
					string sValue = collection.Get(arrayPrefix);
					if(sValue != null)
					{
						inst = GetConvertible(sValue).ToType(elementType, CultureInfo.CurrentCulture);
					}
				}
				else
				{
					inst = Deserialize(collection, arrayPrefix, elementType);
				}

				if(inst != null)
				{
					arrayInstance.Add(inst);
				}
			}

			return arrayInstance;
		}

		protected virtual string[] GetArrayPrefixes(NameValueCollection collection, string prefix)
		{
			List<string> arrayPrefixes = new List<string>();

			prefix = string.Concat(prefix, "[").ToLower();
			int prefixLength = prefix.Length;
			string[] names = collection.AllKeys;
			foreach(string name in names)
			{
				if(name.IndexOf(prefix, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					int bracketIndex = name.IndexOf(']', prefixLength);
					if(bracketIndex > prefixLength)
					{
						string arrayPrefix = name.Substring(0, bracketIndex + 1).ToLower();
						if(!arrayPrefixes.Contains(arrayPrefix))
						{
							arrayPrefixes.Add(arrayPrefix);
						}
					}
				}
			}

			return arrayPrefixes.ToArray();
		}

		private static readonly Dictionary<Type, PropertyInfo[]> _cachedProperties = new Dictionary<Type, PropertyInfo[]>();
		private static readonly object _syncRoot = new object();

		protected static PropertyInfo[] GetProperties(Type targetType)
		{
			PropertyInfo[] properties;
			if(!_cachedProperties.TryGetValue(targetType, out properties))
			{
				lock(_syncRoot)
				{
					if(!_cachedProperties.TryGetValue(targetType, out properties))
					{
						properties = targetType.GetProperties();
						_cachedProperties.Add(targetType, properties);
					}
				}
			}
			return properties;
		}

		protected virtual bool SetValue(object instance, PropertyInfo property, object value)
		{
			if(property.CanWrite)
			{
				property.SetValue(instance, value, null);
				return true;
			}

			return false;
		}

		protected virtual bool SetValue(object instance, PropertyInfo property, IConvertible oValue)
		{
			try
			{
				object convertedValue = oValue.ToType(property.PropertyType, CultureInfo.CurrentCulture);
				return SetValue(instance, property, convertedValue);
			}
			catch(InvalidCastException)
			{
			}
			catch(FormatException)
			{
			}

			return false;
		}

		protected virtual object CreateInstance(Type targetType)
		{
			return Activator.CreateInstance(targetType);
		}

		protected virtual object GetComplexProperty(object instance, PropertyInfo property)
		{
			// If property is already initialized (via object's constructor) use that
			// Otherwise attempt to new it
			object complexProperty = property.GetValue(instance, null);
			if(complexProperty == null)
			{
				complexProperty = CreateInstance(property.PropertyType);
				if(!SetValue(instance, property, complexProperty))
				{
					return null;
				}
			}
			return complexProperty;
		}

		protected virtual bool IsSimpleProperty(Type propertyType)
		{
			if(propertyType.IsArray)
			{
				return false;
			}

			bool isSimple = propertyType.IsPrimitive ||
			                propertyType.IsEnum ||
			                propertyType == typeof(String) ||
			                propertyType == typeof(Guid) ||
			                propertyType == typeof(DateTime) ||
			                propertyType == typeof(Decimal);

			if(isSimple)
			{
				return true;
			}

			TypeConverter tconverter = TypeDescriptor.GetConverter(propertyType);

			return tconverter.CanConvertFrom(typeof(String));
		}
	}
}
