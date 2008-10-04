using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcContrib.UI.Ajax.JavaScriptElements
{
	/// <summary>
	/// Dictionary that can be used to convert a set of key value pairs into a JavaScript object.
	/// </summary>
	public class JavaScriptDictionary : Dictionary<string, object>, IJavaScriptElement
	{
		/// <summary>
		/// Creates a new instance of the JavaScriptDictionary class
		/// </summary>
		public JavaScriptDictionary()
		{
		}

		/// <summary>
		/// Creates a new instance of the JavaScriptDictionary class using the specified IDictionary.
		/// </summary>
		/// <param name="dictionary">The underlying dictionary to use</param>
		public JavaScriptDictionary(IDictionary<string, object> dictionary) : base(dictionary)
		{
		}

		public string ToJavaScript()
		{
			var query = from pair in this
						let value = JavaScriptLiteral.FormatItem(pair.Value)
						where !string.IsNullOrEmpty(value)
			            select pair.Key + ":" + value;

			var output = string.Join(", ", query.ToArray());

			return "{" + output + "}";
		}
	}
}