namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Generate a validation message (text inside a span element).
	/// </summary>
	public class ValidationMessage: LiteralBase<ValidationMessage>, IElement
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