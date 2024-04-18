namespace Minerals.AutoCommands.Tests
{
    [TestClass]
    public class CommandStatementTests : VerifyBase
    {
        private static Type[] PossibleArguments => [typeof(TestCommandHelpers.TestCommand1), typeof(TestCommandHelpers.TestCommand2), typeof(TestCommandHelpers.TestCommand3), typeof(TestCommandHelpers.TestCommand4)];

        [TestMethod]
        public void SingleCommandPipeline_ShouldExecuteAll()
        {
            var writer = Substitute.For<ICommandWriter>();
            var parser = TestCommandHelpers.CreateSubstituteForCommandParser();

            new CommandPipeline("Test", "1.0.0", "test")
                .UseCommandParser(parser)
                .UseCommandWriter(writer)
                .UsePossibleArguments(PossibleArguments)
                .Evaluate(["test1"])?
                .Execute();

            writer.DidNotReceive().WriteDebug(Arg.Any<string>());
            writer.Received(1).WriteInfo("TestCommand1 Execute: ");
            writer.DidNotReceive().WriteWarning(Arg.Any<string>());
            writer.DidNotReceive().WriteError(Arg.Any<string>());
        }

        [TestMethod]
        public void MultiCommandPipeline_ShouldExecuteAll()
        {
            var writer = Substitute.For<ICommandWriter>();
            var parser = TestCommandHelpers.CreateSubstituteForCommandParser();

            new CommandPipeline("Test", "1.0.0", "test")
                .UseCommandParser(parser)
                .UseCommandWriter(writer)
                .UsePossibleArguments(PossibleArguments)
                .Evaluate(["test1", "test2", "test3"])?
                .Execute();

            writer.DidNotReceive().WriteDebug(Arg.Any<string>());
            writer.Received(3).WriteInfo(Arg.Any<string>());
            writer.DidNotReceive().WriteWarning(Arg.Any<string>());
            writer.DidNotReceive().WriteError(Arg.Any<string>());
            Received.InOrder(() =>
            {
                writer.WriteInfo("TestCommand1 Execute: ");
                writer.WriteInfo("TestCommand2 Execute: ");
                writer.WriteInfo("TestCommand3 Execute: ");
            });
        }

        [TestMethod]
        public void SingleCommandPipeline_ShouldExecuteAllWithValue()
        {
            var writer = Substitute.For<ICommandWriter>();
            var parser = TestCommandHelpers.CreateSubstituteForCommandParser();

            new CommandPipeline("Test", "1.0.0", "test")
                .UseCommandParser(parser)
                .UseCommandWriter(writer)
                .UsePossibleArguments(PossibleArguments)
                .Evaluate(["test4", "value1"])?
                .Execute();

            writer.DidNotReceive().WriteDebug(Arg.Any<string>());
            writer.Received(1).WriteInfo("TestCommand4 Execute: value1");
            writer.DidNotReceive().WriteWarning(Arg.Any<string>());
            writer.DidNotReceive().WriteError(Arg.Any<string>());
        }

        [TestMethod]
        public void NoValue_ShouldOutputCommandValueNotFoundException()
        {
            var writer = Substitute.For<ICommandWriter>();
            var parser = TestCommandHelpers.CreateSubstituteForCommandParser();

            new CommandPipeline("Test", "1.0.0", "test")
                .UseCommandParser(parser)
                .UseCommandWriter(writer)
                .UsePossibleArguments(PossibleArguments)
                .Evaluate(["test4"])?
                .Execute();

            writer.Received(1).WriteHelpForException(Arg.Any<CommandValueNotFoundException>());
        }

        [TestMethod]
        public void MultiCommandPipeline_ShouldOutputCommandArgumentNotSupportedException()
        {
            var writer = Substitute.For<ICommandWriter>();
            var parser = TestCommandHelpers.CreateSubstituteForCommandParser();

            new CommandPipeline("Test", "1.0.0", "test")
                .UseCommandParser(parser)
                .UseCommandWriter(writer)
                .UsePossibleArguments(PossibleArguments)
                .Evaluate(["test2", "test2"])?
                .Execute();

            writer.Received(1).WriteHelpForException(Arg.Any<CommandArgumentNotSupportedException>());
        }

        [TestMethod]
        public void InvalidCommand_ShouldOutputCommandArgumentNotFoundException()
        {
            var writer = Substitute.For<ICommandWriter>();
            var parser = TestCommandHelpers.CreateSubstituteForCommandParser();

            new CommandPipeline("Test", "1.0.0", "test")
                .UseCommandParser(parser)
                .UseCommandWriter(writer)
                .UsePossibleArguments(PossibleArguments)
                .Evaluate(["test1", "test5"])?
                .Execute();

            writer.Received(1).WriteHelpForException(Arg.Any<CommandArgumentNotFoundException>());
        }

        [TestMethod]
        public void NoCommands_ShouldOutputCommandNotFoundException()
        {
            var writer = Substitute.For<ICommandWriter>();
            var parser = TestCommandHelpers.CreateSubstituteForCommandParser();

            new CommandPipeline("Test", "1.0.0", "test")
                .UseCommandParser(parser)
                .UseCommandWriter(writer)
                .UsePossibleArguments(PossibleArguments)
                .Evaluate([])?
                .Execute();

            writer.Received(1).WriteHelpForException(exception: Arg.Any<CommandNotFoundException>());
        }

        [TestMethod]
        public void InvalidFirstCommand_ShouldOutputCommandNotFoundException()
        {
            var writer = Substitute.For<ICommandWriter>();
            var parser = TestCommandHelpers.CreateSubstituteForCommandParser();

            new CommandPipeline("Test", "1.0.0", "test")
                .UseCommandParser(parser)
                .UseCommandWriter(writer)
                .UsePossibleArguments(PossibleArguments)
                .Evaluate(["test9"])?
                .Execute();

            writer.Received(1).WriteHelpForException(exception: Arg.Any<CommandNotFoundException>());
        }
    }
}