using System.Configuration;
using MvcContrib.BrailViewEngine;

namespace MVCContrib.UnitTests.BrailViewEngine
{
	using NUnit.Framework;

	[TestFixture]
	[Category("BrailViewEngine")]
	public class BooViewEngineOptionsTester
	{

		[Test]
		public void Can_Create_Options()
		{
			BooViewEngineOptions options = new BooViewEngineOptions();

			options.Debug = true;
			options.SaveToDisk = true;
			options.BatchCompile = true;
			options.CommonScriptsDirectory = "CommonScripts";
			options.SaveDirectory = "SaveDirectory";
		}

		[Test]
		public void Can_Read_From_AppConfig()
		{
			BooViewEngineOptions options = ConfigurationManager.GetSection("brail") as BooViewEngineOptions;
			Assert.IsNotNull(options);
		}
	}
}