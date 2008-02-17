using MvcContrib.BrailViewEngine;

namespace MVCContrib.UnitTests.BrailViewEngine
{
	using NUnit.Framework;

	[TestFixture]
	[Category("BrailViewEngine")]
	public class RawAttributeTester
	{
		[Test]
		public void Does_Not_Modify_Input()
		{
			RawAttribute raw = new RawAttribute();

			Assert.AreEqual("MvcContrib.BrailViewEngine.RawAttribute.Transform", raw.TransformMethodName);
			Assert.AreEqual("this & that", RawAttribute.Transform("this & that"));
			Assert.AreEqual("this & that", RawAttribute.Transform((object)"this & that"));
		}
	}
}