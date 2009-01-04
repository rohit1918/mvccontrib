using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public class FileUpload : TextInput<FileUpload>
	{
		public FileUpload(string name) : base(HtmlInputType.File, name) { }
	}
}
