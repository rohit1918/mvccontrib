using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using MvcContrib.Rest;
using MvcContrib.Rest.Routing.Descriptors;
using MvcContrib.Rest.Routing.Ext;
using MvcContrib.Rest.Routing.Fragments;

namespace MvcContrib.Rest.Routing.Fragments
{
	public class ControllerFragment : BasePatternFragment
	{
		private readonly List<string> _controllerPathNames;
		private readonly List<ControllerRoutingDescriptor> _controllerRoutingDescriptors;

		public ControllerFragment()
		{
			_controllerRoutingDescriptors = new List<ControllerRoutingDescriptor>(0);
			_controllerPathNames = new List<string>(0);
			Name = "controller";
		}

		public ControllerFragment(ControllerRoutingDescriptor controllerRoutingDescriptor)
			: this(new[] {controllerRoutingDescriptor}) {}

		public ControllerFragment(IEnumerable<ControllerRoutingDescriptor> controllerRoutingDescriptors) : this()
		{
			if (controllerRoutingDescriptors != null)
			{
				_controllerRoutingDescriptors = new List<ControllerRoutingDescriptor>(controllerRoutingDescriptors);
				_controllerPathNames =
					new List<string>(
						from descriptor in controllerRoutingDescriptors
						select RestfulRoutingExtensions.ToSeoFriendlyUrlFragment(descriptor.Name));
			}
			PatternBuilder =
				() =>
				new RegexPatternBuilder().BuildRequiredNamedCapturingPattern(Name,
				                                                             new RegexPatternBuilder().BuildPattern(
				                                                             	_controllerPathNames));
		}

		public ControllerFragment(string name, string defaultValue, IEnumerable<string> acceptedValues) : this()
		{
			Name = name;
			DefaultValue = defaultValue;
			PatternBuilder =
				() =>
				new RegexPatternBuilder().BuildRequiredNamedCapturingPattern(Name,
				                                                             new RegexPatternBuilder().BuildPattern(
				                                                             	acceptedValues));
		}

		public IEnumerable<ControllerRoutingDescriptor> ControllerRoutingDescriptors
		{
			get { return _controllerRoutingDescriptors.AsEnumerable(); }
		}

		public override string PathFragment
		{
			get { return base.PathFragment; }
			set { throw new NotImplementedException("Cannot manually set the Path Fragment"); }
		}

		public ControllerFragment Add(ControllerRoutingDescriptor controllerRoutingDescriptor)
		{
			return Add(controllerRoutingDescriptor.Name.ToSeoFriendlyUrlFragment(), controllerRoutingDescriptor);
		}

		public ControllerFragment Add(string urlPathPart, ControllerRoutingDescriptor controllerRoutingDescriptor)
		{
			_controllerRoutingDescriptors.Add(controllerRoutingDescriptor);
			_controllerPathNames.Add(urlPathPart.ToLower());
			return this;
		}

		public override bool Matches(string urlPart, RouteData routeData)
		{
			if (!base.Matches(urlPart, routeData))
			{
				return false;
			}
			if (_controllerRoutingDescriptors.Count > 0)
			{
				routeData.Values[Name] =
					_controllerRoutingDescriptors[_controllerPathNames.IndexOf(((string)routeData.Values[Name]).ToLower())].Name;
			}
			return true;
		}

		public override bool TryBuildUrl(StringBuilder virtualPath, RequestContext requestContext, RouteValueDictionary routeData, VirtualPathData virtualPathData)
		{
			if (!routeData.ContainsKey(Name))
			{
				return HasDefaultValue;
			}

			if (_controllerRoutingDescriptors.Count == 0)
			{
				virtualPath.Append('/').Append(Convert.ToString(routeData[Name]));
				return true;
			}

			object value = routeData[Name];

			var controllerType = value as Type;
			string controllerName = Convert.ToString(value);

			int index =
				ControllerRoutingDescriptors.IndexOf(
					descriptor => (FindControllerRoutingDescriptor(descriptor, controllerType, controllerName)));

			if (index >= 0)
			{
				virtualPath.Append('/').Append(_controllerPathNames[index]);
				return true;
			}
			return false;
		}

		protected virtual bool FindControllerRoutingDescriptor(ControllerRoutingDescriptor descriptor, Type controllerTypeToFind, string controllerNameToFind)
		{
			if (controllerTypeToFind != null)
			{
				return descriptor.ControllerType == controllerTypeToFind;
			}
			return descriptor.Name.Equals(controllerNameToFind, StringComparison.OrdinalIgnoreCase);
		}
	}
}