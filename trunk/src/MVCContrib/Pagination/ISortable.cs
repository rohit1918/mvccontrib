namespace MvcContrib.Pagination
{
	///<summary>
	/// A collection of objects that can be sorted by column as well as split into pages
	///</summary>
	public interface ISortable : IPagination
	{
		///<summary>
		/// The column to sort on
		///</summary>
		string SortColumn { get; }

		///<summary>
		/// The direction to sort
		///</summary>
		bool SortDescending { get; }
	}

	/// <summary>
	/// Generic form of <see cref="ISortable"/>
	/// </summary>
	/// <typeparam name="T">Type of object being paged</typeparam>
	public interface ISortable<T> : ISortable, IPagination<T>
	{
		 
	}
}
