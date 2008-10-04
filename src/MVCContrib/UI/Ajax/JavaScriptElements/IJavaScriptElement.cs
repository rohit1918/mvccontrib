namespace MvcContrib.UI.Ajax.JavaScriptElements
{
	/// <summary>
	/// An item that can be converted to JavaScript.
	/// </summary>
	public interface IJavaScriptElement
	{
		/// <summary>
		/// Converts the IJavaScriptElement to a string.
		/// </summary>
		/// <returns>A string representation of the javascript element</returns>
		string ToJavaScript();
	}
}