namespace Minerals.AutoCommands.Tests.Interfaces
{
    public class ICommandGeneratorTests
    {
        [Fact]
        public Task Interface_ShouldGenerate()
        {
            return TestsHelpers.VerifyGenerator(new ICommandGenerator(), []);
        }
    }
}