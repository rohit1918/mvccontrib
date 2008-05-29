using System.Text.RegularExpressions;
using MvcContrib.NHamlViewEngine.Exceptions;
using MvcContrib.NHamlViewEngine.Properties;

namespace MvcContrib.NHamlViewEngine.Rules
{
	public class CommentMarkupRule : MarkupRule
	{
		private static readonly Regex _commentRegex
			= new Regex(@"^(\[[\w\s\.]*\])?(.*)$", RegexOptions.Compiled | RegexOptions.Singleline);

		public override char Signifier
		{
			get { return '/'; }
		}


		public override bool MergeMultiLine
		{
			get { return true; }
		}

		public override BlockClosingAction Render(CompilationContext compilationContext)
		{
			Match match = _commentRegex.Match(compilationContext.CurrentInputLine.NormalizedText);

			if (!match.Success)
			{
				SyntaxException.Throw(compilationContext.CurrentInputLine,
															Resources.ErrorParsingTag, compilationContext.CurrentInputLine);
			}

			string ieBlock = match.Groups[1].Value;
			string content = match.Groups[2].Value;

			string openingTag = compilationContext.CurrentInputLine.Indent + "<!--";
			string closingTag = "-->";

			if (!string.IsNullOrEmpty(ieBlock))
			{
				openingTag += ieBlock + '>';
				closingTag = "<![endif]" + closingTag;
			}

			if (string.IsNullOrEmpty(content))
			{
				compilationContext.ViewBuilder.AppendOutputLine(openingTag);
				closingTag = compilationContext.CurrentInputLine.Indent + closingTag;
			}
			else
			{
				if (content.Length > 50)
				{
					compilationContext.ViewBuilder.AppendOutputLine(openingTag);
					compilationContext.ViewBuilder.AppendOutput(compilationContext.CurrentInputLine.Indent + "  ");
					compilationContext.ViewBuilder.AppendOutputLine(content);
				}
				else
				{
					compilationContext.ViewBuilder.AppendOutput(openingTag + content);
					closingTag = ' ' + closingTag;
				}
			}

			return delegate { compilationContext.ViewBuilder.AppendOutputLine(closingTag); };
		}
	}
}