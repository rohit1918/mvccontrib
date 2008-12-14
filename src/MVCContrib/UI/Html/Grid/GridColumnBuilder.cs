using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using MvcContrib.UI.Html.Grid.Legacy;

namespace MvcContrib.UI.Html.Grid
{
	/// <summary>
	/// Constructs GridColumn objects representing the columns to be rendered in a grid.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GridColumnBuilder<T> : IRootGridColumnBuilder<T>, IGridSections<T>, IEnumerable<GridColumn<T>>
		where T : class
	{
		private readonly List<GridColumn<T>> columns = new List<GridColumn<T>>(); //Final collection of columns to render
		public Action<T> RowStartBlock { get; set; }
		public Action<T, bool> RowStartWithAlternateBlock { get; set; }
		public Action<T> RowEndBlock { get; set; }

		public IExpressionColumnBuilder<T> For(Expression<Func<T, object>> expression)
		{
			var column = new GridColumn<T>
			             	{
			             		Name = ExpressionToName(expression),
			             		ColumnDelegate = expression.Compile(),
			             	};

			columns.Add(column);
			return column;
		}

		public INestedGridColumnBuilder<T> For(Func<T, object> func, string name)
		{
			var column = new GridColumn<T>
			             	{
			             		Name = name,
			             		ColumnDelegate = func,
			             		DoNotSplit = true
			             	};
			columns.Add(column);
			return column;
		}

		public ISimpleColumnBuilder<T> For(string name)
		{
			var column = new GridColumn<T> {Name = name, DoNotSplit = true};
			columns.Add(column);
			return column;
		}

		public void RowStart(Action<T> block)
		{
			RowStartBlock = block;
		}

		public void RowStart(Action<T, bool> block)
		{
			RowStartWithAlternateBlock = block;
		}

		public void RowEnd(Action<T> block)
		{
			RowEndBlock = block;
		}

		/// <summary>
		/// Grabs the property name from a member expression.
		/// </summary>
		/// <param name="expression">The expression</param>
		/// <returns>The name of the property</returns>
		public static string ExpressionToName(Expression<Func<T, object>> expression)
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


		IEnumerator IEnumerable.GetEnumerator()
		{
			return columns.GetEnumerator();
		}

		public IEnumerator<GridColumn<T>> GetEnumerator()
		{
			return columns.GetEnumerator();
		}

		public GridColumn<T> this[int index]
		{
			get { return columns[index]; }
		}

		public void AddColumn(GridColumn<T> column)
		{
			columns.Add(column);
		}
	}
}