using System;
using System.Web.Mvc;
using MvcContrib.Attributes;
using MvcContrib.MetaData;

namespace MvcContrib.UnitTests
{
	class FakeReturnBinder : AbstractReturnBinderAttribute, IReturnBinder
	{
		public override void Bind(IController controller, ControllerContext controllerContext, Type returnType, object returnValue)
		{
			((TestController)controller).ReturnBinderInvoked = true;
		}
	}
}