﻿using System;
using Microsoft.Practices.ObjectBuilder;
using MvcContrib.ObjectBuilder;
using NUnit.Framework;
using Rhino.Mocks;

namespace MVCContrib.UnitTests.ObjectBuilder
{
	[TestFixture, Category("ObjectBuilderFactory")]
	public class ContainerXmlConfigTester
	{
		private MockRepository _mocks;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
		}

		[Test]
		public void ValidXmlTest()
		{
			IBuilder<BuilderStage> builder = new BuilderBase<BuilderStage>();

			ContainerXmlConfig config = new ContainerXmlConfig(validConfig);
			config.ApplyConfiguration(builder);

			Assert.AreEqual(8, builder.Policies.Count);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvalidTypeConfigTest()
		{
			IBuilder<BuilderStage> builder = new BuilderBase<BuilderStage>();

			ContainerXmlConfig config = new ContainerXmlConfig(invalidTypeConfig);
			config.ApplyConfiguration(builder);
		}

		#region configs

		private string invalidTypeConfig =
			@"<?xml version=""1.0"" encoding=""utf-16""?>
<ContainerConfig xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""container-config"">
<Mappings>
    <Mapping    FromType=""MVCContrib.UnitTests.ObjectBuilder.DependencyContainerTester+INotExisting,MVCContrib.UnitTests"" 
                ToType=""MVCContrib.UnitTests.ObjectBuilder.DependencyContainerTester+NotExisting,MVCContrib.UnitTests"" />
</Mappings>
</ContainerConfig>
";

		private string validConfig =
			@"<?xml version=""1.0"" encoding=""utf-16""?>
<ContainerConfig xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""container-config"">
<Mappings>
    <Mapping    FromType=""MVCContrib.UnitTests.ObjectBuilder.DependencyContainerTester+IBar,MVCContrib.UnitTests"" 
                ToType=""MVCContrib.UnitTests.ObjectBuilder.DependencyContainerTester+Bar,MVCContrib.UnitTests"" />
    <Mapping    FromType=""MVCContrib.UnitTests.ObjectBuilder.DependencyContainerTester+IFoo,MVCContrib.UnitTests"" 
                ToType=""MVCContrib.UnitTests.ObjectBuilder.DependencyContainerTester+Foo,MVCContrib.UnitTests"" />
</Mappings>
<BuildRules>
    <BuildRule Mode=""Instance"" Type=""MVCContrib.UnitTests.ObjectBuilder.DependencyContainerTester+Foo,MVCContrib.UnitTests"">
        <Method Name=""SetSomething"">
            <Value Type=""System.String"">Foo</Value>
        </Method>
    </BuildRule>
    <BuildRule Mode=""Instance"" Type=""MVCContrib.UnitTests.ObjectBuilder.DependencyContainerTester+Foo,MVCContrib.UnitTests"">
        <Property Name=""SomethingElse"">
            <Value Type=""System.String"">Bar</Value>
        </Property>
    </BuildRule>
    <BuildRule Mode=""Singleton"" Type=""MVCContrib.UnitTests.ObjectBuilder.DependencyContainerTester+Bar2,MVCContrib.UnitTests"">
        <Constructor>
            <Value Type=""System.String"">Bar2</Value>
        </Constructor>
    </BuildRule>
    <BuildRule Mode=""Instance"" Type=""MVCContrib.UnitTests.ObjectBuilder.DependencyContainerTester+Foo2,MVCContrib.UnitTests"">
        <Constructor>
            <Ref Type=""MVCContrib.UnitTests.ObjectBuilder.DependencyContainerTester+Bar2,MVCContrib.UnitTests"" />
        </Constructor>
    </BuildRule>
</BuildRules>
</ContainerConfig>
";

		#endregion
	}
}