using System;
using Microsoft.Practices.ObjectBuilder;
using MvcContrib.ObjectBuilder.Configuration;
using NUnit.Framework;

namespace MvcContrib.UnitTests.ObjectBuilder
{
	[TestFixture, Category("ObjectBuilderFactory")]
	public class ContainerXmlConfigTester
	{
		[Test]
		public void ValidXmlTest()
		{
			IBuilder<BuilderStage> builder = new BuilderBase<BuilderStage>();

			var config = new ContainerXmlConfig(validConfig);
			config.ApplyConfiguration(builder);

			Assert.AreEqual(8, builder.Policies.Count);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvalidTypeConfigTest()
		{
			IBuilder<BuilderStage> builder = new BuilderBase<BuilderStage>();

			var config = new ContainerXmlConfig(invalidTypeConfig);
			config.ApplyConfiguration(builder);
		}

		#region configs

		private string invalidTypeConfig =
			@"<?xml version=""1.0"" encoding=""utf-16""?>
<ContainerConfig xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""container-config"">
<Mappings>
    <Mapping    FromType=""MvcContrib.UnitTests.ObjectBuilder.DependencyContainerTester+INotExisting,MVCContrib.UnitTests"" 
                ToType=""MvcContrib.UnitTests.ObjectBuilder.DependencyContainerTester+NotExisting,MVCContrib.UnitTests"" />
</Mappings>
</ContainerConfig>
";

		private string validConfig =
			@"<?xml version=""1.0"" encoding=""utf-16""?>
<ContainerConfig xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""container-config"">
<Mappings>
    <Mapping    FromType=""MvcContrib.UnitTests.ObjectBuilder.DependencyContainerTester+IBar,MVCContrib.UnitTests"" 
                ToType=""MvcContrib.UnitTests.ObjectBuilder.DependencyContainerTester+Bar,MVCContrib.UnitTests"" />
    <Mapping    FromType=""MvcContrib.UnitTests.ObjectBuilder.DependencyContainerTester+IFoo,MVCContrib.UnitTests"" 
                ToType=""MvcContrib.UnitTests.ObjectBuilder.DependencyContainerTester+Foo,MVCContrib.UnitTests"" />
</Mappings>
<BuildRules>
    <BuildRule Mode=""Instance"" Type=""MvcContrib.UnitTests.ObjectBuilder.DependencyContainerTester+Foo,MVCContrib.UnitTests"">
        <Method Name=""SetSomething"">
            <Value Type=""System.String"">Foo</Value>
        </Method>
    </BuildRule>
    <BuildRule Mode=""Instance"" Type=""MvcContrib.UnitTests.ObjectBuilder.DependencyContainerTester+Foo,MVCContrib.UnitTests"">
        <Property Name=""SomethingElse"">
            <Value Type=""System.String"">Bar</Value>
        </Property>
    </BuildRule>
    <BuildRule Mode=""Singleton"" Type=""MvcContrib.UnitTests.ObjectBuilder.DependencyContainerTester+Bar2,MVCContrib.UnitTests"">
        <Constructor>
            <Value Type=""System.String"">Bar2</Value>
        </Constructor>
    </BuildRule>
    <BuildRule Mode=""Instance"" Type=""MvcContrib.UnitTests.ObjectBuilder.DependencyContainerTester+Foo2,MVCContrib.UnitTests"">
        <Constructor>
            <Ref Type=""MvcContrib.UnitTests.ObjectBuilder.DependencyContainerTester+Bar2,MVCContrib.UnitTests"" />
        </Constructor>
    </BuildRule>
</BuildRules>
</ContainerConfig>
";

		#endregion
	}
}
