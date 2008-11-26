using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.UI.Html.Grid;

namespace MvcContrib.Pagination
{
	/// <summary>
	/// Executes an IQueryable in order to created a paged set of objects.
	/// The query is not executed until the QueryablePagination is enumerated or one of its properties is invoked.
	/// The results of the query are cached.
	/// </summary>
	/// <typeparam name="T">Type of objects in the collection.</typeparam>
	public class QueryablePagination<T> : EnumerablePagination<T>
	{
		/// <summary>
		/// The query to execute.
		/// </summary>
		public IQueryable<T> Query { get; protected set; }

		/// <summary>
		/// Creates a new instance of the <see cref="QueryablePagination{T}"/> class.
		/// </summary>
		/// <param name="query">The query to page.</param>
		/// <param name="pageNumber">The current page number.</param>
		/// <param name="pageSize">Number of items per page.</param>
		public QueryablePagination(IQueryable<T> query, int pageNumber, int pageSize)
			: base(query, pageNumber, pageSize)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
			Query = query;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="QueryablePagination{T}"/> class.
		/// </summary>
		/// <param name="query">The query to page.</param>
		/// <param name="gridParams">An initialized GridParams from the request</param>
		public QueryablePagination(IQueryable<T> query, GridParams gridParams)
			: base(query, gridParams)
		{
			Query = query;
		}

		/// <summary>
		/// Executes the query if it has not already been executed.
		/// </summary>
		protected override void TryExecuteQuery()
		{
			//Results is not null, means query has already been executed.
			if (results != null)
				return;
			totalItems = Query.Count();
			results = ExecuteQuery();
		}

		/// <summary>
		/// Calls Queryable.Skip/Take to perform the pagination.
		/// </summary>
		/// <returns>The paged set of results.</returns>
		protected virtual IList<T> ExecuteQuery()
		{
			int numberToSkip = (PageNumber - 1) * PageSize;
			return Query.Skip(numberToSkip).Take(PageSize).ToList();
		}
	}
}
