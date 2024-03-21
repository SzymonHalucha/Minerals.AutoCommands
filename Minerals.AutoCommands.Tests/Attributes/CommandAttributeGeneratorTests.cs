namespace Minerals.AutoCommands.Tests.Attributes
{
    public class CommandAttributeGeneratorTests
    {
        [Fact]
        public Task Attribute_ShouldGenerate()
        {
            return TestsHelpers.VerifyGenerator(new CommandAttributeGenerator(), []);
        }
    }
}