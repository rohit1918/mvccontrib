using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.Attributes;
using MvcContrib.Filters;
using MvcContrib.Interfaces;

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

			MethodInfo[] actionMethods = metaData.ControllerType.GetMethods(BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance);
			foreach (MethodInfo actionMethod in actionMethods)
			{
				if (actionMethod.DeclaringType == typeof(object) 
					|| actionMethod.DeclaringType == typeof(Controller)
					|| ! IsValidAction(actionMethod) 
					|| actionMethod.IsSpecialName)
				{
					continue;
				}

				ActionMetaData actionMetaData = CreateActionMetaData(metaData, actionMethod);

				ParameterInfo[] actionMethodParameters = actionMethod.GetParameters();
				foreach (ParameterInfo actionMethodParameter in actionMethodParameters)
				{
					ActionParameterMetaData parameterMetaData = CreateParameterMetaData(metaData, actionMetaData, actionMethodParameter);
					parameterMetaData.ParameterBinder = GetParameterBinder(parameterMetaData);
					actionMetaData.Parameters.Add(parameterMetaData);
				}

				if (IsDefaultAction(actionMetaData))
				{
					if(metaData.DefaultAction == null)
					{
						metaData.DefaultAction = actionMetaData;
					}
					else
					{
						throw new InvalidOperationException(
							string.Format("Multiple DefaultAction attributes were found on controller '{0}'. A controller can only have 1 DefaultAction specified.", controllerType.Name));
					}
				}

				metaData.Actions.Add(actionMetaData);
			}

			return metaData;
		}

		protected virtual bool IsProperty(MethodInfo method)
		{
			if(method.IsSpecialName)
			{
				return true;
			}
			return false;
		}

		protected virtual bool IsDefaultAction(ActionMetaData actionMetaData)
		{
			object[] attributes = actionMetaData.MethodInfo.GetCustomAttributes(typeof(DefaultActionAttribute), false);

			if(attributes.Length > 0)
			{
				return true;
			}
			return false;
		}

		protected virtual bool IsValidAction(MethodInfo actionMethod)
		{
			object[] attributes = actionMethod.GetCustomAttributes(typeof(NonActionAttribute), false);

			if(attributes.Length > 0)
			{
				return false;
			}

			return true;
		}

		protected virtual ControllerMetaData CreateControllerMetaData(Type controllerType)
		{
			return new ControllerMetaData(controllerType);
		}

		protected virtual ActionMetaData CreateActionMetaData(ControllerMetaData containingMetaData, MethodInfo actionMethod)
		{
			return new ActionMetaData(actionMethod);
		}

		protected virtual ActionParameterMetaData CreateParameterMetaData(ControllerMetaData controllerMetaData, ActionMetaData actionMetaData, ParameterInfo parameter)
		{
			return new ActionParameterMetaData(parameter);
		}

		protected virtual IParameterBinder GetParameterBinder(ActionParameterMetaData parameterMetaData)
		{
			object[] attributes = parameterMetaData.ParameterInfo.GetCustomAttributes(typeof(IParameterBinder), false);

			if( attributes != null && attributes.Length > 0 )
			{
				return attributes[0] as IParameterBinder;
			}
			else if(parameterMetaData.IsValid)
			{
				return new SimpleParameterBinder();
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
