using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace MvcContrib.UI.DataList
{
	/// <summary>
	/// 
	/// </summary>
	/// <example>
	/// <code>
	///       Html.DataList(_datasource)
	///           .ItHas(3).Rows
	///           .CellTemplate(x =&gt; { Writer.Write(x.ToLower()); })
	///           .CellCondition(x =&gt; x == &quot;test1&quot;)
	///           .EmptyDateSourceTemplate(() =&gt; { Writer.Write(&quot;There is no data available.&quot;); })
	///           .NoItemTemplate(() =&gt; { Writer.Write(&quot;No Data.&quot;); });
	/// </code>
	/// Shouold render:
	/// &lt;table&gt;&lt;tr&gt;&lt;td&gt;test1&lt;/td&gt;&lt;/tr&gt;&lt;tr&gt;&lt;td&gt;No Data.&lt;/td&gt;&lt;/tr&gt;&lt;tr&gt;&lt;td&gt;No Data.&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;
	/// </example>
	/// <typeparam name="T"></typeparam>
	public class DataList<T>
	{
		private readonly IEnumerable<T> _dataSource;
		private readonly Hash _tableAttributes;
		private Action _emptyDataSourceTemplate;
		private Action _noItemTemplate;
		private Func<T, bool> _cellCondition = x => true;
		private Hash _cellAttribute;
		private const string TABLE = "table";

		protected TextWriter Writer { get; set; }
		public RepeatDirection RepeatDirection { get; set; }
		//This is called columns but can mean rows anyone have a better name?
		public int TimesToRepeatColumns { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DataList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="dataSource">The data source.</param>
		/// <param name="writer">The writer.</param>
		public DataList(IEnumerable<T> dataSource, TextWriter writer)
			: this(dataSource, 0, RepeatDirection.Vertical, writer) {}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="dataSource">The data source.</param>
		/// <param name="writer">The writer.</param>
		/// <param name="tableAttributes">The table attributes.</param>
		public DataList(IEnumerable<T> dataSource, TextWriter writer, Hash tableAttributes)
			: this(dataSource, 0, RepeatDirection.Horizontal, writer, tableAttributes) {}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="dataSource">The datasource.</param>
		/// <param name="itemsInRepeatDirection">The amount of items in repeat direction.</param>
		/// <param name="repeatDirection">The repeat direction.</param>
		/// <param name="writer">The writer.</param>
		public DataList(IEnumerable<T> dataSource, int itemsInRepeatDirection, RepeatDirection repeatDirection,
		                TextWriter writer)
			: this(dataSource, itemsInRepeatDirection, repeatDirection, writer, null) {}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="dataSource">The data source.</param>
		/// <param name="itemsInRepeatDirection">The amount of items in repeat direction.</param>
		/// <param name="repeatDirection">The repeat direction.</param>
		/// <param name="writer">The writer.</param>
		/// <param name="tableAttributes">The table attributes.</param>
		public DataList(IEnumerable<T> dataSource, int itemsInRepeatDirection, RepeatDirection repeatDirection,
		                TextWriter writer, Hash tableAttributes)
		{
			_dataSource = dataSource;
			TimesToRepeatColumns = itemsInRepeatDirection;
			RepeatDirection = repeatDirection;
			_tableAttributes = tableAttributes;
			Writer = writer;
		}


		/// <summary>
		/// The main cell template.
		/// </summary>
		/// <param name="contentTemplate">The template.</param>
		/// <returns></returns>
		public virtual DataList<T> CellTemplate(Action<T> contentTemplate)
		{
			ItemRenderer = contentTemplate;
			return this;
		}

		protected void Write(string text)
		{
			Writer.Write(text);
		}

		/// <summary>
		/// If you provide an empty date source the it will use this template in the first cell.
		/// </summary>
		/// <param name="template">The template.</param>
		/// <returns></returns>
		public virtual DataList<T> EmptyDateSourceTemplate(Action template)
		{
			_emptyDataSourceTemplate = template;
			return this;
		}

		/// <summary>
		/// If you have lets say two items and you repeat 3 times
		/// then one cell is going to be empty so fill it with this template.
		/// </summary>
		/// <param name="template">The template.</param>
		/// <returns></returns>
		public virtual DataList<T> NoItemTemplate(Action template)
		{
			_noItemTemplate = template;
			return this;
		}

		/// <summary>
		/// Direction to repeat the items.
		/// </summary>
		/// <returns></returns>
		public virtual DataListOptions<T> RepeatOn()
		{
			return new DataListOptions<T>(this);
		}

		/// <summary>
		/// Filters the items that will be rendered (This should normally be done in the database)
		/// </summary>
		/// <param name="func">The condition to check.</param>
		/// <returns></returns>
		public virtual DataList<T> CellCondition(Func<T, bool> func)
		{
			_cellCondition = func;
			return this;
		}

		/// <summary>
		/// Attributes that go on every cell (&lt;td id=&quot;foo&quot; class=&quot;bar&quot;&gt;).
		/// </summary>
		/// <example>CellAttributes(id =&gt; &quot;foo&quot;, @class =&gt; &quot;bar&quot;)</example>
		/// <param name="attributes">The attributes.</param>
		/// <returns></returns>
		public virtual DataList<T> CellAttributes(params Func<object, object>[] attributes)
		{
			_cellAttribute = new Hash(attributes);
			return this;
		}

		/// <summary>
		/// Gets or sets the item renderer. You should use <see cref="CellTemplate"/>
		/// </summary>
		/// <value>The item renderer.</value>
		public virtual Action<T> ItemRenderer { get; set; }


		/// <summary>
		/// Checks if a item should be rendered this checks <see cref="CellCondition" />.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		protected virtual bool ShouldRenderCell(T item)
		{
			return _cellCondition(item);
		}

		/// <summary>
		/// Renders the cell.
		/// </summary>
		/// <param name="item">The item.</param>
		protected virtual void RenderCell(T item)
		{
			Writer.Write(string.Format("<td{0}>", _cellAttribute.BuildHtmlAttributes(true)));
			if(ItemRenderer != null)
			{
				ItemRenderer(item);
			}
			Writer.Write("</td>");
		}

		/// <summary>
		/// The amount of Columns/Rows to render.
		/// </summary>
		/// <param name="amount">The amount.</param>
		/// <returns></returns>
		public virtual DataListOptions<T> ItHas(int amount)
		{
			TimesToRepeatColumns = amount;
			return new DataListOptions<T>(this);
		}


		/// <summary>
		/// Renders this DataList.
		/// </summary>
		public virtual void Render()
		{
			Write(string.Format("<{0}{1}>", TABLE, _tableAttributes.BuildHtmlAttributes(true)));
			BuildTable();
			Write(string.Format("</{0}>", TABLE));
		}

		/// <summary>
		/// Renders to the TextWriter, and returns null. 
		/// This is by design so that it can be used with inline syntax in views.
		/// </summary>
		/// <returns>null</returns>
		public override string ToString()
		{
			Render();
			return null;
		}

		private void BuildTable()
		{
			IList<T> items = _dataSource.Where(x => _cellCondition(x)).ToList();

			int count = items.Count();

			if(count < 1)
			{
				Write("<tr><td>");
				if(_emptyDataSourceTemplate != null)
				{
					_emptyDataSourceTemplate();
				}
				Write("</td></tr>");
				return;
			}

			int tmpRepeatColumns = TimesToRepeatColumns < 1 ? count : TimesToRepeatColumns;


			if(RepeatDirection == RepeatDirection.Horizontal)
			{
				RenderHorizontal(count, tmpRepeatColumns, items);
			}
			else
			{
				RenderVertical(count, tmpRepeatColumns, items);
			}
		}

		private void RenderHorizontal(int itemCount, int repeatColumns, IList<T> items)
		{
			int rows = CalculateAmountOfCellsInOtherDirection(itemCount, repeatColumns);
			int columns = repeatColumns;

			int i = 0;


			for(int row = 0; row < rows; row++)
			{
				Write("<tr>");
				for(int column = 0; column < columns; column++)
				{
					if(i + 1 <= items.Count)
					{
						RenderCell(items[i]);
					}
					else
					{
						RenderNoItemCell();
					}
					i++;
				}
				Write("</tr>");
			}
		}

		private void RenderVertical(int itemCount, int repeatColumns, IList<T> items)
		{
			int rows = repeatColumns;
			int columns = CalculateAmountOfCellsInOtherDirection(itemCount, repeatColumns);

			int i = 0;

			int indexCol;
			for(int row = 0; row < rows; row++)
			{
				indexCol = row + 1;
				Write("<tr>");
				for(int column = 0; column < columns; column++)
				{
					if(i + 1 <= items.Count)
					{
						RenderCell(items[indexCol - 1]);
					}
					else
					{
						RenderNoItemCell();
					}

					i++;
					indexCol += columns;
				}
				Write("</tr>");
			}
		}

		private void RenderNoItemCell()
		{
			Write(string.Format("<td{0}>", _cellAttribute.BuildHtmlAttributes(true)));
			if(_noItemTemplate != null)
			{
				_noItemTemplate();
			}
			Write("</td>");
		}

		private int CalculateAmountOfCellsInOtherDirection(int itemCount, int repeatColumns)
		{
			//Do you have a better way to do this
			int columns = itemCount / repeatColumns;
			if((itemCount % repeatColumns) > 0)
			{
				columns += 1;
			}

			return columns;
		}
	}
}