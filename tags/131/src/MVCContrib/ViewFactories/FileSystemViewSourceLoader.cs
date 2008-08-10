using System;
using System.ComponentModel;
using System.IO;
using System.Web;

namespace MvcContrib.ViewFactories
{
	public class FileSystemViewSourceLoader : IViewSourceLoader
	{
		private string _viewRootDirectory;
		private FileSystemWatcher _viewRootDirectoryWatcher;
		private readonly object _syncRoot = new object();
		private readonly EventHandlerList _events = new EventHandlerList();

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

		public string[] ListViews(string directoryName)
		{
			if( ViewRootDirectory == null ) return new string[0];

			var directory = new DirectoryInfo(Path.Combine(ViewRootDirectory, directoryName));

			if (directory.Exists)
			{
				return Array.ConvertAll(directory.GetFiles(), file => Path.Combine(directoryName, file.Name));
			}

			return new string[0];
		}

		private static readonly object ViewRootDirectoryChangedEvent = new object();

		public event FileSystemEventHandler ViewRootDirectoryChanged
		{
			add
			{
				lock(_syncRoot)
				{
					if( _viewRootDirectoryWatcher == null )
					{
						CreateViewRootDirectoryWatcher();
					}
					_events.AddHandler(ViewRootDirectoryChangedEvent, value);
				}
			}
			remove 
			{
				lock (_syncRoot)
				{
					_events.RemoveHandler(ViewRootDirectoryChangedEvent, value);
					Delegate handler = _events[ViewRootDirectoryChangedEvent];
					if(handler == null)
					{
						DisposeViewRootDirectoryWatcher();
					}
				}
			}
		}

		private void CreateViewRootDirectoryWatcher()
		{
			if (Directory.Exists(ViewRootDirectory))
			{
				_viewRootDirectoryWatcher = new FileSystemWatcher(ViewRootDirectory);
				_viewRootDirectoryWatcher.IncludeSubdirectories = true;
				_viewRootDirectoryWatcher.Changed += OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.Created += OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.Deleted += OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.Renamed += OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.EnableRaisingEvents = true;
			}
		}

		private void DisposeViewRootDirectoryWatcher()
		{
			if (_viewRootDirectoryWatcher != null)
			{
				_viewRootDirectoryWatcher.EnableRaisingEvents = false;
				_viewRootDirectoryWatcher.Changed -= OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.Created -= OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.Deleted -= OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.Renamed -= OnViewRootDirectoryChanged;
				_viewRootDirectoryWatcher.Dispose();
				_viewRootDirectoryWatcher = null;
			}
		}

		protected virtual void OnViewRootDirectoryChanged(object sender, FileSystemEventArgs e)
		{
			var handler = (FileSystemEventHandler)_events[ViewRootDirectoryChangedEvent];
			if (handler != null)
			{
				handler(this, e);
			}
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
