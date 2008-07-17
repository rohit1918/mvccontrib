using System;
namespace MvcContrib.Interfaces
{
	public interface IRescue
	{
		bool PerformRescue(Exception exception, System.Web.Mvc.ControllerContext controllerContext);
	}
}
