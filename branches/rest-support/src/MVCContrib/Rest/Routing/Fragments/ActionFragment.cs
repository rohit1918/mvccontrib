using System.Collections;

namespace MvcContrib.Rest.Routing.Fragments
{
	public class ActionFragment : FragmentDecorator
	{
		public ActionFragment(IDictionary actionsAndAliases)
		{
			Inner = new RequiredPatternFragment("action", actionsAndAliases);
		}

		public ActionFragment(string defaultAction, IDictionary actionsAndAliases, bool isOptional)
		{
			if (isOptional)
			{
				Inner = new OptionalPatternFragment("action", defaultAction, actionsAndAliases);
			}
			else
			{
				Inner = new RequiredPatternFragment("action", actionsAndAliases);
			}
		}
	}
}