using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcContrib.ExtendedComponentController;
using MvcContrib.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ComponentControllerFactories
{
	[TestFixture]
	public class IoCComponentControllerFactoryTester
	{
		private IComponentControllerFactory factory;
		private IDependencyResolver dependencyResolver;
		private MockRepository mockRepository;
		
		[SetUp]
		public void SetUp()
		{
			
		}

		[TestFixtureSetUp]
		public void SetUpFixture()
		{
			this.mockRepository = new MockRepository();
			dependencyResolver = mockRepository.CreateMock<IDependencyResolver>();
			factory = new IoCComponentControllerFactory(dependencyResolver);

		}

		[Test]
		public void WhenCreateComponentControllerIsCalled()
		{
			using(mockRepository.Record())
			{
				Expect.Call(dependencyResolver.GetImplementationOf<SampleComponentController2>()).
					Return(
					new SampleComponentController2(new NumberService())
					);
			}
			SampleComponentController2 sampleController2;
			using (mockRepository.Playback())
			{
				sampleController2 = factory.CreateComponentController<SampleComponentController2>() as SampleComponentController2;
			}
			Assert.IsNotNull(sampleController2);
		}

		[TearDown]
		public void TearDown()
		{
			
		}
	}
}
