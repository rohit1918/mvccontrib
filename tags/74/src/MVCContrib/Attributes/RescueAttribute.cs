using System;
using System.Collections.Generic;

namespace MvcContrib.Attributes
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method, AllowMultiple=true)]
	public class RescueAttribute : Attribute
	{
		private readonly string _view;
		private readonly List<Type> _exceptionsTypes = new List<Type>();

		public RescueAttribute(string view)
			: this(view, typeof(Exception))
		{
		}

		public RescueAttribute(string view, params Type[] exceptionTypes)
		{
			if( string.IsNullOrEmpty(view) )
			{
				throw new ArgumentException("view is required", "view");
			}

			_view = view;
			if (exceptionTypes != null)
			{
				_exceptionsTypes.AddRange(exceptionTypes);
			}
		}

		public string View
		{
			get { return _view; }
		}

		public IList<Type> ExceptionTypes
		{
			get { return _exceptionsTypes; }
		}
	}
}
