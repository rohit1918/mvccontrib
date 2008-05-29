using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using MvcContrib.NHamlViewEngine.Exceptions;
using MvcContrib.NHamlViewEngine.Rules;
using MvcContrib.NHamlViewEngine.Utilities;

namespace MvcContrib.NHamlViewEngine
{
	public sealed class TemplateCompiler
	{
		private static readonly Regex _pathCleaner
			= new Regex(@"[-\\/\.:\s]", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly string[] DefaultAutoClosingTags
			= new[] { "META", "IMG", "LINK", "BR", "HR", "INPUT" };

		private readonly StringSet _autoClosingTags
			= new StringSet(DefaultAutoClosingTags);

		private static readonly string[] DefaultUsings
			= new[] { "System", "System.Text", "MvcContrib.NHamlViewEngine", "MvcContrib.NHamlViewEngine.Utilities" };

		private readonly StringSet _usings
			= new StringSet(DefaultUsings);

		private static readonly string[] DefaultReferences = new[]
				{
					typeof(INotifyPropertyChanging).Assembly.Location,
					typeof(Action).Assembly.Location,
					typeof(TemplateCompiler).Assembly.Location
				};

		private readonly StringSet _references
			= new StringSet(DefaultReferences);

		private readonly MarkupRule[] _markupRules = new MarkupRule[128];

		private Type _viewBaseType;

		public TemplateCompiler()
		{
			AddRule(new EofMarkupRule());
			AddRule(new DocTypeMarkupRule());
			AddRule(new TagMarkupRule());
			AddRule(new ClassMarkupRule());
			AddRule(new IdMarkupRule());
			AddRule(new EvalMarkupRule());
			AddRule(new SilentEvalMarkupRule());
			AddRule(new PreambleMarkupRule());
			AddRule(new CommentMarkupRule());
			AddRule(new EscapeMarkupRule());
			AddRule(new PartialMarkupRule());

			ViewBaseType = typeof(object);
		}

		public Type ViewBaseType
		{
			get { return _viewBaseType; }
			set
			{
				Invariant.ArgumentNotNull(value, "value");

				_viewBaseType = value;

				_usings.Add(_viewBaseType.Namespace);
				_references.Add(_viewBaseType.Assembly.Location);
			}
		}

		public void AddUsing(string @namespace)
		{
			Invariant.ArgumentNotEmpty(@namespace, "namespace");

			_usings.Add(@namespace);
		}

		public IEnumerable Usings
		{
			get { return _usings; }
		}

		public void AddReference(string assemblyLocation)
		{
			Invariant.ArgumentNotEmpty(assemblyLocation, "assemblyLocation");

			_references.Add(assemblyLocation);
		}

		public IEnumerable References
		{
			get { return _references; }
		}

		public void AddRule(MarkupRule markupRule)
		{
			Invariant.ArgumentNotNull(markupRule, "markupRule");

			_markupRules[markupRule.Signifier] = markupRule;
		}

		public MarkupRule GetRule(InputLine inputLine)
		{
			Invariant.ArgumentNotNull(inputLine, "line");

			if (inputLine.Signifier >= 128)
			{
				return NullMarkupRule.Instance;
			}

			return _markupRules[inputLine.Signifier] ?? NullMarkupRule.Instance;
		}

		public bool IsAutoClosing(string tag)
		{
			Invariant.ArgumentNotEmpty(tag, "tag");

			return _autoClosingTags.Contains(tag.ToUpperInvariant());
		}

		public Type Compile(string templatePath, params Type[] genericArguments)
		{
			return Compile(templatePath, null, genericArguments);
		}

		public Type Compile(string templatePath, string layoutPath, params Type[] genericArguments)
		{
			return Compile(templatePath, layoutPath, null, genericArguments);
		}

		public Type Compile(string templatePath, string layoutPath,
												ICollection<string> inputFiles, params Type[] genericArguments)
		{
			Invariant.ArgumentNotEmpty(templatePath, "templatePath");
			Invariant.FileExists(templatePath);

			if (!string.IsNullOrEmpty(layoutPath))
			{
				Invariant.FileExists(layoutPath);
			}

			CompilationContext compilationContext
				= new CompilationContext(
					this,
					new ViewBuilder(this, MakeClassName(templatePath), genericArguments),
					templatePath,
					layoutPath);

			Compile(compilationContext);

			if (inputFiles != null)
			{
				compilationContext.CollectInputFiles(inputFiles);
			}

			return BuildView(compilationContext);
		}

		private void Compile(CompilationContext compilationContext)
		{
			while (compilationContext.CurrentNode.Next != null)
			{
				MarkupRule rule = GetRule(compilationContext.CurrentInputLine);

				if (compilationContext.CurrentInputLine.IsMultiline && rule.MergeMultiLine)
				{
					compilationContext.CurrentInputLine.Merge(compilationContext.NextInputLine);
					compilationContext.InputLines.Remove(compilationContext.NextNode);
				}
				else
				{
					rule.Process(compilationContext);
				}
			}

			compilationContext.CloseBlocks();
		}

		private Type BuildView(CompilationContext compilationContext)
		{
			string source = compilationContext.ViewBuilder.Build();

			TypeBuilder typeBuilder = new TypeBuilder(this);

			Type viewType = typeBuilder.Build(source, compilationContext.ViewBuilder.ClassName);

			if (viewType == null)
			{
				ViewCompilationException.Throw(typeBuilder.CompilerResults,
																			 typeBuilder.Source, compilationContext.TemplatePath);
			}

			return viewType;
		}

		private static string MakeClassName(string templatePath)
		{
			return _pathCleaner.Replace(templatePath, "_").TrimStart('_');
		}
	}
}
