using System;
using System.Collections.Generic;
using System.Web.Mvc;
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

			if(actions == null || actions.Count == 0)
			{
				return false;
			}
			else if(actions.Count > 1)
			{
				throw new InvalidOperationException(string.Format("More than one action with name '{0}' found", actionName));
			}

			try
			{
				InvokeActionMethod(actions[0]);
			}
			catch(Exception exc)
			{
				if(!OnError(actionName, actions[0].MethodInfo, exc))
				{
					throw;
				}
			}
			finally
			{
				OnPostAction(actionName, actions[0].MethodInfo);
			}

			return true;
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