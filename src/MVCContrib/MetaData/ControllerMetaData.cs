using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MvcContrib.MetaData
{
	/// <summary>
	/// Meta information about a controller
	/// </summary>
	public class ControllerMetaData
	{
		/// <summary>
		/// Creates a new instance of the ControllerMetaData class for the specified controller type.
		/// </summary>
		/// <param name="controllerType">The type of the controller</param>
		/// <param name="actions">The controller's actions</param>
		/// <param name="defaultAction">The default action on the controller (if specified)</param>
		public ControllerMetaData(Type controllerType, ActionMetaData[] actions, ActionMetaData defaultAction)
		{
			ControllerType = controllerType;
			Actions = actions;
			DefaultAction = defaultAction;
		}

		/// <summary>
		/// The Type of the controller
		/// </summary>
		public Type ControllerType { get; private set; }

		/// <summary>
		/// The action that will be called if there are no other actions on the controller with a matching name.
		/// </summary>
		public ActionMetaData DefaultAction { get; protected set; }

		/// <summary>
		/// Finds the action with the specified name.
		/// </summary>
		/// <param name="name">The name of the action to find</param>
		/// <param name="context">The current Controller Context</param>
		/// <returns>ActionMetaData</returns>
		public ActionMetaData GetAction(string name, ControllerContext context)
		{
			if(string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}

			var actions = Actions.Where(x => x.IsValidForRequest(name, context)).ToList();
			
			if(actions.Count == 0)
			{
				return null;
			}

			if(actions.Count > 1)
			{
				throw new InvalidOperationException(string.Format("More than one action with name '{0}' found", name));	
			}

			return actions[0];
		}

		/// <summary>
		/// Actions on the controller.
		/// </summary>
		public ActionMetaData[] Actions { get; protected set; }
	}
}