using System.Collections;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Elements;

namespace MvcContrib.FluentHtml
{
	public static class ViewDataContainerExtensions
	{
		public static TextBox TextBox(this IViewDataContainer view, string name)
		{
			return new TextBox(name).Value(view.ViewData.Eval(name));
		}

		public static Password Password(this IViewDataContainer view, string name)
		{
			return new Password(name).Value(view.ViewData.Eval(name));
		}

		public static Select Select(this IViewDataContainer view, string name)
		{
			return new Select(name).Selected(view.ViewData.Eval(name));
		}

		public static MultiSelect MultiSelect(this IViewDataContainer view, string name)
		{
			return new MultiSelect(name).Selected(view.ViewData.Eval(name)as IEnumerable);
		}

		public static Hidden Hidden(this IViewDataContainer view, string name)
		{
			return new Hidden(name).Value(view.ViewData.Eval(name));
		}

		public static TextArea TextArea(this IViewDataContainer view, string name)
		{
			return new TextArea(name).Value(view.ViewData.Eval(name));
		}

		public static CheckBox CheckBox(this IViewDataContainer view, string name)
		{
			var checkbox = new CheckBox(name).Value(true);
			var chkd = view.ViewData.Eval(name) as bool?;
			if (chkd.HasValue)
			{
				checkbox.Checked(chkd.Value);
			}
			return checkbox;
		}

		public static SubmitButton SubmitButton(this IViewDataContainer view, string text)
		{
			return new SubmitButton(text);
		}

		public static Literal Literal(this IViewDataContainer view, object value)
		{
			return new Literal().Value(value);
		}

		public static Literal FormLiteral(this IViewDataContainer view, string name)
		{
			return new FormLiteral(name).Value(view.ViewData.Eval(name));
		}

		public static FileUpload FileUpload(this IViewDataContainer view, string name)
		{
			return new FileUpload(name).Value(view.ViewData.Eval(name));
		}
	}
}
