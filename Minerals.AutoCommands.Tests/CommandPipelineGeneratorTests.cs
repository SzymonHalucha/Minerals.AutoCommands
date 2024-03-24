namespace Minerals.AutoCommands.Tests
{
    [TestClass]
    public class CommandPipelineGeneratorTests : VerifyBase
    {
        public CommandPipelineGeneratorTests()
        {
            VerifyExtensions.InitializeGlobalSettings();
        }

        [TestMethod]
        public Task PartialClass_ShouldGenerateCommandPipeline()
        {
            const string source = """
            using Minerals.AutoCommands;

            namespace Minerals.Tests
            {
                [CommandPipeline]
                public partial class PipelineTestClass
                {
                }
            }
            """;

            IIncrementalGenerator[] additional =
            [
                new CommandPipelineAttributeGenerator(),
                new CommandAttributeGenerator(),
                new ICommandPipelineGenerator(),
                new InvalidCommandGenerator(),
                new ICommandGenerator()
            ];
            return this.VerifyIncrementalGenerators(source, new CommandPipelineGenerator(), additional);
        }
    }
}