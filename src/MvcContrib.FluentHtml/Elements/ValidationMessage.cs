namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate a validation message (text inside a span element).
	/// </summary>
	public class ValidationMessage: Literal, IElement
	{
		public override string ToString()
		{
			if (rawValue == null)
			{
				return null;
			}
			if (!builder.Attributes.ContainsKey("class"))
			{
				Class("field-validation-error");
			}
			return base.ToString();
		}
	}
}