using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MvcContrib.Extentions;

namespace MvcContrib.Extentions
{
    public static class FormUIHelper
    {
        #region TextBox
        public static string FormText(this HtmlHelper helper, string htmlName)
        {
            return helper.FormText(htmlName, "", null, 0, 0);
        }
        public static string FormText(this HtmlHelper helper, string htmlName, int size)
        {
            return helper.FormText(htmlName, "", null, size, 0);
        }
        public static string FormText(this HtmlHelper helper, string htmlName, int size, int maxlength)
        {
            return helper.FormText(htmlName, "", null, size, maxlength);
        }
        public static string FormText(this HtmlHelper helper, string htmlName, HtmlAttribList attributes)
        {
            return helper.FormText(htmlName, "", attributes, 0, 0);
        }
        public static string FormText(this HtmlHelper helper, string htmlName, HtmlAttribList attributes, int size)
        {
            return helper.FormText(htmlName, "", attributes, size, 0);
        }
        public static string FormText(this HtmlHelper helper, string htmlName, string value)
        {
            return helper.FormText(htmlName, value, null, 0, 0);
        }
        public static string FormText(this HtmlHelper helper, string htmlName, string value, int size)
        {
            return helper.FormText(htmlName, value, null, size, 0);
        }
        public static string FormText(this HtmlHelper helper, string htmlName, string value, int size, int maxlength)
        {
            return helper.FormText(htmlName, value, null, size, maxlength);
        }
        public static string FormText(this HtmlHelper helper, string htmlName, string value, HtmlAttribList attributes)
        {
            return helper.FormText(htmlName, value, attributes, 0, 0);
        }
        public static string FormText(this HtmlHelper helper, string htmlName, string value, HtmlAttribList attributes, int size)
        {
            return helper.FormText(htmlName, value, attributes, size, 0);
        }
        public static string FormText(this HtmlHelper helper, string htmlName, string value, HtmlAttribList attributes, int size, int maxlength)
        {
            return helper.MakeInputTag(INPUT_TYPES.TEXT, htmlName, value,attributes,size, maxlength,false);
        }
        #endregion

        #region Password
        public static string FormPassword(this HtmlHelper helper, string htmlName)
        {
            return helper.FormPassword(htmlName, "", null, 0, 0);
        }
        public static string FormPassword(this HtmlHelper helper, string htmlName, int size)
        {
            return helper.FormPassword(htmlName, "", null, size, 0);
        }
        public static string FormPassword(this HtmlHelper helper, string htmlName, int size, int maxlength)
        {
            return helper.FormPassword(htmlName, "", null, size, maxlength);
        }
        public static string FormPassword(this HtmlHelper helper, string htmlName, HtmlAttribList attributes)
        {
            return helper.FormPassword(htmlName, "", attributes, 0, 0);
        }
        public static string FormPassword(this HtmlHelper helper, string htmlName, HtmlAttribList attributes, int size)
        {
            return helper.FormPassword(htmlName, "", attributes, size, 0);
        }
        public static string FormPassword(this HtmlHelper helper, string htmlName, string value)
        {
            return helper.FormPassword(htmlName, value, null, 0, 0);
        }
        public static string FormPassword(this HtmlHelper helper, string htmlName, string value, int size)
        {
            return helper.FormPassword(htmlName, value, null, size, 0);
        }
        public static string FormPassword(this HtmlHelper helper, string htmlName, string value, int size, int maxlength)
        {
            return helper.FormPassword(htmlName, value, null, size, maxlength);
        }
        public static string FormPassword(this HtmlHelper helper, string htmlName, string value, HtmlAttribList attributes)
        {
            return helper.FormPassword(htmlName, value, attributes, 0, 0);
        }
        public static string FormPassword(this HtmlHelper helper, string htmlName, string value, HtmlAttribList attributes, int size)
        {
            return helper.FormPassword(htmlName, value, attributes, size, 0);
        }
        public static string FormPassword(this HtmlHelper helper, string htmlName, string value, HtmlAttribList attributes, int size, int maxlength)
        {
            return helper.MakeInputTag(INPUT_TYPES.PASSWORD, htmlName, value, attributes, size, maxlength, false);
        }
        #endregion

