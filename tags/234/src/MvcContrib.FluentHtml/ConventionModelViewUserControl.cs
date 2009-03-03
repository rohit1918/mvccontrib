using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml
{
	public class ConventionModelViewUserControl<T> : ModelViewUserControl<T> where T : class
	{
		public ConventionModelViewUserControl()
			: base(new DefaultMaxLengthMemberBehavior(), new DefaultRequiredMemberBehavior()) { }
	}
}
