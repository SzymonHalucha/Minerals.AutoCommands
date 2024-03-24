namespace Minerals.AutoCommands.Tests.Attributes
{
    [TestClass]
    public class CommandPipelineAttributeGeneratorTests : VerifyBase
    {
        public CommandPipelineAttributeGeneratorTests()
        {
            VerifyExtensions.InitializeGlobalSettings();
        }

        [TestMethod]
        public Task Attribute_ShouldGenerate()
        {
            return this.VerifyIncrementalGenerators(new CommandPipelineAttributeGenerator());
        }
    }
}