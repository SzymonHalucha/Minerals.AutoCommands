namespace Minerals.AutoCommands.Tests
{
    [TestClass]
    public class CommandsGeneratorTests : VerifyBase
    {
        public CommandsGeneratorTests()
        {
            var references = VerifyExtensions.GetAppReferences
            (
                typeof(object),
                typeof(CommandPipelineHandlers),
                typeof(CommandStatementAttribute),
                typeof(CommandOrderException),
                typeof(ICommandStatement),
                typeof(CommandObject)
            );
            VerifyExtensions.Initialize(references);
        }

        [TestMethod]
        public Task CommandStatement_ShouldGenerate()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands.Interfaces;
            using Minerals.AutoCommands.Attributes;

            namespace Minerals.Examples
            {
                [CommandStatement("test1")]
                public partial class TestCommand1
                {
                    public Type[] RequiredArguments { get; } = [];
                    public Type[] PossibleArguments { get; } = [];
                    public string Description { get; } = "";
                    public string Usage { get; } = "";

                    public void ShowHelp() { }
                    public bool Execute() => true;
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandsGenerator());
        }

        [TestMethod]
        public Task CommandArgument_ShouldGenerate()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands.Interfaces;
            using Minerals.AutoCommands.Attributes;

            namespace Minerals.Examples
            {
                [CommandArgument("--arg1")]
                public partial class TestArgument1
                {
                    public string[] PossibleValues { get; } = [];
                    public string Description { get; } = "";
                    public string Usage { get; } = "";
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandsGenerator());
        }
    }
}

// namespace Minerals.Examples
// {
//     public static class Program
//     {
//         public static void Main()
//         {
//             string[] args = ["test1", "--arg1", "value1"];
//             var pipeline = new CommandPipeline();
//             var command = pipeline.Evaluate(args, StringComparison.Ordinal);
//             if (command is ICommandStatement statement)
//             {
//                 statement.Execute();
//             }
//         }
//     }

//     [CommandStatement("test1")]
//     public partial class TestCommand1
//     {
//         public Type[] RequiredArguments { get; } = [];
//         public Type[] PossibleArguments { get; } = [typeof(TestArgument1)];
//         public string Description { get; } = "";
//         public string Usage { get; } = "";

//         public void ShowHelp()
//         {
//             Console.WriteLine($"{GetType().Name}: Help");
//         }

//         public bool Execute()
//         {
//             Console.WriteLine($"{GetType().Name}: Executed");
//             foreach (var item in Arguments)
//             {
//                 if (item is ICommandArgument arg)
//                 {
//                     Console.WriteLine($"{arg.GetType().Name}: {arg.Value}");
//                 }
//                 else if (item is ICommandStatement statement)
//                 {
//                     statement.Execute();
//                 }
//             }
//             return true;
//         }
//     }

//     [CommandArgument("--arg1")]
//     public partial class TestArgument1
//     {
//         public string[] PossibleValues { get; } = [];
//         public string Description { get; } = "";
//         public string Usage { get; } = "";
//     }
// }