using System.Collections.Generic;

namespace MvcContrib.Rest.Routing.Descriptors
{
	public interface IRuleFragmentDescriptor
	{
		IEnumerable<IFragmentDescriptor> Fragments { get; }
		void AddFragment(IFragmentDescriptor fragment);

		void AddNestedRule(IRuleFragmentDescriptor rule);
	}
}