        #region TextArea
        public static string FormTextArea(this HtmlHelper helper, string htmlName)
        {
            return helper.FormTextArea(htmlName, "", null, 0, 0);
        }
        public static string FormTextArea(this HtmlHelper helper, string htmlName, int cols, int rows)
        {
            return helper.FormTextArea(htmlName, "", null, cols, rows);
        }
        public static string FormTextArea(this HtmlHelper helper, string htmlName, HtmlAttribList attributes)
        {
            return helper.FormTextArea(htmlName, "", attributes, 0, 0);
        }
        public static string FormTextArea(this HtmlHelper helper, string htmlName, HtmlAttribList attributes, int cols, int rows)
        {
            return helper.FormTextArea(htmlName, "", attributes, cols, rows);
        }
        public static string FormTextArea(this HtmlHelper helper, string htmlName, string value)
        {
            return helper.FormTextArea(htmlName, value, null, 0, 0);
        }
        public static string FormTextArea(this HtmlHelper helper, string htmlName, string value, int cols, int rows)
        {
            return helper.FormTextArea(htmlName, value, null, cols, rows);
        }
        public static string FormTextArea(this HtmlHelper helper, string htmlName, string value, HtmlAttribList attributes)
        {
            return helper.FormTextArea(htmlName, value, attributes, 0, 0);
        }
        public static string FormTextArea(this HtmlHelper helper, string htmlName, string value, HtmlAttribList attributes, int cols, int rows)
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

            atts.AddAttribute("id", htmlName);
            atts.AddAttribute("name", htmlName);

            if (cols > 0)
            {
                atts.AddAttribute("cols", cols.ToString());
            }

            if (rows > 0)
            {
                atts.AddAttribute("rows", rows.ToString());
            }

            return helper.GenerateXHTMLTag("textarea", atts, System.Web.HttpUtility.HtmlEncode(value.ToString()), true);



        }
        #endregion

        #region Hidden
        public static string FormHidden(this HtmlHelper helper, string htmlName, string value)
        {
            return helper.FormHidden(htmlName, value, null);
        }
        public static string FormHidden(this HtmlHelper helper, string htmlName, string value, HtmlAttribList attributes)
        {
            return helper.MakeInputTag(INPUT_TYPES.HIDDEN, htmlName, value, attributes, 0, 0, false);
        }
        #endregion

        #region CheckBox
        public static string FormCheckBox(this HtmlHelper helper, string htmlName)
        {
            return helper.FormCheckBox(htmlName, htmlName, "", "", false, null);
        }
        public static string FormCheckBox(this HtmlHelper helper, string htmlName, bool selected)
        {
            return helper.FormCheckBox(htmlName, htmlName, "", "", selected, null);
        }
        public static string FormCheckBox(this HtmlHelper helper, string htmlName, string value, bool selected)
        {
            return helper.FormCheckBox(htmlName, htmlName, value, "", selected, null);
        }
        public static string FormCheckBox(this HtmlHelper helper, string htmlName, string value)
        {
            return helper.FormCheckBox(htmlName, htmlName, value, "", false, null);
        }
        public static string FormCheckBox(this HtmlHelper helper, string htmlName, HtmlAttribList attributes)
        {
            return helper.FormCheckBox(htmlName, htmlName, "", "", false, attributes);
        }
        public static string FormCheckBox(this HtmlHelper helper, string htmlName, bool selected, HtmlAttribList attributes)
        {
            return helper.FormCheckBox(htmlName, htmlName, "", "", selected, attributes);
        }
        public static string FormCheckBox(this HtmlHelper helper, string htmlName, string value, bool selected, HtmlAttribList attributes)
        {
            return helper.FormCheckBox(htmlName, htmlName, value, "", selected, attributes);
        }
        public static string FormCheckBox(this HtmlHelper helper, string htmlName, string value, HtmlAttribList attributes)
        {
            return helper.FormCheckBox(htmlName, htmlName, value, "", false, attributes);
        }
        public static string FormCheckBox(this HtmlHelper helper, string htmlName, string value, string text)
        {
            return helper.FormCheckBox(htmlName, value, text, false, null);
        }
        public static string FormCheckBox(this HtmlHelper helper, string htmlName, string value, string text, bool selected)
        {
            return helper.FormCheckBox(htmlName, htmlName, value, text, selected, null);
        }
        public static string FormCheckBox(this HtmlHelper helper, string htmlName, string value, string text, HtmlAttribList attributes) 
        {
            return helper.FormCheckBox(htmlName, htmlName, value, text, false, attributes);
        }
        public static string FormCheckBox(this HtmlHelper helper, string htmlName, string value, string text, bool selected, HtmlAttribList attributes)
        {
            return helper.FormCheckBox(htmlName, htmlName, value, text, false, attributes);
        }
        public static string FormCheckBox(this HtmlHelper helper, string htmlName, string id, string value, string text, bool selected, HtmlAttribList attributes)
        {
            string extText = string.Empty;
            if (text != null && text != string.Empty)
            {
                extText = " " + text;
            }
            return helper.MakeInputTag(INPUT_TYPES.CHECKBOX, htmlName, id, value, attributes, 0, 0, selected) + extText;
        }
        #endregion

