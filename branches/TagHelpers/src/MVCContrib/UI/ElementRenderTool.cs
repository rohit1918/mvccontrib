using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcContrib.UI
{
	public class ElementRenderTool
	{
		private IElement _element;
		public ElementRenderTool(IElement el)
		{
			_element = el;
		}
		public override string ToString()
		{
			return RenderElement(_element);
		}
		protected string RenderElement(IElement el)
		{
			string tagFormat = "<{0} {1}{2}>{3}{4}";
			string innerTextformat = "{0}";
			if (el.EscapeInnerText && (!string.IsNullOrEmpty(el.InnerText)))
			{
				innerTextformat = "/*<![CDATA[*/\r\n{0}\r\n//]]>";
			}
			string closeTagformat = "</{0}>";
			bool closeTag = el.UseFullCloseTag;
			if (string.IsNullOrEmpty(el.Tag))
			{
				throw new ArgumentException("tag must contain a value");
			}
			if (!closeTag)
			{
				if (!string.IsNullOrEmpty(el.InnerText))
				{
					closeTag = true;
				}
			}

			return string.Format(tagFormat,
				el.Tag,
				RenderAttributes(el.Attributes),
				(!closeTag ? "/" : ""),
				string.Format(innerTextformat, el.InnerText),
				(closeTag ? string.Format(closeTagformat, el.Tag) : ""));
		}

		protected string RenderAttributes(IAttributes attribs)
		{
			if (attribs.Count > 4)
			{
				int totalLength = attribs.GetEstLength();
				StringBuilder sb = new StringBuilder(totalLength + 10);
				foreach (KeyValuePair<string, string> attrib in attribs)
				{
					//format " [attribute]="[value encoded]""
					sb.Append(" ").Append(attrib.Key).Append("=\"");
					if (attrib.Key == "id")
						sb.Append(EncodeAttribute(attrib.Value.Replace('.', '-'))).Append("\"");
					else
						sb.Append(EncodeAttribute(attrib.Value)).Append("\"");
				}
				return sb.ToString().Trim();
			}
			else
			{
				string val = string.Empty;
				foreach (KeyValuePair<string, string> attrib in attribs)
				{
					if (attrib.Key == "id")
						val += string.Format(" {0}=\"{1}\"", attrib.Key, EncodeAttribute(attrib.Value.Replace('.', '-')));
					else
						val += string.Format(" {0}=\"{1}\"", attrib.Key, EncodeAttribute(attrib.Value));
				}
				return val.Trim();
			}
		}

		protected string EncodeAttribute(string attributeValue)
		{
			return System.Web.HttpUtility.HtmlEncode(attributeValue);
		}
	}
}
