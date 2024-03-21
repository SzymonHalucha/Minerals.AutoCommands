namespace Minerals.AutoCommands.Tests.Attributes
{
    public class CommandPipelineAttributeGeneratorTests
    {
        [Fact]
        public Task Attribute_ShouldGenerate()
        {
            return TestsHelpers.VerifyGenerator(new CommandPipelineAttributeGenerator(), []);
        }
    }
}