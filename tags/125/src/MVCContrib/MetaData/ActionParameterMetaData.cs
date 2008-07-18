using System.Reflection;
using System.Web.Mvc;

namespace MvcContrib.MetaData
{
	public class ActionParameterMetaData
	{
		public ActionParameterMetaData(ParameterInfo parameterInfo)
		{
			_parameterInfo = parameterInfo;
		}

		private readonly ParameterInfo _parameterInfo;
		public ParameterInfo ParameterInfo
		{
			get { return _parameterInfo; }
		}

		private IParameterBinder _parameterBinder;
		public IParameterBinder ParameterBinder
		{
			get { return _parameterBinder; }
			set { _parameterBinder = value; }
		}

		public object Bind(ControllerContext context)
		{
			if( _parameterBinder != null )
			{
				return _parameterBinder.Bind(_parameterInfo.ParameterType, _parameterInfo.Name, context);
			}
			else
			{
				return null;
			}
		}

		public bool IsValid
		{
			get
			{
				if( _parameterInfo.IsOut || _parameterInfo.ParameterType.IsByRef )
				{
					return false;
				}

				return true;
			}
		}
	}
}