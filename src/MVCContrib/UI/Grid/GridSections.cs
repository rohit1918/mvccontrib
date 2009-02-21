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
	public class GridSections<T> : IDictionary<GridSection, GridSection<T>> where T : class
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

		#region IDictionary members

		GridSection<T> IDictionary<GridSection, GridSection<T>>.this[GridSection key]
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

		void ICollection<KeyValuePair<GridSection, GridSection<T>>>.Add(KeyValuePair<GridSection, GridSection<T>> item)
		{
			((ICollection<KeyValuePair<GridSection, GridSection<T>>>)_sections).Add(item);
		}

		void ICollection<KeyValuePair<GridSection, GridSection<T>>>.Clear()
		{
			_sections.Clear();
		}

		bool ICollection<KeyValuePair<GridSection, GridSection<T>>>.Contains(KeyValuePair<GridSection, GridSection<T>> item)
		{
			return ((ICollection<KeyValuePair<GridSection, GridSection<T>>>)_sections).Contains(item);
		}

		void ICollection<KeyValuePair<GridSection, GridSection<T>>>.CopyTo(KeyValuePair<GridSection, GridSection<T>>[] array,int arrayIndex)
		{
			((ICollection<KeyValuePair<GridSection, GridSection<T>>>)_sections).CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<GridSection, GridSection<T>>>.Remove(KeyValuePair<GridSection, GridSection<T>> item)
		{
			return ((ICollection<KeyValuePair<GridSection, GridSection<T>>>)_sections).Remove(item);
		}

		int ICollection<KeyValuePair<GridSection, GridSection<T>>>.Count
		{
			get { return _sections.Count; }
		}

		bool ICollection<KeyValuePair<GridSection, GridSection<T>>>.IsReadOnly
		{
			get { return false; }
		}

		bool IDictionary<GridSection, GridSection<T>>.ContainsKey(GridSection key)
		{
			return _sections.ContainsKey(key);
		}

		void IDictionary<GridSection, GridSection<T>>.Add(GridSection key, GridSection<T> value)
		{
			_sections.Add(key, value);
		}

		bool IDictionary<GridSection, GridSection<T>>.Remove(GridSection key)
		{
			return _sections.Remove(key);
		}

		bool IDictionary<GridSection, GridSection<T>>.TryGetValue(GridSection key, out GridSection<T> value)
		{
			return _sections.TryGetValue(key, out value);
		}


		ICollection<GridSection> IDictionary<GridSection, GridSection<T>>.Keys
		{
			get { return _sections.Keys; }
		}

		ICollection<GridSection<T>> IDictionary<GridSection, GridSection<T>>.Values
		{
			get { return _sections.Values; }
		}

		#endregion

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