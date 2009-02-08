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
		private Action<ViewContext, TextWriter> _customHeaderRenderer;
		private Action<ViewContext, TextWriter, T> _customItemRenderer;

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
		/// Custom item renderer
		/// </summary>
		public Action<ViewContext, TextWriter, T> CustomItemRenderer
		{
			get { return _customItemRenderer; }
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

		public IGridColumn<T> Header(string header)
		{
			_customHeaderRenderer = (c, w) => w.Write(header);
			return this;
		}

		public IGridColumn<T> HeaderPartial(string partialName)
		{
			_customHeaderRenderer = (c, w) => {
				var view = FindView(c, partialName);
				view.Render(c, w);
			};
			return this;
		}

		public IGridColumn<T> Partial(string partialName)
		{
			_customItemRenderer = (context, writer, item) => {
				var view = FindView(context, partialName);
				var newViewData = new ViewDataDictionary<T>(item);
				var newContext = new ViewContext(context, context.View, newViewData, context.TempData);
				view.Render(newContext, writer);
			};
			return this;
		}

		//TODO: Referencing ViewEngines here is nasty. Think of a better way to do this.
		private IView FindView(ViewContext context, string viewName)
		{
			var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);

			if (viewResult.View == null) {
				// we need to generate an exception containing all the locations we searched
				var locationsText = new StringBuilder();
				foreach (string location in viewResult.SearchedLocations) {
					locationsText.AppendLine();
					locationsText.Append(location);
				}

				throw new InvalidOperationException(string.Format(CouldNotFindView, viewName, locationsText));
			}

			return viewResult.View;
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