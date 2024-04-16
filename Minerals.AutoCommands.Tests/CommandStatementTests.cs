namespace Minerals.AutoCommands.Tests
{
    [TestClass]
    public class CommandStatementTests : VerifyBase
    {
        [TestMethod]
        public void SingleCommandStatement_ShouldExecuteAll()
        {
            var writer = Substitute.For<ICommandWriter>();

            new TestCommandHelpers.TestCommand1 { Writer = writer }.Execute();

            writer.DidNotReceive().WriteDebug(Arg.Any<string>());
            writer.Received(1).WriteInfo("TestCommand1 Executed");
            writer.DidNotReceive().WriteWarning(Arg.Any<string>());
            writer.DidNotReceive().WriteError(Arg.Any<string>());
        }

        [TestMethod]
        public void SingleCommandPipeline_ShouldExecuteAll()
        {
            var writer = Substitute.For<ICommandWriter>();
            var parser = TestCommandHelpers.CreateSubstituteForCommandParser();

            new CommandPipeline("Test", "1.0.0", "test")
                .UseCommandParser(parser)
                .UseCommandWriter(writer)
                .Evaluate(["test1"])
                .Execute();

            writer.DidNotReceive().WriteDebug(Arg.Any<string>());
            writer.Received(1).WriteInfo("TestCommand1 Executed");
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
                .Evaluate(["test1", "test2", "test3"])
                .Execute();

            writer.DidNotReceive().WriteDebug(Arg.Any<string>());
            writer.Received(3).WriteInfo(Arg.Any<string>());
            writer.DidNotReceive().WriteWarning(Arg.Any<string>());
            writer.DidNotReceive().WriteError(Arg.Any<string>());
            Received.InOrder(() =>
            {
                writer.WriteInfo("TestCommand1 Executed");
                writer.WriteInfo("TestCommand2 Executed");
                writer.WriteInfo("TestCommand3 Executed");
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
                .Evaluate(["test4", "value1"])
                .Execute();

            writer.DidNotReceive().WriteDebug(Arg.Any<string>());
            writer.Received(1).WriteInfo("TestCommand4 Executed: value1");
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
                .Evaluate(["test4"])?
                .Execute();

            writer.DidNotReceive().WriteDebug(Arg.Any<string>());
            writer.Received(6).WriteInfo(Arg.Any<string>());
            writer.DidNotReceive().WriteWarning(Arg.Any<string>());
            writer.Received(1).WriteError(Arg.Any<string>());
            Received.InOrder(() =>
            {
                writer.WriteError("ERROR: Value for the argument 'test4' was not found.");
                writer.WriteInfo("Test 1.0.0");
                writer.WriteInfo("");
                writer.WriteInfo("USAGE: test [Command] [Options] [Arguments]");
                writer.WriteInfo("");
                writer.WriteInfo("Use 'test --help' to get more information about this tool.");
                writer.WriteInfo("");
            });
        }

        [TestMethod]
        public void MultiCommandPipeline_ShouldOutputCommandNotSupportedException()
        {
            var writer = Substitute.For<ICommandWriter>();
            var parser = TestCommandHelpers.CreateSubstituteForCommandParser();

            new CommandPipeline("Test", "1.0.0", "test")
                .UseCommandParser(parser)
                .UseCommandWriter(writer)
                .Evaluate(["test2", "test1"])?
                .Execute();

            writer.DidNotReceive().WriteDebug(Arg.Any<string>());
            writer.Received(6).WriteInfo(Arg.Any<string>());
            writer.DidNotReceive().WriteWarning(Arg.Any<string>());
            writer.Received(1).WriteError(Arg.Any<string>());
            Received.InOrder(() =>
            {
                writer.WriteError("ERROR: The argument named 'test1' is not valid for the command named 'test2'.");
                writer.WriteInfo("Test 1.0.0");
                writer.WriteInfo("");
                writer.WriteInfo("USAGE: test [Command] [Options] [Arguments]");
                writer.WriteInfo("");
                writer.WriteInfo("Use 'test test2 --help' to get more information about this command.");
                writer.WriteInfo("");
            });
        }

        [TestMethod]
        public void InvalidCommand_ShouldOutputCommandNotFoundException()
        {
            var writer = Substitute.For<ICommandWriter>();
            var parser = TestCommandHelpers.CreateSubstituteForCommandParser();

            new CommandPipeline("Test", "1.0.0", "test")
                .UseCommandParser(parser)
                .UseCommandWriter(writer)
                .Evaluate(["test1", "test5"])?
                .Execute();

            writer.DidNotReceive().WriteDebug(Arg.Any<string>());
            writer.Received(6).WriteInfo(Arg.Any<string>());
            writer.DidNotReceive().WriteWarning(Arg.Any<string>());
            writer.Received(1).WriteError(Arg.Any<string>());
            Received.InOrder(() =>
            {
                writer.WriteError("ERROR: The command with name 'test5' was not found.");
                writer.WriteInfo("Test 1.0.0");
                writer.WriteInfo("");
                writer.WriteInfo("USAGE: test [Command] [Options] [Arguments]");
                writer.WriteInfo("");
                writer.WriteInfo("Use 'test --help' to get more information about this tool.");
                writer.WriteInfo("");
            });
        }
    }
}