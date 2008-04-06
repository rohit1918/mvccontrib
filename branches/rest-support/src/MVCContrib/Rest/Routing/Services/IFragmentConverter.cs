using MvcContrib.Rest.Routing.Descriptors;
using MvcContrib.Rest.Routing.Fragments;

namespace MvcContrib.Rest.Routing.Services
{
	public interface IFragmentConverter
	{
		IFragment ToFragment(IFragmentDescriptor descriptor);
	}
}