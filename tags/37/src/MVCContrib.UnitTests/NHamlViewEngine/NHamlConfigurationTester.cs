using MvcContrib.NHamlViewEngine.Configuration;
using NUnit.Framework;

namespace MvcContrib.UnitTests.NHamlViewEngine
{
	[TestFixture, Category("Integration")]
	public class ConfigurationTester
	{
		[Test]
		public void Can_Read_Production_Setting_From_AppSettings()
		{
			NHamlViewEngineSection section = NHamlViewEngineSection.Read();

			Assert.IsNotNull(section);
			Assert.IsFalse(section.Production);
		}
	}
}