using System.Web.UI.WebControls;
using MvcContrib.ViewFactories;
using NUnit.Framework;

namespace MVCContrib.UnitTests.ViewFactories
{
	[TestFixture, Category("ViewFactories")]
	public class FileSystemViewSourceLoaderTester
	{
		[Test]
		public void HasView_ReturnsFalse_For_Non_Existing_Views()
		{
			FileSystemViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader("C:\\");

			Assert.IsFalse(viewSourceLoader.HasView("D:\\MyLovelyView"));
		}

		[Test]
		public void GetViewSource_ReturnsNull_For_Non_Existing_Views()
		{
			FileSystemViewSourceLoader viewSourceLoader = new FileSystemViewSourceLoader();

			if (viewSourceLoader.ViewRootDirectory == null)
			{
				viewSourceLoader.ViewRootDirectory = "C:\\";
			}

			Assert.IsNull(viewSourceLoader.GetViewSource("D:\\MyLovelyView"));
		}
	}
}
