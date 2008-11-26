using System;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.UI.Html.Grid;

namespace MvcContrib.Pagination
{
	/// <summary>
	/// Extension methods for creating paged lists.
	/// </summary>
	public static class PaginationHelper
	{
		/// <summary>
		/// Converts the specified IEnumerable into an IPagination using the default page size and returns the specified page number.
		/// </summary>
		/// <typeparam name="T">Type of object in the collection</typeparam>
		/// <param name="source">Source enumerable to convert to the paged list.</param>
		/// <param name="pageNumber">The page number to return.</param>
		/// <returns>An IPagination of T</returns>
		public static IPagination<T> AsPagination<T>(this IEnumerable<T> source, int pageNumber)
		{
			return source.AsPagination(pageNumber, EnumerablePagination<T>.DefaultPageSize);
		}

		/// <summary>
		/// Converts the speciied IEnumerable into an IPagination using the specified page size and returns the specified page. 
		/// </summary>
		/// <typeparam name="T">Type of object in the collection</typeparam>
		/// <param name="source">Source enumerable to convert to the paged list.</param>
		/// <param name="pageNumber">The page number to return.</param>
		/// <param name="pageSize">Number of objects per page.</param>
		/// <returns>An IPagination of T</returns>
		public static IPagination<T> AsPagination<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
		{
			if(pageNumber < 1)
			{
				throw new ArgumentOutOfRangeException("pageNumber", "The page number should be greater than or equal to 1.");
			}

			if(source is IQueryable<T>)
				return new QueryablePagination<T>(source.AsQueryable(), pageNumber, pageSize);
			return new EnumerablePagination<T>(source, pageNumber, pageSize);
		}

		/// <summary>
		/// Converts the speciied IEnumerable into an IPagination using the specified page size and returns the specified page. 
		/// </summary>
		/// <typeparam name="T">Type of object in the collection</typeparam>
		/// <param name="source">Source enumerable to convert to the paged list.</param>
		/// <param name="gridParams">The grid params from the request</param>
		/// <returns>An IPagination of T</returns>
		public static IPagination<T> AsPagination<T>(this IEnumerable<T> source, GridParams gridParams)
		{
			if (source is IQueryable<T>)
				return new QueryablePagination<T>(source.AsQueryable(), gridParams);
			return new EnumerablePagination<T>(source, gridParams);
		}
	}
}
