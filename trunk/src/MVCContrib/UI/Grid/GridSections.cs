using System;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Sections for a grid.
	/// </summary>
	public class GridSections<T> : IGridSections<T> where T : class
	{
		private GridRow<T> _row = new GridRow<T>();
		private GridRow<T> _headerRow = new GridRow<T>();

		public GridRow<T> Row
		{
			get { return _row; }
		}

		public GridRow<T> HeaderRow
		{
			get { return _headerRow; }
		}
	}

	public interface IGridSections<T> where T : class
	{
		GridRow<T> Row { get; }
		GridRow<T> HeaderRow { get; }
	}
}
