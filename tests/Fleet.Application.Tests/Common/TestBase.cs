namespace Fleet.Application.Tests.Common;

public abstract class TestBase
{
    protected readonly Fixture Fixture;

    protected TestBase()
    {
        Fixture = new Fixture();
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}
