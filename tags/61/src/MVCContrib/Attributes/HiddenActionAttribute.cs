using System;

namespace MvcContrib.Attributes
{
	/// <summary>
	/// Attribute that defines an action as hidden, meaning that it cannot be invoked by an HTTP request.
	/// </summary>
	/// <example>
	/// <![CDATA[
	///		public class MyController : ConventionController
	///		{
	///			[HiddenAction]
	///			public class AnActionThatCannotBeCalled() { }
	///		}
	/// ]]>
	/// </example>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class HiddenActionAttribute : Attribute
	{
	}
}
