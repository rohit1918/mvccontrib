using System.Collections.Generic;
using System.Linq;

namespace MvcContrib.Rest.Routing.Descriptors.Fragments
{
	public class RuleFragmentDescriptor : IRuleFragmentDescriptor
	{
		private IEnumerable<IFragmentDescriptor> _fragments;

		#region IRuleFragmentDescriptor Members

		public void AddFragment(IFragmentDescriptor fragment)
		{
			if(_fragments == null)
			{
				_fragments = new[] {fragment};
			}
			else
			{
				_fragments = _fragments.Concat(new[] {fragment});
			}
		}

		public void AddNestedRule(IRuleFragmentDescriptor rule)
		{
			if(_fragments == null)
			{
				_fragments = rule.Fragments;
			}
			else
			{
				_fragments = _fragments.Concat(rule.Fragments);
			}
		}

		public IEnumerable<IFragmentDescriptor> Fragments
		{
			get { return _fragments ?? Enumerable.Empty<IFragmentDescriptor>(); }
		}

		#endregion
	}
}