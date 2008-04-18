namespace MvcContrib.NHamlViewEngine.Rules
{
	public class EvalMarkupRule : MarkupRule
	{
		public override char Signifier
		{
			get { return '='; }
		}
    
		public override bool MergeMultiLine
		{
			get { return true; }
		}

		public override BlockClosingAction Render(CompilationContext compilationContext)
		{
			compilationContext.ViewBuilder.AppendOutput(compilationContext.CurrentInputLine.Indent);
			compilationContext.ViewBuilder.AppendCodeLine(compilationContext.CurrentInputLine.NormalizedText.Trim());

			return null;
		}
	}
}