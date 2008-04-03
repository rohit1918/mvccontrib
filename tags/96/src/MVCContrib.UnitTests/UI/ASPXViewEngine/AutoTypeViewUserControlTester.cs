using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MvcContrib.UI.ASPXViewEngine;
using System.Web.Mvc;
using System.Collections;


namespace MvcContrib.UnitTests.UI.ASPXViewEngine
{
	[TestFixture]
	public class AutoTypeViewUserControlTester
	{
		private class TestViewData
		{
			public string StringValue { get; set; }
			public bool BoolValue { get; set; }
			public Uri UriValue { get; set; }
		}

		[Test]
		public void Accepts_Correct_Type_Without_Conversion()
		{
			AutoTypeViewUserControlTestingSubclass<TestViewData> viewPage = new AutoTypeViewUserControlTestingSubclass<TestViewData>();
			TestViewData data = new TestViewData();
			viewPage.SetViewData(data);
			Assert.AreSame(data, viewPage.ViewData, "ViewData was not set without conversion");
		}

		[Test]
		public void Accepts_IDictionary()
		{
			AutoTypeViewUserControlTestingSubclass<TestViewData> viewPage = new AutoTypeViewUserControlTestingSubclass<TestViewData>();

			IDictionary<string, object> data = new Dictionary<string, object>();
			Uri uriValue = new Uri("http://www.google.com/");
			data["StringValue"] = "hello";
			data["BoolValue"] = true;
			data["UriValue"] = uriValue;
			data["NonExistentValue"] = new object();

			viewPage.SetViewData(data);
			Assert.AreEqual("hello", viewPage.ViewData.StringValue);
			Assert.AreEqual(true, viewPage.ViewData.BoolValue);
			Assert.AreSame(uriValue, viewPage.ViewData.UriValue);
		}

		[Test]
		public void Accepts_Anonymous_Type()
		{
			AutoTypeViewUserControlTestingSubclass<TestViewData> viewPage = new AutoTypeViewUserControlTestingSubclass<TestViewData>();

			IDictionary<string, object> data = new Dictionary<string, object>();
			Uri uriValue = new Uri("http://www.google.com/");
			viewPage.SetViewData(new
			{
				StringValue = "nice",
				BoolValue = true,
				UriValue = uriValue,
				SomeOtherValue = new object(),
				SomeRandomNullValue = (object)null
			});

			Assert.AreEqual("nice", viewPage.ViewData.StringValue);
			Assert.AreEqual(true, viewPage.ViewData.BoolValue);
			Assert.AreSame(uriValue, viewPage.ViewData.UriValue);
		}

		[Test]
		public void Accepts_Null()
		{
			AutoTypeViewUserControlTestingSubclass<TestViewData> viewPage = new AutoTypeViewUserControlTestingSubclass<TestViewData>();
			viewPage.SetViewData(null);
			try
			{
				object data = viewPage.ViewData;
				Assert.Fail("Shouldn't be able to retrieve the ViewData from this ViewUserControl when it's NULL.");
			}
			catch (InvalidOperationException)
			{
				// The fact that this exception is thrown demonstrates that the ViewData is still NULL
				// (Can't retrieve ViewData from a ViewUserControl that can't find an IViewDataContainer.)
			}
		}

		/// <summary>
		/// Exposes the protected SetViewData() method for the purpose of testing
		/// </summary>
		private class AutoTypeViewUserControlTestingSubclass<T> : AutoTypeViewUserControl<T>
		{
			public new void SetViewData(object viewData)
			{
				base.SetViewData(viewData);
			}
		}
	}
}