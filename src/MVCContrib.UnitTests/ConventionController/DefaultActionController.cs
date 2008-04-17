using System;
using System.Web.Mvc;
using MvcContrib.Attributes;

namespace MvcContrib.UnitTests.ConventionController
{
	class DefaultActionController : MvcContrib.ConventionController
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