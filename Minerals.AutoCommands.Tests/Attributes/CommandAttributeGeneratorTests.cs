namespace Minerals.AutoCommands.Tests.Attributes
{
    [TestClass]
    public class CommandAttributeGeneratorTests : VerifyBase
    {
        public CommandAttributeGeneratorTests()
        {
            VerifyExtensions.InitializeGlobalSettings();
        }

        [TestMethod]
        public Task Attribute_ShouldGenerate()
        {
            return this.VerifyIncrementalGenerators(new CommandAttributeGenerator());
        }
    }
}