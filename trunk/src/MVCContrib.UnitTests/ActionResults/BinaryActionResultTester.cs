using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ActionResults;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using System;

namespace MvcContrib.UnitTests.ActionResults
{
	[TestFixture]
	public class BinaryResultTester
	{
		private MockRepository _mocks;
		private ControllerContext _controllerContext;
		private BinaryResult target;
		private readonly byte[] content = new byte[100];
		private const string contentType = "application/ms-excel";
		private const bool asAttachment = true;
		private const string filename = "test.pdf";

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			_controllerContext = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), _mocks.DynamicMock<ControllerBase>());
			target = new BinaryResult(content, contentType, asAttachment, filename);
		}

		[Test]
		public void Constructor_parameters_should_populate_corresponding_properties()
		{
			Assert.That(target.Content, Is.EqualTo(content));
			Assert.That(target.ContentType, Is.EqualTo(contentType));
			Assert.That(target.AsAttachment, Is.EqualTo(asAttachment));
			Assert.That(target.Filename, Is.EqualTo(filename));
		}

		[Test]
		public void Should_set_content_type()
		{
			target.ExecuteResult(_controllerContext);
			Assert.That(contentType, 
				Is.EqualTo(_controllerContext.HttpContext.Response.ContentType));
		}

		[Test]
		public void Response_should_write_content()
		{
			var sss = System.Net.Mime.MediaTypeNames.Application.Pdf;
			Console.WriteLine(sss);
			var response = _controllerContext.HttpContext.Response;
			using (_mocks.Record())
			{
				response.ClearHeaders();
				response.ClearContent();
				response.AppendHeader("Content-Disposition", "attachment; Filename=" + filename);
				response.AppendHeader("Content-Length", content.Length.ToString()); 
				response.BinaryWrite(content);
				response.Buffer = true;
				response.End();
			}
			target.ExecuteResult(_controllerContext);
			_mocks.VerifyAll();
		}
	}
}
