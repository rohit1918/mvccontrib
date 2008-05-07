using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using MvcContrib.NHamlViewEngine.Properties;
using MvcContrib.NHamlViewEngine.Utilities;

namespace MvcContrib.NHamlViewEngine.Exceptions
{
	[Serializable]
	public sealed class SyntaxException : Exception
	{
		public static void Throw(InputLine inputLine, string errorFormat, params object[] values)
		{
			string message = StringUtils.FormatCurrentCulture(Resources.SyntaxError,
																												inputLine.LineNumber,
																												StringUtils.FormatCurrentCulture(errorFormat, values),
																												inputLine.Text);

			throw new SyntaxException(message, inputLine);
		}

		private readonly InputLine _inputLine;

		public SyntaxException()
		{
		}

		public SyntaxException(string message)
			: base(message)
		{
		}

		public SyntaxException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		private SyntaxException(string message, InputLine inputLine)
			: base(message)
		{
			_inputLine = inputLine;
		}

		private SyntaxException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public InputLine InputLine
		{
			get { return _inputLine; }
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			info.AddValue("_line", _inputLine);
		}
	}
}