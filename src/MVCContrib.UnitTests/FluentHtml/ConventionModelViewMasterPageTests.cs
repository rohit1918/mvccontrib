using MvcContrib.FluentHtml;
using MvcContrib.UnitTests.FluentHtml.Fakes;
using NUnit.Framework;

namespace MvcContrib.UnitTests.FluentHtml
{
	[TestFixture]
	public class ConventionModelViewMasterPageTests : ConventionViewModelContainerTestBase<ConventionModelViewMasterPage<FakeModel>, FakeModel>
	{
	}
}