        #region CheckBox List
        public static string[] FormCheckBoxList(this HtmlHelper helper, string htmlName, IDictionary<string, string> options)
        {
            return helper.FormCheckBoxList(htmlName, options, null, null);
        }
        public static string[] FormCheckBoxList(this HtmlHelper helper, string htmlName, IDictionary<string, string> options, HtmlAttribList attributes)
        {
            return helper.FormCheckBoxList(htmlName, options, null, attributes);
        }
        public static string[] FormCheckBoxList(this HtmlHelper helper, string htmlName, IDictionary<string, string> options, IList<string> selectedVals, HtmlAttribList attributes)
        {
            if (htmlName == null)
            {
                throw new ArgumentException("htmlName required");
            }
            if (htmlName.Trim() == null)
            {
                throw new ArgumentException("htmlName required");
            }
            htmlName = htmlName.Replace("\"", "");

            if (selectedVals == null)
            {
                selectedVals = new List<String>();
            }

            List<string> renderedOptions = new List<string>();
            int ItemNum = 0;
            foreach (string key in options.Keys)
            {
                string val = options[key];
                bool checkedVal = false;
                if (selectedVals.Contains(val))
                {
                    checkedVal = true;
                }
                renderedOptions.Add(helper.FormCheckBox(htmlName, htmlName + "_" + ItemNum.ToString(), val, key, checkedVal, attributes));
                ItemNum++;
            }
            return renderedOptions.ToArray();

        }
        #endregion

        #region Radio
        public static string FormRadio(this HtmlHelper helper, string htmlName)
        {
            return helper.FormRadio(htmlName, htmlName, "", "", false, null);
        }
        public static string FormRadio(this HtmlHelper helper, string htmlName, bool selected)
        {
            return helper.FormRadio(htmlName, htmlName, "", "", selected, null);
        }
        public static string FormRadio(this HtmlHelper helper, string htmlName, string value, bool selected)
        {
            return helper.FormRadio(htmlName, htmlName, value, "", selected, null);
        }
        public static string FormRadio(this HtmlHelper helper, string htmlName, string value)
        {
            return helper.FormRadio(htmlName, htmlName, value, "", false, null);
        }
        public static string FormRadio(this HtmlHelper helper, string htmlName, HtmlAttribList attributes)
        {
            return helper.FormRadio(htmlName, htmlName, "", "", false, attributes);
        }
        public static string FormRadio(this HtmlHelper helper, string htmlName, bool selected, HtmlAttribList attributes)
        {
            return helper.FormRadio(htmlName, htmlName, "", "", selected, attributes);
        }
        public static string FormRadio(this HtmlHelper helper, string htmlName, string value, bool selected, HtmlAttribList attributes)
        {
            return helper.FormRadio(htmlName, htmlName, value, "", selected, attributes);
        }
        public static string FormRadio(this HtmlHelper helper, string htmlName, string value, HtmlAttribList attributes)
        {
            return helper.FormRadio(htmlName, htmlName, value, "", false, attributes);
        }
        public static string FormRadio(this HtmlHelper helper, string htmlName, string value, string text)
        {
            return helper.FormRadio(htmlName, htmlName, value, text, false, null);
        }
        public static string FormRadio(this HtmlHelper helper, string htmlName, string value, string text, bool selected)
        {
            return helper.FormRadio(htmlName, htmlName, value, text, selected, null);
        }
        public static string FormRadio(this HtmlHelper helper, string htmlName, string value, string text, HtmlAttribList attributes)
        {
            return helper.FormRadio(htmlName, htmlName, value, text, false, attributes);
        }
        public static string FormRadio(this HtmlHelper helper, string htmlName, string value, string text, bool selected, HtmlAttribList attributes)
        {
            return helper.FormRadio(htmlName, htmlName, value, text, selected, attributes);
        }
        public static string FormRadio(this HtmlHelper helper, string htmlName, string id,string value, string text, bool selected, HtmlAttribList attributes)
        {

            string extText = string.Empty;
            if (text != null && text != string.Empty)
            {
                extText = " " + text;
            }
            return helper.MakeInputTag(INPUT_TYPES.RADIO, htmlName, id, value, attributes, 0, 0, selected) + extText;
        }
        #endregion

