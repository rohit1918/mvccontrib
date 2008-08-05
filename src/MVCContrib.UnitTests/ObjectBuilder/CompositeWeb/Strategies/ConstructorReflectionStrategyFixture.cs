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
using ConstructorReflectionStrategy=
	Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies.ConstructorReflectionStrategy;
using CreationStrategy=Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies.CreationStrategy;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder.Strategies
{
	// These "modes" describe the classed of behavior provided by DI.
	// 1. I need a new X. Don'typeToBuild reuse any existing ones.
	// 2. I need the unnamed X. Create it if it doesn'typeToBuild exist, else return the existing one.
	// 3. I need the X named Y. Create it if it doesn'typeToBuild exist, else return the existing one.
	// 4. I want the unnamed X. Return null if it doesn'typeToBuild exist.
	// 5. I want the X named Y. Return null if it doesn'typeToBuild exist.

	[TestFixture, Category("ObjectBuilderFactory")]
	public class ConstructorReflectionStrategyFixture
	{
		// Value type creation

		[Test]
		public void CanCreateValueTypesWithConstructorInjectionStrategyInPlace()
		{
			MockBuilderContext context = CreateContext();

			Assert.AreEqual(0, context.HeadOfChain.BuildUp(context, typeof(int), null, null));
		}

		// Invalid attribute combination

		[Test]
		[ExpectedException(typeof(InvalidAttributeException))]
		public void SpecifyingMultipleConstructorsThrows()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof(MockInvalidDualConstructorAttributes), null, null);
		}

		[Test]
		[ExpectedException(typeof(InvalidAttributeException))]
		public void SpecifyingCreateNewAndDependencyThrows()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof(MockInvalidDualParameterAttributes), null, null);
		}

		// Default behavior

		[Test]
		public void DefaultBehaviorIsMode2ForUndecoratedParameter()
		{
			MockBuilderContext context = CreateContext();

			var obj1 =
				(MockUndecoratedObject)context.HeadOfChain.BuildUp(context, typeof(MockUndecoratedObject), null, null);
			var obj2 =
				(MockUndecoratedObject)context.HeadOfChain.BuildUp(context, typeof(MockUndecoratedObject), null, null);

			Assert.AreSame(obj1.Foo, obj2.Foo);
		}

		[Test]
		public void WhenSingleConstructorIsPresentDecorationIsntRequired()
		{
			MockBuilderContext context = CreateContext();

			var obj1 =
				(MockUndecoratedConstructorObject)
				context.HeadOfChain.BuildUp(context, typeof(MockUndecoratedConstructorObject), null, null);
			var obj2 =
				(MockUndecoratedConstructorObject)
				context.HeadOfChain.BuildUp(context, typeof(MockUndecoratedConstructorObject), null, null);

			Assert.IsNotNull(obj1.Foo);
			Assert.AreSame(obj1.Foo, obj2.Foo);
		}

		// Mode 1

		[Test]
		public void CreateNewAttributeAlwaysCreatesNewObject()
		{
			MockBuilderContext context = CreateContext();

			var depending1 =
				(MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, "Foo");
			var depending2 =
				(MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, "Bar");

			Assert.IsNotNull(depending1);
			Assert.IsNotNull(depending2);
			Assert.IsNotNull(depending1.Foo);
			Assert.IsNotNull(depending2.Foo);
			Assert.IsFalse(ReferenceEquals(depending1.Foo, depending2.Foo));
		}

		[Test]
		public void NamedAndUnnamedObjectsInLocatorDontGetUsedForCreateNew()
		{
			MockBuilderContext context = CreateContext();
			var unnamed = new object();
			var named = new object();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), null), unnamed);
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), "Foo"), named);

			var depending1 =
				(MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, null);
			var depending2 =
				(MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, null);

			Assert.IsFalse(depending1.Foo == unnamed);
			Assert.IsFalse(depending2.Foo == unnamed);
			Assert.IsFalse(depending1.Foo == named);
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
			MockBuilderContext context = CreateContext();

			var depending1 =
				(MockDependingObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingObject), null, null);
			var depending2 =
				(MockDependingObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingObject), null, null);

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
			MockBuilderContext context = CreateContext();

			var depending1 =
				(MockDependingNamedObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingNamedObject), null, null);
			var depending2 =
				(MockDependingNamedObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingNamedObject), null, null);

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
			object intValue = 25;
			parent.Add(new DependencyResolutionLocatorKey(typeof(int), null), intValue);
			var child = new Locator(parent);
			MockBuilderContext context = CreateContext(child);

			context.HeadOfChain.BuildUp(context, typeof(SearchUpMockObject), null, null);
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
			var child = new Locator(parent);
			child.Add(new DependencyResolutionLocatorKey(typeof(int), null), childValue);
			MockBuilderContext context = CreateContext(child);

			var obj =
				(SearchLocalMockObject)context.HeadOfChain.BuildUp(context, typeof(SearchLocalMockObject), null, null);

			Assert.AreEqual(15, obj.Value);
		}

		// Caching

		[Test]
		public void RelectionStrategyCacheConstructorReflectionInfo()
		{
			var mockReflectionStrategy = new MockConstructorReflectionStrategy();

			MockBuilderContext context1 = CreateContext(mockReflectionStrategy);
			MockBuilderContext context2 = CreateContext(mockReflectionStrategy);

			int ctorCount = typeof(MockDecoratedCtorObject).GetConstructors().Length;

			// building the object twice should call once to GetMembers and MemberRequiresProcessing
			context1.HeadOfChain.BuildUp(context1, typeof(MockDecoratedCtorObject), null, null);
			context2.HeadOfChain.BuildUp(context2, typeof(MockDecoratedCtorObject), null, null);

			Assert.AreEqual(1, mockReflectionStrategy.GetMembersCalledCount);
			Assert.AreEqual(ctorCount, mockReflectionStrategy.MemberRequiresProcessingCalledCount);
		}


		// Helpers        

		private MockBuilderContext CreateContext()
		{
			return CreateContext(new Locator());
		}

		private MockBuilderContext CreateContext(Locator locator)
		{
			var result = new MockBuilderContext(locator);
			result.InnerChain.Add(new SingletonStrategy());
			result.InnerChain.Add(new ConstructorReflectionStrategy());
			result.InnerChain.Add(new CreationStrategy());
			result.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			result.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			return result;
		}

		private MockBuilderContext CreateContext(MockConstructorReflectionStrategy mockReflectionStrategy)
		{
			var context = new MockBuilderContext(new Locator());
			context.InnerChain.Add(new SingletonStrategy());
			context.InnerChain.Add(mockReflectionStrategy);
			context.InnerChain.Add(new CreationStrategy());
			context.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			context.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			return context;
		}

		public class MockConstructorReflectionStrategy : ConstructorReflectionStrategy
		{
			public int MemberRequiresProcessingCalledCount;
			public int GetMembersCalledCount;

			protected override IEnumerable<IReflectionMemberInfo<ConstructorInfo>> GetMembers(IBuilderContext context,
			                                                                                  Type typeToBuild, object existing,
			                                                                                  string idToBuild)
			{
				GetMembersCalledCount++;
				return base.GetMembers(context, typeToBuild, existing, idToBuild);
			}

			protected override bool MemberRequiresProcessing(IReflectionMemberInfo<ConstructorInfo> member)
			{
				MemberRequiresProcessingCalledCount++;
				return base.MemberRequiresProcessing(member);
			}
		}

		public class SearchUpMockObject
		{
			public int Value;

			public SearchUpMockObject(
				[Dependency(SearchMode = SearchMode.Up, NotPresentBehavior = NotPresentBehavior.Throw)] int value)
			{
				Value = value;
			}
		}

		public class SearchLocalMockObject
		{
			public int Value;

			public SearchLocalMockObject(
				[Dependency(SearchMode = SearchMode.Local, NotPresentBehavior = NotPresentBehavior.Throw)] int value
				)
			{
				Value = value;
			}
		}

		public class ThrowingMockObject
		{
			[InjectionConstructor]
			public ThrowingMockObject([Dependency(NotPresentBehavior = NotPresentBehavior.Throw)] object foo)
			{
			}
		}

		public class NamedThrowingMockObject
		{
			[InjectionConstructor]
			public NamedThrowingMockObject([Dependency(Name = "Foo", NotPresentBehavior = NotPresentBehavior.Throw)] object foo)
			{
			}
		}

		public class MockDependingObject
		{
			private object injectedObject;

			public MockDependingObject([Dependency] object injectedObject)
			{
				this.injectedObject = injectedObject;
			}

			public virtual object InjectedObject
			{
				get { return injectedObject; }
				set { injectedObject = value; }
			}
		}

		public class MockOptionalDependingObject
		{
			public MockOptionalDependingObject
				(
				[Dependency(NotPresentBehavior = NotPresentBehavior.ReturnNull)] object injectedObject
				)
			{
				InjectedObject = injectedObject;
			}

			public object InjectedObject { get; set; }
		}

		public class MockOptionalDependingObjectWithName
		{
			public MockOptionalDependingObjectWithName
				(
				[Dependency(Name = "Foo", NotPresentBehavior = NotPresentBehavior.ReturnNull)] object injectedObject
				)
			{
				InjectedObject = injectedObject;
			}

			public object InjectedObject { get; set; }
		}

		public class MockDependingNamedObject
		{
			public MockDependingNamedObject([Dependency(Name = "Foo")] object injectedObject)
			{
				InjectedObject = injectedObject;
			}

			public object InjectedObject { get; set; }
		}

		public class MockDependsOnIFoo
		{
			public MockDependsOnIFoo([Dependency(CreateType = typeof(Foo))] IFoo foo)
			{
				Foo = foo;
			}

			public IFoo Foo { get; set; }
		}

		public class MockDependsOnNamedIFoo
		{
			public MockDependsOnNamedIFoo([Dependency(Name = "Foo", CreateType = typeof(Foo))] IFoo foo)
			{
				Foo = foo;
			}

			public IFoo Foo { get; set; }
		}

		public class MockRequiresNewObject
		{
			private object foo;

			public MockRequiresNewObject([CreateNew] object foo)
			{
				this.foo = foo;
			}

			public virtual object Foo
			{
				get { return foo; }
				set { foo = value; }
			}
		}

		public interface IFoo
		{
		}

		public class Foo : IFoo
		{
		}

		private class MockInvalidDualParameterAttributes
		{
			[InjectionConstructor]
			public MockInvalidDualParameterAttributes([CreateNew] [Dependency] object obj)
			{
			}
		}

		private class MockInvalidDualConstructorAttributes
		{
			[InjectionConstructor]
			public MockInvalidDualConstructorAttributes(object obj)
			{
			}

			[InjectionConstructor]
			public MockInvalidDualConstructorAttributes(int i)
			{
			}
		}

		private class MockUndecoratedObject
		{
			public object Foo;

			[InjectionConstructor]
			public MockUndecoratedObject(object foo)
			{
				Foo = foo;
			}
		}

		private class MockUndecoratedConstructorObject
		{
			public object Foo;

			public MockUndecoratedConstructorObject(object foo)
			{
				Foo = foo;
			}
		}

		private class MockDecoratedCtorObject
		{
			[InjectionConstructor]
			public MockDecoratedCtorObject()
			{
			}
		}
	}
}
