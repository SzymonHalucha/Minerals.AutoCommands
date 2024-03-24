namespace Minerals.AutoCommands.Tests
{
    [TestClass]
    public class InvalidCommandGeneratorTests : VerifyBase
    {
        public InvalidCommandGeneratorTests()
        {
            VerifyExtensions.InitializeGlobalSettings();
        }

        [TestMethod]
        public Task Class_ShouldGenerate()
        {
            return this.VerifyIncrementalGenerators(new InvalidCommandGenerator());
        }
    }
}