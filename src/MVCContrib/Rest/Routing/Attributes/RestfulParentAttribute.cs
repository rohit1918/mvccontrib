using System;
using System.Web.Mvc;

namespace MvcContrib.Rest.Routing.Attributes
{
	public class RestfulParentAttribute : RestfulNodeAttribute
	{
		/// <summary>The Parent <see cref="IController">Controller</see> <see cref="Type"/> of the Route.</summary>
		/// <remarks>this will overwrite <see cref="ParentControllers"/> if it was already set.</remarks>
		public Type ParentController
		{
			get { return ParentControllers[0]; }
			set { ParentControllers = new[] {value}; }
		}

		/// <summary>The Parent Controller Types of the route.</summary>
		/// <remarks>Use multiple if the route can be accesible from multiple parents, use a additional attributes
		/// for additional level of nesting from parents.</remarks>
		/// <value>The <see cref="IController">controller</see> <see cref="Type"/></value>
		public Type[] ParentControllers { get; set; }
	}
}