/*
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using MvcContrib.Castle;
using MvcContrib.Filters;
using MvcContrib.MetaData;
using MvcContrib.Services;
using MVCContrib.UnitTests.IoC;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.MetaData
{
	[TestFixture]
	public class FilterTester
	{
		private FilteredController _controller;
		private MockRepository _mocks;

		[SetUp]
		public void Setup()
		{
			_controller = new FilteredController();
			_mocks = new MockRepository();
		}

		private void SetupHttpContext(Controller controller, string requestType)
		{
			RouteData fakeRouteData = new RouteData();
			fakeRouteData.Values.Add("Action", "Index");
			fakeRouteData.Values.Add("Controller", "Home");

			HttpContextBase context = _mocks.DynamicMock<HttpContextBase>();
			HttpRequestBase request = _mocks.DynamicMock<HttpRequestBase>();

			SetupResult.For(context.Request).Return(request);
			SetupResult.For(request.RequestType).Return(requestType);

			_mocks.Replay(context);
			_mocks.Replay(request);

			ControllerContext controllerContext = new ControllerContext(context, fakeRouteData, _controller);
			controller.ControllerContext = controllerContext;
		}


		[Test]
		public void ControllerDescriptorShouldFindFilters()
		{
			ControllerDescriptor descriptor = new ControllerDescriptor();
			ControllerMetaData metaData = descriptor.GetMetaData(_controller);
			ActionMetaData action = metaData.GetAction("MultipleFilters");

			Assert.AreEqual(3, action.Filters.Count);
			Assert.AreEqual(1, action.Filters[0].ExecutionOrder);
			Assert.AreEqual(50, action.Filters[1].ExecutionOrder);
			Assert.AreEqual(100, action.Filters[2].ExecutionOrder);
		}

		[Test]
		public void ActionShouldBeInvokedIfFilterReturnsTrue()
		{
			bool result = _controller.DoInvokeAction("SuccessfulFilter");

			Assert.IsTrue(result);
			Assert.IsTrue(_controller.SuccessfulFilterCalled);
		}

		[Test]
		public void ActionShouldNotBeInvokedIfFilterReturnsFalse()
		{
			bool result = _controller.DoInvokeAction("UnsuccessfulFilter");

			Assert.IsFalse(result);
			Assert.IsFalse(_controller.UnSuccessfulFilterCalled);
		}

		[Test]
		public void FilterThatImplementsIFilterAttributeAwareShouldReceiveAttribute()
		{
			bool result = _controller.DoInvokeAction("AttributeAwareFilter");

			Assert.IsTrue(result);
			Assert.IsTrue(_controller.AttributeAwareFilterCalled);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void InvalidFilterShouldThrow()
		{
			_controller.DoInvokeAction("InvalidFilter");
		}

		[Test]
		public void ActionShouldNotBeInvokedIfOneFilterReturnsTrueAndAnotherReturnsFalse()
		{
			bool result = _controller.DoInvokeAction("MultipleFilters");

			Assert.IsFalse(result);
			Assert.IsFalse(_controller.MultipleFiltersCalled);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void PostOnlyShouldReturnFalseIfRequestTypeIsNotPost()
		{
			SetupHttpContext(_controller, "GET");
			_controller.DoInvokeAction("PostOnly");
		}

		[Test]
		public void PostOnlyShouldReturnTrueIfRequestTypeIsPost()
		{
			SetupHttpContext(_controller, "POST");

			bool result = _controller.DoInvokeAction("PostOnly");

			Assert.IsTrue(result);
			Assert.IsTrue(_controller.PostOnlyCalled);
		}

		[Test]
		public void ShouldCreateUsingDependencyResolver()
		{
			IWindsorContainer container = new WindsorContainer();
			container.AddComponent(typeof(DependentFilter).Name, typeof(DependentFilter));
			container.AddComponent(typeof(IDependency).Name, typeof(IDependency), typeof(SimpleDependency));
			container.AddComponent(typeof(FilterReturnsTrue).Name, typeof(FilterReturnsTrue));
			container.AddComponent(typeof(FilterReturnsFalse).Name, typeof(FilterReturnsFalse));
			container.AddComponent(typeof(AttributeAwareFilter).Name, typeof(AttributeAwareFilter));
			container.AddComponent(typeof(PostOnlyFilterImpl).Name, typeof(PostOnlyFilterImpl));

			DependencyResolver.InitializeWith(new WindsorDependencyResolver(container));

			bool result = _controller.DoInvokeAction("DependentFilter");

			Assert.IsTrue(result);
			Assert.IsTrue(_controller.DependentFilterCalled);
		}

		[Test]
		public void Should_Not_Throw_If_Not_Registered_With_IoC()
		{
			IWindsorContainer container = new WindsorContainer();
			DependencyResolver.InitializeWith(new WindsorDependencyResolver(container));
			_controller.DoInvokeAction("SuccessfulFilter");
		}

		class DependentFilter : IFilter
		{
			private readonly IDependency _dependency;

			public DependentFilter(IDependency dependency)
			{
				_dependency = dependency;
			}

			public bool Execute(ControllerContext context, ActionMetaData action)
			{
				return _dependency != null && _dependency is SimpleDependency;
			}
		}

		class FilterReturnsTrue : IFilter
		{
			public bool Execute(ControllerContext context, ActionMetaData action)
			{
				return true;
			}
		}

		class FilterReturnsFalse : IFilter
		{
			public bool Execute(ControllerContext context, ActionMetaData action)
			{
				return false;
			}
		}

		[Filter(typeof(FilterReturnsTrue))]
		class FilteredController : ConventionController
		{
			public bool SuccessfulFilterCalled = false;
			public bool UnSuccessfulFilterCalled = false;
			public bool AttributeAwareFilterCalled = false;
			public bool MultipleFiltersCalled = false;
			public bool PostOnlyCalled = false;
			public bool DependentFilterCalled = false;

			public bool DoInvokeAction(string action)
			{
				return InvokeAction(action);
			}

			[Filter(typeof(FilterReturnsTrue))]
			public void SuccessfulFilter()
			{
				SuccessfulFilterCalled = true;
			}

			[Filter(typeof(FilterReturnsFalse))]
			public void UnsuccessfulFilter()
			{
				UnSuccessfulFilterCalled = true;
			}

			[Filter(typeof(AttributeAwareFilter))]
			public void AttributeAwareFilter()
			{
				AttributeAwareFilterCalled = true;
			}

			[Filter(typeof(FilterReturnsTrue), ExecutionOrder = 1)]
			[Filter(typeof(FilterReturnsFalse), ExecutionOrder = 100)]
			public void MultipleFilters()
			{
				MultipleFiltersCalled = true;
			}

			[PostOnly]
			public void PostOnly()
			{
				PostOnlyCalled = true;
			}
		}
	}
}
*/
