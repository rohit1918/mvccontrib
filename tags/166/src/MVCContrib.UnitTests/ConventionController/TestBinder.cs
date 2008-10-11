using System;
using System.Web.Mvc;
using MvcContrib.Attributes;
using MvcContrib.MetaData;

namespace MvcContrib.UnitTests.ConventionController
{
	public class TestBinder : AbstractParameterBinderAttribute
	{
		public TestBinder() : base(null)
		{
		}

		public override object GetValue(ControllerContext controllerContext, string modelName, Type modelType, ModelStateDictionary modelState) {
			((TestController)controllerContext.Controller).BinderFilterOrdering += "Binder";
			return null;
		}
	}
}