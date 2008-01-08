using System.Collections.Generic;
using MvcContrib.NHamlViewEngine;
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

		public class ViewBaseGeneric<T>
		{
			public T Foo
			{
				get
				{
					object o = 9;

					return (T)o;
				}
			}
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

			AssertRender("CustomBaseClass", templateCompiler, "int");
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
	}
}