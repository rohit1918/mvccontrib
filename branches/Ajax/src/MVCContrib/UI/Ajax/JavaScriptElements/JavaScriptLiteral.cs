namespace MvcContrib.UI.Ajax.JavaScriptElements
{
	/// <summary>
	/// Literal javascript element.
	/// </summary>
	public class JavaScriptLiteral : IJavaScriptElement
	{
		private readonly string _contents;

		/// <summary>
		/// Creates a new instance of the JavaScriptLiteral class.
		/// </summary>
		/// <param name="contents">The contents of the literal</param>
		public JavaScriptLiteral(string contents)
		{
			_contents = contents;
		}

		public string ToJavaScript()
		{
			return _contents;
		}


		/// <summary>
		/// Takes an object and converts it to a javascript string. 
		/// If it is a string, it is enclosed in single quotes.
		/// If it is an instance of IJavaScriptElement, the ToJavaScript method is called.
		/// If it is null or an empty string, null is returned
		/// In all other cases, ToString is called.
		/// </summary>
		/// <param name="item">The item to format</param>
		/// <returns></returns>
		public static string FormatItem(object item)
		{
			if (item == null || string.Empty.Equals(item)) return null;

			if(item is string)
			{
				return "'" + item + "'";
			}
			if(item is IJavaScriptElement)
			{
				return ((IJavaScriptElement)item).ToJavaScript();
			}

			return item.ToString();
		}
	}
}