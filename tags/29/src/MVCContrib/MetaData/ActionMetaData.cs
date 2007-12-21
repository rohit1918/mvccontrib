using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace MvcContrib.MetaData
{
	public class ActionMetaData
	{
		public ActionMetaData(MethodInfo methodInfo)
		{
			_methodInfo = methodInfo;
		}

		public string Name
		{
			get { return _methodInfo.Name; }
		}

		private readonly MethodInfo _methodInfo;
		public MethodInfo MethodInfo
		{
			get { return _methodInfo; }
		}

		private readonly List<ActionParameterMetaData> _parameters = new List<ActionParameterMetaData>();
		public IList<ActionParameterMetaData> Parameters
		{
			get { return _parameters; }
		}

		public object InvokeMethod(IController controller, object[] parameters)
		{
			return _methodInfo.Invoke(controller, parameters);
		}
	}
}