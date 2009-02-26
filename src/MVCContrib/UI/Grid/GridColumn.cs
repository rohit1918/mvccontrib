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
	public class GridColumn<T> : IGridColumn<T> where T : class
	{
		private string _name;
		private bool _doNotSplit;
		private readonly Func<T, object> _columnValueFunc;
		private Func<T, bool> _cellCondition = x => true;
		private string _format;
		private bool _visible = true;
		private bool _htmlEncode = true;
		private readonly IDictionary<string, object> _headerAttributes = new Dictionary<string, object>();
		private Func<GridRowViewData<T>, IDictionary<string, object>> _attributes = x => new Dictionary<string, object>();
		private Action<RenderingContext> _customHeaderRenderer;
		private Action<RenderingContext, T> _customItemRenderer;

		/// <summary>
		/// Creates a new instance of the GridColumn class
		/// </summary>
		public GridColumn(Func<T, object> columnValueFunc, string name)
		{
			_name = name;
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

		IGridColumn<T> IGridColumn<T>.Attributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes)
		{
			_attributes = attributes;
			return this;
		}

		/// <summary>
		/// Custom header renderer
		/// </summary>
		public Action<RenderingContext> CustomHeaderRenderer
		{
			get { return _customHeaderRenderer; }
			set { _customHeaderRenderer = value; }
		}

		/// <summary>
		/// Custom item renderer
		/// </summary>
		public Action<RenderingContext, T> CustomItemRenderer
		{
			get { return _customItemRenderer; }
			set { _customItemRenderer = value; }
		}

		/// <summary>
		/// Additional attributes for the column header
		/// </summary>
		public IDictionary<string, object> HeaderAttributes
		{
			get { return _headerAttributes; }
		}

		/// <summary>
		/// Additional attributes for the cell
		/// </summary>
		public Func<GridRowViewData<T>, IDictionary<string, object>> Attributes
		{
			get { return _attributes; }
		}

		public IGridColumn<T> Named(string name)
		{
			_name = name;
			_doNotSplit = true;
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
}