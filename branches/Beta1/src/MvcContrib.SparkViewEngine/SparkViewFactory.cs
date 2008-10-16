using MvcContrib.ViewFactories;
using Spark;

namespace MvcContrib.SparkViewEngine
{
	public class SparkViewFactory : Spark.Web.Mvc.SparkViewFactory, IViewSourceLoaderContainer
	{
		public SparkViewFactory()
			: this(null, null)
		{

		}

		public SparkViewFactory(ISparkSettings settings)
			: this(null, settings)
		{

		}

		public SparkViewFactory(IViewSourceLoader loader)
			: this(loader, null)
		{

		}

		public SparkViewFactory(IViewSourceLoader loader, ISparkSettings settings)
			: base(settings)
		{
			ViewSourceLoader = loader ?? new FileSystemViewSourceLoader();
			ViewFolder = new ViewSourceLoaderAdapter(this);
		}

		public IViewSourceLoader ViewSourceLoader { get; set; }
	}
}
