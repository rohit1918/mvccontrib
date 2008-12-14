using System.Collections;

namespace MvcContrib.UI.Html.Grid
{
	/// <summary>
	/// Grid Options
	/// </summary>
	public class GridOptions
	{
		public string EmptyMessageText { get; set; }
		public string PaginationFormat { get; set; }
		public string PaginationSingleFormat { get; set; }
		public string PaginationFirst { get; set; }
		public string PaginationPrev { get; set; }
		public string PaginationNext { get; set; }
		public string PaginationLast { get; set; }
		public string PageQueryName { get; set; }

		/// <summary>
		/// Creates a new instance of the GridOptions class.
		/// </summary>
		public GridOptions()
		{
			PageQueryName = "page";
			PaginationLast = "last";
			PaginationNext = "next";
			PaginationPrev = "prev";
			PaginationFirst = "first";
			PaginationSingleFormat = "Showing {0} of {1} ";
			PaginationFormat = "Showing {0} - {1} of {2} ";
			EmptyMessageText = "There is no data available.";
		}

		
	}
}