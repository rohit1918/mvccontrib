using System;
using System.Web.Mvc;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Grid section
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GridSection<T> : IGridSection<T> where T : class
	{
		private readonly Action<GridRowViewData<T>, RenderingContext> _sectionRenderer = (x, y) => { };

		/// <summary>
		/// Creates a new instance of the GridSection class.
		/// </summary>
		/// <param name="sectionRenderer">A delegate to invoke when the section is rendered</param>
		public GridSection(Action<GridRowViewData<T>, RenderingContext> sectionRenderer)
		{
			_sectionRenderer = sectionRenderer;
		}

		/// <summary>
		/// Renders the grid section.
		/// </summary>
		public void Render(RenderingContext context, GridRowViewData<T> row)
		{
			_sectionRenderer(row, context);
		}
	}

	/// <summary>
	/// Grid section
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IGridSection<T> where T : class
	{
		/// <summary>
		/// Renders the grid section.
		/// </summary>
		void Render(RenderingContext context, GridRowViewData<T> row);
	}
}