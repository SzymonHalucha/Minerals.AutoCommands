namespace Minerals.AutoCommands.Benchmarks
{
    public class CommandsGeneratorBenchmarks
    {
        public BenchmarkGeneration Baseline { get; set; } = default!;
        public BenchmarkGeneration FullGeneration { get; set; } = default!;
        public BenchmarkGeneration BaselineDouble { get; set; } = default!;
        public BenchmarkGeneration FullGenerationDouble { get; set; } = default!;

        private const string _withoutBaseClass = """
        using System;

        namespace Minerals.Examples
        {
            public class TestCommand1
            {
                public string[] Aliases { get; } = ["test1"];
                public string Description { get; } = "Lorem ipsum sit dolor amet 1";
                public Type[] PossibleArguments { get; } = [typeof(TestCommand2)];

                public void Execute() { }
            }

            public class TestCommand2
            {
                public string[] Aliases { get; } = ["test2"];
                public string Description { get; } = "Lorem ipsum sit dolor amet 2";
                public Type[] PossibleArguments { get; } = [typeof(TestCommand1)];

                public void Execute() { }
            }
        }
        """;

        private const string _withBaseClass = """
        using System;
        using Minerals.AutoCommands;

        namespace Minerals.Examples
        {
            public class TestCommand1 : CommandStatement
            {
                public override string[] Aliases { get; } = ["test1"];
                public override string Description { get; } = "Lorem ipsum sit dolor amet 1";
                public override Type[] PossibleArguments { get; } = [typeof(TestCommand2)];

                public override void Execute() { }
            }

            public class TestCommand2 : CommandStatement
            {
                public override string[] Aliases { get; } = ["test2"];
                public override string Description { get; } = "Lorem ipsum sit dolor amet 2";
                public override Type[] PossibleArguments { get; } = [typeof(TestCommand1)];

                public override void Execute() { }
            }
        }
        """;

        [GlobalSetup]
        public void Initialize()
        {
            var references = BenchmarkGenerationExtensions.GetAppReferences
            (
                typeof(object),
                typeof(CommandPipeline),
                typeof(CommandException),
                typeof(ICommandStatement),
                typeof(CommandStatementObject)
            );
            Baseline = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withoutBaseClass,
                references
            );
            FullGeneration = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withBaseClass,
                new CommandParserGenerator(),
                references
            );
            BaselineDouble = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withoutBaseClass,
                references
            );
            FullGenerationDouble = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withBaseClass,
                new CommandParserGenerator(),
                references
            );
            BaselineDouble.RunAndSaveGeneration();
            BaselineDouble.AddSourceCode("// Test Comment");
            FullGenerationDouble.RunAndSaveGeneration();
            FullGenerationDouble.AddSourceCode("// Test Comment");
        }

        [Benchmark(Baseline = true)]
        public void SingleGeneration_Baseline()
        {
            Baseline.RunGeneration();
        }

        [Benchmark]
        public void SingleGeneration_Full()
        {
            FullGeneration.RunGeneration();
        }

        [Benchmark]
        public void DoubleGeneration_Baseline()
        {
            BaselineDouble.RunGeneration();
        }

        [Benchmark]
        public void DoubleGeneration_Full()
        {
            FullGenerationDouble.RunGeneration();
        }
    }
}