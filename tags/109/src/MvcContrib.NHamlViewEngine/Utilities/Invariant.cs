using System;
using System.IO;
using MvcContrib.NHamlViewEngine.Properties;

namespace MvcContrib.NHamlViewEngine.Utilities
{
	internal static class Invariant
	{
		public static void ArgumentNotNull(object argument, string argumentName)
		{
			if(argument == null)
			{
				throw new ArgumentNullException(argumentName);
			}
		}

		public static void ArgumentNotEmpty(string argument, string argumentName)
		{
			if(argument == null)
			{
				throw new ArgumentNullException(argumentName);
			}

			if(argument.Length == 0)
			{
				throw new ArgumentOutOfRangeException(
					StringUtils.FormatCurrentCulture(Resources.StringCannotBeEmpty, argumentName));
			}
		}

		public static void IsNotNull(object obj)
		{
			if (obj == null)
			{
				throw new InvalidOperationException(Resources.ObjectNull);
			}
		}

		public static void FileExists(string path)
		{
			ArgumentNotEmpty(path, "path");

			if(!File.Exists(path))
			{
				throw new FileNotFoundException(path);
			}
		}
	}
}