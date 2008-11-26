using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.UI.Html.Grid;

namespace MvcContrib.Pagination
{
	/// <summary>
	/// Uses a collection of objects, assuming that the paging has already occurred
	/// </summary>
	/// <typeparam name="T">Type of objects in the collection.</typeparam>
	public class EnumerablePagination<T> : IPagination<T>
	{
		/// <summary>
		/// Default page size.
		/// </summary>
		public const int DefaultPageSize = 20;
		protected IList<T> results;
		protected int totalItems;

		/// <summary>
		/// The query to execute.
		/// </summary>
		public IEnumerable<T> Collection { get; protected set; }
		public int PageNumber { get; protected set; }
		public int PageSize { get; protected set; }
		/// <summary>
		/// Creates a new instance of the <see cref="QueryablePagination{T}"/> class.
		/// </summary>
		/// <param name="collection">The collection of items.</param>
		/// <param name="pageNumber">The current page number.</param>
		/// <param name="pageSize">Number of items per page.</param>
		public EnumerablePagination(IEnumerable<T> collection, int pageNumber, int pageSize)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
			Collection = collection;
			totalItems = collection.Count();
		}

		/// <summary>
		/// Creates a new instance of the <see cref="QueryablePagination{T}"/> class.
		/// </summary>
		/// <param name="collection">The collection of items.</param>
		/// <param name="gridParams">An initialized GridParams from the request</param>
		public EnumerablePagination(IEnumerable<T> collection, GridParams gridParams)
			: this(collection, gridParams.PageNumber, gridParams.PageSize)
		{
			QueryKey = gridParams.QueryKey;
			if (gridParams.TotalItems != null)
				totalItems = gridParams.TotalItems.Value;
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			TryExecuteQuery();
			foreach (var item in results)
			{
				yield return item;
			}
		}

		/// <summary>
		/// Does nothing for the collection pagination class, used for the queryable pagination
		/// </summary>
		protected virtual void TryExecuteQuery()
		{
			results = Collection.ToList();
		}

		public IEnumerator GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		public int TotalItems
		{
			get
			{
				TryExecuteQuery();
				return totalItems;
			}
		}

		public int TotalPages
		{
			get { return (int)Math.Ceiling(((double)TotalItems) / PageSize); }
		}

		public int FirstItem
		{
			get
			{
				TryExecuteQuery();
				return ((PageNumber - 1) * PageSize) + 1;
			}
		}

		public int LastItem
		{
			get
			{
				return FirstItem + results.Count - 1;
			}
		}

		public bool HasPreviousPage
		{
			get { return PageNumber > 1; }
		}

		public bool HasNextPage
		{
			get { return PageNumber < TotalPages; }
		}

		public virtual string QueryKey
		{
			get;
			protected set;
		}

	}
}
