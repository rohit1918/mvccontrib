using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.Attributes;

namespace MvcContrib.MetaData
{
	public class ControllerDescriptor : IControllerDescriptor
	{
		public ControllerMetaData GetMetaData(IController controller)
		{
			if( controller == null )
			{
				throw new ArgumentNullException("controller");
			}

			return GetMetaData(controller.GetType());
		}

		public ControllerMetaData GetMetaData(Type controllerType)
		{
			if( controllerType == null )
			{
				throw new ArgumentNullException("controllerType");
			}

			ControllerMetaData metaData = CreateControllerMetaData(controllerType);

			RescueAttribute[] controllerRescues = GetRescues(metaData.ControllerType, true);

			MethodInfo[] actionMethods = metaData.ControllerType.GetMethods(BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance);
			foreach (MethodInfo actionMethod in actionMethods)
			{
				if (actionMethod.DeclaringType == typeof(object))
				{
					continue;
				}

				ActionMetaData actionMetaData = CreateActionMetaData(metaData, actionMethod);

				RescueAttribute[] actionRescues = GetRescues(actionMethod, false);

				foreach (RescueAttribute rescue in actionRescues)
				{
					actionMetaData.Rescues.Add(rescue);
				}

				foreach (RescueAttribute rescue in controllerRescues)
				{
					actionMetaData.Rescues.Add(rescue);
				}

				ParameterInfo[] actionMethodParameters = actionMethod.GetParameters();
				foreach (ParameterInfo actionMethodParameter in actionMethodParameters)
				{
					ActionParameterMetaData parameterMetaData = new ActionParameterMetaData(actionMethodParameter);
					parameterMetaData.ParameterBinder = GetParameterBinder(actionMethodParameter);
					actionMetaData.Parameters.Add(parameterMetaData);
				}

				metaData.Actions.Add(actionMetaData);
			}

			return metaData;
		}

		protected virtual ControllerMetaData CreateControllerMetaData(Type controllerType)
		{
			return new ControllerMetaData(controllerType);
		}

		protected virtual ActionMetaData CreateActionMetaData(ControllerMetaData containingMetaData, MethodInfo actionMethod)
		{
			return new ActionMetaData(actionMethod);
		}

		protected virtual RescueAttribute[] GetRescues(ICustomAttributeProvider attributeProvider, bool inherit)
		{
			return (RescueAttribute[])attributeProvider.GetCustomAttributes(typeof(RescueAttribute), inherit);
		}

		protected virtual IParameterBinder GetParameterBinder(ICustomAttributeProvider attributeProvider)
		{
			object[] attributes = attributeProvider.GetCustomAttributes(typeof(IParameterBinder), false);

			if( attributes != null && attributes.Length > 0 )
			{
				return attributes[0] as IParameterBinder;
			}

			return null;
		}
	}

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
