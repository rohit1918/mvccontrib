using MvcContrib.FluentHtml.Elements;
using MvcContrib.ModelAttributes;

namespace MvcContrib.FluentHtml.Behaviors
{
    public class DefaultRequiredMemberBehavior : IRequiredMemberBehaviorMarker
    {
        public void Execute(IMemberElement element)
        {
            var helper = new MemberBehaviorHelper<RequiredAttribute>();
            var attribute = helper.GetAttribute(element);
            if (attribute != null)
            {
                element.SetAttr("class", "required");
            }
        }
    }
}