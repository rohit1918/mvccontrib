using MvcContrib.Rest.Routing;
using MvcContrib.Rest.Routing.Descriptors;

namespace MvcContrib.Rest.Routing.Services
{
	public interface IRestfulRuleConverter
	{
		RestfulRoutingRule ToRestfulRoutingRule(IRestfulRuleDescriptor rule);
	}
}