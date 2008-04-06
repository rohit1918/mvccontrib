using System;

namespace MvcContrib.Rest.Routing.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class RestfulControllerAttribute : RestfulNodeAttribute
	{
	}
}