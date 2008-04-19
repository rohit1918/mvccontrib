using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Policy;
using System.Security.Principal;
using System.Web.Mvc;
using System.Xml;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests
{
    [TestFixture]
    public class ViewDataExtensionsForViewDataTester
    {
        [Test]
        public void ShouldRetrieveSingleObjectByType()
        {
            var bag = new Dictionary<string, object>();
            var url = new Url("/asdf"); //arbitrary object
            bag.Add(url);
            var viewData = new ViewData(bag);

            Assert.That(viewData.Get<Url>(), Is.EqualTo(url));
            Assert.That(viewData.Get(typeof(Url)), Is.EqualTo(url));
        }

        [Test, ExpectedException(ExceptionType = typeof(ArgumentException),
            ExpectedMessage = "No object exists that is of type 'System.Net.Mail.MailMessage'.")]
        public void ShouldGetObjectBasedOnType()
        {
            var url = new Url("/1");
            var identity = new GenericIdentity("name");

            var bag = new Dictionary<string, object>();
            bag.Add(identity).Add(url);
            var viewData = new ViewData(bag);

            viewData.Get(typeof(MailMessage));
        }

        [Test, ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "No object exists with key 'System.Security.Policy.Url'.")]
        public void ShouldGetMeaningfulExceptionIfObjectDoesntExist()
        {
            var bag = new Dictionary<string, object>();
            var viewData = new ViewData(bag);
            var url = viewData.Get<Url>();
        }

        [Test]
        public void ShouldReportContainsCorrectly()
        {
            var bag = new Dictionary<string, object>();
            bag.Add(new Url("/2"));
            var viewData = new ViewData(bag);

            Assert.That(viewData.Contains<Url>());
            Assert.That(viewData.Contains(typeof(Url)));
        }

        [Test]
        public void ShouldManageMoreThanOneObjectPerType()
        {
            var bag = new Dictionary<string, object>();
            bag.Add("key1", new Url("/1"));
            bag.Add("key2", new Url("/2"));
            var viewData = new ViewData(bag);

            Assert.That(viewData.Get<Url>("key1").Value, Is.EqualTo("/1"));
            Assert.That(viewData.Get<Url>("key2").Value, Is.EqualTo("/2"));
        }

        [Test, ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "No object exists with key 'foobar'.")]
        public void ShouldGetMeaningfulExceptionIfObjectDoesntExistByKey()
        {
            var bag = new Dictionary<string, object>();
            var viewData = new ViewData(bag);
            var url = viewData.Get<Url>("foobar");
        }

        [Test]
        public void ShouldBeAbleToGetADefaultValueIfTheKeyDoesntExist()
        {
            DateTime theDate = DateTime.Parse("April 04, 2005");
            DateTime defaultDate = DateTime.Parse("October 31, 2005");

            var bag = new Dictionary<string, object>();
            var viewData = new ViewData(bag);

            Assert.That(viewData.GetOrDefault("some_date", defaultDate), Is.EqualTo(defaultDate));

            bag.Add("some_date", theDate);
            var viewData2 = new ViewData(bag);
            Assert.That(viewData2.GetOrDefault("some_date", defaultDate), Is.EqualTo(theDate));
        }

        [Test]
        public void ShouldHandleProxiedObjectsByType()
        {
            var mailMessageStub = MockRepository.GenerateStub<MailMessage>();
            var bag = new Dictionary<string, object>();
            bag.Add(mailMessageStub);
            var viewData = new ViewData(bag);
            var message = viewData.Get<MailMessage>();

            Assert.That(message, Is.EqualTo(mailMessageStub));
        }

        [Test]
        public void ShouldInitializeWithProxiesAndResolveCorrectly()
        {
            var messageProxy = MockRepository.GenerateStub<MailMessage>();
            var xmlDocumentProxy = MockRepository.GenerateStub<XmlDocument>();

            var bag = new Dictionary<string, object>();
            bag.Add(messageProxy).Add(xmlDocumentProxy);
            var viewData = new ViewData(bag);

            Assert.That(viewData.Get<MailMessage>(), Is.EqualTo(messageProxy));
            Assert.That(viewData.Get<XmlDocument>(), Is.EqualTo(xmlDocumentProxy));
        }

        [Test]
        public void ShouldInitializeWithKeys()
        {
            var bag = new Dictionary<string, object>();
            bag.Add("key1", 2);
            bag.Add("key2", 3);
            var viewData = new ViewData(bag);
            Assert.That(viewData.Contains("key1"));
            Assert.That(viewData.Contains("key2"));
        }
    }
}