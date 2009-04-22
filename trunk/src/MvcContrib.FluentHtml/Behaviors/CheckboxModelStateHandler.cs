using System.Web.Mvc;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.FluentHtml.Behaviors
{
	public class CheckboxModelStateHandler : IModelStateHandler
	{
		public bool Handle(IElement element, ModelState state)
		{
			var checkbox = element as ICheckbox;
			if(checkbox != null)
			{
				var isChecked = state.Value.ConvertTo(typeof(bool?)) as bool?;

				if(isChecked.HasValue)
				{
					checkbox.Checked(isChecked.Value);
					return true;
				}
			}

			return false;
		}
	}
}