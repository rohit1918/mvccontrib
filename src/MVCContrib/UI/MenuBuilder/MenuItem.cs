using System;
using System.IO;
using System.Web.Mvc;
using MvcContrib.UI.Html;

namespace MvcContrib.UI.MenuBuilder
{
	///<summary>
	/// Used internally to create the menu, use MvcContrib.UI.MenuBuilder.Menu to create a menu.
	///</summary>
	public class MenuItem
	{
		public string Title { get; set; }
		public string Icon { get; set; }
		public string HelpText { get; set; }
		public string ActionUrl { get; set; }

		public string AnchorClass { get; set; }
		public string IconClass { get; set; }
		public string ItemClass { get; set; }
		public string IconDirectory { get; set; }


		protected bool Prepared { get; set; }

		public MenuItem()
		{
			Prepared = false;
		}

		protected virtual void RenderLink(TextWriter writer)
		{
			writer.Write(string.Format("<a{0}{1}>", ActionUrl.AsAttribute("href"),HelpText.AsAttribute("title")));
			RenderIcon(writer);
			RenderTitle(writer);
			writer.Write(string.Format("</a>"));
		}

		protected virtual void RenderIcon(TextWriter writer)
		{
			if (!string.IsNullOrEmpty(Icon))
			{
				string iconPath = (IconDirectory ?? "") + Icon;
				writer.Write(string.Format("<img border=\"0\"{0}{1}{2}/>", iconPath.AsAttribute("src"), Title.AsAttribute("alt"),
				                           IconClass.AsClassAttribute()));
			}
		}

		protected virtual void RenderTitle(TextWriter writer)
		{
			if (!string.IsNullOrEmpty(Title))
				writer.Write(Title);
		}

		/// <summary>
		/// Renders the menu according to the current RequestContext to the specified TextWriter
		/// </summary>
		/// <param name="requestContext">The current RequestContext</param>
		/// <param name="writer">The TextWriter for output</param>
		public virtual void RenderHtml(ControllerContext requestContext, TextWriter writer)
		{
			if (Prepare(requestContext))
				RenderHtml(writer);
		}

		/// <summary>
		/// Used internally to render the menu. Do not call this directly without first calling Prepare, or call RenderHtml(RequestContext ...)
		/// </summary>
		/// <param name="writer">The TextWriter to output the HTML to</param>
		public virtual void RenderHtml(TextWriter writer)
		{
			if(!Prepared)
				throw new InvalidOperationException("Must call Prepare before RenderHtml(TextWriter) or call RenderHtml(RequestContext, TextWriter)");
			writer.Write(string.Format("<li{0}>", ItemClass.AsClassAttribute())); 
			RenderLink(writer);
			writer.Write("</li>");
		}

		/// <summary>
		/// Called internally by RenderHtml(RequestContext, TextWriter) to remove empty items from lists and generate urls.
		/// Can also be called externally to prepare the menu for serialization into Json/Xml
		/// </summary>
		/// <param name="requestContext">The current RequestContext</param>
		/// <returns>if this item should be rendered</returns>
		public virtual bool Prepare(ControllerContext requestContext)
		{
			Prepared = true;
			return true;
		}
		
		public MenuItem SetTitle(string title)
		{
			Title = title;
			return this;
		}

		public MenuItem SetIcon(string icon)
		{
			Icon = icon;
			return this;
		}

		public MenuItem SetHelpText(string helpText)
		{
			HelpText = helpText;
			return this;
		}

		public MenuItem SetActionUrl(string actionUrl)
		{
			ActionUrl = actionUrl;
			return this;
		}

		public MenuItem SetAnchorClass(string anchorClass)
		{
			AnchorClass = anchorClass;
			return this;
		}

		public MenuItem SetIconClass(string iconClas)
		{
			IconClass = iconClas;
			return this;
		}

		public MenuItem SetItemClass(string itemClass)
		{
			ItemClass = itemClass;
			return this;
		}


		public MenuItem SetIconDirectory(string iconDirectory)
		{
			IconDirectory = iconDirectory;
			return this;
		}

	}
}