using System.Collections;
namespace MvcContrib.UI.Tags
{
	public class Link : ScriptableElement
	{
		private const string HREF = "href";
		private const string TARGET = "target";
		public Link() : base("a")
		{
		}

		public Link(string url) : base("a")
		{
			this.Href = url;
		}

		public Link(string url, IDictionary attributes)
			: base("a",attributes)
		{
			this.Href = url;
		}

		public virtual string Href
		{
			get { return NullGet(HREF); }
			set { NullSet(HREF, value); }
		}

		//this is not XHTML Strict compliant.
		public virtual string Target
		{
			get { return NullGet(TARGET); }
			set { NullSet(TARGET, value); }
		}
	}
}