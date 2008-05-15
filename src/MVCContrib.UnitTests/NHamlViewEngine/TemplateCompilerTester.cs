using System;
using System.Collections.Generic;
using MvcContrib.NHamlViewEngine;
using MvcContrib.NHamlViewEngine.Rules;
using NUnit.Framework;

namespace MvcContrib.UnitTests.NHamlViewEngine
{
	[TestFixture]
	public class TemplateCompilerTester : TestFixtureBase
	{
		public class ViewBase
		{
			public int Foo
			{
				get { return 9; }
			}
		}

		public class ViewBaseGeneric<T> where T : new()
		{
			public int Foo
			{
				get
				{
					object o = new T();
					var list = (List<int>)(object)o;

					return list.Count + 9;
				}
			}
		}

		[Test]
		public void BadSignifier()
		{
			var templateCompiler = new TemplateCompiler();

			var inputLine = new InputLine(Convert.ToChar(128).ToString(), 1);

			Assert.AreSame(NullMarkupRule.Instance, templateCompiler.GetRule(inputLine));
		}

		[Test]
		public void ViewBaseClass()
		{
			TemplateCompiler templateCompiler = new TemplateCompiler();
			templateCompiler.ViewBaseType = typeof(ViewBase);

			AssertRender("CustomBaseClass", templateCompiler);
		}

		[Test]
		public void ViewBaseClassGeneric()
		{
			TemplateCompiler templateCompiler = new TemplateCompiler();
			templateCompiler.ViewBaseType = typeof(ViewBaseGeneric<>);

			AssertRender("CustomBaseClass", templateCompiler, typeof(List<int>));
		}

		[Test]
		public void DuplicateUsings()
		{
			TemplateCompiler templateCompiler = new TemplateCompiler();
			templateCompiler.AddUsing("System");

			AssertRender("List");
		}

		[Test]
		public void CollectInputFiles()
		{
			TemplateCompiler templateCompiler = new TemplateCompiler();

			List<string> inputFiles = new List<string>();

			templateCompiler.Compile(TemplatesFolder + "Partials.haml",
															 TemplatesFolder + "Application.haml", inputFiles);

			Assert.AreEqual(3, inputFiles.Count);
		}

		[Test]
		public void script_tag_not_auto_closing()
		{
			TemplateCompiler templateCompiler = new TemplateCompiler();
			Assert.IsFalse(templateCompiler.IsAutoClosing("script"), "Script was auto-closed");
		}
	}
}
