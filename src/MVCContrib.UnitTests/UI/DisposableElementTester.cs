using System.IO;
using System.Web.Mvc;
using MvcContrib.UI;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.UI
{
	[TestFixture]
	public class DisposableElementTester
	{
		[Test]
		public void DisposableElement_should_write_start_tag_when_instantiated()
		{
			var writer = new StringWriter();
			var builder = new TagBuilder("form");
			
			new DisposableElement(writer, builder);

			Assert.That(writer.ToString(), Is.EqualTo("<form>"));
		}

		[Test]
		public void DisposableElement_should_write_end_tag_when_disposed()
		{
			var writer = new StringWriter();
			var builder = new TagBuilder("form");

			using(new DisposableElement(writer, builder))
			{
			}

			Assert.That(writer.ToString(), Is.EqualTo("<form></form>"));
		}
	}
}