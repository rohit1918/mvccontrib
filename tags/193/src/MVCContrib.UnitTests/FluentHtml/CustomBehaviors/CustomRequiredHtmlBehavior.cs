using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.ModelAttributes;

namespace MvcContrib.FluentHtml.Tests.CustomBehaviors
{
	public class CustomRequiredHtmlBehavior : IMemberBehavior
	{
		public void Execute(IMemberElement element)
		{
			var helper = new MemberBehaviorHelper<RequiredAttribute>();
			var attribute = helper.GetAttribute(element);
			if (attribute != null)
			{
				element.SetAttr("class", "req");
			}
		}
	}
}
