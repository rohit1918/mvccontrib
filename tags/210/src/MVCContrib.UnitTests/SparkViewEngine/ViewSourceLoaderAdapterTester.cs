using System.IO;
using MvcContrib.SparkViewEngine;
using MvcContrib.ViewFactories;
using NUnit.Framework;
using Rhino.Mocks;
using Spark.FileSystem;

namespace MvcContrib.UnitTests.SparkViewEngine
{
	[TestFixture]
	[Category("SparkViewEngine")]
	public class ViewSourceLoaderAdapterTester
	{
		private MockRepository _mocks;

		[SetUp]
		public void Init()
		{
			_mocks = new MockRepository();
		}

		[Test]
		public void HasView_And_ListViews_Calls_Through()
		{
			var loader = _mocks.StrictMock<IViewSourceLoader>();
			loader.Expect(c => c.HasView("Hello\\World.spark")).Return(true);
			loader.Expect(c => c.HasView("Hello\\NoSuchFile.spark")).Return(false);
			loader.Expect(c => c.ListViews("Hello")).Return(new[] { "World.spark" });

			var container = _mocks.StrictMock<IViewSourceLoaderContainer>();
			container.Expect(c => c.ViewSourceLoader).Return(loader).Repeat.Times(3);

			_mocks.ReplayAll();
			IViewFolder viewFolder = new ViewSourceLoaderAdapter(container);
			Assert.IsTrue(viewFolder.HasView("Hello\\World.spark"));
			Assert.IsFalse(viewFolder.HasView("Hello\\NoSuchFile.spark"));
			var views = viewFolder.ListViews("Hello");
			Assert.AreEqual(1, views.Count);
			_mocks.VerifyAll();
		}

		[Test]
		public void GetViewSource_Calls_Through()
		{
			var stream = new MemoryStream(new byte[] { 1, 2, 3 });

			var source = _mocks.StrictMock<IViewSource>();
			source.Expect(c => c.LastModified).Return(12345);
			source.Expect(c => c.OpenViewStream()).Return(stream);

			var loader = _mocks.StrictMock<IViewSourceLoader>();
			loader.Expect(c => c.GetViewSource("Hello\\World.spark")).Return(source);

			var container = _mocks.StrictMock<IViewSourceLoaderContainer>();
			container.Expect(c => c.ViewSourceLoader).Return(loader);

			_mocks.ReplayAll();
			var viewFolder = new ViewSourceLoaderAdapter(container);
			var viewFile = viewFolder.GetViewSource("Hello\\World.spark");
			Assert.AreEqual(12345, viewFile.LastModified);
			Assert.AreSame(stream, viewFile.OpenViewStream());
			_mocks.VerifyAll();
		}
	}
}
