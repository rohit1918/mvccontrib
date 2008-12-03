using System.Collections;

namespace MvcContrib.UI.Html.Grid
{
	/// <summary>
	/// Grid Options
	/// </summary>
	public class GridOptions
	{
		private const string Empty_Text_Key = "empty";
		private const string Pagination_Format_Text_Key = "paginationFormat";
		private const string Page_Query_Name_Text_Key = "page";
		private const string Pagination_Single_Format_Text_Key = "paginationSingleFormat";
		private const string Pagination_First_Text_Key = "first";
		private const string Pagination_Prev_Text_Key = "prev";
		private const string Pagination_Next_Text_Key = "next";
		private const string Pagination_Last_Text_Key = "last";

		public string EmptyMessageText { get; set; }
		public string PaginationFormat { get; set; }
		public string PaginationSingleFormat { get; set; }
		public string PaginationFirst { get; set; }
		public string PaginationPrev { get; set; }
		public string PaginationNext { get; set; }
		public string PaginationLast { get; set; }
		public string PageQueryName { get; set; }

		/// <summary>
		/// Creates a new instance of the GridOptions class and will attempt to extract a default set of options from the specified dictionary.
		/// This constructor is present for backwards compatibility with the Legacy Grid.
		/// </summary>
		/// <param name="options"></param>
		public GridOptions(IDictionary options) : this()
		{
			LoadFromDictionary(options);
		}

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
		}

		private void LoadFromDictionary(IDictionary options)
		{
			if(options == null) return;

			if(options.Contains(Empty_Text_Key))
			{
				EmptyMessageText = options[Empty_Text_Key] as string;
				options.Remove(Empty_Text_Key);
			}

			if(options.Contains(Pagination_Format_Text_Key))
			{
				PaginationFormat = options[Pagination_Format_Text_Key] as string;
				options.Remove(Pagination_Format_Text_Key);
			}

			if(options.Contains(Pagination_Single_Format_Text_Key))
			{
				PaginationSingleFormat = options[Pagination_Single_Format_Text_Key] as string;
				options.Remove(Pagination_Single_Format_Text_Key);
			}

			if(options.Contains(Pagination_First_Text_Key))
			{
				PaginationFirst = options[Pagination_First_Text_Key] as string;
				options.Remove(Pagination_First_Text_Key);
			}

			if(options.Contains(Pagination_Prev_Text_Key))
			{
				PaginationPrev = options[Pagination_Prev_Text_Key] as string;
				options.Remove(Pagination_Prev_Text_Key);
			}

			if(options.Contains(Pagination_Next_Text_Key))
			{
				PaginationNext = options[Pagination_Next_Text_Key] as string;
				options.Remove(Pagination_Next_Text_Key);
			}

			if(options.Contains(Pagination_Last_Text_Key))
			{
				PaginationLast = options[Pagination_Last_Text_Key] as string;
				options.Remove(Pagination_Last_Text_Key);
			}

			if(options.Contains(Page_Query_Name_Text_Key))
			{
				PageQueryName = options[Page_Query_Name_Text_Key] as string;
				options.Remove(Page_Query_Name_Text_Key);
			}
		}
	}
}