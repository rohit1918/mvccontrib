using System;
using System.Web.Mvc;
using MvcContrib.Castle;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ViewFactories
{
	[TestFixture, Category("NVelocityViewEngine")]
	public class ExtensionDuckTester
	{
	    private MockRepository _mocks;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockRepository();
        }

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Depends_On_Instance()
		{
			new ExtensionDuck(null);
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void Does_Not_Support_Get()
		{
			ExtensionDuck duck = new ExtensionDuck(new object());
			duck.GetInvoke(null);
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void Does_Not_Support_Set()
		{
			ExtensionDuck duck = new ExtensionDuck(new object());
			duck.SetInvoke(null, null);
		}

		[Test]
		public void Returns_Null_For_Empty_Invoke()
		{
			ExtensionDuck duck = new ExtensionDuck(new object());
			Assert.IsNull(duck.Invoke(string.Empty));
		}

		[Test]
		public void ForCoverage()
		{
			object o = new ExtensionDuck(new object()).Introspector;
		}

        [Test]
        public void CanAddExtensionsToHtmlExtensionDuck()
        {
            var viewContext = _mocks.DynamicViewContext("someView");
            var viewDataContainer = _mocks.DynamicMock<IViewDataContainer>();

            HtmlExtensionDuck.AddExtension(typeof(HtmlExtensionForTesting));
            var htmlExtensionDuck = new HtmlExtensionDuck(viewContext, viewDataContainer);

            object result = htmlExtensionDuck.Invoke("Foo");

            Assert.That(result, Is.EqualTo("Bar"));
                        
        }       
	}

    public static class HtmlExtensionForTesting
    {
        public static string Foo(this HtmlHelper html)
        {
            return "Bar";
        }
    }
}