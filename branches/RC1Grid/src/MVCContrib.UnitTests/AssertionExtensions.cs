using NUnit.Framework;

namespace MvcContrib.UnitTests
{
	public static class AssertionExtensions
	{
		public static void ShouldNotBeNull(this object actual)
		{
			Assert.IsNotNull(actual);
		}

		public static void ShouldEqual(this object actual, object expected)
		{
			Assert.AreEqual(expected, actual);
		}

		public static void ShouldBeTheSameAs(this object actual, object expected)
		{
			Assert.AreSame(expected, actual);
		}
	}
}