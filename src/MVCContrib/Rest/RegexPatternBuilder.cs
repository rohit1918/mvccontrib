using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcContrib.Rest
{
	public class RegexPatternBuilder
	{
		public static readonly string OptionalGuidPattern =
			@"(?:\{?[a-fA-F0-9]{8}(?:-(?:[a-fA-F0-9]){4}){3}-[a-fA-F0-9]{12}\}?)?";

		public static readonly string OptionalPositiveInt32Pattern = @"[0123456789]{0,10}";
		public static readonly string OptionalPositiveInt64Pattern = @"[0123456789]{0,19}";

		public static readonly string RequiredGuidPattern =
			@"(?:\{?[a-fA-F0-9]{8}(?:-(?:[a-fA-F0-9]){4}){3}-[a-fA-F0-9]{12}\}?){1}";

		public static readonly string RequiredPositiveInt32Pattern = @"[0123456789]{1,10}";
		public static readonly string RequiredPositiveInt64Pattern = @"[0123456789]{1,19}";

		public virtual string ToCaseInsensativeRegexClass(string value)
		{
			var buffer = new StringBuilder(value.Length * 2 + 2);
			ToCaseInsensativeRegexClass(buffer, value);
			return buffer.ToString();
		}

		public virtual void ToCaseInsensativeRegexClass(StringBuilder buffer, string value)
		{
			foreach(char character in value)
			{
				if(char.IsLetter(character))
					buffer.Append("[").Append(char.ToLower(character)).Append(char.ToUpper(character)).Append("]");
				else
					buffer.Append(character);
			}
		}

		public virtual string BuildPattern(IEnumerable<string> acceptedValues)
		{
			var buffer = new StringBuilder();
			if(acceptedValues == null || acceptedValues.Count() == 0)
			{
				buffer.Append(@"[a-zA-Z0-9_\-]+?");
			}
			else
			{
				foreach(string value in acceptedValues)
				{
					ToCaseInsensativeRegexClass(buffer, value);
					buffer.Append("|");
				}
				buffer.Remove(buffer.Length - 1, 1);
			}
			return buffer.ToString();
		}

		public virtual string BuildRequiredNamedCapturingPattern(string name, string patternToCapture)
		{
			return "(?<" + name + ">" + patternToCapture + "){1}";
		}

		public virtual string BuildOptionalNamedCapturingPattern(string name, string patternToCapture)
		{
			return "(?<" + name + ">" + patternToCapture + "){1}";
		}
	}
}