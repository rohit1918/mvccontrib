using System;
using System.Collections.Generic;

namespace MvcContrib.MetaData
{
	public class ControllerMetaData
	{
		public ControllerMetaData(Type controllerType)
		{
			_controllerType = controllerType;
		}

		private readonly Type _controllerType;

		public Type ControllerType
		{
			get { return _controllerType; }
		}

		private readonly List<ActionMetaData> _actions = new List<ActionMetaData>();

		public IList<ActionMetaData> Actions
		{
			get { return _actions; }
		}

		public IList<ActionMetaData> GetActions(string name)
		{
			return
				_actions.FindAll(
					delegate(ActionMetaData action) { return action.Name.Equals(name, StringComparison.OrdinalIgnoreCase); });
		}
	}
}