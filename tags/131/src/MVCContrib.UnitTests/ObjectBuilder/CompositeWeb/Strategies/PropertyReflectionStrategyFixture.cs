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
using PropertyReflectionStrategy=Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies.PropertyReflectionStrategy;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder.Strategies
{
	// These "modes" describe the classed of behavior provided by DI.
	// 1. I need a new X. Don'typeToBuild reuse any existing ones.
	// 2. I need the unnamed X. Create it if it doesn't exist, else return the existing one.
	// 3. I need the X named Y. Create it if it doesn't exist, else return the existing one.
	// 4. I want the unnamed X. Return null if it doesn't exist.
	// 5. I want the X named Y. Return null if it doesn't exist.

	[TestFixture, Category("ObjectBuilderFactory")]
	public class PropertyReflectionStrategyFixture
	{
		// Invalid attribute combination

		[Test]
		[ExpectedException(typeof(InvalidAttributeException))]
		public void SpecifyingCreateNewAndDependencyThrows()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof(MockInvalidDualAttributes), null, null);
		}

		// Existing policy

		[Test]
		public void PropertyReflectionWillNotOverwriteAPreExistingPolicyForAProperty()
		{
			MockBuilderContext context = CreateContext();
			var policy = new PropertySetterPolicy();
			policy.Properties.Add("Foo", new PropertySetterInfo("Foo", new ValueParameter(typeof(object), 12)));
			context.Policies.Set<IPropertySetterPolicy>(policy, typeof(MockRequiresNewObject), null);

			var obj =
				(MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, null);

			Assert.AreEqual(12, obj.Foo);
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
			MockBuilderContext context;

			context = CreateContext();
			var depending1 =
				(MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, null);

			context = CreateContext();
			var depending2 =
				(MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, null);

			Assert.IsNotNull(depending1);
			Assert.IsNotNull(depending2);
			Assert.IsNotNull(depending1.Foo);
			Assert.IsNotNull(depending2.Foo);
			Assert.IsFalse(depending1.Foo == depending2.Foo);
		}

		[Test]
		public void NamedAndUnnamedObjectsInLocatorDontGetUsedForCreateNew()
		{
			MockBuilderContext context;
			var unnamed = new object();
			var named = new object();

			context = CreateContext();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), null), unnamed);
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), "Foo"), named);
			var depending1 =
				(MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, null);

			context = CreateContext();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), null), unnamed);
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), "Foo"), named);
			var depending2 =
				(MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, null);

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
			MockBuilderContext context;

			context = CreateContext();
			var depending1 =
				(MockDependingObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingObject), null, null);

			context = CreateContext(context.Locator);
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
			MockBuilderContext context;

			context = CreateContext();
			var depending1 =
				(MockDependingNamedObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingNamedObject), null, null);

			context = CreateContext(context.Locator);
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
			GC.Collect();
			context.HeadOfChain.BuildUp(context, typeof(SearchLocalMockObject), null, null);
			GC.KeepAlive(parentValue);
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
			GC.Collect();
			var obj =
				(SearchLocalMockObject)context.HeadOfChain.BuildUp(context, typeof(SearchLocalMockObject), null, null);

			Assert.AreEqual(15, obj.Value);
			GC.KeepAlive(parentValue);
			GC.KeepAlive(childValue);
		}

		// Caching

		[Test]
		public void RelectionStrategyCachePropertyReflectionInfo()
		{
			var mockReflectionStrategy = new MockPropertyReflectionStrategy();

			MockBuilderContext context1 = CreateContext(mockReflectionStrategy);
			MockBuilderContext context2 = CreateContext(mockReflectionStrategy);

			int propCount = typeof(MockDecoratedPropertyObject).GetProperties().Length;

			// building the object twice should call once to GetMembers and MemberRequiresProcessing
			context1.HeadOfChain.BuildUp(context1, typeof(MockDecoratedPropertyObject), null, null);
			context2.HeadOfChain.BuildUp(context2, typeof(MockDecoratedPropertyObject), null, null);

			Assert.AreEqual(1, mockReflectionStrategy.GetMembersCalledCount);
			Assert.AreEqual(propCount, mockReflectionStrategy.MemberRequiresProcessingCalledCount);
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
			result.InnerChain.Add(new PropertyReflectionStrategy());
			result.InnerChain.Add(new CreationStrategy());
			result.InnerChain.Add(new PropertySetterStrategy());
			result.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			result.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			return result;
		}

		private MockBuilderContext CreateContext(MockPropertyReflectionStrategy mockReflectionStrategy)
		{
			var context = new MockBuilderContext(new Locator());
			context.InnerChain.Add(new SingletonStrategy());
			context.InnerChain.Add(mockReflectionStrategy);
			context.InnerChain.Add(new CreationStrategy());
			context.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			context.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			return context;
		}

		public class MockPropertyReflectionStrategy : PropertyReflectionStrategy
		{
			public int MemberRequiresProcessingCalledCount;
			public int GetMembersCalledCount;

			public MockPropertyReflectionStrategy()
			{
				GetMembersCalledCount = 0;
				MemberRequiresProcessingCalledCount = 0;
			}

			protected override IEnumerable<IReflectionMemberInfo<PropertyInfo>> GetMembers(IBuilderContext context,
			                                                                               Type typeToBuild, object existing,
			                                                                               string idToBuild)
			{
				GetMembersCalledCount++;
				return base.GetMembers(context, typeToBuild, existing, idToBuild);
			}

			protected override bool MemberRequiresProcessing(IReflectionMemberInfo<PropertyInfo> member)
			{
				MemberRequiresProcessingCalledCount++;
				return base.MemberRequiresProcessing(member);
			}
		}

		#region Mock Classes

		public class SearchUpMockObject
		{
			[Dependency(SearchMode = SearchMode.Up, NotPresentBehavior = NotPresentBehavior.Throw)]
			public int Value { get; set; }
		}

		public class SearchLocalMockObject
		{
			[Dependency(SearchMode = SearchMode.Local, NotPresentBehavior = NotPresentBehavior.Throw)]
			public int Value { get; set; }
		}

		public class ThrowingMockObject
		{
			[Dependency(NotPresentBehavior = NotPresentBehavior.Throw)]
			public object InjectedObject
			{
				set { }
			}
		}

		public class NamedThrowingMockObject
		{
			[Dependency(Name = "Foo", NotPresentBehavior = NotPresentBehavior.Throw)]
			public object InjectedObject
			{
				set { }
			}
		}

		public class MockInvalidDualAttributes
		{
			[CreateNew]
			[Dependency]
			public int Value { get; set; }
		}

		private interface ISomeInterface
		{
		}

		private class MockDependsOnInterface
		{
			[Dependency]
			public ISomeInterface Value { get; set; }
		}

		public class MockDependingObject
		{
			[Dependency]
			public virtual object InjectedObject { get; set; }
		}

		public class MockDependingObjectDerived : MockDependingObject
		{
			public override object InjectedObject
			{
				get { return base.InjectedObject; }
				set { base.InjectedObject = value; }
			}
		}

		public class MockOptionalDependingObject
		{
			[Dependency(NotPresentBehavior = NotPresentBehavior.ReturnNull)]
			public object InjectedObject { get; set; }
		}

		public class MockOptionalDependingObjectWithName
		{
			[Dependency(Name = "Foo", NotPresentBehavior = NotPresentBehavior.ReturnNull)]
			public object InjectedObject { get; set; }
		}

		public class MockDependingNamedObject
		{
			[Dependency(Name = "Foo")]
			public object InjectedObject { get; set; }
		}

		public class MockDependsOnIFoo
		{
			[Dependency(CreateType = typeof(Foo))]
			public IFoo Foo { get; set; }
		}

		public class MockDependsOnNamedIFoo
		{
			[Dependency(Name = "Foo", CreateType = typeof(Foo))]
			public IFoo Foo { get; set; }
		}

		public class MockRequiresNewObject
		{
			[CreateNew]
			public virtual object Foo { get; set; }
		}

		public class MockRequiresNewObjectDerived : MockRequiresNewObject
		{
			public override object Foo
			{
				get { return base.Foo; }
				set { base.Foo = value; }
			}
		}

		public interface IFoo
		{
		}

		public class Foo : IFoo
		{
		}

		private class MockDecoratedPropertyObject
		{
			[Dependency]
			public string Foo { get; set; }

			public int MyProperty { get; set; }
		}

		#endregion
	}
}
