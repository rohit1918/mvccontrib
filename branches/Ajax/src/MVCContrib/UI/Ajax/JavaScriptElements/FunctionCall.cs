using System.Linq;

namespace MvcContrib.UI.Ajax.JavaScriptElements
{
	/// <summary>
	/// A JavaScript function call.
	/// <example>
	/// new FunctionCall("Foo", 1, "Two", new JavaScriptDictionary { {"foo", "bar"}, {"baz", "blah"} });
	/// //would generate Foo(1, 'Two', {foo:'bar', baz:'blah'});
	/// </example>
	/// </summary>
	public class FunctionCall : IJavaScriptElement
	{
		private readonly object[] _args;
		private readonly string _name;
		private const string _format = "{0}({1});";

		/// <summary>
		/// Creates a new instance of the FunctionCall class.
		/// </summary>
		/// <param name="name">The name of the function</param>
		public FunctionCall(string name) : this(name, null)
		{
		}

		/// <summary>
		/// Creates a new instance of the FunctionCall class
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="args">Function arguments</param>
		public FunctionCall(string name, params object[] args)
		{
			_args = args;
			_name = name;
		}

		public string ToJavaScript()
		{
			return string.Format(_format, _name, FormatArgs());
		}

		private string FormatArgs()
		{
			if(_args == null) return string.Empty;

			var argsFormatted = _args.Select(arg => JavaScriptLiteral.FormatItem(arg)).ToArray();

			return string.Join(", ", argsFormatted);
		}
	}
}