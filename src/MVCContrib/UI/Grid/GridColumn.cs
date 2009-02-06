using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Column for the grid
	/// </summary>
	public class GridColumn<T> : IGridColumn<T>
	{
		private string _name;
		private bool _doNotSplit;
		private readonly Func<T, object> _columnValueFunc;
		private Func<T, bool> _cellCondition = x => true;
		private string _format;
		private bool _visible = true;
		private bool _htmlEncode = true;
		private readonly IDictionary<string, object> _headerAttributes = new Dictionary<string, object>();
		private Action<ViewContext, TextWriter> _customHeaderRenderer;
		private const string CouldNotFindView = "The view '{0}' or its master could not be found. The following locations were searched:{1}";
		/// <summary>
		/// Creates a new instance of the GridColumn class
		/// </summary>
		public GridColumn(Func<T, object> columnValueFunc)
		{
			_columnValueFunc = columnValueFunc;
		}

		public bool Visible
		{
			get { return _visible; }
		}

		/// <summary>
		/// Name of the column
		/// </summary>
		public string Name
		{
			get
			{
				if(_doNotSplit)
				{
					return _name;
				}
				return SplitPascalCase(_name);
			}
		}

		/// <summary>
		/// Custom header renderer
		/// </summary>
		public Action<ViewContext, TextWriter> CustomHeaderRenderer
		{
			get { return _customHeaderRenderer; }
		}

		/// <summary>
		/// Additional attributes for the column header
		/// </summary>
		public IDictionary<string, object> HeaderAttributes
		{
			get { return _headerAttributes; }
		}

		public IGridColumn<T> Named(string name)
		{
			_name = name;
			return this;
		}

		public IGridColumn<T> DoNotSplit()
		{
			_doNotSplit = true;
			return this;
		}

		public IGridColumn<T> Format(string format)
		{
			_format = format;
			return this;
		}

		public IGridColumn<T> CellCondition(Func<T, bool> func)
		{
			_cellCondition = func;
			return this;
		}

		IGridColumn<T> IGridColumn<T>.Visible(bool isVisible)
		{
			_visible = isVisible;
			return this;
		}

		public IGridColumn<T> DoNotEncode()
		{
			_htmlEncode = false;
			return this;
		}

		IGridColumn<T> IGridColumn<T>.HeaderAttributes(IDictionary<string, object> attributes)
		{
			foreach(var attribute in attributes)
			{
				_headerAttributes.Add(attribute);
			}

			return this;
		}

		public IGridColumn<T> WithHeader(string header)
		{
			_customHeaderRenderer = (c, w) => w.Write(header);
			return this;
		}

		public IGridColumn<T> WithHeaderPartial(string partialName)
		{
			//TODO: Referencing ViewEngines here is nasty. Think of a better way to do this.
			_customHeaderRenderer = CreateCustomRenderer(partialName);
			return this;
		}

		private Action<ViewContext, TextWriter> CreateCustomRenderer(string partialName)
		{
			return (c, w) => {
				var viewResult = ViewEngines.Engines.FindPartialView(c, partialName);

				if(viewResult.View == null)
				{
					// we need to generate an exception containing all the locations we searched
					var locationsText = new StringBuilder();
					foreach (string location in viewResult.SearchedLocations) {
						locationsText.AppendLine();
						locationsText.Append(location);
					}

					throw new InvalidOperationException(string.Format(CouldNotFindView, partialName, locationsText));
				}

				viewResult.View.Render(c, w);
			};
		}

		private string SplitPascalCase(string input)
		{
			if (string.IsNullOrEmpty(input)) return input;
			return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
		}

		/// <summary>
		/// Gets the value for a particular cell in this column
		/// </summary>
		/// <param name="instance">Instance from which the value should be obtained</param>
		/// <returns>Item to be rendered</returns>
		public object GetValue(T instance)
		{
			if(! _cellCondition(instance))
			{
				return null;
			}

			var value = _columnValueFunc(instance);

			if (!string.IsNullOrEmpty(_format)) 
			{
				value = string.Format(_format, value);
			}

			if(_htmlEncode && value != null)
			{
				value = HttpUtility.HtmlEncode(value.ToString());
			}


			return value;
		}
	}

	/// <summary>
	/// Grid Column fluent interface
	/// </summary>
	public interface IGridColumn<T>
	{
		/// <summary>
		/// Specified an explicit name for the column.
		/// </summary>
		/// <param name="name">Name of column</param>
		/// <returns></returns>
		IGridColumn<T> Named(string name);
		/// <summary>
		/// If the property name is PascalCased, it should not be split part.
		/// </summary>
		/// <returns></returns>
		IGridColumn<T> DoNotSplit();
		/// <summary>
		/// A custom format to use when building the cell's value
		/// </summary>
		/// <param name="format">Format to use</param>
		/// <returns></returns>
		IGridColumn<T> Format(string format);
		/// <summary>
		/// Delegate used to hide the contents of the cells in a column.
		/// </summary>
		IGridColumn<T> CellCondition(Func<T, bool> func);

		/// <summary>
		/// Determines whether the column should be displayed
		/// </summary>
		/// <param name="isVisible"></param>
		/// <returns></returns>
		IGridColumn<T> Visible(bool isVisible);

		/// <summary>
		/// Do not HTML Encode the output
		/// </summary>
		/// <returns></returns>
		IGridColumn<T> DoNotEncode();

		/// <summary>
		/// Defines additional attributes for the column heading.
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		IGridColumn<T> HeaderAttributes(IDictionary<string, object> attributes);

		/// <summary>
		/// Defines a custom format for rendering the column header.
		/// </summary>
		/// <param name="header">The format to use.</param>
		/// <returns></returns>
		IGridColumn<T> WithHeader(string header);

		/// <summary>
		/// Specifies that a partial view should be used to render the column header.
		/// </summary>
		/// <param name="partialName"></param>
		/// <returns></returns>
		IGridColumn<T> WithHeaderPartial(string partialName);
	}
}