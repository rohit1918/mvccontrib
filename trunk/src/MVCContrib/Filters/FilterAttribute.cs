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
			try
			{
				IFilter filter;

				//If no dependency resoler is present, try and use Activator.
				if(DependencyResolver.Resolver == null)
				{
					filter = (IFilter)Activator.CreateInstance(FilterType);
				}
				else
				{
					filter = DependencyResolver.Resolver.GetImplementationOf<IFilter>(FilterType);
				}

				if(filter is IFilterAttributeAware)
					((IFilterAttributeAware)filter).Attribute = this;

				return filter;
			}
			catch(Exception ex)
			{
				string exceptionMessage =
					string.Format("Could not create filter of type '{0}'. If you are using the Dependency Resolver, ensure that the filter is registered with the IoC container. Please review the inner exception for more details.", FilterType.Name);

				throw new InvalidOperationException(exceptionMessage, ex);
			}
		}
	}
}
