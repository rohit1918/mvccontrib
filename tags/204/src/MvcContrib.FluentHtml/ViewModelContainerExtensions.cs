using System;
using System.Collections;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;

namespace MvcContrib.FluentHtml
{
	/// <summary>
	/// Extensions of IViewModelContainer&lt;T&gt;
	/// </summary>
	public static class ViewModelContainerExtensions
	{
		/// <summary>
		/// Generate an HTML input element of type 'text' and set its value from the ViewModel based on the expression provided.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member associated with the element.</param>
		public static TextBox TextBox<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), view.MemberBehaviors)
				.Value(expression.GetValueFrom(view.ViewModel));
		}

		/// <summary>
		/// Generate an HTML input element of type 'password' and set its value from the ViewModel based on the expression provided.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member associated with the element.</param>
		public static Password Password<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new Password(expression.GetNameFor(), expression.GetMemberExpression(), view.MemberBehaviors)
				.Value(expression.GetValueFrom(view.ViewModel));
		}

		/// <summary>
		/// Generate an HTML input element of type 'hidden' and set its value from the ViewModel based on the expression provided.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member associated with the element.</param>
		public static Hidden Hidden<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new Hidden(expression.GetNameFor(), expression.GetMemberExpression(), view.MemberBehaviors)
				.Value(expression.GetValueFrom(view.ViewModel));
		}

		/// <summary>
		/// Generate an HTML input element of type 'checkbox' and set its value from the ViewModel based on the expression provided.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member associated with the element.</param>
		public static CheckBox CheckBox<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			var checkbox = new CheckBox(expression.GetNameFor(), expression.GetMemberExpression(), view.MemberBehaviors);
			var val = expression.GetValueFrom(view.ViewModel) as bool?;
			if (val.HasValue)
			{
				checkbox.Checked(val.Value);
			}
			return checkbox;
		}

		/// <summary>
		/// Generate an HTML textarea element and set its value from the ViewModel based on the expression provided.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member associated with the element.</param>
		public static TextArea TextArea<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new TextArea(expression.GetNameFor(), expression.GetMemberExpression(), view.MemberBehaviors)
				.Value(expression.GetValueFrom(view.ViewModel));
		}

		/// <summary>
		/// Generate an HTML select element and set the selected option value from the ViewModel based on the expression provided.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member associated with the element.</param>
		public static Select Select<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new Select(expression.GetNameFor(), expression.GetMemberExpression(), view.MemberBehaviors)
				.Selected(expression.GetValueFrom(view.ViewModel));
		}

		/// <summary>
		/// Generate an HTML select element and set the selected options from the ViewModel based on the expression provided.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member associated with the element.</param>
		public static MultiSelect MultiSelect<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new MultiSelect(expression.GetNameFor(), expression.GetMemberExpression(), view.MemberBehaviors)
				.Selected(expression.GetValueFrom(view.ViewModel) as IEnumerable);
		}

		/// <summary>
		/// Generate an HTML span element and set its inner text from the ViewModel based on the expression provided.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member associated with the element.</param>
		public static Literal Literal<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new Literal().Value(expression.GetValueFrom(view.ViewModel));
		}

		/// <summary>
		/// Generate an HTML span element and hidden input element.  Set the inner text of the span element and the value 
		/// of the hidden input element from the ViewModel based on the expression provided.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member associated with the element.</param>
		public static Literal FormLiteral<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new FormLiteral(expression.GetNameFor()).Value(expression.GetValueFrom(view.ViewModel));
		}
	}
}
