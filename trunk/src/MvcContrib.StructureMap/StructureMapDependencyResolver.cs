using StructureMap;

namespace MvcContrib.StructureMap
{
	using System;
	using MvcContrib.Interfaces;

	public class StructureMapDependencyResolver : IDependencyResolver
	{
		public TType GetImplementationOf<TType>()
		{
			return (TType)GetImplementationOf(typeof(TType));
		}

		public Interface GetImplementationOf<Interface>(Type type)
		{
			return (Interface)GetImplementationOf(type);
		}

		public object GetImplementationOf(Type type)
		{
			try
			{
				return ObjectFactory.GetInstance(type);
			}
			catch(StructureMapException)
			{
				return null;
			}
		}
	}
}