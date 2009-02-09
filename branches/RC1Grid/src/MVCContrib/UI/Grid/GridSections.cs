using System.Collections;
using System.Collections.Generic;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Sections for a grid.
	/// </summary>
	public class GridSections<T> : IGridSections<T>
	{
		private Dictionary<GridSection, GridSection<T>> _sections = new Dictionary<GridSection, GridSection<T>>();

		public IGridSection<T> RowStart()
		{
			var section = new GridSection<T>();
			_sections[GridSection.RowStart] = section;
			return section;
		}

		IEnumerator<KeyValuePair<GridSection, GridSection<T>>> IEnumerable<KeyValuePair<GridSection, GridSection<T>>>.GetEnumerator()
		{
			return _sections.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _sections.GetEnumerator();
		}

		GridSection<T> IGridSections<T>.this[GridSection key]
		{
			get
			{
				GridSection<T> section;
				return _sections.TryGetValue(key, out section) ? section : null;
			}
		}
	}

	public interface IGridSections<T> : IEnumerable<KeyValuePair<GridSection, GridSection<T>>>
	{
		GridSection<T> this[GridSection key] { get; }
	}

	/// <summary>
	/// Grid section
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GridSection<T> : IGridSection<T>
	{
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