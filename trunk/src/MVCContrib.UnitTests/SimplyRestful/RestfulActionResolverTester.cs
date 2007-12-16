using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using MvcContrib.SimplyRestful;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.SimplyRestful
{
	[TestFixture]
	public class RestfulActionResolverTester
	{
		private MockRepository _mocks;
		private IHttpContext _httpContext;
		private IHttpRequest _httpRequest;
		private RouteData _routeData;
		private NameValueCollection _form;
		private RequestContext _requestContext;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_httpContext = _mocks.DynamicMock<IHttpContext>();
			_httpRequest = _mocks.DynamicMock<IHttpRequest>();
		}

		[Test, ExpectedException(typeof(NullReferenceException))]
		public void ResolveAction_WithNullRequest_Throws()
		{
			IRestfulActionResolver resolver = new RestfulActionResolver();

			using (_mocks.Record())
			{
				SetupResult.For(_httpContext.Request).Return(null);
				_requestContext = new RequestContext(_httpContext, new RouteData());
			}
			using (_mocks.Playback())
			{
				Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.None));
			}
		}

		[Test]
		public void ResolveAction_WithEmptyRequestHttpMethod_ReturnsNoAction()
		{
			IRestfulActionResolver resolver = new RestfulActionResolver();

			using (_mocks.Record())
			{
				GivenContext("", null);
			}
			using (_mocks.Playback())
			{
				Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.None));
			}
		}

		[Test]
		public void ResolveAction_WithNonPostRequest_ReturnsNoAction()
		{
			IRestfulActionResolver resolver = new RestfulActionResolver();
			using (_mocks.Record())
			{
				GivenContext("GET", null);
			}
			using (_mocks.Playback())
			{
				Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.None));
			}
		}

		[Test]
		public void ResolveAction_WithPostRequestAndNullForm_ReturnsNoAction()
		{
			IRestfulActionResolver resolver = new RestfulActionResolver();
			using (_mocks.Record())
			{
				GivenContext("POST", null);
			}
			using (_mocks.Playback())
			{
				Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.None));
			}
		}

		[Test]
		public void ResolveAction_WithPostRequestAndEmptyFormMethodValue_ReturnsNoAction()
		{
			IRestfulActionResolver resolver = new RestfulActionResolver();
			using (_mocks.Record())
			{
				GivenContext("POST", "");
			}
			using (_mocks.Playback())
			{
				Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.None));
			}
		}

		[Test]
		public void ResolveAction_WithPostRequestAndInvalidFormMethodValue_ReturnsNoAction()
		{
			IRestfulActionResolver resolver = new RestfulActionResolver();
			using (_mocks.Record())
			{
				GivenContext("POST", "GOOSE");
			}
			using (_mocks.Playback())
			{
				Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.None));
			}
		}

		[Test]
		public void ResolveAction_WithPostRequestAndFormMethodValuePUT_ReturnsUpdateAction()
		{
			IRestfulActionResolver resolver = new RestfulActionResolver();
			using (_mocks.Record())
			{
				GivenContext("POST", "PUT");
			}
			using (_mocks.Playback())
			{
				Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.Update));
			}
		}

		[Test]
		public void ResolveAction_WithPostRequestAndFormMethodValueDELETE_ReturnsDestroyAction()
		{
			IRestfulActionResolver resolver = new RestfulActionResolver();
			using (_mocks.Record())
			{
				GivenContext("POST", "DELETE");
			}
			using (_mocks.Playback())
			{
				Assert.That(resolver.ResolveAction(_requestContext), Is.EqualTo(RestfulAction.Destroy));
			}
		}

		private void GivenContext(string httpMethod, string formMethod)
		{
			SetupResult.For(_httpContext.Request).Return(_httpRequest);
			SetupResult.For(_httpRequest.HttpMethod).Return(httpMethod);

			_routeData = new RouteData();
			_routeData.Values.Add("controller", "testcontroller");
			_routeData.Values.Add("action", "SomeWeirdAction");
			if (formMethod != null)
			{
				_form = new NameValueCollection();
				_form.Add("_method", formMethod);
				SetupResult.For(_httpRequest.Form).Return(_form);
			}
			_requestContext = new RequestContext(_httpContext, _routeData);
		}
	}
}
