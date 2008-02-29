namespace MvcContrib.NHamlViewEngine.Rules
{
	public class IdMarkupRule : TagMarkupRule
	{
		public override char Signifier
		{
			get { return '#'; }
		}

		protected override string PreprocessLine(InputLine inputLine)
		{
			return "div#" + inputLine.NormalizedText;
		}
	}
}