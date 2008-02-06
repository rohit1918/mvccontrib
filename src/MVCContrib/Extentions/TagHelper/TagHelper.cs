using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;

namespace MvcContrib.Extentions
{
    public class HtmlAttribute
    {
        public HtmlAttribute()
        {
        }
        public HtmlAttribute(string attribute, string value)
        {
            Attribute = attribute;
            Value = value;
        }
        private string _attribute;
        private string _value;
        public string Attribute
        {
            get
            {
                return _attribute;
            }
            set
            {
                _attribute = MakeAttributeNameSafe(value);
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is HtmlAttribute)
            {
                if (((HtmlAttribute)obj).Attribute == this.Attribute)
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return this.Attribute + "=\"" + System.Web.HttpUtility.HtmlEncode(this.Value) + "\"";
        }

        public override int GetHashCode()
        {
            return Attribute.GetHashCode();
        }
        public static string MakeAttributeNameSafe(string name)
        {
            return name.ToLower().Replace(" ", "").Replace("<", "").Replace("&", "").Replace("\"", "").Replace("'", ""); //todo: Properly clean the attribute name
        }
    }

    public class HtmlAttribList : IEnumerable<HtmlAttribute>
    {
        private List<HtmlAttribute> _items = new List<HtmlAttribute>();
        public HtmlAttribList()
            : base()
        {
        }
        public HtmlAttribList(object attributes)
            : base()
        {

            PropertyInfo[] PIList = attributes.GetType().GetProperties();
            foreach (PropertyInfo pi in PIList)
            {
                string name = pi.Name;
                object objVal = pi.GetValue(attributes, null);
                string val = objVal.ToString();
                this.Add(new HtmlAttribute(name, val));
            }
        }
        public override string ToString()
        {
            return _items.ToFormattedList("{0} ").Trim();
        }
        public void AddAttribute(string newAttrib, string value)
        {
            this.Add(new HtmlAttribute(newAttrib, value.ToString()));
        }

        public void AddAttribute(string newAttrib, string value, int Index)
        {
            this.Add(new HtmlAttribute(newAttrib, value.ToString()), Index);
        }

        public void Add(HtmlAttribute attribute)
        {
            foreach (HtmlAttribute ha in this._items)
            {
                if (ha.Equals(attribute))
                {
                    throw new Exception("Attribute alread added");
                }
            }
            _items.Add(attribute);
        }

        public void Add(HtmlAttribute attribute, int Index)
        {
            foreach (HtmlAttribute ha in this._items)
            {
                if (ha.Equals(attribute))
                {
                    throw new Exception("Attribute alread added");
                }
            }
            _items.Insert(Index,attribute);
        }

        public void Remove(string AttributeName)
        {
            HtmlAttribute val = null;
            foreach (HtmlAttribute ha in this._items)
            {
                if (ha.Attribute == HtmlAttribute.MakeAttributeNameSafe(AttributeName))
                {
                    val = ha;
                    break;
                }
            }
            if (val != null)
            {
                _items.Remove(val);
            }
        }

        public void Clear()
        {
            _items.Clear();
        }

        #region IEnumerable<HtmlAttribute> Members

        public IEnumerator<HtmlAttribute> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion
    }

    public enum INPUT_TYPES
    {
        BUTTON,
        CHECKBOX,
        FILE,
        HIDDEN,
        IMAGE,
        PASSWORD,
        RADIO,
        RESET,
        SUBMIT,
        TEXT
    }

