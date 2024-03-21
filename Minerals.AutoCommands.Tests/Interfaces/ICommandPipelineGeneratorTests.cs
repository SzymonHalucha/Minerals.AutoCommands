namespace Minerals.AutoCommands.Tests.Interfaces
{
    public class ICommandPipelineGeneratorTests
    {
        [Fact]
        public Task Interface_ShouldGenerate()
        {
            return TestsHelpers.VerifyGenerator(new ICommandPipelineGenerator(), []);
        }
    }
}