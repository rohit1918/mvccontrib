using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.Attributes;
using MvcContrib.Filters;

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
			FilterAttribute[] controllerFilters = GetFilters(metaData.ControllerType);

			MethodInfo[] actionMethods = metaData.ControllerType.GetMethods(BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance);
			foreach (MethodInfo actionMethod in actionMethods)
			{
				if (actionMethod.DeclaringType == typeof(object) || ! IsValidAction(actionMethod))
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

				List<FilterAttribute> filters = new List<FilterAttribute>(controllerFilters);
				filters.AddRange(GetFilters(actionMethod));
				SortFilters(filters);

				foreach(FilterAttribute filter in filters)
				{
					actionMetaData.Filters.Add(filter);
				}

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

		private static void SortFilters(List<FilterAttribute> filters)
		{
			filters.Sort(delegate (FilterAttribute first, FilterAttribute second)
			             	{
			             		return first.ExecutionOrder - second.ExecutionOrder;
			             	});
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
			object[] attributes = actionMethod.GetCustomAttributes(typeof(HiddenActionAttribute), false);

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

		protected virtual RescueAttribute[] GetRescues(ICustomAttributeProvider attributeProvider, bool inherit)
		{
			return (RescueAttribute[])attributeProvider.GetCustomAttributes(typeof(RescueAttribute), inherit);
		}

		protected virtual FilterAttribute[] GetFilters(ICustomAttributeProvider attributeProvider)
		{
			return (FilterAttribute[])attributeProvider.GetCustomAttributes(typeof(FilterAttribute), true);
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
