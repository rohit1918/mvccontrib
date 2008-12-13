using MvcContrib.FluentHtml.Elements;
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
			//NOTE: Is it better to use relfection here or IElement.SetAttr method?  What if it's a tag where maxlength is meaningless, like select?
			//NOTE: We could instead create a Set method for each type of HTML attribute in all types of IElement and have a heirarchy of interfaces to expose them.  Makes the API too noisy?
			//NOTE: This second reflection step will only happen when the model attibute is found.  Maybe not so bad?
			var method = helper.GetMethod(element, "MaxLength");
			if (method == null)
			{
				return;
			}
			helper.InvokeMethod(method, element, attribute.Length);
		}
	}
}
