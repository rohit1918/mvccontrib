using System;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.UI.Html.Grid;

namespace MvcContrib.Pagination
{
	/// <summary>
	/// Extension methods for creating paged lists.
	/// </summary>
	public static class SortableHelper
	{
		/// <summary>
		/// Converts the specified IEnumerable into an ISortable using the default page size and returns the specified page number.
		/// </summary>
		/// <typeparam name="T">Type of object in the collection</typeparam>
		/// <param name="source">Source enumerable to convert to the paged and sorted list.</param>
		/// <param name="pageNumber">The page number to return.</param>
		/// <param name="sortColumn">The column currently sorted on</param>
		/// <param name="sortDescending">The direction for the sort</param>
		/// <returns>An ISortable of T</returns>
		public static ISortable<T> AsSortable<T>(this IEnumerable<T> source, int pageNumber, string sortColumn, bool sortDescending)
		{
			return source.AsSortable(pageNumber, EnumerablePagination<T>.DefaultPageSize, sortColumn, sortDescending);
		}

		/// <summary>
		/// Converts the speciied IEnumerable into an IPagination using the specified page size and returns the specified page. 
		/// </summary>
		/// <typeparam name="T">Type of object in the collection</typeparam>
		/// <param name="source">Source enumerable to convert to the paged list.</param>
		/// <param name="pageNumber">The page number to return.</param>
		/// <param name="pageSize">Number of objects per page.</param>
		/// <param name="sortColumn">The column currently sorted on</param>
		/// <param name="sortDescending">The direction for the sort</param>
		/// <returns>An ISortable of T</returns>
		public static ISortable<T> AsSortable<T>(this IEnumerable<T> source, int pageNumber, int pageSize, string sortColumn, bool sortDescending)
		{
			if (pageNumber < 1)
			{
				throw new ArgumentOutOfRangeException("pageNumber", "The page number should be greater than or equal to 1.");
			}

			if (source is IQueryable<T>)
				return new QueryableSortable<T>(source.AsQueryable(), pageNumber, pageSize, sortColumn, sortDescending);
			return new EnumerableSortable<T>(source, pageNumber, pageSize, sortColumn, sortDescending);
		}

		/// <summary>
		/// Converts the speciied IEnumerable into an ISortable using the specified page size and returns the specified page. 
		/// </summary>
		/// <typeparam name="T">Type of object in the collection</typeparam>
		/// <param name="source">Source enumerable to convert to the paged list.</param>
		/// <param name="gridParams">The grid params from the request</param>
		/// <returns>An ISortable of T</returns>
		public static ISortable<T> AsSortable<T>(this IEnumerable<T> source, GridParams gridParams)
		{
			if (source is IQueryable<T>)
				return new QueryableSortable<T>(source.AsQueryable(), gridParams);
			return new EnumerableSortable<T>(source, gridParams);
		}
	}
}
