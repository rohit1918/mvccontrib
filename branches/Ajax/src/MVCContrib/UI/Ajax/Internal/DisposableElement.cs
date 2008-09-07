using System;
using System.IO;
using System.Web.Mvc;

namespace MvcContrib.UI.Ajax.Internal
{
	/// <summary>
	/// Represents an HTML element. When instantiated, the opening tag will be written to the output.
	/// When disposed, the closing tag will be written to the output.
	/// </summary>
	public class DisposableElement : IDisposable
	{
		private readonly TextWriter _writer;
		private readonly TagBuilder _tag;

		/// <summary>
		/// Creates a new instance of the <see cref="DisposableElement"/> class.
		/// </summary>
		/// <param name="writer">Writer to which the output should be written</param>
		/// <param name="tag">The opening </param>
		public DisposableElement(TextWriter writer, TagBuilder tag)
		{
			_writer = writer;
			_tag = tag;
			_writer.Write(_tag.ToString(TagRenderMode.StartTag));
		}

		public void Dispose()
		{
			_writer.Write(_tag.ToString(TagRenderMode.EndTag));	
		}
	}
}