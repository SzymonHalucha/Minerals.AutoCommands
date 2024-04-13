namespace Minerals.AutoCommands.Benchmarks
{
    public class CommandsGeneratorBenchmarks
    {
        public BenchmarkGeneration Baseline { get; set; } = default!;
        public BenchmarkGeneration PipelineGeneration { get; set; } = default!;
        public BenchmarkGeneration FullGeneration { get; set; } = default!;
        public BenchmarkGeneration BaselineDouble { get; set; } = default!;
        public BenchmarkGeneration FullGenerationDouble { get; set; } = default!;

        private const string _withoutAttributes = """
        using System;

        namespace Minerals.Examples
        {
            public partial class TestCommand1
            {
                public Type[] RequiredArguments { get; } = [];
                public Type[] PossibleArguments { get; } = [];
                public string Description { get; } = "";
                public string Usage { get; } = "";

                public void ShowHelp() { }
                public bool Execute() => true;
            }

            public partial class TestArgument1
            {
                public string[] PossibleValues { get; } = [];
                public string Description { get; } = "";
                public string Usage { get; } = "";
            }
        }
        """;

        private const string _withAttributes = """
        using System;
        using Minerals.AutoCommands;
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

            [CommandArgument("--arg1")]
            public partial class TestArgument1
            {
                public string[] PossibleValues { get; } = [];
                public string Description { get; } = "";
                public string Usage { get; } = "";
            }
        }
        """;

        [GlobalSetup]
        public void Initialize()
        {
            var references = BenchmarkGenerationExtensions.GetAppReferences
            (
                typeof(object),
                typeof(CommandPipelineHandlers),
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
            PipelineGeneration = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withoutAttributes,
                new CommandPipelineGenerator(),
                references
            );
            FullGeneration = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withAttributes,
                new CommandsGenerator(),
                [new CommandPipelineGenerator()],
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
                [new CommandPipelineGenerator()],
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
        public void SingleGeneration_OnlyPipeline()
        {
            PipelineGeneration.RunGeneration();
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