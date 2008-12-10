using NUnit.Framework;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Html;
using MvcContrib.FluentHtml.Tests.Helpers;

namespace MvcContrib.FluentHtml.Tests
{
	[TestFixture]
	public class FileUploadTests
	{
		[Test]
		public void basic_file_upload_renders_with_corect_tag_and_type()
		{
			new FileUpload("x").ToString()
				.ShouldHaveHtmlNode("x")
				.ShouldBeNamed(HtmlTag.Input)
				.ShouldHaveAttribute(HtmlAttribute.Type).WithValue(HtmlInputType.File);
		}
	}
}
