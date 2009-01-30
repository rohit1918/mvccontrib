using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using MvcContrib.UI.Html;

namespace MvcContrib.UI.MenuBuilder
{
	///<summary>
	/// Used internally to create the menu, use MvcContrib.UI.MenuBuilder.Menu to create a menu.
	///</summary>
	public class MenuList : MenuItem, ICollection<MenuItem>
	{
		private readonly List<MenuItem> items;

		public string ListClass { get; set; }
		
		public MenuList()
		{
			items = new List<MenuItem>();
			ListClass = "";
		}

		public void Add(MenuItem item)
		{
			items.Add(item);
		}

		public void Clear()
		{
			items.Clear();
		}

		public bool Contains(MenuItem item)
		{
			return items.Contains(item);
		}

		public void CopyTo(MenuItem[] array, int arrayIndex)
		{
			items.CopyTo(array, arrayIndex);
		}

		public bool Remove(MenuItem item)
		{
			return items.Remove(item);
		}

		public int Count
		{
			get
			{
				return items.Count;
			}
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public MenuItem this[int i]
		{
			get { return items[i]; }
		}

		public IEnumerator<MenuItem> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		protected virtual void RenderItems(TextWriter writer)
		{
			if (items.Count <= 0) 
				return;
			writer.Write(string.Format("<ul{0}>", ListClass.AsClassAttribute()));
			foreach (var menuBase in items)
				menuBase.RenderHtml(writer);
			writer.Write("</ul>");
		}
        
		public override void RenderHtml(TextWriter writer)
		{
			if (!Prepared)
				throw new InvalidOperationException("Must call Prepare before RenderHtml(TextWriter) or call RenderHtml(RequestContext, TextWriter)");
			if (!IsRootList)
			{
				if (items.Count == 1)
				{
					items[0].RenderHtml(writer); //if there is only one item, don't render this menu instead skip to the item
					return;
				}
				writer.Write(string.Format("<li{0}>", ItemClass.AsClassAttribute()));
				RenderLink(writer);
			}
			RenderItems(writer);
			if (!IsRootList)
				writer.Write("</li>");
		}

		public MenuList SetListClass(string listClass)
		{
			ListClass = listClass;
			return this;
		}

		protected virtual bool IsRootList
		{
			get
			{
				return Title == null && Icon == null;
			}
		}

		public override bool Prepare(ControllerContext requestContext)
		{
			var iCopy = new List<MenuItem>(items);
			foreach (var menuBase in iCopy)
			{
				if (menuBase.Prepare(requestContext) == false)
					items.Remove(menuBase);
			}
			Prepared = true;
			return items.Count > 0;
		}
	}
}