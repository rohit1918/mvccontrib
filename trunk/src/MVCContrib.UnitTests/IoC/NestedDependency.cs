namespace MvcContrib.UnitTests.IoC
{
    public class NestedDependency : INestedDependency, IDependency
    {
        private IDependency _dependency;

        public NestedDependency(IDependency dependency)
        {
            _dependency = dependency;
        }

        public IDependency Dependency
        {
            get { return _dependency; }
            set { _dependency = value; }
        }
    }
}