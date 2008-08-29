/*
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.MetaData;

namespace MvcContrib
{
	/// <summary>
	/// Custom action invoker for use with the convention controller. 
	/// The ConventionControllerActionInvoker uses the Controller Meta Data in order to locate filters and actions.
	/// </summary>
	public class ConventionControllerActionInvoker : ControllerActionInvoker
	{
		private ControllerMetaData _metaData;
		private IControllerDescriptor _controllerDescriptor;

		/// <summary>
		/// The action currently being executed.
		/// </summary>
		public ActionMetaData SelectedAction { get; private set; }

		/// <summary>
		/// Information about the controller.
		/// </summary>
		public ControllerMetaData MetaData
		{
			get
			{
				if(_metaData == null)
				{
					_metaData = ControllerDescriptor.GetMetaData(ControllerContext.Controller);
				}
				return _metaData;
			}
		}

		/// <summary>
		/// Descriptor used to obtain metadata about the current controller. By default, this will be a CachedControllerDescriptor.
		/// </summary>
		public IControllerDescriptor ControllerDescriptor
		{
			get
			{
				if(_controllerDescriptor == null)
				{
					_controllerDescriptor = new CachedControllerDescriptor();
				}
				return _controllerDescriptor;
			}
			set { _controllerDescriptor = value; }
		}


		/// <summary>
		/// Creates a new instance of the ConventionControllerActionInvoker class.
		/// </summary>
		/// <param name="controllerContext">The controller context for use with the current request.</param>
		public ConventionControllerActionInvoker(ControllerContext controllerContext) : base(controllerContext)
		{
		}

		protected override ActionResult InvokeActionMethod(MethodInfo methodInfo, IDictionary<string, object> parameters)
		{
			//Bind parameters here to ensure that filters have finished executing.
			PerformBinding(parameters); 
			return base.InvokeActionMethod(methodInfo, parameters);
		}

		protected virtual void PerformBinding(IDictionary<string, object> values)
		{
			foreach (var parameter in SelectedAction.Parameters)
			{
				values[parameter.ParameterInfo.Name] = parameter.Bind(ControllerContext);
			}
		}

		//Override the built in parameter binding. The default binding for MVC happens before filters are invoked. 
		//Our parameter binding takes place in the PerformBinding method, which happens as part of InvokeActionMethod.
		//This is done so that filters can pre-process the parameter values if necessary.
		protected override IDictionary<string, object> GetParameterValues(MethodInfo methodInfo, IDictionary<string, object> values)
		{
			return values ?? new Dictionary<string, object>();
		}

		protected override MethodInfo FindActionMethod(string actionName, IDictionary<string, object> values) {
			SelectedAction = FindActionMetaData(actionName);
			return SelectedAction == null ? null : SelectedAction.MethodInfo;
		}

		/// <summary>
		/// Finds the ActionMetaData with the specified action name.
		/// </summary>
		/// <param name="actionName">Name of the action to locate.</param>
		/// <returns>ActionMetaData or null if no action can be found with the specified name.</returns>
		public virtual ActionMetaData FindActionMetaData(string actionName)
		{
			var actions = MetaData.GetActions(actionName);

			if(actions == null || actions.Count == 0)
			{
				//No matching action found - see if there is a "catch all" action.
				if(MetaData.DefaultAction != null)
				{
					return MetaData.DefaultAction;
				}
				else
				{
					return null;
				}
			}

			if(actions.Count > 1)
			{
				throw new InvalidOperationException(string.Format("More than one action with name '{0}' found", actionName));
			}

			return actions[0];
		}
	}
}
*/
