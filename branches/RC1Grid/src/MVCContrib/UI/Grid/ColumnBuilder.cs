using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Builds grid columns
	/// </summary>
	public class ColumnBuilder<T> : ICollection<GridColumn<T>> where T : class 
	{
		private readonly List<GridColumn<T>> _columns = new List<GridColumn<T>>();

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

			_columns.Add(column);
			return column;
		}

		/// <summary>
		/// Specifies that a custom column should be constructed with the specified name.
		/// </summary>
		/// <param name="name"></param>
		public IGridColumn<T> For(string name) 
		{
			var column = new GridColumn<T>(x => string.Empty);
			_columns.Add(column);
			return column.Named(name).Partial(name);
		}

		public IEnumerator<GridColumn<T>> GetEnumerator()
		{
			return _columns.GetEnumerator();
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

		void ICollection<GridColumn<T>>.Add(GridColumn<T> column)
		{
			_columns.Add(column);
		}

		void ICollection<GridColumn<T>>.Clear()
		{
			_columns.Clear();
		}

		bool ICollection<GridColumn<T>>.Contains(GridColumn<T> column)
		{
			return _columns.Contains(column);
		}

		void ICollection<GridColumn<T>>.CopyTo(GridColumn<T>[] array, int arrayIndex)
		{
			_columns.CopyTo(array, arrayIndex);
		}

		bool ICollection<GridColumn<T>>.Remove(GridColumn<T> column)
		{
			return _columns.Remove(column);
		}

		int ICollection<GridColumn<T>>.Count
		{
			get { return _columns.Count; }
		}

		bool ICollection<GridColumn<T>>.IsReadOnly
		{
			get { return false; }
		}
	}
}