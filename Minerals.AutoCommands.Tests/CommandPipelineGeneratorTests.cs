namespace Minerals.AutoCommands.Tests
{
    public class CommandPipelineGeneratorTests
    {
        [Fact]
        public Task PartialClass_ShouldGenerateCommandPipeline()
        {
            string source = TestsHelpers.MakeTestNamespace("",
            """
            [Minerals.AutoCommands.CommandPipeline]
            public partial class PipelineTestClass
            {
            }
            """);

            IIncrementalGenerator[] additional =
            [
                new CommandPipelineAttributeGenerator(),
                new CommandAttributeGenerator(),
                new ICommandPipelineGenerator(),
                new InvalidCommandGenerator(),
                new ICommandGenerator()
            ];
            return TestsHelpers.VerifyGenerator(source, new CommandPipelineGenerator(), additional);
        }
    }
}