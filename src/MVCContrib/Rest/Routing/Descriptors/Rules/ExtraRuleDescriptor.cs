using System.Collections.Generic;
using System.Linq;

namespace MvcContrib.Rest.Routing.Descriptors.Rules
{
	public abstract class ExtraRuleDescriptor : IRestfulRuleDescriptor
	{
		private readonly List<IFragmentDescriptor> _fragments = new List<IFragmentDescriptor>();

		protected List<IFragmentDescriptor> ListOfFragments
		{
			get { return _fragments; }
		}

		protected abstract IEnumerable<IFragmentDescriptor> DependantFragments { get; }

		#region IRestfulRuleDescriptor Members

		public IRestfulRuleSetDescriptor RuleSet { get; set; }

		public int Weight
		{
			get { return (from fragment in _fragments select fragment.Weight).Sum(); }
		}

		public int Levels
		{
			get { return Fragments.Count(); }
		}

		public virtual IEnumerable<IFragmentDescriptor> Fragments
		{
			get { return PrefixFragments.Concat(DependantFragments).Concat(ListOfFragments.AsEnumerable()); }
		}

		public IEnumerable<CustomRouteMatcher> CustomRouteMatchers
		{
			get { yield break; }
		}

		public IEnumerable<IFragmentDescriptor> PrefixFragments { get; set; }

		public IRestfulRuleDescriptor AddFragment(IFragmentDescriptor fragment)
		{
			_fragments.Add(fragment);
			return this;
		}

		#endregion
	}
}