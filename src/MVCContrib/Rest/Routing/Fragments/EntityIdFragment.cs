namespace MvcContrib.Rest.Routing.Fragments
{
	public class EntityIdFragment : RequiredPatternFragment
	{
		public EntityIdFragment(string pattern) : base("id", pattern, 1) {}
		public EntityIdFragment(string name, string pattern) : base(name, pattern, 1) {}

		public override string Name
		{
			get { return Id; }
			protected set { Id = value; }
		}

		public virtual string Id { get; set; }
	}
}