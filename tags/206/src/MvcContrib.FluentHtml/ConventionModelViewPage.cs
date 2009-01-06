using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml
{
    public class ConventionModelViewPage<T> : ModelViewPage<T> where T : class
    {
        public ConventionModelViewPage()
            : base(new DefaultMaxLengthMemberBehavior(), new DefaultRequiredMemberBehavior())
        {
            memberBehaviors.Add(new ValidationMemberBehavior(() => ViewData.ModelState));
        }
    }
}
