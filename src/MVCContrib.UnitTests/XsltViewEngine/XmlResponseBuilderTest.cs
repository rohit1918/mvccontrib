using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MVCContrib.UnitTests.XsltViewEngine.Helpers;
using MvcContrib.XsltViewEngine;
using NUnit.Framework;

namespace MVCContrib.UnitTests.XsltViewEngine
{
	[TestFixture]
	public class XmlResponseBuilderTest : ViewTestBase
	{
		private void BuildGetRequest()
		{
			Request.PhysicalApplicationPath = "http://testing/mycontroller/test";
			Identity.Name = "";
			Version version = new Version(1, 1);
			Browser.EcmaScriptVersion = version;
			Browser.Browser = "Firefox 2.0.11";
			Request.UserHostName = "::1";
			Request.RequestType = Request.HttpMethod = "GET";
			Request.QueryString["myQueryString"] = "myQueryStringValue";
		}

		[Test]
		public void AppendDataSourceToResponse_Via_XmlNode()
		{
			string xml = "<MyEntities><MyEntity><ID>1</ID><Name>MyEntityName</Name></MyEntity></MyEntities>";
			BuildGetRequest();

			XmlDocument expected = LoadXmlDocument("ResponseBuilderMessageWithStringXmlDataSource.xml");

			XmlResponseBuilder responseBuilder = new XmlResponseBuilder(HttpContext);

			responseBuilder.InitMessageStructure();
			responseBuilder.AppendDataSourceToResponse(xml);

			Assert.AreEqual(expected.OuterXml, responseBuilder.Message.OuterXml);
		}

		[Test]
		public void AppendDataSourceToResponse_Via_XmlReader()
		{
			string xml = "<MyEntities><MyEntity><ID>1</ID><Name>MyEntityName</Name></MyEntity></MyEntities>";
			BuildGetRequest();

			XmlDocument expected = LoadXmlDocument("ResponseBuilderMessageWithStringXmlDataSource.xml");

			XmlResponseBuilder responseBuilder = new XmlResponseBuilder(HttpContext);

			responseBuilder.InitMessageStructure();
			responseBuilder.AppendDataSourceToResponse(XmlReader.Create(new StringReader(xml)));

			Assert.AreEqual(expected.OuterXml, responseBuilder.Message.OuterXml);
		}

		[Test]
		public void AppendPage_For_A_GetRequest_WithPageVars()
		{
			BuildGetRequest();

			XmlDocument expected = LoadXmlDocument("ResponseBuilderMessageWithPageVars.xml");

			XmlResponseBuilder responseBuilder = new XmlResponseBuilder(HttpContext);
			Dictionary<string, string> pageVars = new Dictionary<string, string>();
			pageVars.Add("myPageVar", "pageVar");
			responseBuilder.InitMessageStructure();
			responseBuilder.AppendPage("MyPage", "http://mysite.com/mycontroller/mypage", pageVars);

			Assert.AreEqual(expected.OuterXml, responseBuilder.Message.OuterXml);
		}

		[Test]
		public void CreateNewNode()
		{
			XmlDocument expected = new XmlDocument();

			expected.LoadXml("<myElement myAttribute=\"attrValue\">myElementValue</myElement>");

			XmlResponseBuilder responseBuilder = new XmlResponseBuilder(HttpContext);
			XmlElement actual = responseBuilder.CreateNewNode("myElement", "myElementValue", "myAttribute", "attrValue");

			Assert.IsNotNull(actual);
			Assert.AreEqual(expected.DocumentElement.InnerXml, actual.InnerXml);
		}

		[Test]
		public void CreateNewNodeExtensionMethod()
		{
			XmlDocument expected = new XmlDocument();

			expected.LoadXml("<myElement myAttribute=\"attrValue\">myElementValue</myElement>");

			XmlDocument message = new XmlDocument();
			XmlElement actual = message.CreateNewNode("myElement", "myElementValue", "myAttribute", "attrValue");
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected.DocumentElement.InnerXml, actual.InnerXml);
		}

		[Test]
		public void InitMessageStructure_For_A_GetRequest()
		{
			BuildGetRequest();
			XmlDocument expected = LoadXmlDocument("ResponseBuilderMessage.xml");

			XmlResponseBuilder responseBuilder = new XmlResponseBuilder(HttpContext);

			responseBuilder.InitMessageStructure();

			Assert.AreEqual(expected.OuterXml, responseBuilder.Message.OuterXml);
		}

		[Test]
		public void InitMessageStructure_For_A_GetRequest_WithMessages()
		{
			BuildGetRequest();
			XmlDocument expected = LoadXmlDocument("ResponseBuilderMessageWithMessages.xml");

			XmlResponseBuilder responseBuilder = new XmlResponseBuilder(HttpContext);

			responseBuilder.InitMessageStructure();
			responseBuilder.AddMessage("This is the message", "INFO");
			responseBuilder.AddMessage("This is a message for a control", "INFO", "controlId");

			Assert.AreEqual(expected.OuterXml, responseBuilder.Message.OuterXml);
		}

		[Test]
		public void InitMessageStructure_For_A_PostRequest()
		{
			Request.PhysicalApplicationPath = "http://testing/mycontroller/test";
			Identity.Name = "";
			Version version = new Version(1, 1);
			Browser.EcmaScriptVersion = version;
			Browser.Browser = "Firefox 2.0.11";
			Request.UserHostName = "::1";
			Request.RequestType = Request.HttpMethod = "POST";
			Request.Form["myFormVariable"] = "myFormVariableValue";


			XmlDocument expected = LoadXmlDocument("ResponseBuilderPostMessage.xml");

			XmlResponseBuilder responseBuilder = new XmlResponseBuilder(HttpContext);

			responseBuilder.InitMessageStructure();

			Assert.AreEqual(expected.OuterXml, responseBuilder.Message.OuterXml);
		}

		[Test]
		public void InitMessageStructure_For_A_PostRequest_WithALowEcmaScriptVersion()
		{
			Request.PhysicalApplicationPath = "http://testing/mycontroller/test";
			Identity.Name = "";
			Version version = new Version(0, 4);
			Browser.EcmaScriptVersion = version;
			Browser.Browser = "Firefox 2.0.11";
			Request.UserHostName = "::1";
			Request.RequestType = Request.HttpMethod = "POST";
			Request.Form["myFormVariable"] = "myFormVariableValue";


			XmlDocument expected = LoadXmlDocument("ResponseBuilderPostMessageNoJavascript.xml");

			XmlResponseBuilder responseBuilder = new XmlResponseBuilder(HttpContext);

			responseBuilder.InitMessageStructure();

			Assert.AreEqual(expected.OuterXml, responseBuilder.Message.OuterXml);
		}

		[Test]
		public void InstantiateTest()
		{
			Request.PhysicalApplicationPath = Environment.CurrentDirectory.Replace("\\", "/");
			XmlResponseBuilder responseBuilder = new XmlResponseBuilder(HttpContext);

			Assert.IsNotNull(responseBuilder);
			Assert.AreEqual(Environment.CurrentDirectory.Replace("\\", "/"), responseBuilder.AppPath);
		}
	}
}