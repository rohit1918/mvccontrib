namespace MvcContrib.UI.Html.Grid
{
	///<summary>
	///</summary>
	public class GridParams
	{
		public const int DefaultPageSize = 20;

		public int PageNumber { get; set; }
		public string SortColumn { get; set; }
		public bool SortDescending { get; set; }
		public string QueryKey { get; set; }

		private int pageSize;
		public int PageSize
		{
			get { return pageSize == 0 ? DefaultPageSize : pageSize; }
			set { pageSize = value; }
		}

		public int? TotalItems { get; set; }
	}
}
