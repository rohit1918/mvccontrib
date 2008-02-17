using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using MvcContrib.UiHelpers;

namespace MVCContrib.UnitTests.UIHelpers
{
    [TestFixture]
    public class TextInputTests
    {
        private HtmlHelper _helper;

        [SetUp]
        public void setup()
        {
            MockRepository mocks = new MockRepository();
            IHttpContext httpContext = mocks.CreateMock<IHttpContext>();
            IController controller = mocks.CreateMock<IController>();
            _helper =
                new HtmlHelper(new ViewContext(httpContext, new RouteData(), controller, new object(),
                                               new TempDataDictionary(httpContext)));
        }

        [Test]
        public void can_render_text_input()
        {
            Assert.AreEqual("<input type=\"text\" />", _helper.TextInput());
        }

        [Test]
        public void can_render_text_input_with_attributes()
        {
            Assert.AreEqual("<input type=\"text\" size=\"20\" maxLength=\"30\" id=\"id\" />",
                            _helper.TextInput(new {id = "id", size = 20, maxLength = 30}));
        }

        [Test]
        public void can_render_text_input_with_null_attributes()
        {
            Assert.AreEqual("<input type=\"text\" />", _helper.TextInput(null));
        }

        [Test]
        public void can_render_text_input_with_reserved_csharp_attribute_names()
        {
            Assert.AreEqual("<input type=\"text\" class=\"class\" id=\"id\" />",
                            _helper.TextInput(new {id = "id", _class = "class"}));
        }
    }
}