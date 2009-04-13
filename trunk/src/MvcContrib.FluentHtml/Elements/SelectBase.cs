using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Collections.Generic;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for select elements.
	/// </summary>
	/// <typeparam name="T">The derived type.</typeparam>
	public abstract class SelectBase<T> : OptionsElementBase<T> where T : SelectBase<T>
	{
		protected SelectBase(string name) : base(HtmlTag.Select, name) { }

		protected SelectBase(string name, MemberExpression forMember, IEnumerable<IBehaviorMarker> behaviors)
			: base(HtmlTag.Select, name, forMember, behaviors) { }

		/// <summary>
		/// Set the 'size' attribute.
		/// </summary>
		/// <param name="value">The value of the 'size' attribute.</param>
		/// <returns></returns>
		public virtual T Size(int value)
		{
			Attr(HtmlAttribute.Size, value);
			return (T)this;
		}

		protected string _firstOptionText;
		public virtual T FirstOptionText(string firstOptionText)
		{
			_firstOptionText = firstOptionText;
			return (T)this;
		}

		protected bool _hideFirstOption;
		public virtual T HideFirstOptionWhen(bool hideFirstOption)
		{
			_hideFirstOption = hideFirstOption;
			return (T)this;
		}

		protected override void PreRender()
		{
			builder.InnerHtml = RenderOptions();
			base.PreRender();
		}

		public override TagRenderMode TagRenderMode
		{
			get { return TagRenderMode.Normal; }
		}

		private string RenderOptions()
		{
			if (_options == null)
			{
				return null;
			}

			var sb = new StringBuilder();

			if (_firstOptionText != null && _hideFirstOption == false)
			{
				sb.Append(GetFirstOption());
			}

			foreach (var options in _options)
			{
				sb.Append(GetOption(options));
			}

			return sb.ToString();
		}

		protected virtual Option GetFirstOption()
		{
			return new Option()
				.Value(string.Empty)
				.Text(_firstOptionText)
				.Selected(false);
		}

		protected virtual Option GetOption(object option)
		{
			var value = _valueFieldSelector(option);
			var text = _textFieldSelector(option);

			return new Option()
				.Value(value == null ? string.Empty : value.ToString())
				.Text(text == null ? string.Empty : text.ToString())
				.Selected(IsSelectedValue(value));
		}
	}
}