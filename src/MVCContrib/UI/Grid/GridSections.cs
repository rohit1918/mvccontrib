using System;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Sections for a grid.
	/// </summary>
	public class GridSections<T> : IGridSections<T> where T : class
	{
		private const int NUMGRIDSECTIONS = 2;
		private readonly IGridSection<T>[] sections = new GridSection<T>[NUMGRIDSECTIONS];

		public IGridSection<T> this[GridSection gridSection]
		{
			get
			{
				if ((int)gridSection > NUMGRIDSECTIONS - 1 || (int)gridSection < 0)
					throw new ArgumentException("Unknown Grid Section: " + gridSection, "gridSection");
				return sections[(int)gridSection];
			}
			set
			{
				if ((int)gridSection > NUMGRIDSECTIONS - 1 || (int)gridSection < 0)
					throw new ArgumentException("Unknown Grid Section: " + gridSection, "gridSection");
				sections[(int)gridSection] = value;
			}
		}
	}

	public interface IGridSections<T> where T : class
	{
		IGridSection<T> this[GridSection gridSection] { get; set; }
	}

	/// <summary>
	/// Sections for a grid.
	/// </summary>
	public enum GridSection
	{
		/// <summary>
		/// Start of each row
		/// </summary>
		RowStart = 0,
		/// <summary>
		/// End of each row
		/// </summary>
		RowEnd = 1
	}
}
