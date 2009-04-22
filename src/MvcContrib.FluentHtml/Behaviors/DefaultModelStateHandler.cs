using System.Web.Mvc;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.FluentHtml.Behaviors
{
	public class DefaultModelStateHandler : IModelStateHandler
	{
		public bool Handle(IElement element, ModelState state)
		{
			var supportsValue = element as ISupportsValue;

			if(supportsValue != null)
			{
				var value = state.Value.ConvertTo(typeof(string));
				supportsValue.Value(value);
				return true;
			}

			return false;
		}
	}
}