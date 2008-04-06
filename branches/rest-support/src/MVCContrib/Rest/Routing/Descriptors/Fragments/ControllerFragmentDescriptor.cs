namespace MvcContrib.Rest.Routing.Descriptors.Fragments
{
	public class ControllerFragmentDescriptor : BaseFragmentDescriptor
	{
		public ControllerFragmentDescriptor(IRestfulRuleDescriptor rule) : base(rule)
		{
			Name = "controller";
		}
	}
}