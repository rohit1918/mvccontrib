using System;

namespace MVCContrib.Attributes
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public class DeserializeAttribute : Attribute
	{
	    private readonly string _prefix;

	    public DeserializeAttribute(string prefix)
		{
			_prefix = prefix;
		}

	    public string Prefix
		{
			get { return _prefix; }
		}
	}
}
