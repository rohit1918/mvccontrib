using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	/// <summary>
	/// Base class for form elements.
	/// </summary>
	/// <typeparam name="T">Derived type</typeparam>
	public abstract class FormElement<T> : DisableableElement<T>, IMemberElement where T : FormElement<T>, IElement
	{
		protected MemberExpression forMember;
		private readonly IEnumerable<IMemberBehavior> behaviors;

		protected FormElement(string tag, string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: this(tag, name)
		{
			this.forMember = forMember;
			this.behaviors = behaviors;
		}

		protected FormElement(string tag, string name) : base(tag)
		{
			builder.MergeAttribute(HtmlAttribute.Name, name, true);
		}

		/// <summary>
		/// Expression indicating the view model member assocaited with the element.</param>
		/// </summary>
		public virtual MemberExpression ForMember
		{
			get { return forMember; }
		}

		public override string ToString()
		{
			InferIdFromName();
			ApplyBehaviors();
			PreRender();
			return base.ToString();
		}

		protected virtual void PreRender() { }

		/// <summary>
		/// Determines how the HTML element is closed.
		/// </summary>
		public override TagRenderMode TagRenderMode
		{
			get { return TagRenderMode.SelfClosing; }
		}

		private void ApplyBehaviors()
		{
			if(behaviors == null)
			{
				return;
			}
			foreach(var behavior in behaviors)
			{
				behavior.Execute(this);
			}
		}

		private void InferIdFromName()
		{
			if (builder.Attributes.ContainsKey(HtmlAttribute.Id))
			{
				return;
			}
			var name = builder.Attributes[HtmlAttribute.Name];
			if (name != null)
			{
                Attr(HtmlAttribute.Id, name.FormatAsHtmlId());
			}
		}
	}
}
