using System;
using System.Web.Mvc;
using MvcContrib.Attributes;
using MvcContrib.MetaData;

namespace MvcContrib.UnitTests.ConventionController
{
	class DefaultActionController : Controller
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
