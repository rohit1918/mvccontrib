using System;
using System.Web.Mvc;

namespace MvcContrib.MetaData
{
	public interface IParameterBinder
	{
		object Bind(Type targetType, string paramName, ControllerContext context);
	}
}