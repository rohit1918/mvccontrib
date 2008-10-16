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

		public override ModelBinderResult BindModel(ModelBindingContext bindingContext)
		{
			((TestController)bindingContext.Controller).BinderFilterOrdering += "Binder";
			return null;
		}
	}
}