using System.Collections.Generic;
using MvcContrib.UI.Html.Grid;

namespace MvcContrib.Pagination
{
	public class EnumerableSortable<T> : EnumerablePagination<T>, ISortable<T>
	{
		public EnumerableSortable(IEnumerable<T> collection, int pageNumber, int pageSize, string sortColumn, bool sortDirection)
			: base(collection, pageNumber, pageSize)
		{
			SortColumn = sortColumn;
			SortDescending = sortDirection;
		}

		public EnumerableSortable(IEnumerable<T> collection, GridParams data)
			: base(collection, data)
		{
			SortColumn = data.SortColumn;
			SortDescending = data.SortDescending;
		}

		///<summary>
		/// The column to sort on
		///</summary>
		public string SortColumn { get; protected set; }

		///<summary>
		/// The direction to sort
		///</summary>
		public bool SortDescending { get; protected set; }
	}
}
