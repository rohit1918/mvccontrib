using System;
using System.Web.Mvc;
using MvcContrib.Attributes;

namespace MvcContrib.UnitTests
{
	class DefaultActionController : ConventionController
	{
		public bool DefaultActionCalled = false;

		[DefaultAction]
		public ActionResult DefaultAction()
		{
			DefaultActionCalled = true;
			return new EmptyResult();
		}
	}
}