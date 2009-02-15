using System.Collections;
using System.Collections.Generic;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Sections for a grid.
	/// </summary>
	public class GridSections<T> : IDictionary<GridSection, GridSection<T>>
	{
		private readonly Dictionary<GridSection, GridSection<T>> _sections = new Dictionary<GridSection, GridSection<T>>();

		public IGridSection<T> RowStart()
		{
			var section = new GridSection<T>();
			_sections[GridSection.RowStart] = section;
			return section;
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
	public class GridSection<T> : IGridSection<T> {}

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