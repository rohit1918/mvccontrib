namespace MvcContrib.Rest.Routing.Descriptors.Fragments
{
	public class StaticFragmentDescriptor : BaseFragmentDescriptor
	{
		public StaticFragmentDescriptor(IRestfulRuleDescriptor rule, string path) : base(rule)
		{
			UrlPart = path;
		}
	}
}