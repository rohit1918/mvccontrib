using System;
using MvcContrib.Services;

namespace MvcContrib.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class FilterAttribute : Attribute
	{
		private int _executionOrder = 50;
		private readonly Type _filterType;

		public int ExecutionOrder
		{
			get { return _executionOrder; }
			set { _executionOrder = value; }
		}

		public Type FilterType
		{
			get { return _filterType; }
		}

		public FilterAttribute(Type filterType)
		{
			_filterType = filterType;
		}

		public IFilter CreateFilter()
		{
			IFilter filter = DependencyResolver.GetImplementationOf<IFilter>(FilterType);

			if(filter is IFilterAttributeAware)
				((IFilterAttributeAware)filter).Attribute = this;

			return filter;
		}
	}
}