        #region Radio List

        public static string[] FormRadioList(this HtmlHelper helper, string htmlName, IDictionary<string, string> options )
        {
            return helper.FormRadioList(htmlName, options, null, null);
        }
        public static string[] FormRadioList(this HtmlHelper helper, string htmlName, IDictionary<string, string> options, HtmlAttribList attributes)
        {
            return helper.FormRadioList(htmlName, options, null, attributes);
        }
        public static string[] FormRadioList(this HtmlHelper helper, string htmlName, IDictionary<string, string> options, IList<string> selectedVals, HtmlAttribList attributes)
        {
            if (htmlName == null)
            {
                throw new ArgumentException("htmlName required");
            }
            if (htmlName.Trim() == null)
            {
                throw new ArgumentException("htmlName required");
            }
            htmlName = htmlName.Replace("\"", "");

            List<string> renderedOptions = new List<string>();
            int ItemNum = 0;

            if (selectedVals == null)
            {
                selectedVals = new List<String>();
            }

            foreach (string key in options.Keys)
            {
                string val = options[key];
                bool checkedVal = false;
                if (selectedVals.Contains(val))
                {
                    checkedVal = true;
                }
                renderedOptions.Add(helper.FormRadio(htmlName, htmlName + "_" + ItemNum.ToString(), val, key, checkedVal, attributes));
                ItemNum++;
            }
            return renderedOptions.ToArray();

        }
        #endregion

        #region Button
        public static string FormButton(this HtmlHelper helper, string htmlName, string Text, string onclick)
        {
            HtmlAttribList attributes = new HtmlAttribList();
            if (onclick != null && onclick != string.Empty)
                attributes.AddAttribute("onclick", onclick);
            return helper.FormButton(htmlName, Text, attributes);
        }
        public static string FormButton(this HtmlHelper helper, string htmlName, string Text, string onclick, HtmlAttribList attributes)
        {
            if (attributes == null)
                attributes = new HtmlAttribList();
            if (onclick != null && onclick != string.Empty)
                attributes.AddAttribute("onclick", onclick);
            return helper.FormButton(htmlName, Text, attributes);
        }
        public static string FormButton(this HtmlHelper helper, string htmlName, string Text)
        {
            return helper.FormButton(htmlName, Text, new HtmlAttribList());
        }
        public static string FormButton(this HtmlHelper helper, string htmlName, string Text, HtmlAttribList attributes)
        {
            return helper.MakeInputTag(INPUT_TYPES.BUTTON, htmlName, Text, attributes, 0, 0, false);
        }
        #endregion

        #region Submit
        public static string FormSubmit(this HtmlHelper helper, string htmlName )
        {
            return helper.FormSubmit(htmlName, "Submit", null);
        }
        public static string FormSubmit(this HtmlHelper helper, string htmlName, string Text)
        {
            return helper.FormSubmit(htmlName, Text, null);
        }
        public static string FormSubmit(this HtmlHelper helper, string htmlName, HtmlAttribList attributes)
        {
            return helper.FormSubmit(htmlName, "Submit", attributes);
        }
        public static string FormSubmit(this HtmlHelper helper, string htmlName, string Text, HtmlAttribList attributes)
        {
            return helper.MakeInputTag(INPUT_TYPES.SUBMIT, htmlName, Text, attributes, 0, 0, false);
        }
        #endregion

