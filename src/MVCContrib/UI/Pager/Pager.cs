using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using MvcContrib.Pagination;

namespace MvcContrib.UI.Pager
{
	/// <summary>
	/// Renders a pager component from an IPagination datasource.
	/// </summary>
	public class Pager
	{
		private readonly IPagination _pagination;
		private readonly HttpRequestBase _request;

		private string _paginationFormat = "Showing {0} - {1} of {2} ";
		private string _paginationSingleFormat = "Showing {0} of {1} ";
		private string _paginationFirst = "first";
		private string _paginationPrev = "prev";
		private string _paginationNext = "next";
		private string _paginationLast = "last";
		private string _pageQueryName = "page";
		private Func<int, string> urlBuilder;

		/// <summary>
		/// Creates a new instance of the Pager class.
		/// </summary>
		/// <param name="pagination">The IPagination datasource</param>
		/// <param name="request">The current HTTP Request</param>
		public Pager(IPagination pagination, HttpRequestBase request)
		{
			_pagination = pagination;
			_request = request;

			urlBuilder = CreateDefaultUrl;
		}

		/// <summary>
		/// Specifies the query string parameter to use when generating pager links. The default is 'page'
		/// </summary>
		public Pager QueryParam(string queryStringParam)
		{
			_pageQueryName = queryStringParam;
			return this;
		}

		/// <summary>
		/// Specifies the format to use when rendering a pagination containing a single page. 
		/// The default is 'Showing {0} of {1}' (eg 'Showing 1 of 3')
		/// </summary>
		public Pager SingleFormat(string format)
		{
			_paginationSingleFormat = format;
			return this;
		}

		/// <summary>
		/// Specifies the format to use when rendering a pagination containing multiple pages. 
		/// The default is 'Showing {0} - {1} of {2}' (eg 'Showing 1 to 3 of 6')
		/// </summary>
		public Pager Format(string format)
		{
			_paginationFormat = format;
			return this;
		}

		/// <summary>
		/// Text for the 'first' link.
		/// </summary>
		public Pager First(string first)
		{
			_paginationFirst = first;
			return this;
		}

		/// <summary>
		/// Text for the 'prev' link
		/// </summary>
		public Pager Previous(string previous)
		{
			_paginationPrev = previous;
			return this;
		}

		/// <summary>
		/// Text for the 'next' link
		/// </summary>
		public Pager Next(string next)
		{
			_paginationNext = next;
			return this;
		}

		/// <summary>
		/// Text for the 'last' link
		/// </summary>
		public Pager Last(string last)
		{
			_paginationLast = last;
			return this;
		}

		/// <summary>
		/// Uses a lambda expression to generate the URL for the page links.
		/// </summary>
		/// <param name="urlBuilder">Lambda expression for generating the URL used in the page links</param>
		public Pager Link(Func<int, string> urlBuilder)
		{
			this.urlBuilder = urlBuilder;
			return this;
		}


		public override string ToString()
		{
			if(_pagination.TotalItems == 0)
			{
				return null;
			}

			var builder = new StringBuilder();
			builder.Append("<div class='pagination'>");
			builder.Append("<span class='paginationLeft'>");
			if(_pagination.PageSize == 1)
			{
				builder.AppendFormat(_paginationSingleFormat, _pagination.FirstItem, _pagination.TotalItems);
			}
			else
			{
				builder.AppendFormat(_paginationFormat, _pagination.FirstItem, _pagination.LastItem, _pagination.TotalItems);
			}
			builder.Append("</span>");
			builder.Append("<span class='paginationRight'>");

			if(_pagination.PageNumber == 1)
			{
				builder.Append(_paginationFirst);
			}
			else
			{
				builder.Append(CreatePageLink(1, _paginationFirst));
			}

			builder.Append(" | ");

			if(_pagination.HasPreviousPage)
			{
				builder.Append(CreatePageLink(_pagination.PageNumber - 1, _paginationPrev));
			}
			else
			{
				builder.Append(_paginationPrev);
			}


			builder.Append(" | ");

			if(_pagination.HasNextPage)
			{
				builder.Append(CreatePageLink(_pagination.PageNumber + 1, _paginationNext));
			}
			else
			{
				builder.Append(_paginationNext);
			}


			builder.Append(" | ");

			int lastPage = _pagination.TotalPages;

			if(_pagination.PageNumber < lastPage)
			{
				builder.Append(CreatePageLink(lastPage, _paginationLast));
			}
			else
			{
				builder.Append(_paginationLast);
			}


			builder.Append(@"</span></div>");


			return builder.ToString();
		}

		private string CreatePageLink(int pageNumber, string text)
		{
			string link = "<a href=\"{0}\">{1}</a>";
			return string.Format(link, urlBuilder(pageNumber), text);
		}

		private string CreateDefaultUrl(int pageNumber)
		{
			string queryString = CreateQueryString(_request.QueryString);
			string filePath = _request.FilePath;
			string url = string.Format("{0}?{1}={2}{3}", filePath, _pageQueryName, pageNumber, queryString);
			return url;
		}

		private string CreateQueryString(NameValueCollection values)
		{
			var builder = new StringBuilder();

			foreach(string key in values.Keys)
			{
				if(key == "page")
					//Don't re-add any existing 'page' variable to the querystring - this will be handled in CreatePageLink.
				{
					continue;
				}

				foreach(var value in values.GetValues(key))
				{
					builder.AppendFormat("&amp;{0}={1}", key, HttpUtility.HtmlEncode(value));
				}
			}

			return builder.ToString();
		}
	}
}