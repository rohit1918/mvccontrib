using Boo.Lang.Compiler.Ast;
using MvcContrib.BrailViewEngine;

namespace MVCContrib.UnitTests.BrailViewEngine
{
	using NUnit.Framework;

	[TestFixture]
	[Category("BrailViewEngine")]
	public class HtmlAttributeTester
	{
		[Test]
		public void Can_Transform_Data()
		{
			HtmlAttribute html = new HtmlAttribute();

			Assert.AreEqual("MvcContrib.BrailViewEngine.HtmlAttribute.Transform", html.TransformMethodName);
			Assert.AreEqual("this &amp; that", HtmlAttribute.Transform("this & that"));
			Assert.AreEqual("this &amp; that", HtmlAttribute.Transform((object)"this & that"));

			html.Apply(new Method());
		}
	}
}