using System;

namespace MvcContrib.ModelAttributes
{
	public class MaxLengthAttribute : Attribute
	{
		private readonly int length;

		public MaxLengthAttribute(int length)
		{
			this.length = length;
		}

		public int Length
		{
			get { return length; }
		}
	}
}
