using System.IO;
using NUnit.Framework;
using Rhino.Mocks;
using MvcContrib.BrailViewEngine;

namespace MvcContrib.UnitTests.BrailViewEngine
{

	[TestFixture]
	[Category("BrailViewEngine")]
	public class DslProviderTester
	{
		private MockRepository _mocks;
		private BrailBase _view;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
            _view = _mocks.StrictMock<BrailBase>(null, null);
		}

		[Test]
		public void ForCoverage()
		{
			var provider = new DslProvider(_view);
			provider.Register(new HtmlExtension(new StringWriter()));
			provider.QuackGet("Dsl", null);
			provider.QuackGet("NotThere", null);
			provider.QuackInvoke("p");
			provider.QuackInvoke("p", null);
			provider.QuackInvoke("p", null, null);
		}
	}
}
