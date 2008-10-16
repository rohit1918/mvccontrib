using System.Collections.Generic;
using System.IO;
using MvcContrib.ViewFactories;
using Spark.FileSystem;

namespace MvcContrib.SparkViewEngine
{
	public class ViewSourceLoaderAdapter : IViewFolder
	{
		private readonly IViewSourceLoaderContainer _container;

		public ViewSourceLoaderAdapter(IViewSourceLoaderContainer container)
		{
			_container = container;
		}

		public bool HasView(string path)
		{
			return _container.ViewSourceLoader.HasView(path);
		}

		public IList<string> ListViews(string path)
		{
			return _container.ViewSourceLoader.ListViews(path);
		}

		public IViewFile GetViewSource(string path)
		{
			return new ViewFile(_container.ViewSourceLoader.GetViewSource(path));
		}

		public class ViewFile : IViewFile
		{
			private readonly IViewSource _source;

			public ViewFile(IViewSource source)
			{
				_source = source;
			}

			public Stream OpenViewStream()
			{
				return _source.OpenViewStream();
			}

			public long LastModified
			{
				get { return _source.LastModified; }
			}
		}
	}
}