    public static class TagHelper
    {
        public static string GenerateXHTMLTag(this HtmlHelper helper, string tag)
        {
            return GenerateXHTMLTag(helper, tag, null, "", false);
        }
        public static string GenerateXHTMLTag(this HtmlHelper helper, string tag, HtmlAttribList attributes)
        {
            return helper.GenerateXHTMLTag(tag, attributes, string.Empty, false);
        }
        public static string GenerateXHTMLTag(this HtmlHelper helper, string tag, HtmlAttribList attributes, bool requireSeporateClose)
        {
            return helper.GenerateXHTMLTag(tag, attributes, string.Empty, requireSeporateClose);
        }
        public static string GenerateXHTMLTag(this HtmlHelper helper, string tag, HtmlAttribList attributes, string innerText)
        {
            return helper.GenerateXHTMLTag(tag, attributes, innerText, false);
        }
        public static string GenerateXHTMLTag(this HtmlHelper helper, string tag, HtmlAttribList attributes, string innerText, bool requireSeporateClose)
        {
            string tagFormat = "<{0} {1} {2}>{3}{4}";
            string closeTagformat = "</{0}>";
            bool closeTag = requireSeporateClose;
            if (tag == null)
            {
                throw new ArgumentException("tag must contain a value");
            }
            if (tag.Trim() == string.Empty)
            {
                throw new ArgumentException("tag must contain a value");
            }
            if (attributes == null)
            {
                attributes = new HtmlAttribList();
            }
            if (innerText != null)
            {
                if (innerText.Trim() != string.Empty)
                {
                    closeTag = true;
                }
                else
                {
                    innerText = string.Empty;
                }
            }
            else
            {
                innerText = string.Empty;
            }
            return string.Format(tagFormat, tag, attributes.ToString(), (!closeTag ? "/" : ""), innerText, (closeTag ? string.Format(closeTagformat, tag) : ""));
        }

        public static string MakeInputTag(this HtmlHelper helper, INPUT_TYPES type, string htmlName, string value)
        {
            return MakeInputTag(helper, type, htmlName, htmlName, value, null, 0, 0, false);
        }
        public static string MakeInputTag(this HtmlHelper helper, INPUT_TYPES type, string htmlName, string value, HtmlAttribList attributes)
        {
            return MakeInputTag(helper, type, htmlName, htmlName, value, attributes, 0, 0, false);
        }
        public static string MakeInputTag(this HtmlHelper helper, INPUT_TYPES type, string htmlName, string value, HtmlAttribList attributes, int size)
        {
            return MakeInputTag(helper, type, htmlName, htmlName, value, attributes, size, 0, false);
        }
        public static string MakeInputTag(this HtmlHelper helper, INPUT_TYPES type, string htmlName, string value, HtmlAttribList attributes, int size, int maxlength)
        {
            return MakeInputTag(helper, type, htmlName, htmlName, value, attributes, size, maxlength, false);
        }
        public static string MakeInputTag(this HtmlHelper helper, INPUT_TYPES type, string htmlName, string value, HtmlAttribList attributes, int size, int maxlength, bool isChecked)
        {
            return MakeInputTag(helper, type, htmlName, htmlName, value, attributes, size, maxlength, isChecked);
        }

        public static string MakeInputTag(this HtmlHelper helper, INPUT_TYPES type, string htmlName, string id, string value, HtmlAttribList attributes, int size, int maxlength, bool isChecked)
        {
            HtmlAttribList atts;

            if (htmlName == null)
            {
                throw new ArgumentException("htmlName required");
            }
            if (htmlName.Trim() == null)
            {
                throw new ArgumentException("htmlName required");
            }
            htmlName = htmlName.Replace("\"", "");

            if (attributes != null)
                atts = attributes;
            else
                atts = new HtmlAttribList();

            atts.AddAttribute("id", ((id==string.Empty || id==null)?htmlName:id));
            atts.AddAttribute("name", htmlName);

            if (size > 0)
            {
                atts.AddAttribute("size", size.ToString());
            }
            if (maxlength > 0)
            {
                atts.AddAttribute("maxlength", maxlength.ToString());
            }

            atts.AddAttribute("type", type.ToString().ToLower(), 0);

            if (value != null)
            {
                atts.AddAttribute("value", value.ToString());
            }

            if (isChecked)
            {
                atts.AddAttribute("checked", "checked");
            }
            return helper.GenerateXHTMLTag("input", atts);
        }

    }
}
