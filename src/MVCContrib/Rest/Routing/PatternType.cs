using System.Text.RegularExpressions;

namespace MvcContrib.Rest.Routing
{
	/// <summary>A pre-defined pattern to use for matching parameter values.</summary>
	/// <remarks>Defaults to <see cref="PatternType.Custom"/></remarks>
	/// <seealso cref="RegexPatternBuilder.RequiredPositiveInt32Pattern"/>
	/// <seealso cref="RegexPatternBuilder.OptionalPositiveInt32Pattern"/>
	/// <seealso cref="RegexPatternBuilder.RequiredPositiveInt64Pattern"/>
	/// <seealso cref="RegexPatternBuilder.OptionalPositiveInt64Pattern"/>
	/// <seealso cref="RegexPatternBuilder.RequiredGuidPattern"/>
	/// <seealso cref="RegexPatternBuilder.OptionalGuidPattern"/>
	public enum PatternType
	{
		/// <summary>A custom <see cref="Regex">regex pattern</see></summary>
		Custom = 0,

		/// <summary>A <see cref="Regex">regex pattern</see> for matching <c>0</c> and positive <see cref="int" />&apos;s</summary>
		/// <seealso cref="RegexPatternBuilder.RequiredPositiveInt32Pattern"/>
		/// <seealso cref="RegexPatternBuilder.OptionalPositiveInt32Pattern"/>
		Int32 = 1,

		/// <summary>A <see cref="Regex">regex pattern</see> for matching <c>0</c> and positive <see cref="long" />&apos;s</summary>
		/// <seealso cref="RegexPatternBuilder.RequiredPositiveInt64Pattern"/>
		/// <seealso cref="RegexPatternBuilder.OptionalPositiveInt64Pattern"/>
		Int64 = 2,

		/// <summary>A <see cref="Regex">regex pattern</see> for matching <see cref="Guid"/>&apos;s</summary>
		/// <seealso cref="RegexPatternBuilder.RequiredGuidPattern"/>
		/// <seealso cref="RegexPatternBuilder.OptionalGuidPattern"/>
		Guid = 4
	}
}