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
			return new TextBox(expression.GetNameFor(view), expression.GetMemberExpression(), view.MemberBehaviors)
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
			return new Password(expression.GetNameFor(view), expression.GetMemberExpression(), view.MemberBehaviors)
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
			return new Hidden(expression.GetNameFor(view), expression.GetMemberExpression(), view.MemberBehaviors)
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
			var checkbox = new CheckBox(expression.GetNameFor(view), expression.GetMemberExpression(), view.MemberBehaviors);
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
			return new TextArea(expression.GetNameFor(view), expression.GetMemberExpression(), view.MemberBehaviors)
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
			return new Select(expression.GetNameFor(view), expression.GetMemberExpression(), view.MemberBehaviors)
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
			return new MultiSelect(expression.GetNameFor(view), expression.GetMemberExpression(), view.MemberBehaviors)
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
		public static FormLiteral FormLiteral<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new FormLiteral(expression.GetNameFor(view)).Value(expression.GetValueFrom(view.ViewModel));
		}

		/// <summary>
		/// Generate an HTML input element of type 'radio'.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member associated with the element.</param>
		public static RadioButton RadioButton<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new RadioButton(expression.GetNameFor(view), expression.GetMemberExpression(), view.MemberBehaviors);
		}

		/// <summary>
		/// Generate a set of HTML input elements of type 'radio' -- each wrapped inside a label.  The whole thing is wrapped in an HTML 
		/// div element.  The values of the input elements and he label text are taken from the options specified.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member associated with the element.</param>
		public static RadioSet RadioSet<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new RadioSet(expression.GetNameFor(view), expression.GetMemberExpression(), view.MemberBehaviors)
				.Selected(expression.GetValueFrom(view.ViewModel));
		}

		/// <summary>
		/// If ModelState contains an error for the specified view model member, generate an HTML span element with the 
		/// ModelState error message is the specified message is null.   If no class is specified the class attribute 
		/// of the span element will be 'field-validation-error'.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member associated with the element.</param>
		public static ValidationMessage ValidationMessage<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return ValidationMessage(view, expression, null);
		}

		/// <summary>
		/// If ModelState contains an error for the specified view model member, generate an HTML span element with the 
		/// specified message as inner text, or with the ModelState error message is the specified message is null.  If no
		/// class is specified the class attribute of the span element will be 'field-validation-error'.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member associated with the element.</param>
		/// <param name="message">The error message.</param>
		public static ValidationMessage ValidationMessage<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression, string message) where T : class
		{
			string errorMessage = null;
			var name = expression.GetNameFor(view);
			if (view.ViewData.ModelState.ContainsKey(name))
			{
				var modelState = view.ViewData.ModelState[name];
				if(modelState != null && modelState.Errors != null && modelState.Errors.Count > 0)
				{
					errorMessage = modelState.Errors[0] == null
						? null
						: message ?? modelState.Errors[0].ErrorMessage;
				}
			}
			return new ValidationMessage().Value(errorMessage);
		}


		/// <summary>
		/// Generate an HTML input element of type 'hidden,' set it's name from the expression with '.Index' appended.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member to use to derive the 'name' attribute.</param>
		public static Hidden Index<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return new Hidden(expression.GetNameFor(view) + ".Index", expression.GetMemberExpression(), view.MemberBehaviors);
		}

		/// <summary>
		/// Generate an HTML input element of type 'hidden,' set it's name from the expression with '.Index' appended
		/// and set its value from the ViewModel from the valueExpression provided.
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member to use to derive the 'name' attribute.</param>
		/// <param name="valueExpression">Expression indicating the ViewModel member to use to get the value of the element.</param>
		public static Hidden Index<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression,
			Expression<Func<T, object>> valueExpression) where T : class
		{
			var name = expression.GetNameFor(view) + ".Index";
			var value = valueExpression.GetValueFrom(view.ViewModel);
			var id = name.GenerateHtmlId() + (value == null
				? null
				: "_" + value.ToString().GenerateHtmlId());
			var hidden = new Hidden(name, expression.GetMemberExpression(), view.MemberBehaviors).Value(value).Id(id);
			return hidden;
		}

		/// <summary>
		/// Returns a name to match the value for the HTML name attribute form elements using the same expression. 
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member.</param>
		public static string NameFor<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return expression.GetNameFor();
		}

		/// <summary>
		/// Returns a name to match the value for the HTML id attribute form elements using the same expression. 
		/// </summary>
		/// <typeparam name="T">The type of the ViewModel.</typeparam>
		/// <param name="view">The view.</param>
		/// <param name="expression">Expression indicating the ViewModel member.</param>
		public static string IdFor<T>(this IViewModelContainer<T> view, Expression<Func<T, object>> expression) where T : class
		{
			return expression.GetNameFor().GenerateHtmlId();
		}
	}
}