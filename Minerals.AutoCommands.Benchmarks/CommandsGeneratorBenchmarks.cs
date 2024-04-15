namespace Minerals.AutoCommands.Benchmarks
{
    public class CommandsGeneratorBenchmarks
    {
        public BenchmarkGeneration Baseline { get; set; } = default!;
        public BenchmarkGeneration FullGeneration { get; set; } = default!;
        public BenchmarkGeneration BaselineDouble { get; set; } = default!;
        public BenchmarkGeneration FullGenerationDouble { get; set; } = default!;

        private const string _withoutAttributes = """
        using System;

        namespace Minerals.Examples
        {
            public partial class TestCommand1
            {
                public string[] Aliases { get; } = new string[] { "test1" };
                public Type[] RequiredArguments { get; } = new Type[] { };
                public Type[] PossibleArguments { get; } = new Type[] { };
                public string Description { get; } = "";
                public string Group { get; } = "";

                public bool Execute()
                {
                    return true;
                }
            }

            public partial class TestValue1
            {
                public string[] Aliases { get; } = new string[] { "--arg1" };
                public string[] PossibleValues { get; } = new string[] { };
                public string Description { get; } = "";
                public string Group { get; } = "";
            }
        }
        """;

        private const string _withAttributes = """
        using System;
        using Minerals.AutoCommands.Attributes;

        namespace Minerals.Examples
        {
            [CommandStatement]
            public partial class TestCommand1
            {
                public string[] Aliases { get; } = new string[] { "test1" };
                public Type[] RequiredArguments { get; } = new Type[] { };
                public Type[] PossibleArguments { get; } = new Type[] { };
                public string Description { get; } = "";
                public string Group { get; } = "";

                public bool Execute()
                {
                    return true;
                }
            }

            [CommandValue]
            public partial class TestValue1
            {
                public string[] Aliases { get; } = new string[] { "--arg1" };
                public string[] PossibleValues { get; } = new string[] { };
                public string Description { get; } = "";
                public string Group { get; } = "";
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
                typeof(CommandStatementAttribute),
                typeof(CommandOrderException),
                typeof(ICommandStatement),
                typeof(CommandObject)
            );
            Baseline = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withoutAttributes,
                references
            );
            FullGeneration = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withAttributes,
                new CommandsGenerator(),
                references
            );
            BaselineDouble = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withoutAttributes,
                references
            );
            FullGenerationDouble = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withAttributes,
                new CommandsGenerator(),
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