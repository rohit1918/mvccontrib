using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Profile;
using System.Web.SessionState;
using HttpSessionStateBase = System.Web.HttpSessionStateBase;

namespace MvcContrib.UnitTests.BrailViewEngine
{
	public class TestHttpContext : HttpContextBase
	{
	}

	public class TestHttpHandler : IHttpHandler
	{
		private bool _IsReusable;

		public void ProcessRequest(HttpContext context)
		{
			throw new NotImplementedException();
		}

		public bool IsReusable
		{
			get { return _IsReusable; }
			set { _IsReusable = value; }
		}
	}

	public class TestHttpRequest : HttpRequestBase
	{
	}

	public class TestHttpResponse : HttpResponseBase{

	}

	public class TestHttpServerUtility : HttpServerUtilityBase
	{
	}

	public class TestHttpSessionState :  HttpSessionStateBase
	{
	}
}
