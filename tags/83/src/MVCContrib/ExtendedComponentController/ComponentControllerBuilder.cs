using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcContrib.ExtendedComponentController
{
	public class ComponentControllerBuilder
	{
		private static ComponentControllerBuilder _current;
		private IComponentControllerFactory _currentComponentControllerFactory;

		public static ComponentControllerBuilder Current
		{
			get
			{
				if (_current == null)
					_current = new ComponentControllerBuilder();
				return _current;
			}
		}
	
		public IComponentControllerFactory GetComponentControllerFactory()
		{
			if (_current == null)
				this._currentComponentControllerFactory = new DefaultComponentControllerFactory();
			return _currentComponentControllerFactory;
		}

		public void SetComponentControllerFactory(IComponentControllerFactory factory)
		{
			if (factory == null)
				this._currentComponentControllerFactory = new DefaultComponentControllerFactory();
			else
				this._currentComponentControllerFactory = factory;
		}

		public void SetComponentControllerFactory(Type t)
		{
			if (t != null)
				this._currentComponentControllerFactory = Activator.CreateInstance(t) as IComponentControllerFactory;
			else
				this._currentComponentControllerFactory = new DefaultComponentControllerFactory();
		}

	}
}
