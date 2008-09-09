//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory
//-------------------------------------------------------------------------------
// Copyright (C) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//-------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder.Strategies.Mocks;
using Microsoft.Practices.ObjectBuilder;
using NUnit.Framework;
using CreationStrategy=Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies.CreationStrategy;
using MethodReflectionStrategy=Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies.MethodReflectionStrategy;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder.Strategies
{
	[TestFixture, Category("ObjectBuilderFactory")]
	public class MethodReflectionStrategyFixture
	{
		// Invalid attribute combination

		[Test]
		[ExpectedException(typeof(InvalidAttributeException))]
		public void SpecifyingCreateNewAndDependencyThrows()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof(MockInvalidDualAttributes), null, null);
		}

		// Attribute Inheritance

		[Test]
		public void CanInheritDependencyAttribute()
		{
			MockBuilderContext context = CreateContext();

			var depending =
				(MockDependingObjectDerived)context.HeadOfChain.BuildUp(context, typeof(MockDependingObjectDerived), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNotNull(depending.InjectedObject);
		}

		[Test]
		public void CanInheritCreateNewAttribute()
		{
			MockBuilderContext context = CreateContext();

			var depending =
				(MockRequiresNewObjectDerived)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObjectDerived), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNotNull(depending.Foo);
		}

		// Non creatable stuff

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowsIfConcreteTypeToCreateCannotBeCreated()
		{
			MockBuilderContext context = CreateContext();
			context.HeadOfChain.BuildUp(context, typeof(MockDependsOnInterface), null, null);
		}

		// Mode 1

		[Test]
		public void CreateNewAttributeAlwaysCreatesNewObject()
		{
			var context1 = CreateContext();
			var depending1 =
				(MockRequiresNewObject)context1.HeadOfChain.BuildUp(context1, typeof(MockRequiresNewObject), null, null);

			var context2 = CreateContext();
			var depending2 =
				(MockRequiresNewObject)context2.HeadOfChain.BuildUp(context2, typeof(MockRequiresNewObject), null, null);

			Assert.IsNotNull(depending1);
			Assert.IsNotNull(depending2);
			Assert.IsNotNull(depending1.Foo);
			Assert.IsNotNull(depending2.Foo);
			Assert.IsFalse(depending1.Foo == depending2.Foo);
		}

		[Test]
		public void NamedAndUnnamedObjectsInLocatorDontGetUsedForCreateNew()
		{
			var unnamed = new object();
			var named = new object();

			var context1 = CreateContext();
			context1.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), null), unnamed);
			context1.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), "Foo"), named);
			var depending1 =
				(MockRequiresNewObject)context1.HeadOfChain.BuildUp(context1, typeof(MockRequiresNewObject), null, null);

			var context2 = CreateContext();
			context2.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), null), unnamed);
			context2.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), "Foo"), named);
			var depending2 =
				(MockRequiresNewObject)context2.HeadOfChain.BuildUp(context2, typeof(MockRequiresNewObject), null, null);

			Assert.IsFalse(depending1.Foo == unnamed);
			Assert.IsFalse(depending1.Foo == unnamed);
			Assert.IsFalse(depending2.Foo == named);
			Assert.IsFalse(depending2.Foo == named);
		}

		// Mode 2

		[Test]
		public void CanInjectExistingUnnamedObjectIntoProperty()
		{
			// Mode 2, with an existing object
			MockBuilderContext context = CreateContext();
			var dependent = new object();
			context.InnerLocator.Add(new DependencyResolutionLocatorKey(typeof(object), null), dependent);

			object depending = context.HeadOfChain.BuildUp(context, typeof(MockDependingObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockDependingObject);
			Assert.AreSame(dependent, ((MockDependingObject)depending).InjectedObject);
		}

		[Test]
		public void InjectionCreatingNewUnnamedObjectWillOnlyCreateOnce()
		{
			// Mode 2, both flavors
			var context1 = CreateContext();
			var depending1 =
				(MockDependingObject)context1.HeadOfChain.BuildUp(context1, typeof(MockDependingObject), null, null);

			var context2 = CreateContext(context1.Locator);
			var depending2 =
				(MockDependingObject)context2.HeadOfChain.BuildUp(context2, typeof(MockDependingObject), null, null);

			Assert.AreSame(depending1.InjectedObject, depending2.InjectedObject);
		}

		[Test]
		public void InjectionCreatesNewObjectIfNotExisting()
		{
			// Mode 2, no existing object
			MockBuilderContext context = CreateContext();

			object depending = context.HeadOfChain.BuildUp(context, typeof(MockDependingObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockDependingObject);
			Assert.IsNotNull(((MockDependingObject)depending).InjectedObject);
		}

		[Test]
		public void CanInjectNewInstanceWithExplicitTypeIfNotExisting()
		{
			// Mode 2, explicit type
			MockBuilderContext context = CreateContext();

			var depending = (MockDependsOnIFoo)context.HeadOfChain.BuildUp(
			                                                 	context, typeof(MockDependsOnIFoo), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNotNull(depending.Foo);
		}

		// Mode 3

		[Test]
		public void CanInjectExistingNamedObjectIntoProperty()
		{
			// Mode 3, with an existing object
			MockBuilderContext context = CreateContext();
			var dependent = new object();
			context.InnerLocator.Add(new DependencyResolutionLocatorKey(typeof(object), "Foo"), dependent);

			object depending = context.HeadOfChain.BuildUp(context, typeof(MockDependingNamedObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockDependingNamedObject);
			Assert.AreSame(dependent, ((MockDependingNamedObject)depending).InjectedObject);
		}

		[Test]
		public void InjectionCreatingNewNamedObjectWillOnlyCreateOnce()
		{
			// Mode 3, both flavors
			var context1 = CreateContext();
			var depending1 =
				(MockDependingNamedObject)context1.HeadOfChain.BuildUp(context1, typeof(MockDependingNamedObject), null, null);

			var context2 = CreateContext(context1.Locator);
			var depending2 =
				(MockDependingNamedObject)context2.HeadOfChain.BuildUp(context2, typeof(MockDependingNamedObject), null, null);

			Assert.AreSame(depending1.InjectedObject, depending2.InjectedObject);
		}

		[Test]
		public void InjectionCreatesNewNamedObjectIfNotExisting()
		{
			// Mode 3, no existing object
			MockBuilderContext context = CreateContext();

			var depending =
				(MockDependingNamedObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingNamedObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNotNull(depending.InjectedObject);
		}

		[Test]
		public void CanInjectNewNamedInstanceWithExplicitTypeIfNotExisting()
		{
			// Mode 3, explicit type
			MockBuilderContext context = CreateContext();

			var depending =
				(MockDependsOnNamedIFoo)context.HeadOfChain.BuildUp(context, typeof(MockDependsOnNamedIFoo), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNotNull(depending.Foo);
		}

		// Mode 2 & 3 together

		[Test]
		public void NamedAndUnnamedObjectsDontCollide()
		{
			MockBuilderContext context = CreateContext();
			var dependent = new object();
			context.InnerLocator.Add(new DependencyResolutionLocatorKey(typeof(object), null), dependent);

			var depending =
				(MockDependingNamedObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingNamedObject), null, null);

			Assert.IsFalse(ReferenceEquals(dependent, depending.InjectedObject));
		}

		// Mode 4

		[Test]
		public void PropertyIsNullIfUnnamedNotExists()
		{
			// Mode 4, no object provided
			MockBuilderContext context = CreateContext();

			var depending =
				(MockOptionalDependingObject)context.HeadOfChain.BuildUp(context, typeof(MockOptionalDependingObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNull(depending.InjectedObject);
		}

		[Test]
		public void CanInjectExistingUnnamedObjectIntoOptionalDependentProperty()
		{
			// Mode 4, with an existing object
			MockBuilderContext context = CreateContext();
			var dependent = new object();
			context.InnerLocator.Add(new DependencyResolutionLocatorKey(typeof(object), null), dependent);

			object depending = context.HeadOfChain.BuildUp(context, typeof(MockOptionalDependingObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockOptionalDependingObject);
			Assert.AreSame(dependent, ((MockOptionalDependingObject)depending).InjectedObject);
		}

		// Mode 5

		[Test]
		public void PropertyIsNullIfNamedNotExists()
		{
			// Mode 5, no object provided
			MockBuilderContext context = CreateContext();

			var depending =
				(MockOptionalDependingObjectWithName)
				context.HeadOfChain.BuildUp(context, typeof(MockOptionalDependingObjectWithName), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNull(depending.InjectedObject);
		}

		[Test]
		public void CanInjectExistingNamedObjectIntoOptionalDependentProperty()
		{
			// Mode 5, with an existing object
			MockBuilderContext context = CreateContext();
			var dependent = new object();
			context.InnerLocator.Add(new DependencyResolutionLocatorKey(typeof(object), "Foo"), dependent);

			object depending = context.HeadOfChain.BuildUp(context, typeof(MockOptionalDependingObjectWithName), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockOptionalDependingObjectWithName);
			Assert.AreSame(dependent, ((MockOptionalDependingObjectWithName)depending).InjectedObject);
		}

		// NotPresentBehavior.Throw Tests

		[Test]
		[ExpectedException(typeof(DependencyMissingException))]
		public void StrategyThrowsIfObjectNotPresent()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof(ThrowingMockObject), null, null);
		}

		[Test]
		[ExpectedException(typeof(DependencyMissingException))]
		public void StrategyThrowsIfNamedObjectNotPresent()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof(NamedThrowingMockObject), null, null);
		}

		// SearchMode Tests

		[Test]
		public void CanSearchDependencyUp()
		{
			var parent = new Locator();
			// We're having a problem with this test intermittently failing.
			// Since the locator is a weak referencing container, our current
			// theory is that the GC is collecting this dependency before
			// the buildup call runs. By boxing it explicitly, we can keep
			// the object alive until after the test completes.
			object intValue = 25;
			parent.Add(new DependencyResolutionLocatorKey(typeof(int), null), intValue);
			var child = new Locator(parent);
			MockBuilderContext context = CreateContext(child);

			GC.Collect();
			context.HeadOfChain.BuildUp(context, typeof(SearchUpMockObject), null, null);
			GC.KeepAlive(intValue);
		}

		[Test]
		[ExpectedException(typeof(DependencyMissingException))]
		public void LocalSearchFailsIfDependencyIsOnlyUpstream()
		{
			var parent = new Locator();
			object parentValue = 25;
			parent.Add(new DependencyResolutionLocatorKey(typeof(int), null), parentValue);
			var child = new Locator(parent);
			MockBuilderContext context = CreateContext(child);

			context.HeadOfChain.BuildUp(context, typeof(SearchLocalMockObject), null, null);
		}


		[Test]
		public void LocalSearchGetsLocalIfDependencyIsAlsoUpstream()
		{
			var parent = new Locator();
			object parentValue = 25;
			object childValue = 15;
			parent.Add(new DependencyResolutionLocatorKey(typeof(int), null), parentValue);
			var child = new Locator(parent) {{new DependencyResolutionLocatorKey(typeof(int), null), childValue}};
		    MockBuilderContext context = CreateContext(child);

			var obj =
				(SearchLocalMockObject)context.HeadOfChain.BuildUp(context, typeof(SearchLocalMockObject), null, null);

			Assert.AreEqual(15, obj.Value);
		}

		// Caching

		[Test]
		public void RelectionStrategyCacheMethodReflectionInfo()
		{
			var mockReflectionStrategy = new MockMethodReflectionStrategy();

			MockBuilderContext context1 = CreateContext(mockReflectionStrategy);
			MockBuilderContext context2 = CreateContext(mockReflectionStrategy);

			int methodsCount = typeof(MockDecoratedMethodObject).GetMethods().Length;

			// building the object twice should call once to GetMembers and MemberRequiresProcessing
			context1.HeadOfChain.BuildUp(context1, typeof(MockDecoratedMethodObject), null, null);
			context2.HeadOfChain.BuildUp(context2, typeof(MockDecoratedMethodObject), null, null);

			Assert.AreEqual(1, mockReflectionStrategy.GetMembersCalledCount);
			Assert.AreEqual(methodsCount, mockReflectionStrategy.MemberRequiresProcessingCalledCount);
		}

		// Helpers

		private MockBuilderContext CreateContext()
		{
			return CreateContext(new Locator());
		}

		private MockBuilderContext CreateContext(IReadWriteLocator locator)
		{
			var result = new MockBuilderContext(locator);
			result.InnerChain.Add(new SingletonStrategy());
			result.InnerChain.Add(new MethodReflectionStrategy());
			result.InnerChain.Add(new CreationStrategy());
			result.InnerChain.Add(new MethodExecutionStrategy());
			result.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			result.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			return result;
		}

		private MockBuilderContext CreateContext(MockMethodReflectionStrategy mockReflectionStrategy)
		{
			var context = new MockBuilderContext(new Locator());
			context.InnerChain.Add(new SingletonStrategy());
			context.InnerChain.Add(mockReflectionStrategy);
			context.InnerChain.Add(new CreationStrategy());
			context.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			context.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			return context;
		}

		public class MockMethodReflectionStrategy : MethodReflectionStrategy
		{
			public int MemberRequiresProcessingCalledCount;
			public int GetMembersCalledCount;

			public MockMethodReflectionStrategy()
			{
				GetMembersCalledCount = 0;
				MemberRequiresProcessingCalledCount = 0;
			}

			protected override IEnumerable<IReflectionMemberInfo<MethodInfo>> GetMembers(IBuilderContext context,
			                                                                             Type typeToBuild, object existing,
			                                                                             string idToBuild)
			{
				GetMembersCalledCount++;
				return base.GetMembers(context, typeToBuild, existing, idToBuild);
			}

			protected override bool MemberRequiresProcessing(IReflectionMemberInfo<MethodInfo> member)
			{
				MemberRequiresProcessingCalledCount++;
				return base.MemberRequiresProcessing(member);
			}
		}

		#region Mock Classes

		public class SearchUpMockObject
		{
			public int Value;

			[InjectionMethod]
			public void SetValue(
				[Dependency(SearchMode = SearchMode.Up, NotPresentBehavior = NotPresentBehavior.Throw)] int value)
			{
				Value = value;
			}
		}

		public class SearchLocalMockObject
		{
			public int Value;

			[InjectionMethod]
			public void SetValue(
				[Dependency(SearchMode = SearchMode.Local, NotPresentBehavior = NotPresentBehavior.Throw)] int value)
			{
				Value = value;
			}
		}

		public class ThrowingMockObject
		{
			[InjectionMethod]
			public void SetValue([Dependency(NotPresentBehavior = NotPresentBehavior.Throw)] object obj)
			{
			}
		}

		public class NamedThrowingMockObject
		{
			[InjectionMethod]
			public void SetValue([Dependency(Name = "Foo", NotPresentBehavior = NotPresentBehavior.Throw)] object obj)
			{
			}
		}

		public class MockInvalidDualAttributes
		{
			[InjectionMethod]
			public void SetValue([CreateNew] [Dependency] int dummy)
			{
			}
		}

		public class MockDependsOnInterface
		{
			[InjectionMethod]
			public void DoSomething(IFoo foo)
			{
			}
		}

		public class MockDependingObject
		{
			public object InjectedObject;

			[InjectionMethod]
			public virtual void DoSomething([Dependency] object injectedObject)
			{
				InjectedObject = injectedObject;
			}
		}

		public class MockDependingObjectDerived : MockDependingObject
		{
			public override void DoSomething(object injectedObject)
			{
				base.DoSomething(injectedObject);
			}
		}

		public class MockOptionalDependingObject
		{
			public object InjectedObject;

			[InjectionMethod]
			public void SetObject([Dependency(NotPresentBehavior = NotPresentBehavior.ReturnNull)] object foo)
			{
				InjectedObject = foo;
			}
		}

		public class MockOptionalDependingObjectWithName
		{
			public object InjectedObject;

			[InjectionMethod]
			public void SetObject([Dependency(Name = "Foo", NotPresentBehavior = NotPresentBehavior.ReturnNull)] object foo)
			{
				InjectedObject = foo;
			}
		}

		public class MockDependingNamedObject
		{
			public object InjectedObject;

			[InjectionMethod]
			public void SetObject([Dependency(Name = "Foo")] object foo)
			{
				InjectedObject = foo;
			}
		}

		public class MockDependsOnIFoo
		{
			public IFoo Foo;

			[InjectionMethod]
			public void SetFoo([Dependency(CreateType = typeof(Foo))] IFoo foo)
			{
				Foo = foo;
			}
		}

		public class MockDependsOnNamedIFoo
		{
			public IFoo Foo;

			[InjectionMethod]
			public void SetFoo([Dependency(Name = "Foo", CreateType = typeof(Foo))] IFoo foo)
			{
				Foo = foo;
			}
		}

		public class MockRequiresNewObject
		{
			public object Foo;

			[InjectionMethod]
			public virtual void SetFoo([CreateNew] object foo)
			{
				Foo = foo;
			}
		}

		public class MockRequiresNewObjectDerived : MockRequiresNewObject
		{
			public override void SetFoo(object foo)
			{
				base.SetFoo(foo);
			}
		}

		public interface IFoo
		{
		}

		public class Foo : IFoo
		{
		}

		public class MockDecoratedMethodObject
		{
			[InjectionMethod]
			public void InjectionMethod([CreateNew] object foo)
			{
			}

			public void NonInjectionMethod()
			{
			}
		}

		#endregion
	}
}