        #region Reset
        public static string FormReset(this HtmlHelper helper, string htmlName)
        {
            return helper.FormReset(htmlName, "Reset", null);
        }
        public static string FormReset(this HtmlHelper helper, string htmlName, HtmlAttribList attributes)
        {
            return helper.FormReset(htmlName, "Reset", attributes);
        }
        public static string FormReset(this HtmlHelper helper, string htmlName, string Text)
        {
            return helper.FormReset(htmlName, Text, null);
        }
        public static string FormReset(this HtmlHelper helper, string htmlName, string Text, HtmlAttribList attributes)
        {
            return helper.MakeInputTag(INPUT_TYPES.RESET, htmlName, Text, attributes, 0, 0, false);
        }
        #endregion

        #region File
        public static string FormFile(this HtmlHelper helper, string htmlName)
        {
            return helper.FormFile(htmlName, null);
        }
        public static string FormFile(this HtmlHelper helper, string htmlName, HtmlAttribList attributes)
        {
            return helper.MakeInputTag(INPUT_TYPES.FILE, htmlName, null, attributes, 0, 0, false);
        }
        #endregion

        #region Select
        public static string FormSelect(this HtmlHelper helper, string htmlName, IDictionary<string, string> options)
        {
            return helper.FormListBox(htmlName, options, null, 0, null);
        }
        public static string FormSelect(this HtmlHelper helper, string htmlName, IDictionary<string, string> options, HtmlAttribList attributes)
        {
            return helper.FormListBox(htmlName, options, null, 0, attributes);
        }
        public static string FormSelect(this HtmlHelper helper, string htmlName, IDictionary<string, string> options, IList<string> selectedVals)
        {
            return helper.FormListBox(htmlName, options, selectedVals, 0, null);
        }
        public static string FormSelect(this HtmlHelper helper, string htmlName, IDictionary<string, string> options, IList<string> selectedVals, HtmlAttribList attributes)
        {
            return helper.FormListBox(htmlName, options, selectedVals, 0, attributes);
        }
        public static string FormListBox(this HtmlHelper helper, string htmlName, IDictionary<string,string> options, IList<string> selectedVals ,int size , HtmlAttribList attributes)
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

            atts.AddAttribute("id", htmlName);
            atts.AddAttribute("name", htmlName);

            List<string> renderedOptions = new List<string>();

            HtmlAttribList optAttribList = new HtmlAttribList();

            if (selectedVals == null)
            {
                selectedVals = new List<String>(); 
            }

            foreach (string key in options.Keys)
            {
                HtmlAttribList htList = new HtmlAttribList();
                string val = options[key];
                if (selectedVals.Contains(val))
                {
                    optAttribList.AddAttribute("selected", "selected");
                }
                optAttribList.AddAttribute("value", val);
                renderedOptions.Add(helper.GenerateXHTMLTag("option", optAttribList, key, true));
                optAttribList.Clear();
            }

            if (size != 0)
            {
                atts.AddAttribute("size", size.ToString());
            }
            string innerText = renderedOptions.ToFormattedList("\t\t{0}" + Environment.NewLine);
            return helper.GenerateXHTMLTag("select", atts, Environment.NewLine + innerText + "\t", false);

        }
        
        #endregion

        #region Image

        public static string FormImageButton(this HtmlHelper helper, string htmlName, string reletivePath)
        {
            return helper.FormImageButton(htmlName, null, reletivePath, null);
        }
        public static string FormImageButton(this HtmlHelper helper, string htmlName, string reletivePath, HtmlAttribList attributes)
        {
            return helper.FormImageButton(htmlName, null, reletivePath, attributes);
        }
        public static string FormImageButton(this HtmlHelper helper, string htmlName, string value, string reletivePath)
        {
            return helper.FormImageButton(htmlName, value, reletivePath, null);
        }
        public static string FormImageButton(this HtmlHelper helper, string htmlName, string value, string reletivePath, HtmlAttribList attributes)
        {
            string src = helper.ResolveUrl(reletivePath);
            if (attributes == null)
                attributes = new HtmlAttribList();
            attributes.AddAttribute("src", src);
            return helper.MakeInputTag(INPUT_TYPES.IMAGE, htmlName, value, attributes, 0, 0, false);
        }
        #endregion
    }
}