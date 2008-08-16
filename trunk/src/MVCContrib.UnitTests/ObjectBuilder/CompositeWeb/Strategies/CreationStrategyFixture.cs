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
using System.Reflection;
using Microsoft.Practices.ObjectBuilder;
using NUnit.Framework;
using CreationStrategy=Microsoft.Practices.CompositeWeb.ObjectBuilder.Strategies.CreationStrategy;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder.Strategies
{
	[TestFixture, Category("ObjectBuilderFactory")]
	public class CreationStrategyFixture
	{
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CreationStrategyWithNoPoliciesFails()
		{
			MockBuilderContext ctx = CreateContext();

			ctx.HeadOfChain.BuildUp(ctx, typeof(object), null, null);
		}

		[Test]
		public void CreationStrategyUsesSingletonPolicyToLocateCreatedItems()
		{
			MockBuilderContext ctx = CreateContext();
			var container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			object obj = ctx.HeadOfChain.BuildUp(ctx, typeof(object), null, null);

			Assert.AreEqual(1, container.Count);
			Assert.AreSame(obj, ctx.Locator.Get(new DependencyResolutionLocatorKey(typeof(object), null)));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void NoCreationStrategy()
		{
			MockBuilderContext ctx = CreateContext();
			var container = ctx.Locator.Get<ILifetimeContainer>();

			ctx.HeadOfChain.BuildUp(ctx, typeof(object), null, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void NoCreationStrategyWithId()
		{
			MockBuilderContext ctx = CreateContext();
			var container = ctx.Locator.Get<ILifetimeContainer>();

			ctx.HeadOfChain.BuildUp(ctx, typeof(object), null, Guid.NewGuid().ToString());
		}

		[Test]
		public void CreationStrategyOnlyLocatesItemIfSingletonPolicySetForThatType()
		{
			MockBuilderContext ctx = CreateContext();
			var container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			ctx.Policies.Set<ISingletonPolicy>(new SingletonPolicy(false), typeof(object), null);

			object obj = ctx.HeadOfChain.BuildUp(ctx, typeof(object), null, null);

			Assert.AreEqual(0, container.Count);
			Assert.IsNull(ctx.Locator.Get(new DependencyResolutionLocatorKey(typeof(object), null)));
		}

		[Test]
		public void AllCreatedDependenciesArePlacedIntoLocatorAndLifetimeContainer()
		{
			MockBuilderContext ctx = CreateContext();
			var container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			var obj = (MockDependingObject)ctx.HeadOfChain.BuildUp(ctx, typeof(MockDependingObject), null, null);

			Assert.AreEqual(2, container.Count);
			Assert.AreSame(obj, ctx.Locator.Get(new DependencyResolutionLocatorKey(typeof(MockDependingObject), null)));
			Assert.AreSame(obj.DependentObject,
			               ctx.Locator.Get(new DependencyResolutionLocatorKey(typeof(MockDependentObject), null)));
		}

		[Test]
		public void InjectedDependencyIsReusedWhenDependingObjectIsCreatedTwice()
		{
			MockBuilderContext ctx = CreateContext();
			var container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			var obj1 = (MockDependingObject)ctx.HeadOfChain.BuildUp(ctx, typeof(MockDependingObject), null, null);
			var obj2 = (MockDependingObject)ctx.HeadOfChain.BuildUp(ctx, typeof(MockDependingObject), null, null);

			Assert.AreSame(obj1.DependentObject, obj2.DependentObject);
		}

		[Test]
		public void NamedObjectsOfSameTypeAreUnique()
		{
			MockBuilderContext ctx = CreateContext();
			var container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			object obj1 = ctx.HeadOfChain.BuildUp(ctx, typeof(object), null, "Foo");
			object obj2 = ctx.HeadOfChain.BuildUp(ctx, typeof(object), null, "Bar");

			Assert.AreEqual(2, container.Count);
			Assert.IsFalse(ReferenceEquals(obj1, obj2));
		}

		[Test]
		public void CircularDependenciesCanBeResolved()
		{
			MockBuilderContext ctx = CreateContext();
			var container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			var d1 = (CircularDependency1)ctx.HeadOfChain.BuildUp(ctx, typeof(CircularDependency1), null, null);

			Assert.IsNotNull(d1);
			Assert.IsNotNull(d1.Depends2);
			Assert.IsNotNull(d1.Depends2.Depends1);
			Assert.AreSame(d1, d1.Depends2.Depends1);
			Assert.AreEqual(2, container.Count);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatingAbstractTypeThrows()
		{
			MockBuilderContext ctx = CreateContext();
			var container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			ctx.HeadOfChain.BuildUp(ctx, typeof(AbstractClass), null, null);
		}

		[Test]
		public void CanCreateValueTypes()
		{
			MockBuilderContext ctx = CreateContext();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());

			Assert.AreEqual(0, (int)ctx.HeadOfChain.BuildUp(ctx, typeof(int), null, null));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CannotCreateStrings()
		{
			MockBuilderContext ctx = CreateContext();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());

			ctx.HeadOfChain.BuildUp(ctx, typeof(string), null, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void NotFindingAMatchingConstructorThrows()
		{
			MockBuilderContext ctx = CreateContext();
			var policy = new FailingCreationPolicy();
			ctx.Policies.SetDefault<ICreationPolicy>(policy);

			ctx.HeadOfChain.BuildUp(ctx, typeof(object), null, null);
		}

		[Test]
		public void CreationStrategyWillLocateExistingObjects()
		{
			MockBuilderContext ctx = CreateContext();
			var container = ctx.Locator.Get<ILifetimeContainer>();
			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			ctx.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			var obj = new object();

			ctx.HeadOfChain.BuildUp(ctx, typeof(object), obj, null);

			Assert.AreEqual(1, container.Count);
			Assert.AreSame(obj, ctx.Locator.Get(new DependencyResolutionLocatorKey(typeof(object), null)));
		}

		[Test]
		[ExpectedException(typeof(IncompatibleTypesException))]
		public void IncompatibleTypesThrows()
		{
			MockBuilderContext ctx = CreateContext();
			var container = ctx.Locator.Get<ILifetimeContainer>();
			ConstructorInfo ci = typeof(MockObject).GetConstructor(new[] {typeof(int)});
			ICreationPolicy policy = new ConstructorPolicy(ci, new ValueParameter<string>(String.Empty));
			ctx.Policies.Set(policy, typeof(MockObject), null);

			object obj = ctx.HeadOfChain.BuildUp(ctx, typeof(MockObject), null, null);
		}

		[Test]
		public void CreationPolicyWillRecordSingletonsUsingLocalLifetimeContainerOnly()
		{
			var chain = new BuilderStrategyChain();
			chain.Add(new Practices.ObjectBuilder.CreationStrategy());

			var parentLocator = new Locator();
			var container = new LifetimeContainer();
			parentLocator.Add(typeof(ILifetimeContainer), container);

			var childLocator = new Locator(parentLocator);

			var policies = new PolicyList();
			policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));

			var ctx = new BuilderContext(chain, childLocator, policies);

			object obj = ctx.HeadOfChain.BuildUp(ctx, typeof(object), null, null);

			Assert.IsNotNull(obj);
			Assert.IsNull(childLocator.Get(new DependencyResolutionLocatorKey(typeof(object), null)));
		}

		#region Helpers

		private class MockObject
		{
			private int _foo;

			public MockObject(int foo)
			{
				_foo = foo;
			}
		}

		internal class FailingCreationPolicy : ICreationPolicy
		{
			public ConstructorInfo SelectConstructor(IBuilderContext context, Type type, string id)
			{
				return null;
			}

			public object[] GetParameters(IBuilderContext context, Type type, string id, ConstructorInfo ci)
			{
				return new object[] {};
			}
		}

		private MockBuilderContext CreateContext()
		{
			var result = new MockBuilderContext();
			result.InnerChain.Add(new SingletonStrategy());
			result.InnerChain.Add(new CreationStrategy());
			return result;
		}

		private abstract class AbstractClass
		{
			public AbstractClass()
			{
			}
		}

		private class MockDependingObject
		{
			public object DependentObject;

			public MockDependingObject(MockDependentObject obj)
			{
				DependentObject = obj;
			}
		}

		private class MockDependentObject
		{
		}

		private class CircularDependency1
		{
			public CircularDependency2 Depends2;

			public CircularDependency1(CircularDependency2 depends2)
			{
				Depends2 = depends2;
			}
		}

		private class CircularDependency2
		{
			public CircularDependency1 Depends1;

			public CircularDependency2(CircularDependency1 depends1)
			{
				Depends1 = depends1;
			}
		}

		private class MockBuilderContext : BuilderContext
		{
			public IReadWriteLocator InnerLocator;
			public BuilderStrategyChain InnerChain = new BuilderStrategyChain();
			public PolicyList InnerPolicies = new PolicyList();
			public LifetimeContainer lifetimeContainer = new LifetimeContainer();

			public MockBuilderContext()
				: this(new Locator())
			{
			}

			public MockBuilderContext(IReadWriteLocator locator)
			{
				InnerLocator = locator;
				SetLocator(InnerLocator);
				StrategyChain = InnerChain;
				SetPolicies(InnerPolicies);

				if(!Locator.Contains(typeof(ILifetimeContainer)))
					Locator.Add(typeof(ILifetimeContainer), lifetimeContainer);
			}
		}

		#endregion
	}
}
