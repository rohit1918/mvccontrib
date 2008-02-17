using System.Web.Mvc;
using MvcContrib.MetaData;

namespace MvcContrib.Filters
{
	public interface IFilter
	{
		bool Execute(ControllerContext context, ActionMetaData action);
	}
}
