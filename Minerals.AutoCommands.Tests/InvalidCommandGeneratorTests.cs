namespace Minerals.AutoCommands.Tests
{
    public class InvalidCommandGeneratorTests
    {
        [Fact]
        public Task Class_ShouldGenerate()
        {
            return TestsHelpers.VerifyGenerator(new InvalidCommandGenerator(), []);
        }
    }
}