using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml
{
	public class ConventionModelViewUserContol<T> : ModelViewUserControl<T> where T : class
	{
		public ConventionModelViewUserContol() 
			: base(new DefaultMaxLengthMemberBehavior(), new DefaultRequiredMemberBehavior())
		{
            memberBehaviors.Add(new ValidationMemberBehavior(ViewData.ModelState));
		}
	}
}
