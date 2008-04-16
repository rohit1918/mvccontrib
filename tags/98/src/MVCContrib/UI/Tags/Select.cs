using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MvcContrib.UI.Tags
{
	public class Select : ScriptableElement
	{
		private const string ON_FOCUS = "onfocus";
		private const string ON_BLUR = "onblur";
		private const string ON_CHANGE = "onchange";
		private const string MULTIPLE = "multiple";
		private const string DISABLED = "disabled";
		private const string NAME = "name";
		private const string SIZE = "size";

		private readonly List<Option> _options = new List<Option>();
		private string _textField;
		private string _valueField;
		private string _selectedValue;
		private string _firstOption;
		private string _firstOptionValue;

		public Select(IDictionary attributes) : base("select", attributes)
		{
		}

		public Select() : this(Hash.Empty)
		{
		}

		public string Name
		{
			get { return NullGet(NAME); }
			set { NullSet(NAME,value); }
		}

		public override bool UseFullCloseTag
		{
			get { return true; }
		}

		public string OnFocus
		{
			get { return NullGet(ON_FOCUS); }
			set { NullSet(ON_FOCUS, value); }
		}

		public string OnBlur
		{
			get { return NullGet(ON_BLUR); }
			set { NullSet(ON_BLUR, value); }
		}

		public string OnChange
		{
			get { return NullGet(ON_CHANGE); }
			set { NullSet(ON_CHANGE, value); }
		}

		public bool Disabled
		{
			get { return NullGet(DISABLED) == DISABLED; }
			set
			{
				if (value)
					NullSet(DISABLED, DISABLED);
				else
					NullSet(DISABLED, null);
			}
		}

		public int Size
		{
			get
			{
				if (NullGet(SIZE) != null)
				{
					int val;
					if (int.TryParse(NullGet(SIZE), out val))
					{
						return val;
					}
					else
					{
						NullSet(SIZE, null);
						return 0;
					}
				}
				return 0;
			}
			set
			{
				if (value > 0)
				{
					NullSet(SIZE, value.ToString());
				}
				else
				{
					NullSet(SIZE, null);
				}
			}
		}

		public bool Multiple
		{
			get { return NullGet(MULTIPLE) == MULTIPLE; }
			set
			{
				if (value)
					NullSet(MULTIPLE, MULTIPLE);
				else
					NullSet(MULTIPLE, null);
			}
		}

		public void AddOption(string optionValue, string innerText)
		{
			Option option = new Option(new Hash(value => optionValue));
			option.InnerText = innerText;
			_options.Add(option);
		}

		public IList<Option> Options
		{
			get { return _options; }
		}

		public string TextField
		{
			get { return _textField; }
			set { _textField = value; }
		}

		public string ValueField
		{
			get { return _valueField; }
			set { _valueField = value; }
		}

		public string SelectedValue
		{
			get { return _selectedValue; }
			set { _selectedValue = value; }
		}

		public string FirstOption
		{
			get { return _firstOption; }
			set { _firstOption = value; }
		}

		public string FirstOptionValue
		{
			get { return _firstOptionValue; }
			set { _firstOptionValue = value; }
		}

		public override string ToString()
		{
			InnerText = OptionsToString();
			return base.ToString();
		}

		protected virtual string OptionsToString()
		{
			StringBuilder builder = new StringBuilder();
			
			if(FirstOption != null)
			{
				Option option = new Option();
				option.Value = FirstOptionValue;
				option.InnerText = FirstOption;

				builder.Append(option.ToString());
			}

			foreach (Option option in _options)
			{
				if(option.Value == SelectedValue)
					option.Selected = true;

				builder.Append(option.ToString());
			}

			return builder.ToString();
		}
	}
}