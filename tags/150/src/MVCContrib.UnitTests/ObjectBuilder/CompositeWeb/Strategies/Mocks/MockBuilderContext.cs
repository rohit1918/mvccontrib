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
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeWeb.Tests.ObjectBuilder.Strategies.Mocks
{
	public class MockBuilderContext : BuilderContext
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
}