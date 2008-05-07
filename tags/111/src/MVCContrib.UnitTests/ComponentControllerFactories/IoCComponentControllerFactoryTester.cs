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
			this.mockRepository = new MockRepository();
			dependencyResolver = mockRepository.CreateMock<IDependencyResolver>();
			factory = new IoCComponentControllerFactory(dependencyResolver);
		}



		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorThrowsErrorWhenNullDependencyResolverIsProvided()
		{
			IComponentControllerFactory factory = new IoCComponentControllerFactory(null);
		}



		[Test]
		public void WhenCreateComponentControllerIsCalledWithGenerics()
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

		[Test]
		public void WhenCreateComponentControllerIsCalledWithType()
		{
			SampleComponentController2 samplecontroller = new SampleComponentController2(new NumberService());
			using (mockRepository.Record())
			{
				Expect.Call(dependencyResolver.GetImplementationOf(typeof(SampleComponentController2))).
					Return(samplecontroller);
				LastCall.IgnoreArguments();
			}
			SampleComponentController2 sampleController2;
			using (mockRepository.Playback())
			{
				sampleController2 = factory.CreateComponentController(typeof(SampleComponentController2)) as SampleComponentController2;
			}
			Assert.AreSame(samplecontroller,sampleController2);
		}
		[Test]
		public void DisposeWorks()
		{
			var component = factory.CreateComponentController(typeof(SampleComponentController2));
			factory.DisposeComponentController(component);
		}
		[TearDown]
		public void TearDown()
		{
			
		}
	}
}
