using System.IO;
using System.Web;

namespace MvcContrib.ViewFactories
{
	public class FileSystemViewSourceLoader : IViewSourceLoader
	{
		private string _viewRootDirectory;

		public FileSystemViewSourceLoader()
			: this(GetDefaultViewRootDirectory())
		{	
		}

		public FileSystemViewSourceLoader(string viewRootDirectory)
		{
			_viewRootDirectory = viewRootDirectory;
		}

		public virtual bool HasView(string viewPath)
		{
			return CreateFileInfo(viewPath).Exists;
		}

		protected virtual FileInfo CreateFileInfo(string viewPath)
		{
			if (Path.IsPathRooted(viewPath))
			{
				viewPath = viewPath.Substring(Path.GetPathRoot(viewPath).Length);
			}

			return new FileInfo(Path.Combine(_viewRootDirectory, viewPath));
		}

		public virtual IViewSource GetViewSource(string viewPath)
		{
			FileInfo fileInfo = CreateFileInfo(viewPath);

			if (fileInfo.Exists)
			{
				return new FileViewSource(fileInfo);
			}

			return null;
		}

		public string ViewRootDirectory
		{
			get { return _viewRootDirectory; }
			set { _viewRootDirectory = value; }
		}

		protected static string GetDefaultViewRootDirectory()
		{
			HttpContext current = HttpContext.Current;
			if( current != null )
			{
				return current.Request.MapPath("~/Views");
			}

			return null;
		}
	}
}