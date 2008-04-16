using System;
using System.Diagnostics.CodeAnalysis;
using MvcContrib.NHamlViewEngine.Exceptions;
using MvcContrib.NHamlViewEngine.Properties;
using MvcContrib.NHamlViewEngine.Utilities;

namespace MvcContrib.NHamlViewEngine.Rules
{
	public class DocTypeMarkupRule : MarkupRule
	{
		public override char Signifier
		{
			get { return '!'; }
		}

		[SuppressMessage("Microsoft.Globalization", "CA1308")]
		public override BlockClosingAction Render(CompilationContext compilationContext)
		{
			if(compilationContext.CurrentInputLine.NormalizedText.StartsWith("!!", StringComparison.Ordinal))
			{
				string content = compilationContext.CurrentInputLine.NormalizedText.Remove(0, 2).Trim().ToLowerInvariant();

				if(string.IsNullOrEmpty(content))
				{
					compilationContext.ViewBuilder.AppendOutputLine(
						@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">");
				}
				else if(string.Equals(content, "1.1"))
				{
					compilationContext.ViewBuilder.AppendOutputLine(
            @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.1//EN"" ""http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"">");
				}
				else if(string.Equals(content, "strict"))
				{
					compilationContext.ViewBuilder.AppendOutputLine(
            @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">");
				}
				else if(string.Equals(content, "frameset"))
				{
					compilationContext.ViewBuilder.AppendOutputLine(
            @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Frameset//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd"">");
				}
				else if(string.Equals(content, "html"))
				{
					compilationContext.ViewBuilder.AppendOutputLine(
						@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">");
				}
				else if(string.Equals(content, "html strict"))
				{
					compilationContext.ViewBuilder.AppendOutputLine(
						@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN"" ""http://www.w3.org/TR/html4/strict.dtd"">");
				}
				else if(string.Equals(content, "html frameset"))
				{
					compilationContext.ViewBuilder.AppendOutputLine(
						@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Frameset//EN"" ""http://www.w3.org/TR/html4/frameset.dtd"">");
				}
				else
				{
					string[] parts = content.Split(' ');

					if(string.Equals(parts[0], "xml"))
					{
						string encoding = "utf-8";

						if(parts.Length == 2)
						{
							encoding = parts[1];
						}

						compilationContext.ViewBuilder.AppendOutputLine(StringUtils.FormatInvariant(
						                                                	@"<?xml version=""1.0"" encoding=""{0}"" ?>", encoding));
					}
					else
					{
						SyntaxException.Throw(compilationContext.CurrentInputLine, Resources.ErrorParsingTag,
						                      compilationContext.CurrentInputLine);
					}
				}
			}
			else
			{
				compilationContext.ViewBuilder.AppendOutputLine(compilationContext.CurrentInputLine.Text);
			}

			return null;
		}
	}
}