namespace Minerals.AutoCommands.Tests
{
    [TestClass]
    public class CommandParserGeneratorTests : VerifyBase
    {
        public CommandParserGeneratorTests()
        {
            string[] usings =
            [
                "System",
                "System.Linq",
                "System.Text",
                "System.Threading.Tasks",
                "System.Collections.Generic",
            ];
            var references = VerifyExtensions.GetAppReferences
            (
                typeof(CommandPipeline),
                typeof(CommandStatementAttribute),
                typeof(CommandOrderException),
                typeof(ICommandStatement)
            );
            VerifyExtensions.Initialize(usings, references);
        }
    }
}