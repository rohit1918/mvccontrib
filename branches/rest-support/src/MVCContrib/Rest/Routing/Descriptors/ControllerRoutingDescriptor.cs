using System;
using MvcContrib.Rest.Routing.Ext;

namespace MvcContrib.Rest.Routing.Descriptors
{
	public class ControllerRoutingDescriptor : IEquatable<ControllerRoutingDescriptor>
	{
		public ControllerRoutingDescriptor(Type controllerType)
		{
			ControllerType = controllerType;
			Name = controllerType.RemoveTrailingControllerFromTypeName();
			EntityName = Name.Singularize().LowerFirst();
		}

		public string Name { get; private set; }

		public string FullName
		{
			get { return ControllerType.FullName; }
		}

		public string EntityName { get; private set; }

		public string NameSpace
		{
			get { return ControllerType.Namespace; }
		}

		public Type ControllerType { get; private set; }

		#region IEquatable<ControllerRoutingDescriptor> Members

		public bool Equals(ControllerRoutingDescriptor other)
		{
			if(other == null)
			{
				return false;
			}
			if(ReferenceEquals(this, other))
			{
				return true;
			}
			return ControllerType == other.ControllerType;
		}

		#endregion

		public override int GetHashCode()
		{
			return ControllerType.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ControllerRoutingDescriptor);
		}
	}
}