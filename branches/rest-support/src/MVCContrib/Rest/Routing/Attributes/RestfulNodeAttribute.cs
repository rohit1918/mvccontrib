using System;
using System.Collections.Generic;

namespace MvcContrib.Rest.Routing.Attributes
{
	public abstract class RestfulNodeAttribute : Attribute, IRestfulNodePositionAttribute
	{
		#region IRestfulNodePositionAttribute Members

		public int NodePosition { get; set; }

		#endregion

		protected KeyValuePair<string, string> ToValueAndAlias(string value)
		{
			string[] parts = value.Split('=');
			if(parts.Length > 1)
			{
				return new KeyValuePair<string, string>(parts[0], parts[1]);
			}
			return new KeyValuePair<string, string>(parts[0], parts[0]);
		}
	}
}