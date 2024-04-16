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
                typeof(CommandDuplicateException),
                typeof(ICommandStatement),
                typeof(CommandPipeline),
                typeof(CommandStatementObject)
            );
            VerifyExtensions.Initialize(references);
        }

        [TestMethod]
        public Task CommandStatement_ShouldNotGenerateInParser()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands;

            namespace Minerals.Examples
            {
                public class TestCommand1
                {
                    public string[] Aliases { get; } = ["test1"];
                    public string Description { get; } = "Lorem ipsum sit dolor amet 1";
                    public Type[] PossibleArguments { get; } = [];

                    public void Execute() { }
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandParserGenerator());
        }

        [TestMethod]
        public Task CommandStatementSingleAlias_ShouldGenerateInParser()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands;

            namespace Minerals.Examples
            {
                public class TestCommand1 : CommandStatement
                {
                    public override string[] Aliases { get; } = ["test1"];
                    public override string Description { get; } = "Lorem ipsum sit dolor amet 1";
                    public override Type[] PossibleArguments { get; } = [];

                    public override void Execute() { }
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandParserGenerator());
        }

        [TestMethod]
        public Task CommandStatementMultiAlias_ShouldGenerateInParser()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands;

            namespace Minerals.Examples
            {
                public class TestCommand1 : CommandStatement
                {
                    public override string[] Aliases { get; } = ["test1", "test2", "test3"];
                    public override string Description { get; } = "Lorem ipsum sit dolor amet 1";
                    public override Type[] PossibleArguments { get; } = [];

                    public override void Execute() { }
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandParserGenerator());
        }

        [TestMethod]
        public Task CommandStatement_ShouldGenerateExpressionCollection()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands;

            namespace Minerals.Examples
            {
                public class TestCommand1 : CommandStatement
                {
                    public override string[] Aliases => ["test1", "test2", "test3"];
                    public override string Description { get; } = "Lorem ipsum sit dolor amet 1";
                    public override Type[] PossibleArguments { get; } = [];

                    public override void Execute() { }
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandParserGenerator());
        }

        [TestMethod]
        public Task CommandStatement_ShouldGenerateGetCollection()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands;

            namespace Minerals.Examples
            {
                public class TestCommand1 : CommandStatement
                {
                    public override string[] Aliases { get; } = ["test1", "test2", "test3"];
                    public override string Description { get; } = "Lorem ipsum sit dolor amet 1";
                    public override Type[] PossibleArguments { get; } = [];

                    public override void Execute() { }
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandParserGenerator());
        }

        [TestMethod]
        public Task CommandStatement_ShouldGenerateExpressionArray()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands;

            namespace Minerals.Examples
            {
                public class TestCommand1 : CommandStatement
                {
                    public override string[] Aliases => new string[] { "test1", "test2", "test3" };
                    public override string Description { get; } = "Lorem ipsum sit dolor amet 1";
                    public override Type[] PossibleArguments { get; } = [];

                    public override void Execute() { }
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandParserGenerator());
        }

        [TestMethod]
        public Task CommandStatement_ShouldGenerateGetArray()
        {
            const string source = """
            using System;
            using Minerals.AutoCommands;

            namespace Minerals.Examples
            {
                public class TestCommand1 : CommandStatement
                {
                    public override string[] Aliases { get; } = new string[] { "test1", "test2", "test3" };
                    public override string Description { get; } = "Lorem ipsum sit dolor amet 1";
                    public override Type[] PossibleArguments { get; } = [];

                    public override void Execute() { }
                }
            }
            """;
            return this.VerifyIncrementalGenerators(source, new CommandParserGenerator());
        }
    }
}