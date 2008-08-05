using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ControllerFactories
{
    [TestFixture]
    public abstract class SpecBase
    {
        protected MockRepository _mocks;
        [SetUp]
        public void Setup()
        {
            _mocks= new MockRepository();
            BeforeEachSpec();
        }
        protected IDisposable Record()
        {
            return _mocks.Record();
        }
        protected IDisposable Playback()
        {
            return _mocks.Playback();
        }
        protected abstract void BeforeEachSpec();

        [TearDown]
        public void Teardown()
        {
            //_mocks.VerifyAll();
            AfterEachSpec();
            _mocks = null;
        }

        protected abstract void AfterEachSpec();
    }
}
