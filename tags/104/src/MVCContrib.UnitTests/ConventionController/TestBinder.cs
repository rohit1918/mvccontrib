using System;
using System.Web.Mvc;
using MvcContrib.MetaData;

namespace MvcContrib.UnitTests.ConventionController
{
	public class TestBinder : Attribute, IParameterBinder
	{
		public object Bind(Type targetType, string paramName, ControllerContext context)
		{
			((TestController)context.Controller).BinderFilterOrdering += "Binder";
			return null;
		}
	}
}