using System;

namespace MvcContrib.Rest.Routing.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ExtraListingActionAttribute : RestfulNodeAttribute
	{
		public string Alias { get; set; }
	}
}