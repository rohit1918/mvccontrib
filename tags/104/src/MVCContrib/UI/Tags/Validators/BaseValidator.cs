using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MvcContrib.UI.Tags.Validators
{
	public abstract class BaseValidator : Element, IValidator
	{
		private Hash<string> _expandoAttributes = new Hash<string>();

		public BaseValidator(string id, string referenceId, string text) : base("span")
		{
			this.Id = id;
			this.ReferenceId = referenceId;
			this.InnerText = text;
			this.NullSet("style", "display:none;color:red;");
		}

		public BaseValidator(string id, string referenceId, string text, IDictionary attributes) : base("span", attributes)
		{
			this.Id = id;
			this.ReferenceId = referenceId;
			this.InnerText = text;
			this.NullSet("style", "display:none;color:red;");
		}

		public BaseValidator(string id, string referenceId, string text, string validationGroup) : base("span")
		{
			this.Id = id;
			this.ReferenceId = referenceId;
			this.ValidationGroup = validationGroup;
			this.InnerText = text;
			this.NullSet("style", "display:none;color:red;");
		}

		public BaseValidator(string id, string referenceId, string text, string validationGroup, IDictionary attributes) : base("span", attributes)
		{
			this.Id = id;
			this.ReferenceId = referenceId;
			this.ValidationGroup = validationGroup;
			this.InnerText = text;
			this.NullSet("style", "display:none;color:red;");
		}

		public string ReferenceId
		{
			get
			{
				return this.NullExpandoGet("controltovalidate");
			}

			set
			{
				this.NullExpandoSet("controltovalidate", value);
			}
		}

		public string ErrorMessage
		{
			get
			{
				return this.NullExpandoGet("errormessage");
			}

			set
			{
				this.NullExpandoSet("errormessage", value);
			}
		}

		public string ValidationGroup
		{
			get
			{
				return this.NullExpandoGet("validationGroup");
			}

			set
			{
				this.NullExpandoSet("validationGroup", value);
			}
		}

		public abstract string ValidationFunction
		{
			get;
		}

		public virtual void RenderClientHookup(StringBuilder output)
		{
			string name = this.Id.Replace('.', '_');
			string fieldName = this.Id.Replace('.', '-');

			this.NullExpandoSet("evaluationfunction", this.ValidationFunction);
			this.NullExpandoSet("display", "Dynamic");

			// field declaration
			output.Append("var ");
			output.Append(name);
			output.Append(" = document.all ? document.all[\"");
			output.Append(fieldName);
			output.Append("\"] : document.getElementById(\"");
			output.Append(fieldName);
			output.Append("\");");
			output.AppendLine();

			Dictionary<string, string>.Enumerator en = this._expandoAttributes.GetEnumerator();
			while (en.MoveNext())
			{
				output.Append(name);
				output.Append(".");
				output.Append(en.Current.Key);
				output.Append(" = \"");
				output.Append(en.Current.Value);
				output.Append("\";");
				output.AppendLine();
			}
		}

		protected string NullExpandoGet(string key)
		{
			if (this._expandoAttributes.ContainsKey(key))
			{
				return this._expandoAttributes[key];
			}

			return null;
		}

		protected void NullExpandoSet(string key, string value)
		{
			this._expandoAttributes[key] = value;
		}
	}
}
