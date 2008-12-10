using System;
using System.Collections;
using System.Linq.Expressions;
using MvcContrib.FluentHtml.Elements;
using MvcContrib.FluentHtml.Expressions;

namespace MvcContrib.FluentHtml
{
	public static class ViewModelContainerExtensions
	{
		public static TextBox TextBox<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new TextBox(expression.GetNameFor(), expression.GetMemberExpression(), view.MemberBehaviors)
				.Value(expression.GetValueFrom(view.ViewModel));
		}

		public static Password Password<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new Password(expression.GetNameFor(), expression.GetMemberExpression(), view.MemberBehaviors)
				.Value(expression.GetValueFrom(view.ViewModel));
		}

		public static Hidden Hidden<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new Hidden(expression.GetNameFor(), expression.GetMemberExpression(), view.MemberBehaviors)
				.Value(expression.GetValueFrom(view.ViewModel));
		}

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

		public static TextArea TextArea<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new TextArea(expression.GetNameFor(), expression.GetMemberExpression(), view.MemberBehaviors)
				.Value(expression.GetValueFrom(view.ViewModel));
		}

		public static Select Select<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new Select(expression.GetNameFor(), expression.GetMemberExpression(), view.MemberBehaviors)
				.Selected(expression.GetValueFrom(view.ViewModel));
		}

		public static MultiSelect MultiSelect<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new MultiSelect(expression.GetNameFor(), expression.GetMemberExpression(), view.MemberBehaviors)
				.Selected(expression.GetValueFrom(view.ViewModel) as IEnumerable);
		}

		public static Literal Literal<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new Literal().Value(expression.GetValueFrom(view.ViewModel));
		}

		public static Literal FormLiteral<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new FormLiteral(expression.GetNameFor()).Value(expression.GetValueFrom(view.ViewModel));
		}
	}
}
