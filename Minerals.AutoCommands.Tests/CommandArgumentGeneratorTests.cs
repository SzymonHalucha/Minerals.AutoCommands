namespace Minerals.AutoCommands.Tests
{
    [TestClass]
    public class CommandArgumentGeneratorTests : VerifyBase
    {
        public CommandArgumentGeneratorTests()
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