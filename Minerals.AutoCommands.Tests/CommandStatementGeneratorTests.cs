namespace Minerals.AutoCommands.Tests
{
    [TestClass]
    public class CommandStatementGeneratorTests : VerifyBase
    {
        public CommandStatementGeneratorTests()
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