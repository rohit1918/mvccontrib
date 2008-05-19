using System.Web.Mvc;

namespace MvcContrib.ActionResults
{
	/// <summary>
	/// Action result for writing text to the output stream of the HTTP Response.
	/// </summary>
	public class TextResult : ActionResult
	{
		private readonly string _ToWrite;

		/// <summary>
		/// Creates a new instance of the TextResult class.
		/// </summary>
		/// <param name="toRender">The string to write to the output stream of the HTTP Response.</param>
		public TextResult(string toRender)
		{
			_ToWrite = toRender;
		}

		/// <summary>
		/// The string to be written to the output stream.
		/// </summary>
		public string ToWrite
		{
			get { return _ToWrite; }
		}

		/// <summary>
		/// Renders the string specified in the constructor to the output stream of the HTTP Response.
		/// </summary>
		/// <param name="context"></param>
		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.Output.Write(ToWrite);
		}
	}
}