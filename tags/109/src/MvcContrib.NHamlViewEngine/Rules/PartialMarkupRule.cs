using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MvcContrib.NHamlViewEngine.Rules
{
	public class PartialMarkupRule : MarkupRule
	{
		private static readonly Regex _partialRegex
			= new Regex(@"^\s*([\w-\\]+)$", RegexOptions.Compiled | RegexOptions.Singleline);

		public override char Signifier
		{
			get { return '_'; }
		}

		public override void Process(CompilationContext compilationContext)
		{
			Render(compilationContext);
		}

		public override BlockClosingAction Render(CompilationContext compilationContext)
		{
			Match match = _partialRegex.Match(compilationContext.CurrentInputLine.NormalizedText);

			if(match.Success)
			{
				string templateDirectory
					= Path.GetDirectoryName(compilationContext.TemplatePath);

				string partialName = match.Groups[1].Value;
				partialName = partialName.Insert(partialName.LastIndexOf(@"\", StringComparison.OrdinalIgnoreCase) + 1, "_");

				string partialTemplatePath
					= Path.Combine(templateDirectory, partialName + ".haml");

				if (!File.Exists(partialTemplatePath))
				{
					// try one level up

					partialTemplatePath
						= Path.Combine(templateDirectory, @"..\" + partialName + ".haml");
				}

				compilationContext.MergeTemplate(partialTemplatePath);
			}
			else if(!string.IsNullOrEmpty(compilationContext.LayoutPath))
			{
				compilationContext.MergeTemplate(compilationContext.TemplatePath);
			}

			return null;
		}
	}
}