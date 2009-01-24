using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcContrib.MetaData
{
	public class CachedControllerDescriptor : IControllerDescriptor
	{
		private readonly IControllerDescriptor _inner;
		private static readonly Dictionary<Type, ControllerMetaData> _cachedMetaData = new Dictionary<Type, ControllerMetaData>();
		private static readonly object _syncRoot = new object();

		public CachedControllerDescriptor()
			: this(new ControllerDescriptor())
		{
		}

		public CachedControllerDescriptor(IControllerDescriptor inner)
		{
			if( inner == null )
			{
				throw new ArgumentNullException("inner");
			}

			_inner = inner;
		}
		
		public ControllerMetaData GetMetaData(Type controllerType)
		{
			if( controllerType == null )
			{
				throw new ArgumentNullException("controllerType");
			}

			ControllerMetaData metaData;
			if (!_cachedMetaData.TryGetValue(controllerType, out metaData))
			{
				lock (_syncRoot)
				{
					if (!_cachedMetaData.TryGetValue(controllerType, out metaData))
					{
						metaData = _inner.GetMetaData(controllerType);
						_cachedMetaData.Add(controllerType, metaData);
					}
				}
			}

			return metaData;
		}

		public ControllerMetaData GetMetaData(IController controller)
		{
			if( controller == null )
			{
				throw new ArgumentNullException("controller");
			}

			Type controllerType = controller.GetType();
			return GetMetaData(controllerType);
		}
	}
}