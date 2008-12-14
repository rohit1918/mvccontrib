using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace MvcContrib.UI.Html.Grid
{
	public class GridBuilder<T> : IGridBuilder<T> where T : class
	{
		private readonly ViewContext _context;
		private readonly Dictionary<string, string> _htmlAttributes = new Dictionary<string, string>();
		private readonly IList<string> _classes = new List<string>();
		private readonly Dictionary<string, string> _styles = new Dictionary<string, string>();
		private readonly GridColumnBuilder<T> _columnBuilder = new GridColumnBuilder<T>();
		private readonly GridOptions _gridOptions = new GridOptions();
		private IGridRenderer<T> _renderer = new Grid<T>();

		public IEnumerable<T> DataSource { get; protected set; }


		public GridBuilder(ViewContext context)
		{
			_context = context;
		}

		public GridOptions GridOptions
		{
			get { return _gridOptions; }
		}

		public IDictionary<string, string> HtmlAttributes
		{
			get { return _htmlAttributes; }
		}

		public IList<string> Classes
		{
			get { return _classes; }
		}

		public IDictionary<string, string> Styles
		{
			get { return _styles; }
		}

		public IGridBuilder<T> FromViewData(string viewDataKey)
		{
			DataSource = _context.ViewData.Eval(viewDataKey) as IEnumerable<T>;
			return this;
		}

		public IGridBuilder<T> WithData(IEnumerable<T> data)
		{
			DataSource = data;
			return this;
		}

		public IGridBuilder<T> Attribute(string attributeName, string value)
		{
			HtmlAttributes[attributeName] = value;
			return this;
		}

		public override string ToString()
		{
			Render();
			return null;
		}

		public virtual void Render()
		{
			if(Classes.Count > 0)
			{
				string cssClasses = string.Join(" ", Classes.ToArray());
				Attribute("class", cssClasses);
			}

			if(Styles.Count > 0)
			{
				string styles = string.Join(";", Styles.Select(x => x.Key + ":" + x.Value).ToArray());
				Attribute("style", styles);
			}

			_renderer.Render(DataSource, _columnBuilder, _gridOptions, _htmlAttributes, _context);
			
		}

		public IGridBuilder<T> Class(string cssClass)
		{
			Classes.Add(cssClass);
			return this;
		}

		public IGridBuilder<T> Style(string key, string value)
		{
			Styles[key] = value;
			return this;
		}

		public IGridBuilder<T> Columns(Action<IRootGridColumnBuilder<T>> columnBuilder)
		{
			if(columnBuilder == null) throw new ArgumentNullException("columnBuilder");
			columnBuilder(_columnBuilder);
			return this;
		}

		public IGridBuilder<T> Empty(string emptyMessageText)
		{
			_gridOptions.EmptyMessageText = emptyMessageText;
			return this;
		}

		public IGridBuilder<T> RenderUsing(IGridRenderer<T> customRenderer)
		{
			_renderer = customRenderer;
			return this;
		}
	}

	public static class GridExtensions
	{
		public static IGridBuilder<T> Grid<T>(this HtmlHelper helper) where T: class 
		{
			return new GridBuilder<T>(helper.ViewContext);
		}

		public static INestedGridColumnBuilder<T> RenderPartial<T>(this ISimpleColumnBuilder<T> column, string partialName) where T : class
		{
			return column.CustomRenderer((x, writer, ctx) => {
				ViewEngines.DefaultEngine.FindPartialView(ctx, partialName).View.Render(ctx, writer);
			});
		}
	}

	public interface IGridRenderer<T> where T : class 
	{
		void Render(IEnumerable<T> dataSource, GridColumnBuilder<T> columns, GridOptions options, IDictionary htmlAttributes, ViewContext context);
	}


	public interface IGridBuilder<T> where T : class
	{
		IGridBuilder<T> FromViewData(string viewDataKey);
		IGridBuilder<T> WithData(IEnumerable<T> data);
		IGridBuilder<T> Attribute(string attributeName, string value);
		IGridBuilder<T> Class(string cssClass);
		IGridBuilder<T> Style(string key, string value);
		IGridBuilder<T> Columns(Action<IRootGridColumnBuilder<T>> columnBuilder);
		IGridBuilder<T> Empty(string emptyMessageText);
		IGridBuilder<T> RenderUsing(IGridRenderer<T> customRenderer);
	}
}