namespace MVCContrib.UnitTests.IoC
{
    public interface INestedDependency
    {
        IDependency Dependency
        {
            get;
            set;
        }

    }
}