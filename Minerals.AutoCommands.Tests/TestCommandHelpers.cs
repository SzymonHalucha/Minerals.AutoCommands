namespace Minerals.AutoCommands.Tests
{
    public static class TestCommandHelpers
    {
        private static Type[] PossibleArguments =>
        [
            typeof(TestCommand1),
            typeof(TestCommand2),
            typeof(TestCommand3),
            typeof(TestCommand4),
            typeof(TestCommand5)
        ];

        public static ICommandWriter CreateAndRunStandardCommandPipeline(params string[] args)
        {
            var writer = CreateSubstituteForCommandWriter();
            var parser = CreateSubstituteForCommandParser();

            new CommandPipeline("Test", "1.0.0", "test")
                .UseCommandParser(parser)
                .UseCommandWriter(writer)
                .UsePossibleArguments(PossibleArguments)
                .Evaluate(args)?
                .Execute();

            return writer;
        }

        public static ICommandWriter CreateSubstituteForCommandWriter()
        {
            return Substitute.For<ICommandWriter>();
        }

        public static ICommandParser CreateSubstituteForCommandParser()
        {
            var parser = Substitute.For<ICommandParser>();
            parser.Parse(Arg.Any<string>(), StringComparison.Ordinal).Returns(x => null);
            parser.IsAlias(Arg.Any<string>(), StringComparison.Ordinal).Returns(false);
            parser.Configure().Parse("test1", StringComparison.Ordinal).Returns(new TestCommand1());
            parser.Configure().Parse("test2", StringComparison.Ordinal).Returns(new TestCommand2());
            parser.Configure().Parse("test3", StringComparison.Ordinal).Returns(new TestCommand3());
            parser.Configure().Parse("test4", StringComparison.Ordinal).Returns(new TestCommand4());
            parser.Configure().Parse("test5", StringComparison.Ordinal).Returns(new TestCommand5());
            parser.Configure().IsAlias("test1", StringComparison.Ordinal).Returns(true);
            parser.Configure().IsAlias("test2", StringComparison.Ordinal).Returns(true);
            parser.Configure().IsAlias("test3", StringComparison.Ordinal).Returns(true);
            parser.Configure().IsAlias("test4", StringComparison.Ordinal).Returns(true);
            parser.Configure().IsAlias("test5", StringComparison.Ordinal).Returns(true);
            return parser;
        }

        public class TestCommand1 : CommandStatement
        {
            public override string[] Aliases { get; } = ["test1"];
            public override string Description { get; } = "Example Description 1.";
            public override Type[] PossibleArguments { get; } = [typeof(TestCommand2), typeof(TestCommand3), typeof(TestCommand4)];

            public override bool Execute(Dictionary<object, object>? data = null)
            {
                Writer.WriteLineInfo($"{GetType().Name} Execute: {Value}");
                Next?.Execute();
                return true;
            }
        }

        public class TestCommand2 : CommandStatement
        {
            public override string[] Aliases { get; } = ["test2"];
            public override string Description { get; } = "Example Description 2.";
            public override Type[] PossibleArguments { get; } = [typeof(TestCommand1), typeof(TestCommand3), typeof(TestCommand4), typeof(TestCommand5)];

            public override bool Execute(Dictionary<object, object>? data = null)
            {
                Writer.WriteLineInfo($"{GetType().Name} Execute: {Value}");
                Next?.Execute();
                return true;
            }
        }

        public class TestCommand3 : CommandStatement
        {
            public override string[] Aliases { get; } = ["test3"];
            public override string Description { get; } = "Example Description 3.";
            public override Type[] PossibleArguments { get; } = [typeof(TestCommand1), typeof(TestCommand2), typeof(TestCommand4), typeof(TestCommand5)];

            public override bool Execute(Dictionary<object, object>? data = null)
            {
                Writer.WriteLineInfo($"{GetType().Name} Execute: {Value}");
                Next?.Execute();
                return true;
            }
        }

        public class TestCommand4 : CommandStatement
        {
            public override string[] Aliases { get; } = ["test4"];
            public override string Description { get; } = "Example Description 4.";
            public override Type[] PossibleArguments { get; } = [typeof(TestCommand1), typeof(TestCommand2), typeof(TestCommand3)];
            public override bool ValueRequired { get; } = true;

            public override bool Execute(Dictionary<object, object>? data = null)
            {
                Writer.WriteLineInfo($"{GetType().Name} Execute: {Value}");
                Next?.Execute();
                return true;
            }
        }

        public class TestCommand5 : CommandStatement
        {
            public override string[] Aliases { get; } = ["test5"];
            public override string Description { get; } = "Example Description 5.";
            public override Type[] PossibleArguments { get; } = [typeof(TestCommand1), typeof(TestCommand2), typeof(TestCommand3), typeof(TestCommand4)];
            public override Type[] ArgumentsRequired { get; } = [typeof(TestCommand4)];

            public override bool Execute(Dictionary<object, object>? data = null)
            {
                Writer.WriteLineInfo($"{GetType().Name} Execute: {Value}");
                Next?.Execute();
                return true;
            }
        }
    }
}