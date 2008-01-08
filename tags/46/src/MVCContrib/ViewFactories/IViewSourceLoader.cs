using System;
using System.IO;
using System.Web;

namespace MvcContrib.ViewFactories
{
	public interface IViewSourceLoader
	{
		bool HasView(string viewPath);

		IViewSource GetViewSource(string viewPath);

		string ViewRootDirectory { get; set; }
	}

	public interface IViewSource
	{
		Stream OpenViewStream();

		string FullName { get; }

		long LastModified { get; }
	}
}
