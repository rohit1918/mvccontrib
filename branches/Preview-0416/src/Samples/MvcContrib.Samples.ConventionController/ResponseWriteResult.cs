using System.Web.Mvc;

namespace MvcContrib.Samples
{
	public class ResponseWriteResult : ActionResult
	{
		private object _toRender;

		public ResponseWriteResult(object toRender)
		{
			_toRender = toRender;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.Write(_toRender);
		}
	}
}