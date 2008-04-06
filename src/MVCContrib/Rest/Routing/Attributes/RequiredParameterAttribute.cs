using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcContrib.Rest.Routing.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class RequiredParameterAttribute : RestfulNodeAttribute, IRestfulParameterAttribute
	{
		#region IRestfulParameterAttribute Members

		/// <summary>The array of valid values for the required parameter.  Seperate the value and alias with
		/// an equals sign if you want to you use an alias for the value.</summary>
		/// <remarks>This property does not require a value.</remarks>
		/// <example lang="C#">
		/// [RequiredString(NodePosition=3, Name="foo", AcceptedValues=new string[]{"ValueWithAlias=the-alias", "ValueWithNoAlias"}]
		/// </example>
		public string[] AcceptedValues { get; set; }

		IEnumerable<KeyValuePair<string, string>> IRestfulParameterAttribute.AcceptedValuesAndAliases
		{
			get
			{
				if(((IRestfulParameterAttribute)this).AcceptedValues == null)
				{
					return Enumerable.Empty<KeyValuePair<string, string>>();
				}
				return from val in ((IRestfulParameterAttribute)this).AcceptedValues select ToValueAndAlias(val);
			}
		}

		/// <summary>A pre-defined pattern to use to restrict the accepted values.</summary>
		public PatternType PatternType { get; set; }

		/// <summary>A custom <see cref="System.Text.RegularExpressions.Regex">regular expression</see> pattern to use.</summary>
		/// <remarks>Do not include any named groups, the expression will be wrapped with <c lang="C#">&quot;^(?&gt;&quot; + Name + &quot;&lt;&quot; + Pattern + &quot;){1}$&quot;</c> and a named capturing group</remarks>
		public string CustomPattern { get; set; }

		/// <summary>The name of the parameter.</summary>
		/// <remarks>This property is required.</remarks>
		/// <exception cref="ArgumentException">Thrown at runtime if <see cref="Name"/> is <see langword="null" /> or <see cref="string.Empty"/></exception>
		public string Name { get; set; }

		bool IRestfulParameterAttribute.IsOptional
		{
			get { return false; }
			set { throw new NotImplementedException(); }
		}

		string IRestfulParameterAttribute.DefaultValue
		{
			get { return null; }
			set { throw new NotImplementedException(); }
		}

		#endregion
	}
}