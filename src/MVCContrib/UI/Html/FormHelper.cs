using System.Collections;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.Tags;
using System;

namespace MvcContrib.UI.Html
{
	public class FormHelper : BaseHelper, IFormHelper
	{
		public FormHelper(ViewContext viewContext)
		{
			ViewContext = viewContext;
		}

		public string TextField(string name)
		{
			return TextField(name, Hash.Empty);
		}

		public string TextField(string name, IDictionary attributes)
		{
			TextBox options = new TextBox(attributes);
			options.Name = name;
			return TextField(options);
		}

		public string TextField(TextBox options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.Value == null)
				options.Value = ObtainFromViewData(options.Name);

			return options.ToString();
		}

		public string HiddenField(string name)
		{
			return HiddenField(name, Hash.Empty);
		}

		public string HiddenField(string name, IDictionary attributes)
		{
			HiddenField options = new HiddenField(attributes);
			options.Name = name;
			return HiddenField(options);
		}

		public string HiddenField(HiddenField options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.Value == null)
				options.Value = ObtainFromViewData(options.Name);

			return options.ToString();
		}

		public string CheckBoxField(string name)
		{
			return CheckBoxField(name, Hash.Empty);
		}

		public string CheckBoxField(string name, IDictionary attributes)
		{
			CheckBoxField options = new CheckBoxField(attributes);
			options.Name = name;
			return CheckBoxField(options);
		}

		public string CheckBoxField(CheckBoxField options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.Value == null)
				options.Value = "true";

			if(options.Checked == null)
			{
				object value = ObtainFromViewData(options.Name);
				bool isChecked = value != null && value is bool && (bool)value;

				options.Checked = isChecked;
			}

			HiddenField hidden = new HiddenField();
			hidden.Value = "false";
			if(!string.IsNullOrEmpty(options.Id))
				hidden.Id = options.Id + "H";
			hidden.Name = options.Name;

			return options.ToString() + hidden.ToString();
		}

		public string TextArea(string name)
		{
			return TextArea(name, Hash.Empty);
		}

		public string TextArea(string name, IDictionary attributes)
		{
			TextArea options = new TextArea(attributes);
			options.Name = name;
			return TextArea(options);
		}

		public string TextArea(TextArea options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (string.IsNullOrEmpty(options.InnerText))
			{
				object value = ObtainFromViewData(options.Name);
				options.InnerText = value != null ? value.ToString() : null;
			}
			return options.ToString();
		}

		public string Submit()
		{
			return Submit("Submit");
		}

		public string Submit(string value)
		{
			SubmitButton button = new SubmitButton();
			button.Value = value;
			return Submit(button);
		}

		public string Submit(string value, IDictionary attributes)
		{
			SubmitButton button = new SubmitButton(attributes);

			if (button.Value == null)
				button.Value = value;

			return Submit(button);
		}

		public string Submit(SubmitButton options)
		{
			return options.ToString();
		}

		public string Select(string name, object dataSource, string textField, string valueField)
		{
			return Select(name, dataSource, textField, valueField, Hash.Empty);
		}

		public string Select(string name, object dataSource, string textField, string valueField, IDictionary attributes)
		{
			string firstOption = ObtainAndRemove(attributes, "firstOption");
			string firstOptionValue = ObtainAndRemove(attributes, "firstOptionValue");

			Select select = new Select(attributes);
			select.Name = name;
			select.TextField = textField;
			select.ValueField = valueField;
			select.FirstOption = firstOption;
			select.FirstOptionValue = firstOptionValue;
			return Select(dataSource, select);
		}

		public string Select(object dataSource, Select options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.SelectedValue == null)
			{
				object value = ObtainFromViewData(options.Name);
				options.SelectedValue = value != null ? value.ToString() : null;
			}

			ProcessDataSource(dataSource, options.TextField, options.ValueField, delegate(int count, object text, object value)
			{
				options.AddOption(value.ToString(), text.ToString());
			});

			return options.ToString();
		}

		public string RadioField(string name)
		{
			return RadioField(name, Hash.Empty);
		}

		public string RadioField(string name, IDictionary attributes)
		{
			RadioField options = new RadioField(attributes);
			options.Name = name;
			return RadioField(options);
		}

		public string RadioField(RadioField options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.Value == null)
				options.Value = ObtainFromViewData(options.Name);

			return options.ToString();
		}

		public CheckBoxList CheckBoxList(string name, object dataSource, string textField, string valueField)
		{
			return CheckBoxList(name, dataSource, textField, valueField, Hash.Empty);
		}

		public CheckBoxList CheckBoxList(string name, object dataSource, string textField, string valueField, IDictionary attributes)
		{
			CheckBoxList list = new CheckBoxList(attributes);
			list.Name = name;
			list.TextField = textField;
			list.ValueField = valueField;
			return CheckBoxList(dataSource, list);
		}

		public CheckBoxList CheckBoxList(object dataSource, CheckBoxList options)
		{
			ProcessDataSource(dataSource, options.TextField, options.ValueField, delegate(int count, object text, object value)
			{
				CheckBoxField field = new CheckBoxField();
				field.Name = options.Name;
				field.Label = text.ToString();
				field.Id = field.Name + "-" + count;
				field.Value = value;

				options.Add(field);                                                               		
			});

			return options;
		}

		public RadioList RadioList(string name, object dataSource, string textField, string valueField)
		{
			return RadioList(name, dataSource, textField, valueField, Hash.Empty);
		}

		public RadioList RadioList(string name, object dataSource, string textField, string valueField, IDictionary attributes)
		{
			RadioList list = new RadioList(attributes);
			list.Name = name;
			list.TextField = textField;
			list.ValueField = valueField;
			return RadioList(dataSource, list);
		}

		public RadioList RadioList(object dataSource, RadioList options)
		{
			ProcessDataSource(dataSource, options.TextField, options.ValueField, delegate(int count, object text, object value) 
			{
				RadioField field = new RadioField();
				field.Name = options.Name;
				field.Label = text.ToString();
				field.Id = field.Name + "-" + count;
				field.Value = value;

				options.Add(field);
			});
			return options;
		}

		protected void ProcessDataSource(object dataSource, string textField, string valueField, Action<int, object, object> forEachItemInDataSource)
		{
			IEnumerable ds = dataSource as IEnumerable;
			PropertyInfo textProperty = null;
			PropertyInfo valueProperty = null;

			if (ds != null && textField != null && valueField != null)
			{
				IEnumerator enumerator = ds.GetEnumerator();

				if (enumerator.MoveNext())
				{
					Type type = enumerator.Current.GetType();
					textProperty = type.GetProperty(textField);
					valueProperty = type.GetProperty(valueField);
				}
			}

			if (textProperty != null && valueProperty != null)
			{
				int count = 0;

				foreach (object item in ds)
				{
					object value = valueProperty.GetValue(item, null);
					object text = textProperty.GetValue(item, null);

					forEachItemInDataSource(count++, text, value);
				}
			}

		}

		protected virtual object ObtainFromViewData(string id)
		{
			if (Binder != null)
			{
				return Binder.ExtractValue(id, ViewContext);
			}
			else
			{
				return null;
			}
		}

		protected string ObtainAndRemove(IDictionary dictionary, string key)
		{
			if(!dictionary.Contains(key))
				return null;

			string item = dictionary[key].ToString();
			dictionary.Remove(key);
			return item;
		}
	}
}