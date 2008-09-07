using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.Attributes;

namespace MvcContrib.MetaData
{
	/// <summary>
	/// Responsible for populating ControllerMetaData classes. 
	/// </summary>
	public class ControllerDescriptor : IControllerDescriptor
	{
		/// <summary>
		/// Gets metadata for a specified controller instance.
		/// </summary>
		/// <param name="controller">The controller to get metadata for.</param>
		/// <returns>A ControllerMetaData object</returns>
		public ControllerMetaData GetMetaData(IController controller)
		{
			if( controller == null )
			{
				throw new ArgumentNullException("controller");
			}

			return GetMetaData(controller.GetType());
		}

		/// <summary>
		/// Gets metadata for a specified controller type.
		/// </summary>
		/// <param name="controllerType">The controller type to get metadata for.</param>
		/// <returns>A ControllerMetaData object.</returns>
		public ControllerMetaData GetMetaData(Type controllerType)
		{
			if( controllerType == null )
			{
				throw new ArgumentNullException("controllerType");
			}

//			var metaData = CreateControllerMetaData(controllerType);

			var controllerFilters = GetFilters(controllerType);
			var actionMethods = GetActionMethods(controllerType);
			var actions = actionMethods.Select(method => CreateActionMetaData(method, controllerFilters)).ToArray();
			var defaultAction = FindDefaultAction(actions, controllerType);

			return CreateControllerMetaData(controllerType, actions, defaultAction);


			/*foreach (var actionMethod in actionMethods)
			{
				ActionMetaData actionMetaData = CreateActionMetaData(metaData, actionMethod);
				
				actionMetaData.Filters = CreateFilterInfo(controllerFilters, GetFilters(actionMethod), actionMetaData);

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

			return metaData;*/


		}

		protected virtual ActionMetaData FindDefaultAction(ActionMetaData[] actions, Type controllerType)
		{
			var defaultActions = actions.Where(x => x.MethodInfo.IsDefined(typeof(DefaultActionAttribute), false)).ToArray();
			
			if(defaultActions.Length == 0)
			{
				return null;
			}

			if(defaultActions.Length > 1)
			{
				throw new InvalidOperationException(string.Format("Multiple DefaultAction attributes were found on controller '{0}'. A controller can only have 1 DefaultAction specified.", controllerType.Name));
			}

			return defaultActions.First();
		}

		protected virtual MethodInfo[] GetActionMethods(Type controllerType)
		{
			return controllerType
					.GetMethods(BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance)
					.Where(IsValidAction).ToArray();
		}

		/// <summary>
		/// Checks to see whether a MethodInfo is considered a valid action method.
		/// </summary>
		/// <param name="method">The method to check</param>
		/// <returns>True if the action is valid, otherwise false</returns>
		public static bool IsValidAction(MethodInfo method)
		{
			bool isValid =
				!method.IsSpecialName
				&& !method.GetBaseDefinition().DeclaringType.IsAssignableFrom(typeof(Controller))
				&& !Attribute.IsDefined(method, typeof(NonActionAttribute));

			return isValid;
		}

		/// <summary>
		/// Checks whether a MethodInfo object is decorated by an ActionNameAttribute 
		/// </summary>
		/// <param name="method">The MethodInfo object to check</param>
		/// <returns>True if the method has an ActionNameAttribute, otherwise false.</returns>
		public static bool IsAliasedMethod(MethodInfo method)
		{
			return method.IsDefined(typeof(ActionNameAttribute), true);
		}

		protected virtual ControllerMetaData CreateControllerMetaData(Type controllerType, ActionMetaData[] actions, ActionMetaData defaultAction)
		{
			return new ControllerMetaData(controllerType, actions, defaultAction);
		}

		protected virtual ActionMetaData CreateActionMetaData(MethodInfo actionMethod, FilterAttribute[] controllerLevelFilters)
		{
			var actionNameAttributes = (ActionNameAttribute[])actionMethod.GetCustomAttributes(typeof(ActionNameAttribute), true);
			var filters = CreateFilterInfo(controllerLevelFilters, GetFilters(actionMethod));
			var selectors = GetActionSelectionAttributes(actionMethod);

			if(actionNameAttributes.Length > 0)
			{
				return new AliasedActionMetaData(actionMethod, filters, selectors, actionNameAttributes);
			}
            
			return new ActionMetaData(actionMethod, selectors, filters);
		}

		protected virtual ActionSelectionAttribute[] GetActionSelectionAttributes(ICustomAttributeProvider provider)
		{
			return (ActionSelectionAttribute[])provider.GetCustomAttributes(typeof(ActionSelectionAttribute), true);
		}

		protected virtual FilterAttribute[] GetFilters(ICustomAttributeProvider attributeProvider)
		{
			return (FilterAttribute[])attributeProvider.GetCustomAttributes(typeof(FilterAttribute), true);
		}

		protected virtual FilterInfo CreateFilterInfo(FilterAttribute[] controllerFilters, FilterAttribute[] actionFilters) 
		{
			var filters = controllerFilters.Concat(actionFilters).OrderBy(f => f.Order).ToList();

			var filterInfo = new FilterInfo 
			{
				ActionFilters = filters.OfType<IActionFilter>().ToList(),
				AuthorizationFilters = filters.OfType<IAuthorizationFilter>().ToList(),
				ExceptionFilters = filters.OfType<IExceptionFilter>().ToList(),
				ResultFilters = filters.OfType<IResultFilter>().ToList()
			};

			return filterInfo;
		}

		public static bool IsController(Type type) 
		{
			return
				type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
				&& type != typeof(IController)
				&& type != typeof(Controller)
				&& typeof(IController).IsAssignableFrom(type);
		}
	}
}
