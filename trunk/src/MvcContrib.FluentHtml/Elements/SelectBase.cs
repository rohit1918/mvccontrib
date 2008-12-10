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
		protected IEnumerable options;
		protected string dataValueField;
		protected string dataTextField;
		protected IEnumerable _selectedValues;

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
				options = value.Items;
				dataValueField = value.DataValueField;
				dataTextField = value.DataTextField;
				if (value.SelectedValues != null)
				{
					_selectedValues = value.SelectedValues;
				}
			}
			return (T)this;
		}

		public virtual T Options<TKey, TValue>(IDictionary<TKey, TValue> value)
		{
			options = value;
			dataValueField = "Key";
			dataTextField = "Value";
			return (T)this;
		}

		public virtual T Options(IEnumerable value, string valueField, string textField)
		{
			options = value;
			dataValueField = valueField;
			dataTextField = textField;
			return (T)this;
		}

		public virtual T Options<TEnum>() where TEnum : struct
		{
			return Options(EnumToDictionary<TEnum>());
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
			if (options == null)
			{
				return null;
			}
			var enumerator = options.GetEnumerator();
			if (!enumerator.MoveNext())
			{
				return null;
			}
			var type = enumerator.Current.GetType();
			var valueProp = type.GetProperty(dataValueField);
			if (valueProp == null)
			{
				throw new ArgumentException(string.Format("The option list does not contain the specified value property: {0}", dataValueField), "dataValueField");
			}
			var textProp = type.GetProperty(dataTextField);
			if (textProp == null)
			{
				throw new ArgumentException(string.Format("The option list does not contain the specified text property: {0}", dataTextField), "dataTextField");
			}
			var sb = new StringBuilder();
			foreach (var item in options)
			{
				var value = valueProp.GetValue(item, null);
				var text = textProp.GetValue(item, null);
				var option = new Option()
					.Value(value == null ? string.Empty : value.ToString())
					.Text(text == null ? string.Empty : text.ToString())
					.Selected(IsSelectedValue(value));
				sb.Append(option.ToString());
			}
			return sb.ToString();
		}

		private bool IsSelectedValue(object value)
		{
			if (_selectedValues != null)
			{
				var enumerator = _selectedValues.GetEnumerator();
				while (enumerator.MoveNext())
				{
					var selectedValue = enumerator.Current != null && enumerator.Current.GetType().IsEnum
						? (int)enumerator.Current
						: enumerator.Current;
					if (value == null && selectedValue == null ||
						selectedValue != null && selectedValue.Equals(value))
					{
						return true;
					}
				}
			}
			return false;
		}

		protected Dictionary<int, string> EnumToDictionary<TEnum>()
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new ArgumentException("The generic parameter must be an enum", "TEnum");
			}
			var dict = new Dictionary<int, string>();
			var values = Enum.GetValues(typeof(TEnum));
			foreach (var item in values)
			{
				dict.Add(Convert.ToInt32(item), item.ToString());
			}
			return dict;
		}
	}
}
