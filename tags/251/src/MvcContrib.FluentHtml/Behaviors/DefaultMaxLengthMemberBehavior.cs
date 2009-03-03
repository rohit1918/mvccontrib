using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.ModelAttributes;

namespace MvcContrib.FluentHtml.Behaviors
{
	public class DefaultMaxLengthMemberBehavior : IMaxLengthMemberBehaviorMarker
	{
		public void Execute(IMemberElement element)
		{
			var helper = new MemberBehaviorHelper<MaxLengthAttribute>();
			var attribute = helper.GetAttribute(element);
			if (attribute == null)
			{
				return;
			}

			if(element is ISupportsMaxLength)
			{
				element.SetAttr(HtmlAttribute.MaxLength, attribute.Length);
			}
		}
	}
}