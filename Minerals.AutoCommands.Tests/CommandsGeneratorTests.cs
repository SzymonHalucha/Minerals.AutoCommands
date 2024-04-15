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
                typeof(CommandPipeline),
                typeof(CommandStatementAttribute),
                typeof(CommandDuplicateException),
                typeof(ICommandStatement),
                typeof(CommandObject)
            );
            VerifyExtensions.Initialize(references);
        }

        [TestMethod]
        public Task CommandValueAndStatement_ShouldGenerate()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands.Interfaces;
            using Minerals.AutoCommands.Attributes;

            namespace Minerals.Examples
            {
                [CommandStatement]
                public partial class TestCommand1
                {
                    public string[] Aliases { get; } = [ "test1" ];
                    public string Description { get; } = string.Empty;
                    public Type[] PossibleArguments { get; } = [];

                    public override bool Execute()
                    {
                        return true;
                    }
                }

                [CommandValue]
                public partial class TestValue1
                {
                    public string[] Aliases { get; } = [ "--arg1" ];
                    public string Description { get; } = string.Empty;
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandsGenerator());
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
                [CommandStatement]
                public partial class TestCommand1
                {
                    public string[] Aliases { get; } = new string[] { "test1" };
                    public string Description { get; } = string.Empty;
                    public Type[] PossibleArguments { get; } = [];

                    public override bool Execute()
                    {
                        return true;
                    }
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandsGenerator());
        }

        [TestMethod]
        public Task CommandValue_ShouldGenerate()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands.Interfaces;
            using Minerals.AutoCommands.Attributes;

            namespace Minerals.Examples
            {
                [CommandValue]
                public partial class TestValue1
                {
                    public string[] Aliases { get; } = new string[] { "--arg1" };
                    public string Description { get; } = string.Empty;
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandsGenerator());
        }

        [TestMethod]
        public Task CommandValue_ShouldGenerateExpressionCollection()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands.Interfaces;
            using Minerals.AutoCommands.Attributes;

            namespace Minerals.Examples
            {
                [CommandValue]
                public partial class TestValue1
                {
                    public string[] Aliases => [ "--arg1" ];
                    public string Description { get; } = string.Empty;
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandsGenerator());
        }

        [TestMethod]
        public Task CommandValue_ShouldGenerateGetCollection()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands.Interfaces;
            using Minerals.AutoCommands.Attributes;

            namespace Minerals.Examples
            {
                [CommandValue]
                public partial class TestValue1
                {
                    public string[] Aliases { get; } = [ "--arg1" ];
                    public string Description { get; } = string.Empty;
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandsGenerator());
        }

        [TestMethod]
        public Task CommandValue_ShouldGenerateExpressionArray()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands.Interfaces;
            using Minerals.AutoCommands.Attributes;

            namespace Minerals.Examples
            {
                [CommandValue]
                public partial class TestValue1
                {
                    public string[] Aliases => new string[] { "--arg1" };
                    public string Description { get; } = string.Empty;
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandsGenerator());
        }

        [TestMethod]
        public Task CommandValue_ShouldGenerateGetArray()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands.Interfaces;
            using Minerals.AutoCommands.Attributes;

            namespace Minerals.Examples
            {
                [CommandValue]
                public partial class TestValue1
                {
                    public string[] Aliases { get; } = new string[] { "--arg1" };
                    public string Description { get; } = string.Empty;
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandsGenerator());
        }
    }
}