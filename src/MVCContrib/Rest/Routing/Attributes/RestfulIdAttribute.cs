using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using MvcContrib.Rest.Routing.Descriptors.Fragments;

namespace MvcContrib.Rest.Routing.Attributes
{
	/// <summary>Attribute configuration for the <see cref="EntityIdFragment"/></summary>
	/// <remarks>By default the <see cref="Name"/> is set to <c>&quot;id&quot;</c> and
	/// <see cref="PatternType"/> is set to <see cref="int"/></remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class RestfulIdAttribute : Attribute, IRestfulParameterAttribute
	{
		/// <summary>By default the <see cref="Name"/> is set to <c>&quot;id&quot;</c> and
		/// <see cref="PatternType"/> is set to <see cref="int"/></summary>
		public RestfulIdAttribute()
		{
			Name = "id";
			PatternType = PatternType.Int32;
		}

		public string Pattern { get; set; }

		#region IRestfulParameterAttribute Members

		/// <summary><para>The name of the parameter that will appear in the parameters of 
		/// <see cref="RouteData.Values"/>, and be used
		/// for generating a url from the <see cref="IDictionary"/> of parameters
		/// passed to the <see cref="RouteBase.GetVirtualPath"/> method.</para>
		/// <para>This name can be rewritten at rule generation time if the <see cref="EntityIdFragment"/>
		/// is acting as a <see cref="RestfulParentRouteRuleFragmentDescriptor">parent </see>
		/// for the current rule.</para></summary>
		/// <seealso cref="RestfulParentRouteRuleFragmentDescriptor" />
		public string Name { get; set; }

		public PatternType PatternType { get; set; }

		public string CustomPattern { get; set; }

		public string[] AcceptedValues { get; set; }

		public string DefaultValue { get; set; }

		public IEnumerable<KeyValuePair<string, string>> AcceptedValuesAndAliases
		{
			get { return Enumerable.Empty<KeyValuePair<string, string>>(); }
		}

		/// <summary>This property will always return <see langword="false" />.</summary>
		/// <remarks>
		///   <note type="caution">Will throw <see cref="NotImplementedException"/> if you try and set a value for the property.</note>
		///   <note type="implementnotes">Explicit interface implementation so it doesn't show up in intellisense.</note>
		/// </remarks>
		/// <exception cref="NotImplementedException">Thrown if you try to set the value of this property.</exception>
		bool IRestfulParameterAttribute.IsOptional
		{
			get { return false; }
			set { throw new NotImplementedException("IsOptional cannot be set because the id is always required for entity rules."); }
		}

		/// <summary>This property is not required and will always return <c>-1</c>.</summary>
		/// <remarks>
		///   <note type="implementnotes">Explicit interface implementation so it doesn't show up in intellisense.</note>
		/// </remarks>
		int IRestfulNodePositionAttribute.NodePosition
		{
			get { return -1; }
			set { }
		}

		#endregion
	}
}