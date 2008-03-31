using System;
using System.Collections;
using MvcContrib.UI.Tags;

namespace MvcContrib.UI.Html
{
	public class SmartForm<T> : Form
	{
		private readonly T _item;
		private readonly IFormHelper _helper;
		private readonly Action<SmartForm<T>> _block;
		private readonly string _viewDataKey;

		public SmartForm(string viewDataKey, string url, Action<SmartForm<T>> block, IFormHelper helper, T item, IDictionary attributes)
			: this(url, block, helper, item, attributes)
		{
			_viewDataKey = viewDataKey;
		}

		public SmartForm(string url, Action<SmartForm<T>> block, IFormHelper helper, T item, IDictionary attributes) : base(url, GetFormMethod(attributes), attributes)
		{
			_block = block;
			_helper = helper;
			_item = item;
		}

		private static FORM_METHOD GetFormMethod(IDictionary attributes)
		{
			object method = attributes["method"];

			if(method != null)
			{
				string methodStr = method.ToString();
				if("POST".Equals(methodStr, StringComparison.OrdinalIgnoreCase))
				{
					return FORM_METHOD.POST;
				}
				else if("GET".Equals(methodStr, StringComparison.OrdinalIgnoreCase)) 
				{
					return FORM_METHOD.GET;
				}
			}

			return FORM_METHOD.POST;
		}

		public T Item
		{
			get { return _item; }
		}

		public IFormHelper FormHelper
		{
			get { return _helper; }
		}

		public override string ToString()
		{
			BlockRenderer renderer = new BlockRenderer(_helper.ViewContext.HttpContext);
			InnerText = renderer.Capture(() => _block(this) );
			return base.ToString();
		}

		private string FixupName(string name)
		{
			if (_viewDataKey != null)
			{
				return _viewDataKey + "." +  name;
			}

			return name;
		}

		public string TextField(string name)
		{
			name = FixupName(name);

			using(_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.TextField(name);						
			}
		}

		public string TextField(TextBox attributes)
		{
			using(_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.TextField(attributes);
			}
		}

		public string TextField(string name, IDictionary attributes)
		{
			name = FixupName(name);

			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.TextField(name, attributes);
			}
		}

		public string PasswordField(string name)
		{
			name = FixupName(name);

			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.PasswordField(name);
			}
		}

		public string PasswordField(Password attributes)
		{
			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.PasswordField(attributes);
			}
		}

		public string PasswordField(string name, IDictionary attributes)
		{
			name = FixupName(name);

			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.PasswordField(name, attributes);
			}
		}

		public string HiddenField(string name)
		{
			name = FixupName(name);

			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.HiddenField(name);
			}
		}

		public string HiddenField(string name, IDictionary attributes)
		{
			name = FixupName(name);

			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.HiddenField(name, attributes);
			}
		}

		public string HiddenField(HiddenField options)
		{
			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.HiddenField(options);
			}
		}

		public string CheckBoxField(string name)
		{
			name = FixupName(name);

			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.CheckBoxField(name);
			}
		}

		public string CheckBoxField(string name, IDictionary attributes)
		{
			name = FixupName(name);

			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.CheckBoxField(name, attributes);
			}
		}

		public string CheckBoxField(CheckBoxField options)
		{
			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.CheckBoxField(options);
			}
		}

		public string TextArea(string name)
		{
			name = FixupName(name);

			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.TextArea(name);
			}
		}

		public string TextArea(string name, IDictionary attributes)
		{
			name = FixupName(name);

			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.TextArea(name, attributes);
			}
		}

		public string TextArea(TextArea options)
		{
			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.TextArea(options);
			}
		}

		public string Submit()
		{
			return _helper.Submit();
		}

		public string Submit(string value)
		{
			return _helper.Submit(value);
		}

		public string Submit(string value, IDictionary attributes)
		{
			return _helper.Submit(value, attributes);
		}

		public string Submit(SubmitButton options)
		{
			return _helper.Submit(options);
		}

		public string Select(string name, object dataSource, string textField, string valueField)
		{
			name = FixupName(name);

			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.Select(name, dataSource, textField, valueField);
			}
		}

		public string Select(string name, object dataSource, string textField, string valueField, IDictionary attributes)
		{
			name = FixupName(name);

			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.Select(name, dataSource, textField, valueField, attributes);
			}
		}

		public string Select(object dataSource, Select options)
		{
			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.Select(dataSource, options);
			}
		}

		public string RadioField(string name, object value)
		{
			name = FixupName(name);

			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.RadioField(name, value);
			}
		}

		public string RadioField(string name, object value, IDictionary attributes)
		{
			name = FixupName(name);

			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.RadioField(name, value, attributes);
			}
		}

		public string RadioField(RadioField options)
		{
			using (_helper.Binder.NestedBindingScope(Item))
			{
				return _helper.RadioField(options);
			}
		}
	}
}