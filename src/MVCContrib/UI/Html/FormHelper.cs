using System.Collections;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.Tags;
using System;
using System.Web;
using MvcContrib.Services;

namespace MvcContrib.UI.Html
{
	public class FormHelper : BaseHelper, IFormHelper
	{
		public const string CACHE_KEY = "__MvcContribFormHelper__";

		public static IFormHelper GetInstance(ViewContext context)
		{
			if(context.HttpContext.Items.Contains(CACHE_KEY))
				return (IFormHelper)context.HttpContext.Items[CACHE_KEY];

			IFormHelper helper;

			if (DependencyResolver.Resolver != null)
				helper = DependencyResolver.Resolver.GetImplementationOf<IFormHelper>();
			else
				helper = new FormHelper();

			helper.ViewContext = context;
			context.HttpContext.Items.Add(CACHE_KEY, helper);
			return helper;
		}

		public virtual string TextField(string name)
		{
			return TextField(name, Hash.Empty);
		}

		public virtual string TextField(string name, IDictionary attributes)
		{
			TextBox options = new TextBox(attributes);
			options.Name = name;
			return TextField(options);
		}

		public virtual string TextField(TextBox options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.Value == null)
				options.Value = ObtainFromViewData(options.Name);

			return options.ToString();
		}

		public virtual string PasswordField(string name)
		{
			return PasswordField(name, Hash.Empty);
		}

		public virtual string PasswordField(string name, IDictionary attributes)
		{
			Password options = new Password(attributes);
			options.Name = name;
			return PasswordField(options);
		}

		public virtual string PasswordField(Password options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.Value == null)
				options.Value = ObtainFromViewData(options.Name);

			return options.ToString();
		}

		public virtual string HiddenField(string name)
		{
			return HiddenField(name, Hash.Empty);
		}

		public virtual string HiddenField(string name, IDictionary attributes)
		{
			HiddenField options = new HiddenField(attributes);
			options.Name = name;
			return HiddenField(options);
		}

		public virtual string HiddenField(HiddenField options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.Value == null)
				options.Value = ObtainFromViewData(options.Name);

			return options.ToString();
		}

		public virtual string CheckBoxField(string name)
		{
			return CheckBoxField(name, Hash.Empty);
		}

		public virtual string CheckBoxField(string name, IDictionary attributes)
		{
			CheckBoxField options = new CheckBoxField(attributes);
			options.Name = name;
			return CheckBoxField(options);
		}

		public virtual string CheckBoxField(CheckBoxField options)
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

		public virtual string TextArea(string name)
		{
			return TextArea(name, Hash.Empty);
		}

		public virtual string TextArea(string name, IDictionary attributes)
		{
			TextArea options = new TextArea(attributes);
			options.Name = name;
			return TextArea(options);
		}

		public virtual string TextArea(TextArea options)
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

		public virtual string Submit()
		{
			return Submit("Submit");
		}

		public virtual string Submit(string value)
		{
			SubmitButton button = new SubmitButton();
			button.Value = value;
			return Submit(button);
		}

		public virtual string Submit(string value, IDictionary attributes)
		{
			SubmitButton button = new SubmitButton(attributes);

			if (button.Value == null)
				button.Value = value;

			return Submit(button);
		}

		public virtual string Submit(SubmitButton options)
		{
			return options.ToString();
		}

		public virtual string ImageButton(string src, string alt)
		{
			InputImage options = new InputImage(src);
			options.Alt = alt;
			return options.ToString();
		}

		public virtual string ImageButton(string src, string alt, IDictionary attributes)
		{
			InputImage options = new InputImage(src, attributes);
			options.Alt = alt;
			return options.ToString();
		}

		public virtual string ImageButton(InputImage options)
		{
			return options.ToString();
		}

		public virtual string Select(string name, object dataSource, string textField, string valueField)
		{
			return Select(name, dataSource, textField, valueField, Hash.Empty);
		}

		public virtual string Select(string name, object dataSource, string textField, string valueField, IDictionary attributes)
		{
			string firstOption = ObtainAndRemove(attributes, "firstOption");
			string firstOptionValue = ObtainAndRemove(attributes, "firstOptionValue");
			string selectedValue = ObtainAndRemove(attributes, "selectedValue");

			Select select = new Select(attributes);
			select.Name = name;
			select.TextField = textField;
			select.ValueField = valueField;
			select.FirstOption = firstOption;
			select.FirstOptionValue = firstOptionValue;
			select.SelectedValue = selectedValue;
			return Select(dataSource, select);
		}

		public virtual string Select(object dataSource, Select options)
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

		public virtual string RadioField(string name, object value)
		{
			return RadioField(name, value, Hash.Empty);
		}

		public virtual string RadioField(string name, object value, IDictionary attributes)
		{
			RadioField options = new RadioField(attributes);
			options.Name = name;
			options.Value = value;
			return RadioField(options);
		}

		public virtual string RadioField(RadioField options)
		{
			if(options.Value != null)
			{
				object dataValue = ObtainFromViewData(options.Name);
				if(dataValue != null && dataValue.ToString().Equals(options.Value))
				{
					options.Checked = true;
				}
			}

			if (string.IsNullOrEmpty(options.Id))
			{
				string id = options.Name;
				if(options.Value != null)
				{
					id += "-" + options.Value.ToString().Replace(" ", string.Empty);
				}
				options.Id = id;
			}

			return options.ToString();
		}

		public virtual CheckBoxList CheckBoxList(string name, object dataSource, string textField, string valueField)
		{
			return CheckBoxList(name, dataSource, textField, valueField, Hash.Empty);
		}

		public virtual CheckBoxList CheckBoxList(string name, object dataSource, string textField, string valueField, IDictionary attributes)
		{
			CheckBoxList list = new CheckBoxList(attributes);
			list.Name = name;
			list.TextField = textField;
			list.ValueField = valueField;
			return CheckBoxList(dataSource, list);
		}

		public virtual CheckBoxList CheckBoxList(object dataSource, CheckBoxList options)
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

		public virtual RadioList RadioList(string name, object dataSource, string textField, string valueField)
		{
			return RadioList(name, dataSource, textField, valueField, Hash.Empty);
		}

		public virtual RadioList RadioList(string name, object dataSource, string textField, string valueField, IDictionary attributes)
		{
			RadioList list = new RadioList(attributes);
			list.Name = name;
			list.TextField = textField;
			list.ValueField = valueField;
			return RadioList(dataSource, list);
		}

		public virtual RadioList RadioList(object dataSource, RadioList options)
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

		public void For<T>(string viewDataKey, string url, IDictionary attributes, Action<SmartForm<T>> block)
		{
			object raw = ObtainFromViewData(viewDataKey);

			T item;

			if(raw == null)
			{
				item = default(T);
			}
			else
			{
				//TODO: Introduce type checks.
				item = (T)raw;
			}

			SmartForm<T> form = new SmartForm<T>(viewDataKey, url, block, this, item, attributes);
			ViewContext.HttpContext.Response.Output.Write(form.ToString());
		}

		public void For<T>(T dataItem, string url, Action<SmartForm<T>> block)
		{
			For(dataItem, url, Hash.Empty, block);
		}

		public void For<T>(T dataItem, string url, IDictionary attributes, Action<SmartForm<T>> block)
		{
			SmartForm<T> form = new SmartForm<T>(url, block, this, dataItem, attributes);
			ViewContext.HttpContext.Response.Output.Write(form.ToString());
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