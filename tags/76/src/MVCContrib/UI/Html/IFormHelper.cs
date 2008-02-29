using System.Collections;
using MvcContrib.UI.Tags;
using System.Web.Mvc;

namespace MvcContrib.UI.Html
{
	public interface IFormHelper
	{
		ViewContext ViewContext { get; set; }
		string TextField(string name);
		string TextField(TextBox attributes);
		string TextField(string name, IDictionary attributes);
		string PasswordField(string name);
		string PasswordField(Password attributes);
		string PasswordField(string name, IDictionary attributes);
		string HiddenField(string name);
		string HiddenField(string name, IDictionary attributes);
		string HiddenField(HiddenField options);
		string CheckBoxField(string name);
		string CheckBoxField(string name, IDictionary attributes);
		string CheckBoxField(CheckBoxField options);
		string TextArea(string name);
		string TextArea(string name, IDictionary attributes);
		string TextArea(TextArea options);
		string Submit();
		string Submit(string value);
		string Submit(string value, IDictionary attributes);
		string Submit(SubmitButton options);
		string ImageButton(string src, string alt);
		string ImageButton(string src, string alt, IDictionary attributes);
		string ImageButton(InputImage options);
		string Select(string name, object dataSource, string textField, string valueField);
		string Select(string name, object dataSource, string textField, string valueField, IDictionary attributes);
		string Select(object dataSource, Select options);
		string RadioField(string name);
		string RadioField(string name, IDictionary attributes);
		string RadioField(RadioField options);
		CheckBoxList CheckBoxList(string name, object dataSource, string textField, string valueField);
		CheckBoxList CheckBoxList(string name, object dataSource, string textField, string valueField, IDictionary commonAttributes);
		CheckBoxList CheckBoxList(object dataSource, CheckBoxList options);
		RadioList RadioList(string name, object dataSource, string textField, string valueField);
		RadioList RadioList(string name, object dataSource, string textField, string valueField, IDictionary commonAttributes);
		RadioList RadioList(object dataSource, RadioList options);
	}
}