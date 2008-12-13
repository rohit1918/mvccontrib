using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

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

		public void Render(T item, TextWriter writer, Action renderStartCell, Action renderEndCell)
		{
			//Column condition has been specified. Continue to the next column if the condition fails.
			if(ColumnCondition != null && !ColumnCondition())
			{
				return;
			}

			//A custom item section has been specified - render it and continue to the next iteration.
			if(Name != null && CustomRenderer != null)
			{
				CustomRenderer(item);
				return;
			}

			renderStartCell();

			object value = null;

			bool failedCellCondition = false;

			//Cell condition has been specified. Skip rendering of this cell if the cell condition fails.
			if(CellCondition != null && !CellCondition(item))
			{
				failedCellCondition = true;
			}

			if(!failedCellCondition)
			{
				//Invoke the delegate to retrieve the value to be displayed in the cell.
				if(ColumnDelegate != null)
				{
					value = ColumnDelegate(item);
				}
				else //If there isn't a column delegate, attempt to use reflection instead (for anonymous types)
				{
					var property = item.GetType().GetProperty(Name);
					if(property != null)
					{
						value = property.GetValue(item, null);
					}
				}


				if(value != null)
				{
					if(Format != null) //Use custom output format if specified.
					{
						writer.Write(string.Format(Format, value));
					}
					else if(Encode) //HTML-Encode unless encoding has been explicitly disabled for this cell.
					{
						writer.Write(HttpUtility.HtmlEncode(value.ToString()));
					}
					else
					{
						writer.Write(value.ToString());
					}
				}
			}
			renderEndCell();
		}

		public void RenderHeader(TextWriter writer, Action renderHeaderStart, Action renderHeaderEnd)
		{
			//Allow for custom header overrides.
			if (CustomHeader != null) {
				CustomHeader();
			}
			else {
				//Skip if the custom Column Condition fails.
				if (ColumnCondition != null && !ColumnCondition()) {
					return;
				}

				renderHeaderStart();

				writer.Write(Name);

				renderHeaderEnd();
			}
		}

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
		/// The attributs to apply to the header of the column.
		/// </summary>
		IDictionary HeaderAttributes { get; set; }

		/// <summary>
		/// Renders the contents of the cell.
		/// </summary>
		/// <param name="item">The item for which the cell should be rendered</param>
		/// <param name="writer">The textwriter to which he output should be written</param>
		/// <param name="renderStartCell">Action to be called before the cell is rendered</param>
		/// <param name="renderEndCell">Action to be called after the cell is rendered</param>
		void Render(T item, TextWriter writer, Action renderStartCell, Action renderEndCell);


		/// <summary>
		/// Renders the header for the column
		/// </summary>
		/// <param name="writer">The textwriter to which the output should be written</param>
		/// <param name="renderHeaderStart">Action to be called before the header is written</param>
		/// <param name="renderHeaderEnd">Action to be called after the header is written</param>
		void RenderHeader(TextWriter writer, Action renderHeaderStart, Action renderHeaderEnd);
	}
}