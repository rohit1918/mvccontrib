using System;
using Microsoft.Practices.ObjectBuilder;
using MvcContrib.ObjectBuilder;
using NUnit.Framework;

namespace MvcContrib.UnitTests.ObjectBuilder
{
	[TestFixture, Category("ObjectBuilderFactory")]
	public class DependencyContainerTester
	{
		[Test]
		public void InjectTest()
		{
			IFoo classA = new Foo();
			using(var container = new DependencyContainer())
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
			using(var container = new DependencyContainer())
			{
				Assert.IsNull(container.Inject(null));
				Assert.IsNull(container.Inject<Bar>(null));
				Assert.IsNull(container.Inject(null, typeof(Bar)));
			}
		}

		[Test]
		public void SingletonTest()
		{
			using(var container = new DependencyContainer())
			{
				var myFoo = new Foo();
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterSingleton<Bar>();
				container.RegisterInstance(container.Inject(myFoo) as IFoo);

				var classA = container.Get<IFoo>();
				var classB = container.Get<IBar>();
				classB.Name = "Foo";

				Assert.IsNotNull(classA.Dependency);
				Assert.AreEqual(classB.Name, classA.Dependency.Name);
			}
		}

		[Test]
		public void ConstructorTest()
		{
			using(var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterSingleton<Bar>();
				container.RegisterTypeMapping<IFoo, Foo2>();

				var classA = container.Get<IFoo>();

				Assert.IsNotNull(classA.Dependency);
				Assert.IsInstanceOfType(typeof(Foo2), classA);
			}
		}

		[Test]
		public void FindSingletonsTest()
		{
			using(var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterTypeMapping<IFoo, Foo2>();
				container.RegisterSingleton<Bar>();

				var bar1 = container.Get<IBar>();
				var foo1 = container.Get<Foo2>();
				var foo2 = container.Get<Foo2>();
				var bar2 = container.Get<IBar>();

				int count = 0;
				foreach(var bar in container.FindSingletons<IBar>())
					count++;

				Assert.AreEqual(count, 1);
				//non-singletons, shouldn't be found
				foreach(var foo in container.FindSingletons<Foo2>())
					count++;
				Assert.AreEqual(count, 1);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void InvalidTypeTest()
		{
			using(var container = new DependencyContainer())
			{
				var classA = container.Get<IFoo>();
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void InvalidTypeTest2()
		{
			using(var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IFoo, Foo>();
				//dependency bar is not yet registered
				var classA = container.Get<IFoo>();
			}
		}

		[Test]
		public void XmlConfigurationTest()
		{
			string xmlConfig =
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
			using(var container = new DependencyContainer(xmlConfig))
			{
				var classA = container.Get<IFoo>();
				var classB = container.Get<IBar>();
				var bar2 = container.Get<Bar2>();
				var foo2 = container.Get<Foo2>();
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
			using(var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Foo>();
				container.Get<IBar>();
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void UnknownConstructorTest()
		{
			using(var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IFoo, Foo2>();
				container.Get<IFoo>();
			}
		}

		[Test]
		public void PropertySetterTest()
		{
			using (var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterTypeMapping<IFoo, Foo>();
				container.RegisterPropertySetter<Foo>("Something", "FooSomething");
				var foo = container.Get<IFoo>();

				Assert.AreEqual("FooSomething", foo.Something);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void PropertySetterTest_TypeNullException()
		{
			using (var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterTypeMapping<IFoo, Foo>();
				container.RegisterPropertySetter(null, "Something", "FooSomething");
				var foo = container.Get<IFoo>();

				Assert.AreEqual("FooSomething", foo.Something);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void PropertySetterTest_PropertyNullException()
		{
			using (var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterTypeMapping<IFoo, Foo>();
				container.RegisterPropertySetter<Foo>(null, "FooSomething");
				var foo = container.Get<IFoo>();
			}
		}

		[Test]
		[ExpectedException(typeof(ApplicationException))]
		public void PropertySetterTest_InvalidProperty()
		{
			using (var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterTypeMapping<IFoo, Foo>();
				container.RegisterPropertySetter<Foo>("PropertyThatDoesNotExist", "FooSomething");
				var foo = container.Get<IFoo>();
			}
		}


		[Test]
		public void Type_PropertySetterTest()
		{
			using (var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterTypeMapping<IFoo, Foo>();
				container.RegisterPropertySetter<Foo, IBar>("SomeOtherBar");
				var foo = container.Get<IFoo>();

				Assert.IsNotNull(((Foo)foo).SomeOtherBar);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Type_PropertySetterTest_TypeNullException()
		{
			using (var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterTypeMapping<IFoo, Foo>();
				container.RegisterPropertySetter(null, "Something", typeof(IBar));
				var foo = container.Get<IFoo>();
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Type_PropertySetterTest_PropertyNullException()
		{
			using (var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterTypeMapping<IFoo, Foo>();
				container.RegisterPropertySetter<Foo, IBar>(null);
				var foo = container.Get<IFoo>();
			}
		}

		[Test]
		public void MethodInjectionTest()
		{
			using (var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterTypeMapping<IFoo, Foo>();
				container.RegisterMethodInjection<Foo>("SetSomething", new ValueParameter<String>("I've been set"));
				var foo = container.Get<IFoo>();

				Assert.AreEqual("I've been set", foo.Something);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void MethodInjectionTest_TypeNull()
		{
			using (var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterTypeMapping<IFoo, Foo>();
				container.RegisterMethodInjection(null, "SetSomething", new ValueParameter<String>("I've been set"));
				var foo = container.Get<IFoo>();
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void MethodInjectionTest_PropertyNull()
		{
			using (var container = new DependencyContainer())
			{
				container.RegisterTypeMapping<IBar, Bar>();
				container.RegisterTypeMapping<IFoo, Foo>();
				container.RegisterMethodInjection<Foo>(null, new ValueParameter<String>("I've been set"));
				var foo = container.Get<IFoo>();
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

			public IBar SomeOtherBar
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
