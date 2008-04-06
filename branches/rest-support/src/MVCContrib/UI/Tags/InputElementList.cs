using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MvcContrib.UI.Tags
{
	public class InputElementList<T> : ScriptableElement, IEnumerable<T> where T : Input
	{
		private readonly List<T> _elements = new List<T>();
		private string _textField;
		private string _valueField;

		//Have to specify an element name, even though it won't ever render...
		public InputElementList(IDictionary attributes) : base("div", attributes)
		{
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

		public string Name
		{
			get { return NullGet("name"); }
			set { NullSet("name", value); }
		}

		public void Add(T element)
		{
			foreach(KeyValuePair<string, string> attribute in _attributes)
			{
				if(!element.Attributes.ContainsKey(attribute.Key))
				{
					element.Attributes.Add(attribute.Key, attribute.Value);
				}
			}

			_elements.Add(element);	
		}

		public new IEnumerator<T> GetEnumerator()
		{
			return _elements.GetEnumerator();
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			foreach(T element in _elements)
			{
				builder.Append(element.ToString());
			}

			return builder.ToString();
		}

		public string ToFormattedString(string format)
		{
			StringBuilder builder = new StringBuilder();

			foreach (T element in _elements)
			{
				builder.AppendFormat(format, element);
			}

			return builder.ToString();
		}
	}
}