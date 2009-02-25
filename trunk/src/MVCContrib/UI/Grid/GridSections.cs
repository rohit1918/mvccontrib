using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace MvcContrib.UI.Grid
{
	/// <summary>
	/// Sections for a grid.
	/// </summary>
	public class GridSections<T> : IGridSections<T> where T : class
	{
		private const int NUMGRIDSECTIONS = 2;
		private readonly IGridSection<T>[] sections = new GridSection<T>[NUMGRIDSECTIONS];

		public void RowStart(string partialName)
		{
			this[GridSection.RowStart] = new GridSection<T>(partialName);
		}

		public void RowStart(Action<T> rowStartBlock)
		{
			this[GridSection.RowStart] = new GridSection<T>(rowStartBlock);
		}

		public void RowStart(Action<T, GridRowViewData<T>> rowStartBlock)
		{
			this[GridSection.RowStart] = new GridSection<T>(rowStartBlock);
		}

		public void RowEnd(string partialName)
		{
			this[GridSection.RowEnd] = new GridSection<T>(partialName);
		}

		public void RowEnd(Action<T> rowEndBlock)
		{
			this[GridSection.RowEnd] = new GridSection<T>(rowEndBlock);
		}

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
		void RowStart(string partialName);
		void RowStart(Action<T> rowStartBlock);
		void RowStart(Action<T, GridRowViewData<T>> rowStartBlock);
		void RowEnd(string partialName);
		void RowEnd(Action<T> rowEndBlock);
		IGridSection<T> this[GridSection gridSection] { get; set; }
	}

	/// <summary>
	/// Grid section
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GridSection<T> : IGridSection<T> where T : class
	{
		private string _partialName;
		private Action<T, GridRowViewData<T>> actionAlternateBlock;

		/// <summary>
		/// Creates a new instance of the GridSection class using the specified partial name. 
		/// </summary>
		/// <param name="partialName">The name of the partial view to render.</param>
		public GridSection(string partialName)
		{
			_partialName = partialName;
		}

		/// <summary>
		/// Creates a new instance of the GridSection class using the specified action block. 
		/// </summary>
		/// <param name="actionBlock">The action block to render.</param>
		public GridSection(Action<T> actionBlock) : this((x, vd) => actionBlock(x))
		{
			
		}

		/// <summary>
		/// Creates a new instance of the GridSection class using the specified action block. 
		/// </summary>
		/// <param name="actionAlternateBlock">The action block to render.</param>
		public GridSection(Action<T, GridRowViewData<T>> actionAlternateBlock)
		{
			this.actionAlternateBlock = actionAlternateBlock;
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

			if (actionAlternateBlock != null)
			{
				actionAlternateBlock(item, viewData);
			}
			else
			{
				var view = context.ViewEngines.TryLocatePartial(context.ViewContext, _partialName);
				var newViewData = new ViewDataDictionary<GridRowViewData<T>>(viewData);
				var newContext = new ViewContext(context.ViewContext, context.ViewContext.View, newViewData,
												 context.ViewContext.TempData);
				view.Render(newContext, context.Writer);
			}
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
