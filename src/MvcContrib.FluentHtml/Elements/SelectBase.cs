using System;
using System.Collections;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Collections.Generic;
using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Html;

namespace MvcContrib.FluentHtml.Elements
{
	public abstract class SelectBase<T> : FormElement<SelectBase<T>> where T : SelectBase<T>
	{
		protected IEnumerable _options;
		protected string _dataValueField;
		protected string _dataTextField;
		protected IEnumerable _selectedValues;
		protected Func<object, object> _textFieldSelector;
		protected Func<object, object> _valueFieldSelector;

		protected SelectBase(string name) : base(HtmlTag.Select, name) { }

		protected SelectBase(string name, MemberExpression forMember, IEnumerable<IMemberBehavior> behaviors)
			: base(HtmlTag.Select, name, forMember, behaviors) { }

		public IEnumerable SelectedValues
		{
			get { return _selectedValues; }
		}

		public virtual T Size(int value)
		{
			Attr(HtmlAttribute.Size, value);
			return (T)this;
		}

		public virtual T Options(MultiSelectList value)
		{
			if (value != null)
			{
				_options = value.Items;
				_dataValueField = value.DataValueField;
				_dataTextField = value.DataTextField;
				if (value.SelectedValues != null)
				{
					_selectedValues = value.SelectedValues;
				}
			}
			return (T)this;
		}

		public virtual T Options<TKey, TValue>(IDictionary<TKey, TValue> value)
		{
			_options = value;
			_dataValueField = "Key";
			_dataTextField = "Value";
			return (T)this;
		}

		public virtual T Options(IEnumerable value, string valueField, string textField)
		{
			_options = value;
			_dataValueField = valueField;
			_dataTextField = textField;
			return (T)this;
		}

		public virtual T Options<TDataSource>(IEnumerable<TDataSource> values, Func<TDataSource, object> valueFieldSelector, Func<TDataSource, object> textFieldSelector)
		{
			if(valueFieldSelector == null) throw new ArgumentNullException("valueFieldSelector");
			if(textFieldSelector == null) throw new ArgumentNullException("textFieldSelector");

			_options = values;
			//TODO: Is there a better way to do this without making Select have a type parameter of TDataSource?
			_textFieldSelector = x => textFieldSelector((TDataSource)x);
			_valueFieldSelector = x => valueFieldSelector((TDataSource)x);

			return (T)this;
		}

		public virtual T Options<TEnum>() where TEnum : struct
		{
			return Options(EnumToDictionary<TEnum>(null));
		}

		public virtual T Options<TEnum>(string firstOptionText) where TEnum : struct
		{
			return Options(EnumToDictionary<TEnum>(firstOptionText));
		}

		public override string ToString()
		{
			builder.InnerHtml = RenderOptions();
			return base.ToString();
		}

		public override TagRenderMode TagRenderMode
		{
			get { return TagRenderMode.Normal; }
		}

		private string RenderOptions()
		{
			if (_options == null)
			{
				return null;
			}

			if(_textFieldSelector == null || _valueFieldSelector == null)
			{
				var enumerator = _options.GetEnumerator();
				if(!enumerator.MoveNext())
				{
					return null;
				}
				var type = enumerator.Current.GetType();
				var valueProp = type.GetProperty(_dataValueField);
				if(valueProp == null)
				{
					throw new ArgumentException(string.Format("The option list does not contain the specified value property: {0}", _dataValueField), "dataValueField");
				}
				var textProp = type.GetProperty(_dataTextField);
				if(textProp == null)
				{
					throw new ArgumentException(string.Format("The option list does not contain the specified text property: {0}", _dataTextField), "dataTextField");
				}

				_textFieldSelector = x => textProp.GetValue(x, null);
				_valueFieldSelector = x => valueProp.GetValue(x, null);
			}

			var sb = new StringBuilder();

			foreach(var option in BuildOptions())
			{
				sb.Append(option);
			}

			return sb.ToString();
		}

		protected virtual IEnumerable<Option> BuildOptions()
		{
			foreach (var item in _options) 
			{
				var value = _valueFieldSelector(item);
				var text = _textFieldSelector(item);

				var option = new Option()
					.Value(value == null ? string.Empty : value.ToString())
					.Text(text == null ? string.Empty : text.ToString())
					.Selected(IsSelectedValue(value));

				yield return option;
			}
		}

		private bool IsSelectedValue(object value)
		{
			var valueString = value == null ? string.Empty : value.ToString();
			if (_selectedValues != null)
			{
				var enumerator = _selectedValues.GetEnumerator();
				while (enumerator.MoveNext())
				{
					var selectedValueString = enumerator.Current == null 
						? string.Empty
						: enumerator.Current.GetType().IsEnum
							? ((int)enumerator.Current).ToString()
							: enumerator.Current.ToString();
					if (valueString == selectedValueString)
					{
						return true;
					}
				}
			}
			return false;
		}

		protected Dictionary<string, string> EnumToDictionary<TEnum>(string firstOptionText)
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new ArgumentException("The generic parameter must be an enum", "TEnum");
			}
			var dict = new Dictionary<string, string>();
			if (firstOptionText != null)
			{
				dict.Add(string.Empty, firstOptionText);
			}
			var values = Enum.GetValues(typeof(TEnum));
			foreach (var item in values)
			{
				dict.Add(Convert.ToInt32(item).ToString(), item.ToString());
			}
			return dict;
		}
	}
}
