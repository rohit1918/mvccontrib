using System;
using MvcContrib.Castle;
using NUnit.Framework;

namespace MVCContrib.UnitTests.ViewFactories
{
	[TestFixture, Category("NVelocityViewEngine")]
	public class ExtensionDuckTester
	{
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
	}
}