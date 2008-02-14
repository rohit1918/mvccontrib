using System;

namespace MvcContrib.Attributes
{
	/// <summary>
	/// The action marked with this attribute will be called if the specified action cannot be found.
	/// </summary>
	/// <example>
	/// <![CDATA[
	///		public class MyController : ConventionController
	///		{
	///			[DefaultAction]
	///			public void CatchAllAction() { }
	/// 	}
	/// ]]>
	/// </example>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class DefaultActionAttribute : Attribute
	{
		
	}
}
