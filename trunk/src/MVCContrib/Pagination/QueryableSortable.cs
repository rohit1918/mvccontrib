using System.Linq;
using MvcContrib.UI.Html.Grid;

namespace MvcContrib.Pagination
{
	public class QueryableSortable<T> : QueryablePagination<T>, ISortable<T>
	{
		public QueryableSortable(IQueryable<T> query, int pageNumber, int pageSize, string sortColumn, bool sortDirection)
			: base(query, pageNumber, pageSize)
		{
			SortColumn = sortColumn;
			SortDescending = sortDirection;
		}

		public QueryableSortable(IQueryable<T> query, GridParams data)
			: base(query, data)
		{
			SortColumn = data.SortColumn;
			SortDescending = data.SortDescending;
		}

		public string SortColumn { get; protected set; }

		public bool SortDescending { get; protected set; }
	}
}
