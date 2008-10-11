using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class EmailTemplateServiceTest
	{
		private MockRepository _mocks;
		private HttpResponseBase _responseMock;
		private IViewEngine _viewEngineMock;

		private ViewContext _viewContext;
		private EmailTemplateService _service;

		private delegate void RenderViewDelegate(ViewContext context, TextWriter writer);

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_responseMock = _mocks.DynamicMock<HttpResponseBase>();
			_viewEngineMock = _mocks.DynamicMock<IViewEngine>();

			_viewContext = SetupViewContext();
			_service = new EmailTemplateService(_viewEngineMock);
		}

		private ViewContext SetupViewContext()
		{
			var httpContext = _mocks.DynamicMock<HttpContextBase>();
			_responseMock = _mocks.DynamicMock<HttpResponseBase>();
			SetupResult.For(httpContext.Response).Return(_responseMock);
			var requestContext = new RequestContext(httpContext, new RouteData());

			var controller = _mocks.Stub<ControllerBase>();
			var controllerContext = new ControllerContext(requestContext, controller);

			_mocks.Replay(httpContext);

			return new ViewContext(controllerContext, "index", new ViewDataDictionary(), new TempDataDictionary());
		}

		private void WriteToStream(Stream stream, string content)
		{
			var writer = new StreamWriter(stream, Encoding.UTF8);
			writer.Write(content);
			writer.Flush();
		}

		#region Message Rendering

		[Test]
		public void CanRenderMessage()
		{
			string messageBody = "test message body..." + Environment.NewLine;

			using(_mocks.Record())
			{
				SetupResult.For(_responseMock.Filter).PropertyBehavior();
				TextWriter writer = new StringWriter();
				SetupResult.For(_responseMock.Output).Return(writer);
				Expect.Call(_responseMock.ContentEncoding).Return(Encoding.UTF8);
				Expect.Call(() => _responseMock.Flush()).Repeat.Twice();

				var fakeView = _mocks.DynamicMock<IView>();
				fakeView.Expect(x => x.Render(_viewContext, writer)).Do(
					new RenderViewDelegate((context, stream) => WriteToStream(_responseMock.Filter, messageBody)));

				Expect.Call(_viewEngineMock.FindView(_viewContext.Controller.ControllerContext, "index", null)).Return(
					new ViewEngineResult(fakeView));
			}

			MailMessage message;
			using(_mocks.Playback())
			{
				message = _service.RenderMessage(_viewContext);
			}

			Assert.IsNotNull(message);
			Assert.AreEqual(messageBody, message.Body);
			Assert.IsFalse(message.IsBodyHtml);
		}

		[Test]
		public void CanRenderHtmlMessage()
		{
			string messageBody = "<html> <body> <p><b>test</b> message body...</p></body></html>" + Environment.NewLine;

			using(_mocks.Record())
			{
				SetupResult.For(_responseMock.Filter).PropertyBehavior();
				SetupResult.For(_responseMock.ContentEncoding).Return(Encoding.UTF8);
				TextWriter writer = new StringWriter();
				SetupResult.For(_responseMock.Output).Return(writer);

				var fakeView = _mocks.DynamicMock<IView>();
				fakeView.Expect(x => x.Render(_viewContext, writer)).Do(
					new RenderViewDelegate((context, stream) => WriteToStream(_responseMock.Filter, messageBody)));

				Expect.Call(_viewEngineMock.FindView(_viewContext.Controller.ControllerContext, "index", null)).Return(
					new ViewEngineResult(fakeView));
			}

			MailMessage message;
			using(_mocks.Playback())
			{
				message = _service.RenderMessage(_viewContext);
			}

			Assert.AreEqual(messageBody, message.Body);
			Assert.IsTrue(message.IsBodyHtml);
		}

		[Test]
		public void CanPreserveResponseFilter()
		{
			var streamStub = _mocks.Stub<Stream>();

			using(_mocks.Record())
			{
				SetupResult.For(_responseMock.Filter).PropertyBehavior();
				SetupResult.For(_responseMock.ContentEncoding).Return(Encoding.UTF8);

				var fakeView = _mocks.DynamicMock<IView>();
				Expect.Call(_viewEngineMock.FindView(_viewContext.Controller.ControllerContext, "index", null)).Return(
					new ViewEngineResult(fakeView));
			}

			_responseMock.Filter = streamStub;

			using(_mocks.Playback())
			{
				_service.RenderMessage(_viewContext);
			}

			//make sure the response filter we set is still there
			Assert.AreEqual(streamStub.GetHashCode(), _responseMock.Filter.GetHashCode());
		}

		[Test]
		public void CanPreserveReponseFilterOnException()
		{
			var streamStub = _mocks.Stub<Stream>();

			using(_mocks.Record())
			{
				SetupResult.For(_responseMock.Filter).PropertyBehavior();
				SetupResult.For(_responseMock.ContentEncoding).Return(Encoding.UTF8);
				TextWriter writer = new StringWriter();
				SetupResult.For(_responseMock.Output).Return(writer);

				var fakeView = _mocks.DynamicMock<IView>();
				fakeView.Expect(x => x.Render(_viewContext, writer)).Throw(new Exception());

				Expect.Call(_viewEngineMock.FindView(_viewContext.Controller.ControllerContext, "index", null)).Return(
					new ViewEngineResult(fakeView));
			}

			_responseMock.Filter = streamStub;

			using(_mocks.Playback())
			{
				try
				{
					_service.RenderMessage(_viewContext);
				}
				catch
				{
				}
			}

			//make sure the response filter we set is still there
			Assert.AreEqual(streamStub.GetHashCode(), _responseMock.Filter.GetHashCode());
		}

		#endregion

		#region Message Header Processing

		private MailMessage CanProcessMessageHeaders(string header, string value)
		{
			string messageBody = String.Format("{0}: {1}{2}test message body...", header, value, Environment.NewLine);

			using(_mocks.Record())
			{
				SetupResult.For(_responseMock.Filter).PropertyBehavior();
				SetupResult.For(_responseMock.ContentEncoding).Return(Encoding.UTF8);
				var writer = new StringWriter();
				SetupResult.For(_responseMock.Output).Return(writer);

				var fakeView = _mocks.DynamicMock<IView>();
				fakeView.Expect(x => x.Render(_viewContext, writer)).Do(
					new RenderViewDelegate((context, stream) => WriteToStream(_responseMock.Filter, messageBody)));

				Expect.Call(_viewEngineMock.FindView(_viewContext.Controller.ControllerContext, "index", null)).Return(
					new ViewEngineResult(fakeView));
			}

			MailMessage message;
			using(_mocks.Playback())
			{
				message = _service.RenderMessage(_viewContext);
			}

			return message;
		}

		[Test]
		public void CanProcessSubjectHeader()
		{
			MailMessage message = CanProcessMessageHeaders("subject", "test-subject");
			Assert.AreEqual("test-subject", message.Subject);
		}

		[Test]
		public void CanProcessToHeader()
		{
			MailMessage message = CanProcessMessageHeaders("to", "test@test.com");
			Assert.AreEqual("test@test.com", message.To[0].Address);
		}

		[Test]
		public void CanProcessFromHeader()
		{
			MailMessage message = CanProcessMessageHeaders("from", "test@test.com");
			Assert.AreEqual("test@test.com", message.From.Address);
		}

		[Test]
		public void CanProcessCcHeader()
		{
			MailMessage message = CanProcessMessageHeaders("cc", "test@test.com");
			Assert.AreEqual("test@test.com", message.CC[0].Address);
		}

		[Test]
		public void CanProcessBccHeader()
		{
			MailMessage message = CanProcessMessageHeaders("bcc", "test@test.com");
			Assert.AreEqual("test@test.com", message.Bcc[0].Address);
		}

		[Test]
		public void CanProcessReplyToHeader()
		{
			MailMessage message = CanProcessMessageHeaders("Reply-To", "test@test.com");
			Assert.AreEqual("test@test.com", message.ReplyTo.Address);
		}

		[Test]
		public void CanProcessGenericHeader()
		{
			MailMessage message = CanProcessMessageHeaders("X-Spam", "no");
			Assert.AreEqual("no", message.Headers["X-Spam"]);
		}

		[Test]
		public void CanProcessToHeaderWithDisplayName()
		{
			MailMessage message = CanProcessMessageHeaders("to", "test@test.com (John Doe)");
			Assert.AreEqual("test@test.com", message.To[0].Address);
			Assert.AreEqual("John Doe", message.To[0].DisplayName);
		}

		[Test]
		public void CanProcessFromHeaderWithDisplayName()
		{
			MailMessage message = CanProcessMessageHeaders("from", "test@test.com (John Doe)");
			Assert.AreEqual("test@test.com", message.From.Address);
			Assert.AreEqual("John Doe", message.From.DisplayName);
		}

		[Test]
		public void CanProcessCcHeaderWithDisplayName()
		{
			MailMessage message = CanProcessMessageHeaders("cc", "test@test.com (John Doe)");
			Assert.AreEqual("test@test.com", message.CC[0].Address);
			Assert.AreEqual("John Doe", message.CC[0].DisplayName);
		}

		[Test]
		public void CanProcessBccHeaderWithDisplayName()
		{
			MailMessage message = CanProcessMessageHeaders("bcc", "test@test.com (John Doe)");
			Assert.AreEqual("test@test.com", message.Bcc[0].Address);
			Assert.AreEqual("John Doe", message.Bcc[0].DisplayName);
		}

		[Test]
		public void CanProcessReplyToHeaderWithDisplayName()
		{
			MailMessage message = CanProcessMessageHeaders("Reply-To", "test@test.com (John Doe)");
			Assert.AreEqual("test@test.com", message.ReplyTo.Address);
			Assert.AreEqual("John Doe", message.ReplyTo.DisplayName);
		}

		#endregion
	}
}