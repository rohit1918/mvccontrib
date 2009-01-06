using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml
{
	public class ConventionModelViewMasterPage<T> : ModelViewMasterPage<T> where T : class
	{
		public ConventionModelViewMasterPage() 
			: base(new DefaultMaxLengthMemberBehavior(), new DefaultRequiredMemberBehavior())
		{
            memberBehaviors.Add(new ValidationMemberBehavior(ViewData.ModelState));
		}
	}
}
