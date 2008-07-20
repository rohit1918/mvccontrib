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
			viewPage.SetViewData(new ViewDataDictionary(data));
			Assert.AreSame(data, viewPage.ViewData.Model, "ViewData was not set without conversion");
		}

		[Test]
		public void Accepts_IDictionary()
		{
			AutoTypeViewUserControlTestingSubclass<TestViewData> viewPage = new AutoTypeViewUserControlTestingSubclass<TestViewData>();

			var data = new ViewDataDictionary();
			Uri uriValue = new Uri("http://www.google.com/");
			data["StringValue"] = "hello";
			data["BoolValue"] = true;
			data["UriValue"] = uriValue;
			data["NonExistentValue"] = new object();

			viewPage.SetViewData(data);
			Assert.AreEqual("hello", viewPage.ViewData.Model.StringValue);
			Assert.AreEqual(true, viewPage.ViewData.Model.BoolValue);
			Assert.AreSame(uriValue, viewPage.ViewData.Model.UriValue);
		}

		[Test]
		public void Accepts_Anonymous_Type()
		{
			AutoTypeViewUserControlTestingSubclass<TestViewData> viewPage = new AutoTypeViewUserControlTestingSubclass<TestViewData>();

			Uri uriValue = new Uri("http://www.google.com/");
			viewPage.SetViewData(new ViewDataDictionary(new
			{
				StringValue = "nice",
				BoolValue = true,
				UriValue = uriValue,
				SomeOtherValue = new object(),
				SomeRandomNullValue = (object)null
			}));

			Assert.AreEqual("nice", viewPage.ViewData.Model.StringValue);
			Assert.AreEqual(true, viewPage.ViewData.Model.BoolValue);
			Assert.AreSame(uriValue, viewPage.ViewData.Model.UriValue);
		}

		/// <summary>
		/// Exposes the protected SetViewData() method for the purpose of testing
		/// </summary>
		private class AutoTypeViewUserControlTestingSubclass<T> : AutoTypeViewUserControl<T> where T : class 
		{
			public new void SetViewData(ViewDataDictionary viewData)
			{
				base.SetViewData(viewData);
			}
		}
	}
}