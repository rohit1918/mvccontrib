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
		/// <param name="context"></param>
		/// <param name="item"></param>
		/// <param name="isAlternate"></param>
		public void Render(RenderingContext context, T item, bool isAlternate)
		{
			var viewData = new GridRowViewData<T>(item, isAlternate);
			_sectionRenderer(viewData, context);
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
		/// <param name="context"></param>
		/// <param name="item"></param>
		/// <param name="isAlternate"></param>
		void Render(RenderingContext context, T item, bool isAlternate);
	}
}