using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace MvcContrib.EnumerableExtensions
{
	/// <summary>
	/// Extension methods on IEnumerable.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Converts an enumerable into a SelectList.
		/// </summary>
		/// <typeparam name="T">Type of item in the collection</typeparam>
		/// <typeparam name="TValueField">Type of the value field</typeparam>
		/// <param name="items">DataSource to convert into a select list</param>
		/// <param name="valueFieldSelector">Expression used to identify the data value field</param>
		/// <param name="textFieldSelector">Expression used to identify the data text field</param>
		/// <returns>A populated SelectList</returns>
		public static SelectList ToSelectList<T, TValueField>(this IEnumerable<T> items, Expression<Func<T, TValueField>> valueFieldSelector, Expression<Func<T, object>> textFieldSelector)
		{
			string textField = ExpressionToName(textFieldSelector);
			string valueField = ExpressionToName(valueFieldSelector);

			return new SelectList(items, valueField, textField);
		}

		/// <summary>
		/// Converts an enumerable into a SelectList.
		/// </summary>
		/// <typeparam name="T">Type of item in the collection</typeparam>
		/// <typeparam name="TValueField">Type of the value field</typeparam>
		/// <param name="items">DataSource to convert into a select list</param>
		/// <param name="valueFieldSelector">Expression used to identify the data value field</param>
		/// <param name="textFieldSelector">Expression used to identify the data text field</param>
		/// <param name="selectedValue">The selected value</param>
		/// <returns>A populated SelectList</returns>
		public static SelectList ToSelectList<T, TValueField>(this IEnumerable<T> items, Expression<Func<T, TValueField>> valueFieldSelector, Expression<Func<T, object>> textFieldSelector, TValueField selectedValue) 
		{
			string textField = ExpressionToName(textFieldSelector);
			string valueField = ExpressionToName(valueFieldSelector);

			return new SelectList(items, valueField, textField, selectedValue);
		}

		/// <summary>
		/// Converts an enumerable into a SelectList.
		/// </summary>
		/// <typeparam name="T">Type of item in the collection</typeparam>
		/// <typeparam name="TValueField">Type of the value field</typeparam>
		/// <param name="items">DataSource to convert into a select list</param>
		/// <param name="valueFieldSelector">Expression used to identify the data value field</param>
		/// <param name="textFieldSelector">Expression used to identify the data text field</param>
		/// <param name="selectedValueSelector">A predicate that can be used to specify which values should be selected</param>
		/// <returns>A populated SelectList</returns>
		public static MultiSelectList ToSelectList<T, TValueField>(this IEnumerable<T> items, Expression<Func<T, TValueField>> valueFieldSelector, Expression<Func<T, object>> textFieldSelector, Func<T, bool> selectedValueSelector) 
		{
			var selectedItems = items.Where(selectedValueSelector);
			string textField = ExpressionToName(textFieldSelector);
			string valueField = ExpressionToName(valueFieldSelector);

			return new MultiSelectList(items, valueField, textField, selectedItems);
		}

		private static string ExpressionToName<T, TValue>(Expression<Func<T, TValue>> expression)
		{
			var memberExpression = RemoveUnary(expression.Body) as MemberExpression;
			return memberExpression == null ? string.Empty : memberExpression.Member.Name;
		}

		private static Expression RemoveUnary(Expression body)
		{
			var unary = body as UnaryExpression;
			if(unary != null)
			{
				return unary.Operand;
			}
			return body;
		}
	}
}