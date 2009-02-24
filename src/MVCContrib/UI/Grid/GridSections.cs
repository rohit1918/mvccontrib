using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Sections for a grid.
	/// </summary>
	public class GridSections<T> : IGridSections<T> where T : class
	{
		private readonly Dictionary<GridSection, GridSection<T>> _sections = new Dictionary<GridSection, GridSection<T>>();

		public void RowStart(string partialName)
		{
			_sections[GridSection.RowStart] = new GridSection<T>(partialName);
		}

		public void RowEnd(string partialName) 
		{
			_sections[GridSection.RowEnd] = new GridSection<T>(partialName);
		}

		GridSection<T> IGridSections<T>.this[GridSection key]
		{
			get
			{
				GridSection<T> section;
				return _sections.TryGetValue(key, out section) ? section : null;
			}
			set { _sections[key] = value; }
		}

		IEnumerator<KeyValuePair<GridSection, GridSection<T>>> IEnumerable<KeyValuePair<GridSection, GridSection<T>>>.GetEnumerator()
		{
			return _sections.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<GridSection, GridSection<T>>>)this).GetEnumerator();
		}
	}
	
	/// <summary>
	/// A collection of Grid Sections
	/// </summary>
	public interface IGridSections<T> : IEnumerable<KeyValuePair<GridSection, GridSection<T>>> where T : class
	{
		/// <summary>
		/// Obtains the appropriate grid section.
		/// </summary>
		/// <param name="key">The section that should be retrieved</param>
		/// <returns>A GridSection or null</returns>
		GridSection<T> this[GridSection key] { get; set; }
	}


	/// <summary>
	/// Grid section
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GridSection<T> : IGridSection<T> where T : class
	{
		private string _partialName;

		/// <summary>
		/// Creates a new instance of the GridSection class using the specified partial name. 
		/// </summary>
		/// <param name="partialName">The name of the partial view to render.</param>
		public GridSection(string partialName)
		{
			_partialName = partialName;
		}

		/// <summary>
		/// Renders the grid section.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="item"></param>
		/// <param name="isAlternate"></param>
		public void Render(RenderingContext context, T item, bool isAlternate)
		{
			var view = context.ViewEngines.TryLocatePartial(context.ViewContext, _partialName);
			var newViewData = new ViewDataDictionary<GridRowViewData<T>>(new GridRowViewData<T>(item, isAlternate));
			var newContext = new ViewContext(context.ViewContext, context.ViewContext.View, newViewData, context.ViewContext.TempData);
			view.Render(newContext, context.Writer);
		}
	}

	/// <summary>
	/// Grid section
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IGridSection<T> {}

	/// <summary>
	/// Sections for a grid.
	/// </summary>
	public enum GridSection
	{
		/// <summary>
		/// Start of each row
		/// </summary>
		RowStart,
		/// <summary>
		/// End of each row
		/// </summary>
		RowEnd
	}
}