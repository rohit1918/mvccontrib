using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcContrib.Rest.Routing.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class OptionalParameterAttribute : RestfulNodeAttribute, IRestfulParameterAttribute
	{
		#region IRestfulParameterAttribute Members

		/// <summary>The name of the parameter.</summary>
		/// <remarks>This property is required.  A runtime exception will be thrown if the value is <c>null</c> or <see cref="string.Empty"/></remarks>
		public string Name { get; set; }

		public string DefaultValue { get; set; }

		public string[] AcceptedValues { get; set; }

		public PatternType PatternType { get; set; }

		public string CustomPattern { get; set; }

		bool IRestfulParameterAttribute.IsOptional
		{
			get { return true; }
			set { }
		}

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

		#endregion
	}
}