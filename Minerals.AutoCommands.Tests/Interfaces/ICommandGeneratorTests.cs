namespace Minerals.AutoCommands.Tests.Attributes
{
    [TestClass]
    public class ICommandGeneratorTests : VerifyBase
    {
        public ICommandGeneratorTests()
        {
            VerifyExtensions.InitializeGlobalSettings();
        }

        [TestMethod]
        public Task Interface_ShouldGenerate()
        {
            return this.VerifyIncrementalGenerators(new ICommandGenerator());
        }
    }
}