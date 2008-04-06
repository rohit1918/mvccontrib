namespace MvcContrib.Rest.Routing.Descriptors
{
	public class SplitPrefix
	{
		public static readonly SplitPrefix Dot = new SplitPrefix('.');
		public static readonly SplitPrefix Slash = new SplitPrefix('/');
		public static readonly char[] SplitChars = new[] {Slash.Prefix, Dot.Prefix};

		private SplitPrefix(char prefix)
		{
			Prefix = prefix;
		}

		public char Prefix { get; private set; }

		public override string ToString()
		{
			return Prefix.ToString();
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return Prefix.GetHashCode();
		}

		public static implicit operator string(SplitPrefix splitPrefix)
		{
			return splitPrefix.ToString();
		}
	}
}