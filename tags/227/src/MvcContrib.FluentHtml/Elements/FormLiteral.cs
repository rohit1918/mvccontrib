namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generates a literal element (span) accompanied by a hidden input element having the same value.  Use this 
	/// if you want to display a value and also have that same value be included in the form post.
	/// </summary>
	public class FormLiteral : FormLiteralBase<FormLiteral>, IElement
	{
		/// <summary>
		/// Generates a literal element (span) accompanied by a hidden input element having the same value.  Use 
		/// this if you want to display a value and also have that same value be included in the form post.
		/// </summary>
		/// <param name="name">Value used to set the 'name' an 'id' attributes of the element.</param>
		public FormLiteral(string name) : base(name) { }
	}
}
