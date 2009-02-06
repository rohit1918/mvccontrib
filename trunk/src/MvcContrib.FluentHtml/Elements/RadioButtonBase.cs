using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for a radio button.
	/// </summary>
	public abstract class RadioButtonBase<T> : Input<T>, IElement where T : RadioButtonBase<T>
	{
		protected RadioButtonBase(string name) : base(HtmlInputType.Radio, name) { }

		protected RadioButtonBase(string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: base(HtmlInputType.Radio, name, forMember, behaviors) { }

		/// <summary>
		/// Set the checked attribute.
		/// </summary>
		/// <param name="value">Whether the radio button should be checked.</param>
		public virtual T Checked(bool value)
		{
			if (value)
			{
				Attr(HtmlAttribute.Checked, HtmlAttribute.Checked);
			}
			else
			{
				RemoveAttr(HtmlAttribute.Checked);
			}
			return (T)this;
		}

		protected override void InferIdFromName()
		{
			if (!builder.Attributes.ContainsKey(HtmlAttribute.Id))
			{
				Attr(HtmlAttribute.Id, string.Format("{0}{1}",
					builder.Attributes[HtmlAttribute.Name].GenerateHtmlId(),
					elementValue == null 
						? null 
						: string.Format("_{0}", elementValue)));
			}
		}

		public override string ToString()
		{
			InferIdFromName();
			ApplyBehaviors();
			PreRender();
			var radioInputHtml = builder.ToString(TagRenderMode);
			if (!string.IsNullOrEmpty(labelBeforeText) || !string.IsNullOrEmpty(labelAfterText))
			{
				var labelBuilder = GetLabelBuilder();
				labelBuilder.InnerHtml = 
					HttpUtility.HtmlEncode(labelBeforeText) +
					radioInputHtml +
					HttpUtility.HtmlEncode(labelAfterText);
				return labelBuilder.ToString();
			}
			return radioInputHtml;
		}
	}
}