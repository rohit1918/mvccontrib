using System;
using System.Web.Mvc;

namespace MvcContrib.ActionResults
{
	/// <summary>
	/// Action result that writes the binary content object to the response stream  
	/// </summary>
	public class BinaryResult : ActionResult
	{
		public byte[] Content { get; set; }
		/// <summary>
		/// The MIME type of the content.
		/// </summary>
		public string ContentType { get; set; }
		/// <summary>
		/// Whether to specify the stream as an attachments. (When TRUE most 
		/// browsers will prompt the user to save the response instead of 
		/// rendering in the browser.)
		/// </summary>
		public bool AsAttachment { get; set; }
		/// <summary>
		/// The filename to specify in the response.  (Most browsers will default to
		/// this value when saving as an attachment.) 
		/// </summary>
		public string Filename { get; set; }

		/// <summary>
		/// Constructor.  See property descriptions for corresponding parameter details.
		/// </summary>
		/// <param name="content">The binary content.</param>
		/// <param name="contentType">MIME type of the content</param>
		/// <param name="asAttachment">Whether to specify the stream as an attachments. (When TRUE most bowsers will prompt the user to save the response instead of rendering in the browser.)</param>
		/// <param name="filename">The filename to specify in the response.  (Most browsers will default to this value when saving as an attachment.) </param>
		public BinaryResult(byte[] content, string contentType, bool asAttachment, string filename)
		{
			Content = content;
			ContentType = contentType;
			AsAttachment = asAttachment;
			Filename = filename;
		}

		/// <summary>
		/// Writes the binary content object to the response stream.
		/// </summary>
		/// <param name="context"></param>
		public override void ExecuteResult(ControllerContext context)
		{
			if (Content == null) return;

			var response = context.HttpContext.Response;

			response.ClearHeaders();
			response.ClearContent();

			response.ContentType = ContentType;
			response.AppendHeader("Content-Disposition", ContentDisposition);
			response.AppendHeader("Content-Length", Content.Length.ToString());
			response.Buffer = true;
			try
			{

				response.BinaryWrite(Content);
			}
			catch (Exception)
			{
				response.ClearContent();
			}
			finally
			{
				response.End();
			}
		}

		private string ContentDisposition
		{
			get
			{
				return string.Format("{0}Filename={1}",
					AsAttachment
						? "attachment; "
						: string.Empty,
					Filename);
			}
		}
	}
}
