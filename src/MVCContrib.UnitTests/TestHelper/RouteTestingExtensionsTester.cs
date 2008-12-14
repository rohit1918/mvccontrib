using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.TestHelper;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MvcContrib.UnitTests.TestHelper
{
    [TestFixture]
    public class RouteTestingExtensionsTester
    {
        public class FunkyController : Controller
        {
            public ActionResult Index()
            {
                return null;
            }

            public ActionResult Bar(string id)
            {
                return null;
            }

            public ActionResult New()
            {
                return null;
            }
        }

        public class AwesomeController : Controller
        {
        }

        [SetUp]
        public void Setup()
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.MapRoute(
                "default",
                "{controller}/{action}/{id}", 
                new { controller = "Funky", Action = "Index", id ="" });
        }

        [TearDown]
        public void TearDown()
        {
            RouteTable.Routes.Clear();
        }

        [Test]
        public void should_be_able_to_pull_routedata_from_a_string()
        {
            var routeData = "~/charlie/brown".Route();
            Assert.That(routeData, Is.Not.Null);

            Assert.That(routeData.Values.ContainsValue("charlie"));
            Assert.That(routeData.Values.ContainsValue("brown"));
        }

        [Test]
        public void should_be_able_to_match_controller_from_route_data()
        {
            "~/".Route().ShouldMapTo<FunkyController>();            
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void should_be_able_to_detect_when_a_controller_doesnt_match()
        {
            "~/".Route().ShouldMapTo<AwesomeController>();            
        }

        [Test]
        public void should_be_able_to_match_action_with_lambda()
        {
            "~/".Route().ShouldMapTo<FunkyController>(x => x.Index());
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void should_be_able_to_detect_an_incorrect_action()
        {
            "~/".Route().ShouldMapTo<FunkyController>(x=>x.New());
        }

        [Test]
        public void should_be_able_to_match_action_parameters()
        {
            "~/funky/bar/widget".Route().ShouldMapTo<FunkyController>(x => x.Bar("widget"));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void should_be_able_to_detect_invalid_action_parameters()
        {
            "~/funky/bar/widget".Route().ShouldMapTo<FunkyController>(x => x.Bar("something_else"));
        }

        [Test]
        public void should_be_able_to_test_routes_directly_from_a_string()
        {
            "~/funky/bar/widget".ShouldMapTo<FunkyController>(x => x.Bar("widget"));
        }
    }
}