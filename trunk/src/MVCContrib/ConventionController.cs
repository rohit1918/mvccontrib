﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.Attributes;
using MvcContrib.MetaData;

namespace MvcContrib
{
	public class ConventionController : Controller
	{
		protected override bool InvokeAction(string actionName)
		{
			if(string.IsNullOrEmpty(actionName))
			{
				throw new ArgumentException("actionName is required", "actionName");
			}

			IList<ActionMetaData> actions = MetaData.GetActions(actionName);

			ActionMetaData selectedAction;

			if(actions == null || actions.Count == 0)
			{
				return false;
			}
			else if(actions.Count > 1)
			{
				throw new InvalidOperationException(string.Format("More than one action with name '{0}' found", actionName));
			}
			else
			{
				selectedAction = actions[0];
			}

			if (OnPreAction(actionName, selectedAction.MethodInfo))
			{
				try
				{
					InvokeActionMethod(selectedAction);
				}
				catch(Exception exc)
				{
					if(!OnError(selectedAction, exc))
					{
						throw;
					}
				}
				finally
				{
					OnPostAction(actionName, selectedAction.MethodInfo);
				}
			}
			return true;
		}

		protected virtual bool OnError(ActionMetaData action, Exception exception)
		{
			Type innerExceptionType = exception.InnerException.GetType();

			foreach(RescueAttribute rescue in action.Rescues )
			{
				foreach( Type exceptionType in rescue.ExceptionTypes )
				{
					if (exceptionType.IsAssignableFrom(innerExceptionType))
					{
						OnPreRescue(exception);

						if(!string.IsNullOrEmpty(rescue.View))
						{
							string rescueView = string.Concat("Rescues/", rescue.View);

							RenderView(rescueView, exception);

							return true;
						}
					}
				}
			}

			return false;
		}

		protected virtual void OnPreRescue(Exception thrownException)
		{
			
		}

		protected virtual void InvokeActionMethod(ActionMetaData action)
		{
			object[] actionParameters = new object[action.Parameters.Count];

			int index = 0;
			foreach(ActionParameterMetaData actionParameter in action.Parameters)
			{
				actionParameters[index++] = actionParameter.Bind(ControllerContext);
			}

			action.InvokeMethod(this, actionParameters);
		}

		private ControllerMetaData _metaData;

		public ControllerMetaData MetaData
		{
			get
			{
				if(_metaData == null)
				{
					_metaData = ControllerDescriptor.GetMetaData(this);
				}
				return _metaData;
			}
		}

		private IControllerDescriptor _controllerDescriptor;

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
	}
}