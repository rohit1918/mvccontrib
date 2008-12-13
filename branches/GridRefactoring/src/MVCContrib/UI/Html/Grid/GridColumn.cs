using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MvcContrib.UI.Html.Grid
{
	/// <summary>
	/// A column to be rendered as part of a grid.
	/// </summary>
	/// <typeparam name="T">Type of object to be rendered in the grid.</typeparam>
	public class GridColumn<T> : IGridColumn<T>, IExpressionColumnBuilder<T>, ISimpleColumnBuilder<T> where T : class
	{
		/// <summary>
		/// Creates a new instance of the <see cref="GridColumn{T}"/>
		/// </summary>
		public GridColumn()
		{
			Name = string.Empty;
			Encode = true;
		}

		/// <summary>
		/// Delegate that will be invoked on each item in the in the datasource in order to obtain the current item's value.
		/// </summary>
		public Func<T, object> ColumnDelegate { get; set; }

		private string _name;

		/// <summary>
		/// Name of the column
		/// </summary>
		public string Name
		{
			get
			{
				//By default, PascalCased property names should be split and separated by a space (eg "Pascal Cased")
				if(!DoNotSplit)
				{
					return SplitPascalCase(_name);
				}
				return _name;
			}
			set { _name = value; }
		}

		/// <summary>
		/// Custom format for the cell output.
		/// </summary>
		public string Format { get; set; }

		/// <summary>
		/// Whether or not PascalCased names should be split.
		/// </summary>
		public bool DoNotSplit { get; set; }

		/// <summary>
		/// Delegate used to hide the contents of the cells in a column.
		/// </summary>
		public Func<T, bool> CellCondition { get; set; }

		/// <summary>
		/// Delegate used to hide the entire column
		/// </summary>
		public Func<bool> ColumnCondition { get; set; }

		/// <summary>
		/// Delegate that can be used to perform custom rendering actions.
		/// </summary>
		public Action<T> CustomRenderer { get; set; }

		/// <summary>
		/// Delegate used to specify a custom heading.
		/// </summary>
		public Action CustomHeader { get; set; }

		/// <summary>
		/// Whether to HTML-Encode the output (default is true).
		/// </summary>
		public bool Encode { get; set; }

		/// <summary>
		/// The attributs to apply to the header of the column.
		/// </summary>
		public IDictionary HeaderAttributes { get; set; }

		/// <summary>
		/// Replaces pascal casing with spaces. For example "CustomerId" would become "Customer Id".
		/// Strings that already contain spaces are ignored.
		/// </summary>
		/// <param name="input">String to split</param>
		/// <returns>The string after being split</returns>
		protected virtual string SplitPascalCase(string input)
		{
			return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
		}

		INestedGridColumnBuilder<T> INestedGridColumnBuilder<T>.Formatted(string format)
		{
			Format = format;
			return this;
		}

		INestedGridColumnBuilder<T> INestedGridColumnBuilder<T>.DoNotEncode()
		{
			Encode = false;
			return this;
		}

		INestedGridColumnBuilder<T> IExpressionColumnBuilder<T>.DoNotSplit()
		{
			DoNotSplit = true;
			return this;
		}

		INestedGridColumnBuilder<T> INestedGridColumnBuilder<T>.CellCondition(Func<T, bool> condition)
		{
			CellCondition = condition;
			return this;
		}

		INestedGridColumnBuilder<T> INestedGridColumnBuilder<T>.ColumnCondition(Func<bool> condition)
		{
			ColumnCondition = condition;
			return this;
		}

		INestedGridColumnBuilder<T> ISimpleColumnBuilder<T>.Do(Action<T> block)
		{
			CustomRenderer = block;
			return this;
		}

		INestedGridColumnBuilder<T> INestedGridColumnBuilder<T>.Header(Action block)
		{
			CustomHeader = block;
			return this;
		}

		/// <summary>
		/// Applies the specified attributes to the header of the current column.
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		INestedGridColumnBuilder<T> INestedGridColumnBuilder<T>.HeaderAttributes(IDictionary attributes)
		{
			HeaderAttributes = attributes;
			return this;
		}
	}

	public interface IGridColumn<T> where T : class 
	{
		/// <summary>
		/// Delegate that will be invoked on each item in the in the datasource in order to obtain the current item's value.
		/// </summary>
		Func<T, object> ColumnDelegate { get; set; }

		/// <summary>
		/// Name of the column
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Custom format for the cell output.
		/// </summary>
		string Format { get; set; }

		/// <summary>
		/// Whether or not PascalCased names should be split.
		/// </summary>
		bool DoNotSplit { get; set; }

		/// <summary>
		/// Delegate used to hide the contents of the cells in a column.
		/// </summary>
		Func<T, bool> CellCondition { get; set; }

		/// <summary>
		/// Delegate used to hide the entire column
		/// </summary>
		Func<bool> ColumnCondition { get; set; }

		/// <summary>
		/// Delegate that can be used to perform custom rendering actions.
		/// </summary>
		Action<T> CustomRenderer { get; set; }

		/// <summary>
		/// Delegate used to specify a custom heading.
		/// </summary>
		Action CustomHeader { get; set; }

		/// <summary>
		/// Whether to HTML-Encode the output (default is true).
		/// </summary>
		bool Encode { get; set; }

		/// <summary>
		/// The attributs to apply to the header of the column.
		/// </summary>
		IDictionary HeaderAttributes { get; set; }
	}
}