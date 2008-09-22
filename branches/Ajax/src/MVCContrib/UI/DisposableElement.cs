using System;
using System.IO;
using System.Web.Mvc;

namespace MvcContrib.UI
{
	/// <summary>
	/// Represents an HTML element. When instantiated, the opening tag will be written to the output.
	/// When disposed, the closing tag will be written to the output.
	/// </summary>
	public class DisposableElement : IDisposable
	{
		/// <summary>
		/// Output writer
		/// </summary>
		public TextWriter Writer { get; protected set; }
		/// <summary>
		/// Tag Builder
		/// </summary>
		public TagBuilder Tag { get; protected set; }

		/// <summary>
		/// Creates a new instance of the <see cref="DisposableElement"/> class.
		/// </summary>
		/// <param name="writer">Writer to which the output should be written</param>
		/// <param name="tag">The tag to render</param>
		public DisposableElement(TextWriter writer, TagBuilder tag)
		{
			Writer = writer;
			Tag = tag;
			Writer.Write(this.Tag.ToString(TagRenderMode.StartTag));
		}

		public void Dispose()
		{
			Writer.Write(Tag.ToString(TagRenderMode.EndTag));	
		}
	}
}