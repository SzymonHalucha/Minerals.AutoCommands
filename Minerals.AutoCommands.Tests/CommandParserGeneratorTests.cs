namespace Minerals.AutoCommands.Tests
{
    [TestClass]
    public class CommandParserGeneratorTests : VerifyBase
    {
        public CommandParserGeneratorTests()
        {
            var references = VerifyExtensions.GetAppReferences
            (
                typeof(object),
                typeof(CommandPipelineHandlers),
                typeof(CommandStatementAttribute),
                typeof(CommandOrderException),
                typeof(ICommandStatement)
            );
            VerifyExtensions.Initialize(references);
        }
    }
}