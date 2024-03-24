namespace Minerals.AutoCommands.Tests.Attributes
{
    [TestClass]
    public class ICommandPipelineGeneratorTests : VerifyBase
    {
        public ICommandPipelineGeneratorTests()
        {
            VerifyExtensions.InitializeGlobalSettings();
        }

        [TestMethod]
        public Task Interface_ShouldGenerate()
        {
            return this.VerifyIncrementalGenerators(new ICommandPipelineGenerator());
        }
    }
}