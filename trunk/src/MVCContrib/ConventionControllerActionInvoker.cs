using System;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.Attributes;

namespace MvcContrib
{
	/// <summary>
	/// Custom action invoker for use with the convention controller. 
	/// The ConventionControllerActionInvoker uses the Controller Meta Data in order to locate filters and actions.
	/// </summary>
	public class ConventionControllerActionInvoker : ControllerActionInvoker
	{
		protected override ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, string actionName)
		{
			var action = controllerDescriptor.FindAction(controllerContext, actionName);
			return action ?? FindDefaultAction(controllerDescriptor);
		}

		private ActionDescriptor FindDefaultAction(ControllerDescriptor descriptor)
		{
			var defaultActions = descriptor.GetCanonicalActions()
				.Where(x => x.GetCustomAttributes(typeof(DefaultActionAttribute), false).Length == 1)
				.ToList();

			if(defaultActions.Count == 0)
			{
				return null;
			}

			if(defaultActions.Count > 1)
			{
				throw new InvalidOperationException(string.Format("Multiple DefaultAction attributes were found on controller '{0}'. A controller can only have 1 DefaultAction specified.", descriptor.ControllerName));
			}

			return defaultActions[0];
		}
	}
}