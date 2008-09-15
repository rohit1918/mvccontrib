using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MvcContrib.MetaData
{
	/// <summary>
	/// Metadata about a controller action
	/// </summary>
	public class ActionMetaData
	{
		/// <summary>
		/// Creates a new instance of the ActionMetaData class
		/// </summary>
		/// <param name="methodInfo">The MethodInfo that represents the action method</param>
		/// <param name="selectionAttributes">Any ActionSelection attributes defined on the action</param>
		/// <param name="filters">Any filters defined on the action</param>
		public ActionMetaData(MethodInfo methodInfo, ActionSelectionAttribute[] selectionAttributes, FilterInfo filters)
		{
			MethodInfo = methodInfo;
			SelectionAttributes = selectionAttributes;
			Filters = filters;
		}

		/// <summary>
		/// The name of the action
		/// </summary>
		public string Name
		{
			get { return MethodInfo.Name; }
		}

		/// <summary>
		/// The MethodInfo that represents the action method.
		/// </summary>
		public MethodInfo MethodInfo { get; private set; }

		/// <summary>
		/// Any ActionSelection attributes that were defined on the action
		/// </summary>
		public ActionSelectionAttribute[] SelectionAttributes { get; private set; }

		/// <summary>
		/// The filters defined for this action.
		/// </summary>
		public FilterInfo Filters { get; private set; }

		/// <summary>
		/// Checks to see whether the action is valid for the current request.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public virtual bool IsValidForRequest(string name, ControllerContext context)
		{
			return name.Equals(MethodInfo.Name, StringComparison.OrdinalIgnoreCase)
				&& SelectionAttributes.All(attr => attr.IsValidForRequest(context, MethodInfo));
		}
	}

	/// <summary>
	/// Metadata about a controller action with an alias
	/// </summary>
	public class AliasedActionMetaData : ActionMetaData
	{
		/// <summary>
		/// Creates a new instance of the AliasedActionMetaData class
		/// </summary>
		/// <param name="methodInfo">The Methodinfo that represents the action method</param>
		/// <param name="filters">Any filters defined on the action</param>
		/// <param name="selectionAttributes">Any ActionSelection attributes defined on the action</param>
		/// <param name="actionNameAttributes">The ActionNameAttributes that supply aliasing info for the action</param>
		public AliasedActionMetaData(MethodInfo methodInfo, FilterInfo filters, ActionSelectionAttribute[] selectionAttributes, ActionNameAttribute[] actionNameAttributes)
			: base(methodInfo, selectionAttributes, filters)
		{
			this.Aliases = actionNameAttributes;
		}

		/// <summary>
		/// The ActionNameAttribute that supplies aliasing info for the action
		/// </summary>
		public ActionNameAttribute[] Aliases { get; private set; }

		public override bool IsValidForRequest(string name, ControllerContext context) 
		{
			return Aliases.All(attr => attr.IsValidForRequest(context, name, MethodInfo)) 
				&& SelectionAttributes.All(attr => attr.IsValidForRequest(context, MethodInfo));
		}
	}
}
