using Boo.Lang.Compiler.Ast;
using MvcContrib.BrailViewEngine;

namespace MvcContrib.UnitTests
{
	using NUnit.Framework;

	[TestFixture]
	[Category("BrailViewEngine")]
	public class DslMacroTester
	{
		[Test]
		public void ForCoverage()
		{
			MacroStatement statement = new MacroStatement();
			statement.Arguments.Add(new ReferenceExpression("xml"));

			DslMacro macro = new DslMacro();
			macro.Expand(statement);
		}
	}
}