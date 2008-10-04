using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MvcContrib.UI.Ajax.Internal;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using AjaxOptions=MvcContrib.UI.Ajax.AjaxOptions;

namespace MvcContrib.UnitTests.UI.Ajax
{
	[TestFixture]
	public class PrototypeAjaxGeneratorTester
	{
		private AjaxGenerator _generator;
		private MockRepository _mocks;
		private ViewContext _context;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_context = _mocks.DynamicViewContext("Index");
			_generator = new PrototypeAjaxGenerator(new AjaxHelper(_context));
			_mocks.ReplayAll();
		}

		[Test]
		public void IsMvcAjaxRequest_should_return_true_for_an_ajax_request()
		{
			_context.HttpContext.Request.Headers["X-Requested-With"] = "XMLHttpRequest";
			Assert.That(_generator.IsMvcAjaxRequest());
		}

		[Test]
		public void MakeAjaxOptionsFrameworkSpecific_should_return_a_PrototypeAjaxOptionsWrapper()
		{
			Assert.That(_generator.MakeAjaxOptionsFrameworkSpecific(new AjaxOptions()), Is.InstanceOfType(typeof(PrototypeAjaxOptionsWrapper)));
		}
	}
}