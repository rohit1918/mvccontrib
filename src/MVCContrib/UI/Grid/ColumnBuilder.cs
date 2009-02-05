using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Builds grid columns
	/// </summary>
	public class ColumnBuilder<T> : IEnumerable<GridColumn<T>>
	{
		private readonly List<GridColumn<T>> columns = new List<GridColumn<T>>();

		/// <summary>
		/// Specifies a column should be constructed for the specified property.
		/// </summary>
		/// <param name="propertySpecifier">Lambda that specifies the property for which a column should be constructed</param>
		public IGridColumn<T> For(Expression<Func<T, object>> propertySpecifier)
		{
			var column = new GridColumn<T>(propertySpecifier.Compile());
			
			string inferredName = ExpressionToName(propertySpecifier);

			if(!string.IsNullOrEmpty(inferredName))
			{
				column.Named(inferredName);
			}

			columns.Add(column);
			return column;
		}

		public IEnumerator<GridColumn<T>> GetEnumerator()
		{
			return columns.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Grabs the property name from a member expression.
		/// </summary>
		/// <param name="expression">The expression</param>
		/// <returns>The name of the property</returns>
		public static string ExpressionToName<TProperty>(Expression<Func<T, TProperty>> expression)
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