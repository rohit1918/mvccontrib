using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcContrib.Rest.Routing.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class OptionalGuidParameterAttribute : RestfulNodeAttribute, IRestfulParameterAttribute
	{
		#region IRestfulParameterAttribute Members

		public string Name { get; set; }

		public string DefaultValue { get; set; }

		IEnumerable<KeyValuePair<string, string>> IRestfulParameterAttribute.AcceptedValuesAndAliases
		{
			get { return Enumerable.Empty<KeyValuePair<string, string>>(); }
		}

		PatternType IRestfulParameterAttribute.PatternType
		{
			get { return PatternType.Guid; }
			set { throw new NotImplementedException("Cannot set the value of PatternType because it is fixed to PatternType.Guid"); }
		}

		bool IRestfulParameterAttribute.IsOptional
		{
			get { return true; }
			set { throw new NotImplementedException("Cannot set IsOptional because it is fixed to true"); }
		}

		string IRestfulParameterAttribute.CustomPattern
		{
			get { return null; }
			set
			{
				throw new NotImplementedException(
					"Cannot set the CustomPattern because it is fixed to null, and the PatternType is PatternType.Guid");
			}
		}

		string[] IRestfulParameterAttribute.AcceptedValues
		{
			get { return null; }
			set
			{
				throw new NotImplementedException(
					"Cannot set the AcceptedValues because it is fixed to null, and the PatternType is PatternType.Guid");
			}
		}

		#endregion
	}
}