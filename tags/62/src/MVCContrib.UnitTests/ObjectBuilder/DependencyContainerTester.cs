using System;
using Microsoft.Practices.ObjectBuilder;
using MvcContrib.ObjectBuilder;
using NUnit.Framework;

namespace MVCContrib.UnitTests.ObjectBuilder
{
	[TestFixture, Category("ObjectBuilderFactory")]
	public class DependencyContainerTester
	{
		[Test]
		public void InjectTest()
		{
			IFoo classA = new Foo();
			using(DependencyContainer container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterSingleton<IBar>();
				container.Inject(classA);
			}
			Assert.IsNotNull(classA.Dependency);
		}

		[Test]
		public void NullInjections()
		{
			using(DependencyContainer container = new DependencyContainer())
			{
				Assert.IsNull(container.Inject(null));
				Assert.IsNull(container.Inject<Bar>(null));
				Assert.IsNull(container.Inject(null, typeof(Bar)));
			}
		}

		[Test]
		public void SingletonTest()
		{
			using(DependencyContainer container = new DependencyContainer())
			{
				Foo myFoo = new Foo();
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterSingleton<Bar>();
				container.RegisterInstance<IFoo>(container.Inject(myFoo) as IFoo);

				IFoo classA = container.Get<IFoo>();
				IBar classB = container.Get<IBar>();
				classB.Name = "Foo";

				Assert.IsNotNull(classA.Dependency);
				Assert.AreEqual(classB.Name, classA.Dependency.Name);
			}
		}

		[Test]
		public void ConstructorTest()
		{
			using(DependencyContainer container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterSingleton<Bar>();
				container.RegisterTypeMapping<IFoo, Foo2>();

				IFoo classA = container.Get<IFoo>();

				Assert.IsNotNull(classA.Dependency);
				Assert.IsInstanceOfType(typeof(Foo2), classA);
			}
		}

		[Test]
		public void FindSingletonsTest()
		{
			using(DependencyContainer container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterTypeMapping<IFoo, Foo2>();
				container.RegisterSingleton<Bar>();

				IBar bar1 = container.Get<IBar>();
				Foo2 foo1 = container.Get<Foo2>();
				Foo2 foo2 = container.Get<Foo2>();
				IBar bar2 = container.Get<IBar>();

				int count = 0;
				foreach(IBar bar in container.FindSingletons<IBar>())
					count++;

				Assert.AreEqual(count, 1);
				//non-singletons, shouldn't be found
				foreach(Foo2 foo in container.FindSingletons<Foo2>())
					count++;
				Assert.AreEqual(count, 1);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void InvalidTypeTest()
		{
			using(DependencyContainer container = new DependencyContainer())
			{
				IFoo classA = container.Get<IFoo>();
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void InvalidTypeTest2()
		{
			using(DependencyContainer container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IFoo, Foo>();
				//dependency bar is not yet registered
				IFoo classA = container.Get<IFoo>();
			}
		}

		[Test]
		public void XmlConfigurationTest()
		{
			string xmlConfig =
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
			using(DependencyContainer container = new DependencyContainer(xmlConfig))
			{
				IFoo classA = container.Get<IFoo>();
				IBar classB = container.Get<IBar>();
				Bar2 bar2 = container.Get<Bar2>();
				Foo2 foo2 = container.Get<Foo2>();
				classB.Name = "Foo";

				Assert.IsNotNull(classA.Dependency);
				Assert.AreEqual("Foo", classA.Something);
				Assert.AreEqual("Bar", classA.SomethingElse);
				Assert.AreEqual("Bar2", bar2.Name);
				Assert.AreEqual("Bar2", foo2.Dependency.Name);
				Assert.IsInstanceOfType(typeof(Foo), classA);
				Assert.IsInstanceOfType(typeof(Bar), classB);
			}
		}

		[Test]
		[ExpectedException(typeof(IncompatibleTypesException))]
		public void IncompatibleTypesExceptionTest()
		{
			using(DependencyContainer container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Foo>();
				container.Get<IBar>();
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void UnknownConstructorTest()
		{
			using(DependencyContainer container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IFoo, Foo2>();
				container.Get<IFoo>();
			}
		}

		public interface IFoo
		{
			IBar Dependency { get; set; }
			void SetSomething(string something);
			string Something { get; set; }
			string SomethingElse { get; set; }
		}

		public class Foo : IFoo
		{
			[Dependency]
			public IBar Dependency
			{
				get;
				set;
			}

			public void SetSomething(string something)
			{
				Something = something;
			}

			public string Something
			{
				get;
				set;
			}

			public string SomethingElse
			{
				get;
				set;
			}
		}

		public class Foo2 : IFoo
		{
			public Foo2(IBar dep)
			{
				Dependency = dep;
			}

			public IBar Dependency
			{
				get;
				set;
			}

			public void SetSomething(string something)
			{
				Something = something;
			}

			public void SetSomethingElse(string somethingElse)
			{
				SomethingElse = somethingElse;
			}

			public string Something
			{
				get;
				set;
			}

			public string SomethingElse
			{
				get;
				set;
			}
		}

		public interface IBar
		{
			string Name { get; set; }
		}

		public class Bar : IBar
		{
			public string Name
			{
				get;
				set;
			}
		}

		public class Bar2 : IBar
		{
			public Bar2(string name)
			{
				Name = name;
			}

			public string Name
			{
				get;
				set;
			}
		}
	}
}