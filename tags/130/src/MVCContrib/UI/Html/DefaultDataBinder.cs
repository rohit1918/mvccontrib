using System;
using System.Collections;
using System.Reflection;
using System.Web.Mvc;

namespace MvcContrib.UI.Html
{
	//Disclaimer: Uses a modified version of MonoRail's FormHelper.ObtainValue.
	public class DefaultDataBinder : IDataBinder
	{
		public object NestedRootInstance { get; set; }
        
		public object ExtractValue(string target, ViewContext context)
		{
			if (target == null)
				return null;

			string[] pieces;

			object rootInstance = ObtainRootInstance(context, target, out pieces);

			//strongly typed viewdata
			if (rootInstance != null && rootInstance == context.ViewData.Model && pieces.Length > 0)
			{
				return QueryPropertyRecursive(rootInstance, pieces, 0);
			}
			else if (rootInstance != null && pieces.Length > 1)
			{
				return QueryPropertyRecursive(rootInstance, pieces, 1);
			}
			//We're in a nested scope
			else if (NestedRootInstance != null && NestedRootInstance.Equals(rootInstance))
			{
				return QueryPropertyRecursive(rootInstance, pieces, 0);
			}
			return rootInstance;
		}

		public IDisposable NestedBindingScope(object rootDataItem)
		{
			return new DefaultBindingScope(this, rootDataItem);
		}

		public object ObtainRootInstance(ViewContext context, string target)
		{
			if(NestedRootInstance != null)
			{
				return NestedRootInstance;
			}

			if(context.ViewData.Model != null)
			{
				return context.ViewData.Model;
			}

            object rootInstance = null;

			if (context.ViewData.ContainsKey(target))
				rootInstance = context.ViewData[target];
			else if (context.TempData.ContainsKey(target))
				rootInstance = context.TempData[target];


			return rootInstance;
		}

		public object ObtainRootInstance(ViewContext context, string target, out string[] pieces)
		{
			pieces = target.Split('.');

			string root = pieces[0];

			int index;

			bool isIndexed = CheckForExistenceAndExtractIndex(ref root, out index);

			object rootInstance = ObtainRootInstance(context, root);

			if (rootInstance == null)
			{
				return null;
			}

			if (isIndexed)
			{
				AssertIsValidArray(rootInstance, root, index);
			}

			if (!isIndexed && pieces.Length == 1)
			{
				return rootInstance;
			}
			else if (isIndexed)
			{
				rootInstance = GetArrayElement(rootInstance, index);
			}

			return rootInstance;
		}

		public object QueryPropertyRecursive(object rootInstance, string[] propertyPath, int piece)
		{
			string property = propertyPath[piece]; int index;

			Type instanceType = rootInstance.GetType();

			bool isIndexed = CheckForExistenceAndExtractIndex(ref property, out index);

			PropertyInfo propertyInfo = instanceType.GetProperty(property, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

			object instance = null;

			if (propertyInfo == null)
			{
				FieldInfo fieldInfo = instanceType.GetField(property, BindingFlags.GetField | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

				if (fieldInfo != null)
				{
					instance = fieldInfo.GetValue(rootInstance);
				}
			}
			else
			{
				if (!propertyInfo.CanRead)
				{
					throw new Exception(string.Format("Property '{0}' cannot be read", propertyInfo.Name));
				}

				if (propertyInfo.GetIndexParameters().Length != 0)
				{
					throw new Exception(string.Format("Property '{0}' has indexes, which are not supported", propertyInfo.Name, instanceType.FullName));
				}

				instance = propertyInfo.GetValue(rootInstance, null);
			}

			if (isIndexed && instance != null)
			{
				AssertIsValidArray(instance, property, index);

				instance = GetArrayElement(instance, index);
			}

			if (instance == null || piece + 1 == propertyPath.Length)
			{
				return instance;
			}

			return QueryPropertyRecursive(instance, propertyPath, piece + 1);
		}

		private static void AssertIsValidArray(object instance, string property, int index)
		{
			Type instanceType = instance.GetType();

			var list = instance as IList;

			bool validList = false;

			if (list == null && instanceType.IsGenericType)
			{
				Type[] genArgs = instanceType.GetGenericArguments();

				Type genList = typeof(System.Collections.Generic.IList<>).MakeGenericType(genArgs);
				Type genTypeDef = instanceType.GetGenericTypeDefinition().MakeGenericType(genArgs);

				validList = genList.IsAssignableFrom(genTypeDef);
			}

			if (!validList && list == null)
			{
				throw new Exception(
					string.Format(
						"The property {0} is being accessed as an indexed property but does not seem to implement IList. In fact the type is {1}",
						property, instanceType.Name));
			}

			if (index < 0)
			{
				throw new Exception(string.Format("The specified index '{0}' is outside the bounds of the array. Property {1}", index, property));
			}
		}

		private static bool CheckForExistenceAndExtractIndex(ref string property, out int index)
		{
			bool isIndexed = property.IndexOf('[') != -1;

			index = -1;

			if (isIndexed)
			{
				int start = property.IndexOf('[') + 1;
				int len = property.IndexOf(']', start) - start;

				string indexStr = property.Substring(start, len);

				try
				{
					index = Convert.ToInt32(indexStr);
				}
				catch (Exception)
				{
					throw new Exception(string.Format("Could not convert (param {0}) index to Int32. Value is {1}",
													  property, indexStr));
				}

				property = property.Substring(0, start - 1);
			}

			return isIndexed;
		}

		private static object GetArrayElement(object instance, int index)
		{
			var list = instance as IList;

			if (list == null && instance != null && instance.GetType().IsGenericType)
			{
				Type instanceType = instance.GetType();

				Type[] genArguments = instanceType.GetGenericArguments();

				Type genType = instanceType.GetGenericTypeDefinition().MakeGenericType(genArguments);

				// I'm not going to retest for IList implementation as 
				// if we got here, the AssertIsValidArray has run successfully

				PropertyInfo countPropInfo = genType.GetProperty("Count");

				var count = (int)countPropInfo.GetValue(instance, null);

				if (count == 0 || index + 1 > count)
				{
					return null;
				}

				PropertyInfo indexerPropInfo = genType.GetProperty("Item");

				return indexerPropInfo.GetValue(instance, new object[] { index });
			}

			if (list == null || list.Count == 0 || index + 1 > list.Count)
			{
				return null;
			}

			return list[index];
		}

		public class DefaultBindingScope : IDisposable
		{
			private readonly DefaultDataBinder _binder;
			private readonly object _originalRootInstance;

			public DefaultBindingScope(DefaultDataBinder binder, object newRootInstance)
			{
				_binder = binder;
				_originalRootInstance = binder.NestedRootInstance;
				binder.NestedRootInstance = newRootInstance;
			}

			public void Dispose()
			{
				_binder.NestedRootInstance = _originalRootInstance;
			}
		}
	}
}
