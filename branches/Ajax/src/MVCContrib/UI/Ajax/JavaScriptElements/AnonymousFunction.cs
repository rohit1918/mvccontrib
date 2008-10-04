namespace MvcContrib.UI.Ajax.JavaScriptElements
{
	/// <summary>
	/// Class that can be used to generate an anonymous javascript function.
	/// </summary>
	public class AnonymousFunction : IJavaScriptElement
	{
		private string _innerContents;

		/// <summary>
		/// Creates a new instance of the AnonymousFunction class, using the specified string as the inner contents of the function.
		/// </summary>
		/// <param name="innerContents">The inner contents of the function</param>
		public AnonymousFunction(string innerContents)
		{
			_innerContents = innerContents;
		}

		public string ToJavaScript()
		{
			return "function() { " + _innerContents + " }";
		}

	}
}