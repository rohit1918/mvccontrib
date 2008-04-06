using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcContrib.Rest.Routing.Ext
{
	/// <summary>Extension methods for RestfulRouting</summary>
	public static class RestfulRoutingExtensions
	{
		public static Regex CleanControllerNameRegex = new Regex("controller$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public static bool AttributeExists(this Type type, Type attributeType, bool inherit)
		{
			return type.GetCustomAttributes(inherit).Any(obj => obj.GetType() == attributeType);
		}

		public static bool AttributeExists<TAttribute>(this Type type)
		{
			return AttributeExists(type, typeof(TAttribute), false);
		}

		public static string RemoveTrailingControllerFromTypeName(this Type controllerType)
		{
			return CleanControllerNameRegex.Replace(controllerType.Name, "");
		}

		public static int LevelCount(this string pathPart)
		{
			if(string.IsNullOrEmpty(pathPart))
			{
				return 0;
			}
			string[] parts = pathPart.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
			return parts.Length;
		}

		/// <summary>Converts a camel cased name into a seo friendly name by replacing the camelcase with hyphens and making the name all lower case.</summary>
		/// <param name="camelCasedName">The camel cased name to urlalize</param>
		/// <returns>The camel cased name with the camel case replaced by hyphens and converted to all lower case.</returns>
		public static string ToSeoFriendlyUrlFragment(this string camelCasedName)
		{
			return string.IsNullOrEmpty(camelCasedName) ? "" : camelCasedName.Hyphenize().ToLower();
		}

		public static int IndexOf<T>(this IEnumerable<T> enumerable, T value)
		{
			return IndexOf(enumerable, item => value.Equals(item));
		}

		public static int IndexOf<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
		{
			int index = 0;
			foreach(T item in enumerable)
			{
				if(predicate(item))
				{
					return index;
				}
				index++;
			}
			return -1;
		}

		/// <summary>Gets the item at <paramref name="index"/> in the <paramref name="array"/>
		/// if <paramref name="index"/> is within the bounds of the <paramref name="array"/>, othwerise returns <c>default(T)</c></summary>
		/// <typeparam name="T">The type of items in the <paramref name="array"/></typeparam>
		/// <param name="array">The <see cref="Array"/> of items.</param>
		/// <param name="index">The position in the array to try and get an item from.</param>
		/// <returns>The item at array position <paramref name="index"/> if <paramref name="index"/> is in bounds otherwise <c>default(T)</c></returns>
		public static T AtOrDefault<T>(this T[] array, int index)
		{
			return AtOrDefault(array, index, default(T));
		}

		public static T AtOrDefault<T>(this T[] array, int index, T defaultValue)
		{
			if(array == null || index < 0 || index >= array.Length)
			{
				return defaultValue;
			}
			return array[index];
		}

		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			if(enumerable == null || action == null)
			{
				return;
			}

			foreach(T item in enumerable)
			{
				action(item);
			}
		}

		/// <summary>Enumerates through the <paramref name="enumerable"/> calling the <paramref name="action"/> and breaking the loop if <paramref name="action"/> returns <c>false</c></summary>
		/// <typeparam name="T">The type of item in the <see cref="IEnumerable{T}"/></typeparam>
		/// <param name="enumerable">The enumerable</param>
		/// <param name="action">The action to run.</param>
		/// <returns><c>false</c> as soon as <paramref name="action"/> returns <c>false</c> othwerise true.</returns>
		public static bool DoWhile<T>(this IEnumerable<T> enumerable, Func<T, bool> action)
		{
			if(enumerable == null || action == null)
			{
				return true;
			}

			foreach(T item in enumerable)
			{
				if(!action(item))
				{
					return false;
				}
			}

			return true;
		}

		public static bool DoWhileWithIndex<T>(this IEnumerable<T> enumerable, Func<T, int, bool> action)
		{
			return DoWhileWithIndex(enumerable, 0, 1, action);
		}

		public static bool DoWhileWithIndex<T>(this IEnumerable<T> enumerable, int indexStart, Func<T, int, bool> action)
		{
			return DoWhileWithIndex(enumerable, indexStart, 1, action);
		}

		public static bool DoWhileWithIndex<T>(this IEnumerable<T> enumerable, int indexStart, int indexIncrement,
		                                       Func<T, int, bool> action)
		{
			if(enumerable == null || action == null)
			{
				return true;
			}

			int index = indexStart;
			foreach(T item in enumerable)
			{
				int j = index;
				if(!action(item, j))
				{
					return false;
				}
				index += indexIncrement;
			}

			return true;
		}

		public static TReturn DoWhile<T, TReturn>(this IEnumerable<T> enumerable, Func<T, bool> action, TReturn onTrue,
		                                          TReturn onFalse)
		{
			return enumerable.DoWhile(action) ? onTrue : onFalse;
		}

		public static string Concat(this IEnumerable<string> collection)
		{
			return Concat(collection, s => s);
		}

		public static string Concat(this IEnumerable<string> collection, string seperator)
		{
			return Concat(collection, seperator, s => s);
		}

		public static string Concat<T>(this IEnumerable<T> collection, Func<T, string> whatToConcat)
		{
			return Concat(collection, "", whatToConcat);
		}

		public static string Concat<T>(this IEnumerable<T> collection, string seperator, Func<T, string> whatToConcat)
		{
			var buffer = new StringBuilder(64);
			collection.ForEach(item => buffer.Append(whatToConcat(item)).Append(seperator));
			if(!string.IsNullOrEmpty(seperator) && buffer.Length >= seperator.Length)
			{
				buffer.Remove(buffer.Length - seperator.Length, seperator.Length);
			}
			return buffer.ToString();
		}
	}
}