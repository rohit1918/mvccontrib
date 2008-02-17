using System;
using System.IO;
using MvcContrib.NHamlViewEngine;
using NUnit.Framework;

namespace MvcContrib.UnitTests.NHamlViewEngine
{
	[TestFixture]
	public abstract class TestFixtureBase
	{
		protected const string TemplatesFolder = @"NHamlViewEngine\Templates\";
		protected const string ResultsFolder = @"NHamlViewEngine\Results\";

		private TemplateCompiler _templateCompiler;

		[SetUp]
		public void SetUp()
		{
			_templateCompiler = new TemplateCompiler();
		}

		protected void AssertRender(string template)
		{
			AssertRender(template, _templateCompiler);
		}

		protected void AssertRender(string template, string layout)
		{
			Type viewType = _templateCompiler.Compile(
				TemplatesFolder + template + ".haml",
				TemplatesFolder + layout + ".haml");

			ICompiledView view = (ICompiledView)Activator.CreateInstance(viewType);

			string output = view.Render();

			//Console.WriteLine(output);

			Assert.AreEqual(File.ReadAllText(ResultsFolder + layout + ".xhtml"), output);
		}

		protected static void AssertRender(string template, TemplateCompiler templateCompiler,
		                                   params string[] genericArguments)
		{
			Type viewType = templateCompiler.Compile(
				TemplatesFolder + template + ".haml", genericArguments);

			ICompiledView view = (ICompiledView)Activator.CreateInstance(viewType);

			string output = view.Render();

			//Console.WriteLine(output);

			Assert.AreEqual(File.ReadAllText(ResultsFolder + template + ".xhtml"), output);
		}
	}
}