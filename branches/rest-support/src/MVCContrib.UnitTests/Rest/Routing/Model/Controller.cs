using System.Web.Mvc;
using MvcContrib.Rest.Routing.Attributes;

namespace MvcContrib.UnitTests.Rest.Routing.Model
{
	[SimplyRestfulRoute]
	public class AController : Controller
	{
		[ExtraListingAction(Alias = "list"), OptionalParameter(NodePosition = 1, Name = "filter")]
		public void ListByLetter()
		{

		}
	}

	[SimplyRestfulRoute, RestfulParent(ParentController = typeof(AController))]
	public class A1Controller : Controller
	{
	}

	[SimplyRestfulRoute, RestfulParent(ParentController = typeof(A1Controller))]
	public class A2Controller : Controller
	{
	}
}