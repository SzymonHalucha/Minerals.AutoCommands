namespace Minerals.AutoCommands.Tests
{
    [TestClass]
    public class CommandPipelineGeneratorTests : VerifyBase
    {
        public CommandPipelineGeneratorTests()
        {
            var references = VerifyExtensions.GetAppReferences
            (
                typeof(object),
                typeof(CommandPipelineGenerator),
                typeof(Assembly)
            );
            VerifyExtensions.Initialize(references);
        }

        [TestMethod]
        public Task Class_ShouldGenerate()
        {
            return this.VerifyIncrementalGenerators(new CommandPipelineGenerator());
        }
    }
}