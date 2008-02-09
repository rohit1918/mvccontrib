using Boo.Lang.Compiler.Ast;
using MvcContrib.BrailViewEngine;

namespace MVCContrib.UnitTests.BrailViewEngine
{
	using NUnit.Framework;

	[TestFixture]
	[Category("BrailViewEngine")]
	public class ReturnValueVisitorTester
	{
		[Test]
		public void ForCoverage()
		{
			Block block = new Block();
			ReturnStatement statement = new ReturnStatement(new StringLiteralExpression("literal"));
			block.Statements.Add(statement);

			ReturnValueVisitor visitor = new ReturnValueVisitor();
			bool found = visitor.Found;
			visitor.OnReturnStatement(statement);
		}
	}
}