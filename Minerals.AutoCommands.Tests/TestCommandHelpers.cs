namespace Minerals.AutoCommands.Tests
{
    public static class TestCommandHelpers
    {
        public static ICommandParser CreateSubstituteForCommandParser()
        {
            var parser = Substitute.For<ICommandParser>();
            parser.Parse(Arg.Any<string>(), StringComparison.Ordinal).Returns(x => null!);
            parser.Configure().Parse("test1", StringComparison.Ordinal).Returns(new TestCommand1());
            parser.Configure().Parse("test2", StringComparison.Ordinal).Returns(new TestCommand2());
            parser.Configure().Parse("test3", StringComparison.Ordinal).Returns(new TestCommand3());
            parser.Configure().Parse("test4", StringComparison.Ordinal).Returns(new TestCommand4());
            return parser;
        }

        public class TestCommand1 : CommandStatement
        {
            public override string[] Aliases { get; } = ["test1"];
            public override string Description { get; } = "Example description 1.";
            public override Type[] PossibleArguments { get; } = [typeof(TestCommand2), typeof(TestCommand3), typeof(TestCommand4)];

            public override void Execute()
            {
                Writer!.WriteInfo($"{GetType().Name} Executed");
                foreach (var arg in Arguments)
                {
                    arg.Execute();
                }
            }
        }

        public class TestCommand2 : CommandStatement
        {
            public override string[] Aliases { get; } = ["test2"];
            public override string Description { get; } = "Example description 2.";
            public override Type[] PossibleArguments { get; } = [typeof(TestCommand3), typeof(TestCommand4)];

            public override void Execute()
            {
                Writer!.WriteInfo($"{GetType().Name} Executed");
                foreach (var arg in Arguments)
                {
                    arg.Execute();
                }
            }
        }

        public class TestCommand3 : CommandStatement
        {
            public override string[] Aliases { get; } = ["test3"];
            public override string Description { get; } = "Example description 3.";
            public override Type[] PossibleArguments { get; } = [typeof(TestCommand1), typeof(TestCommand2), typeof(TestCommand4)];

            public override void Execute()
            {
                Writer!.WriteInfo($"{GetType().Name} Executed");
                foreach (var arg in Arguments)
                {
                    arg.Execute();
                }
            }
        }

        public class TestCommand4 : CommandStatement
        {
            public override string[] Aliases { get; } = ["test4"];
            public override string Description { get; } = "Example description 4.";
            public override Type[] PossibleArguments { get; } = [];
            public override bool RequireValue => true;

            public override void Execute()
            {
                Writer!.WriteInfo($"{GetType().Name} Executed: {Value}");
            }
        }
    }
}