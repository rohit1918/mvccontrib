using System;
using System.Collections;
using MvcContrib.UI.Tags;
using System.Web.Mvc;

namespace MvcContrib.UI.Html
{
	public interface IFormHelper
	{
		ViewContext ViewContext { get; set; }
		IDataBinder Binder { get; set; }
		[Obsolete] string TextField(string name);
		[Obsolete] string TextField(TextBox attributes);
		[Obsolete] string TextField(string name, IDictionary attributes);
		[Obsolete] string PasswordField(string name);
		[Obsolete] string PasswordField(Password attributes);
		[Obsolete] string PasswordField(string name, IDictionary attributes);
		[Obsolete] string HiddenField(string name);
		[Obsolete] string HiddenField(string name, IDictionary attributes);
		[Obsolete] string HiddenField(HiddenField options);
		[Obsolete] string CheckBoxField(string name);
		[Obsolete] string CheckBoxField(string name, IDictionary attributes);
		[Obsolete] string CheckBoxField(CheckBoxField options);
		[Obsolete] string TextArea(string name);
		[Obsolete] string TextArea(string name, IDictionary attributes);
		[Obsolete] string TextArea(TextArea options);
		[Obsolete] string Submit();
		[Obsolete] string Submit(string value);
		[Obsolete] string Submit(string value, IDictionary attributes);
		[Obsolete] string Submit(SubmitButton options);
		[Obsolete] string ImageButton(string src, string alt);
		[Obsolete] string ImageButton(string src, string alt, IDictionary attributes);
		[Obsolete] string ImageButton(InputImage options);
		[Obsolete] string Select(string name, object dataSource, string textField, string valueField);
		[Obsolete] string Select(string name, object dataSource, string textField, string valueField, IDictionary attributes);
		[Obsolete] string Select(object dataSource, Select options);
		[Obsolete] string Select<T>(string name) where T : struct;
		[Obsolete] string Select<T>(string name, IDictionary attributes) where T : struct;
		[Obsolete] string RadioField(string name, object value);
		[Obsolete] string RadioField(string name, object value, IDictionary attributes);
		[Obsolete] string RadioField(RadioField options);
		[Obsolete] CheckBoxList CheckBoxList(string name, object dataSource, string textField, string valueField);
		[Obsolete] CheckBoxList CheckBoxList(string name, object dataSource, string textField, string valueField, IDictionary commonAttributes);
		[Obsolete] CheckBoxList CheckBoxList(object dataSource, CheckBoxList options);
		[Obsolete] RadioList RadioList(string name, object dataSource, string textField, string valueField);
		[Obsolete] RadioList RadioList(string name, object dataSource, string textField, string valueField, IDictionary commonAttributes);
		[Obsolete] RadioList RadioList(object dataSource, RadioList options);
		[Obsolete] void For<T>(T dataItem, string url, Action<SmartForm<T>> block);
		[Obsolete] void For<T>(T dataItem, string url, IDictionary attributes, Action<SmartForm<T>> block);
		[Obsolete] void For<T>(string viewDataKey, string url, IDictionary attributes, Action<SmartForm<T>> block);
	}
}
