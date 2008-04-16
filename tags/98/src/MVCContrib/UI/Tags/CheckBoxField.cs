using System.Collections;

namespace MvcContrib.UI.Tags
{
	public class CheckBoxField : Input
	{
		private const string CHECKED = "checked";

		public CheckBoxField(IDictionary attributes) : base("checkbox", attributes)
		{
		}

		public CheckBoxField() : this(Hash.Empty)
		{
		}
		bool? _checkSet;
		public bool? Checked
		{
			get {  
				if (_checkSet!= null)
					return (bool?)(NullGet(CHECKED) == CHECKED);
				else 
					return null;}
			set
			{
				_checkSet = value;
				if (value == true)
					NullSet(CHECKED, CHECKED);
				else
					NullSet(CHECKED, null);
			}
		}
	}
}