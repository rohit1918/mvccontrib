using System;
using System.Collections.Generic;
using System.Linq;
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
		private ControllerMetaData _metaData=null;
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
					//_metaData = ControllerDescriptor.GetMetaData(base.  ControllerContext.Controller);
                    throw new NotImplementedException();
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
        protected override ActionDescriptor FindAction(ControllerContext controllerContext, System.Web.Mvc.ControllerDescriptor controllerDescriptor, string actionName)
        {
            if (string.IsNullOrEmpty(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            var action = FindActionMetaData(actionName);

            if (action != null)
            {
                SelectedAction = action;
                return new ReflectedActionDescriptor(action.MethodInfo, actionName, controllerDescriptor);
                //action.MethodInfo;
            }

            return null;
        }
		protected  MethodInfo FindActionMethod(string actionName) 
		{
			if (string.IsNullOrEmpty(actionName)) 
			{
				throw new ArgumentNullException("actionName");
			}

			var action = FindActionMetaData(actionName);
			
			if(action != null)
			{
				SelectedAction = action;
				return action.MethodInfo;
			}

			return null;
		}

		/// <summary>
		/// Finds the ActionMetaData with the specified action name.
		/// </summary>
		/// <param name="actionName">Name of the action to locate.</param>
		/// <returns>ActionMetaData or null if no action can be found with the specified name.</returns>
		public virtual ActionMetaData FindActionMetaData(string actionName)
		{  throw  new NotImplementedException("ControllerContext removed");
        //    var action = MetaData.GetAction(actionName, null);

        //    if(action == null && MetaData.DefaultAction != null)
        //    {
        //        action = MetaData.DefaultAction;
        //    }

        //    return action;
        }
	}
}
