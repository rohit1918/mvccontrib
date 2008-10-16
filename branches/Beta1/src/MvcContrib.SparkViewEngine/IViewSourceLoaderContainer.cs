using MvcContrib.ViewFactories;

namespace MvcContrib.SparkViewEngine
{
	public interface IViewSourceLoaderContainer
	{
		IViewSourceLoader ViewSourceLoader { get; set; }
	}
}
