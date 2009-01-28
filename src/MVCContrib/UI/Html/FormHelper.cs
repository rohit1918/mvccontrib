using System.Collections;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.UI.Tags;
using System;
using MvcContrib.Services;
using System.Collections.Generic;

namespace MvcContrib.UI.Html
{
	[Obsolete]
	public class FormHelper : BaseHelper, IFormHelper
	{
		public const string CACHE_KEY = "__MvcContribFormHelper__";
		
		public static IFormHelper GetInstance(ViewContext context)
		{
			if (context.HttpContext.Items.Contains(CACHE_KEY))
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
			var options = new TextBox(attributes) {Name = name};
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
			var options = new Password(attributes) {Name = name};
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
			var options = new HiddenField(attributes) {Name = name};
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
			var options = new CheckBoxField(attributes) {Name = name};
			return CheckBoxField(options);
		}

		public virtual string CheckBoxField(CheckBoxField options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.Value == null)
				options.Value = "true";

			if (options.Checked == null)
			{
				object value = ObtainFromViewData(options.Name);
				bool isChecked = value != null && value is bool && (bool)value;

				options.Checked = isChecked;
			}

			var hidden = new HiddenField {Value = "false"};
			if (!string.IsNullOrEmpty(options.Id))
				hidden.Id = options.Id + "H";
			hidden.Name = options.Name;

			return string.Concat(options, hidden);
		}

		public virtual string TextArea(string name)
		{
			return TextArea(name, Hash.Empty);
		}

		public virtual string TextArea(string name, IDictionary attributes)
		{
			var options = new TextArea(attributes) {Name = name};
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
			var button = new SubmitButton {Value = value};
			return Submit(button);
		}

		public virtual string Submit(string value, IDictionary attributes)
		{
			var button = new SubmitButton(attributes);

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
			var options = new InputImage(src) {Alt = alt};
			return options.ToString();
		}

		public virtual string ImageButton(string src, string alt, IDictionary attributes)
		{
			var options = new InputImage(src, attributes) {Alt = alt};
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
			Select select = GetSelect(attributes, textField, valueField);
			select.Name = name;
			return Select(dataSource, select);
		}

		public virtual string Select<T>(string name) where T : struct
		{
			return Select<T>(name, null);
		}

		public virtual string Select<T>(string name, IDictionary attributes) where T : struct
		{
			var dataSource = new Dictionary<int, string>();
			if (typeof(T).IsEnum)
			{
				foreach (var item in Enum.GetValues(typeof(T)))
				{
					dataSource.Add(Convert.ToInt32(item), item.ToString());
				}
			}
			var select = GetSelect(attributes, null, null);
			select.Name = name;
			return Select(dataSource, select);
		}

		public virtual string Select(object dataSource, Select options)
		{
			if (string.IsNullOrEmpty(options.Id))
				options.Id = options.Name;

			if (options.SelectedValues.Count == 0)
			{
				object value = ObtainFromViewData(options.Name);
				options.SetSelectedValues(value);
			}

			ProcessDataSource(dataSource, options.TextField, options.ValueField,
				(count, text, value) => options.AddOption(
					value == null ? string.Empty : value.ToString(), 
					text == null ? string.Empty : text.ToString()));

			return options.ToString();
		}

		protected virtual Select GetSelect(IDictionary attributes, string textField, string valueField)
		{
			if (attributes == null) return new Select();

			var firstOption = ObtainAndRemove(attributes, "firstOption");
			var firstOptionValue = ObtainAndRemove(attributes, "firstOptionValue");

			var hasSelectedValue = attributes.Contains("selectedValue");
			var selectedValue = ObtainAndRemove(attributes, "selectedValue");

			var select = new Select(attributes)
				{
					FirstOption = firstOption != null ? firstOption.ToString() : null,
					FirstOptionValue = firstOptionValue != null ? firstOptionValue.ToString() : null,
					TextField= textField,
					ValueField = valueField
				};

			if (hasSelectedValue)
			{
				select.SetSelectedValues(selectedValue);
			}
			return select;
		}

		public virtual string RadioField(string name, object value)
		{
			return RadioField(name, value, Hash.Empty);
		}

		public virtual string RadioField(string name, object value, IDictionary attributes)
		{
			var options = new RadioField(attributes) {Name = name, Value = value};
			return RadioField(options);
		}

		public virtual string RadioField(RadioField options)
		{
			if (options.Value != null)
			{
				object dataValue = ObtainFromViewData(options.Name);
				if (dataValue != null && dataValue.ToString().Equals(options.Value))
				{
					options.Checked = true;
				}
			}

			if (string.IsNullOrEmpty(options.Id))
			{
				string id = options.Name;
				if (options.Value != null)
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
			var list = new CheckBoxList(attributes) {Name = name, TextField = textField, ValueField = valueField};
			return CheckBoxList(dataSource, list);
		}

		public virtual CheckBoxList CheckBoxList(object dataSource, CheckBoxList options)
		{
			ProcessDataSource(dataSource, options.TextField, options.ValueField, delegate(int count, object text, object value)
			{
				var field = new CheckBoxField {Name = options.Name, Label = text.ToString()};
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
			var list = new RadioList(attributes) {Name = name, TextField = textField, ValueField = valueField};
			return RadioList(dataSource, list);
		}

		public virtual RadioList RadioList(object dataSource, RadioList options)
		{
			ProcessDataSource(dataSource, options.TextField, options.ValueField, delegate(int count, object text, object value)
			{
				var field = new RadioField {Name = options.Name, Label = text.ToString()};
				field.Id = field.Name + "-" + count;
				field.Value = value;

				options.Add(field);
			});
			return options;
		}

		protected void ProcessDataSource(object dataSource, string textField, string valueField, Action<int, object, object> forEachItemInDataSource)
		{
			if (dataSource != null && typeof(IDictionary).IsAssignableFrom(dataSource.GetType()))
			{
				var ds = (IDictionary)dataSource;
				var count = 0;
				foreach (DictionaryEntry entry in ds)
				{
					forEachItemInDataSource(count++, entry.Value, entry.Key);
				}
			}
			else
			{
				var ds = dataSource as IEnumerable;

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

					foreach (var item in ds)
					{
						object value = valueProperty.GetValue(item, null);
						object text = textProperty.GetValue(item, null);

						forEachItemInDataSource(count++, text, value);
					}
				}
			}
		}

		public void For<T>(string viewDataKey, string url, IDictionary attributes, Action<SmartForm<T>> block)
		{
			object raw = ObtainFromViewData(viewDataKey);

			T item;

			if (raw == null)
			{
				item = default(T);
			}
			else
			{
				//TODO: Introduce type checks.
				item = (T)raw;
			}

			var form = new SmartForm<T>(viewDataKey, url, block, this, item, attributes);
			ViewContext.HttpContext.Response.Output.Write(form.ToString());
		}

		public void For<T>(T dataItem, string url, Action<SmartForm<T>> block)
		{
			For(dataItem, url, Hash.Empty, block);
		}

		public void For<T>(T dataItem, string url, IDictionary attributes, Action<SmartForm<T>> block)
		{
			var form = new SmartForm<T>(url, block, this, dataItem, attributes);
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

		protected object ObtainAndRemove(IDictionary dictionary, string key)
		{
			if (!dictionary.Contains(key))
				return null;

			var item = dictionary[key];
			dictionary.Remove(key);
			return item.GetType().IsEnum
					? Convert.ToInt32(item).ToString()
					: item;
		}

	}
}
