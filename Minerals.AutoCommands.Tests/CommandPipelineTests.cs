namespace Minerals.AutoCommands.Tests
{
    [TestClass]
    public class CommandPipelineTests : VerifyBase
    {
        [TestMethod]
        public void SingleCommandPipeline_ShouldExecute()
        {
            var writer = TestCommandHelpers.CreateAndRunStandardCommandPipeline("test1");
            writer.DidNotReceiveWithAnyArgs().WriteHelpForException(Arg.Any<CommandException>());
            writer.Received(1).WriteLineInfo("TestCommand1 Execute: ");
            Assert.AreEqual(1, writer.ReceivedCalls().Count());
        }

        [TestMethod]
        public void MultiCommandPipeline_ShouldExecute()
        {
            var writer = TestCommandHelpers.CreateAndRunStandardCommandPipeline("test1", "test2", "test3");
            writer.DidNotReceiveWithAnyArgs().WriteHelpForException(Arg.Any<CommandException>());
            writer.Received(3).WriteLineInfo(Arg.Any<string>());
            Received.InOrder(() =>
            {
                writer.WriteLineInfo("TestCommand1 Execute: ");
                writer.WriteLineInfo("TestCommand2 Execute: ");
                writer.WriteLineInfo("TestCommand3 Execute: ");
            });
            Assert.AreEqual(3, writer.ReceivedCalls().Count());
        }

        [TestMethod]
        public void SingleCommandPipelineWithValue_ShouldExecute()
        {
            var writer = TestCommandHelpers.CreateAndRunStandardCommandPipeline("test4", "value1");
            writer.DidNotReceiveWithAnyArgs().WriteHelpForException(Arg.Any<CommandException>());
            writer.Received(1).WriteLineInfo("TestCommand4 Execute: value1");
            Assert.AreEqual(1, writer.ReceivedCalls().Count());
        }

        [TestMethod]
        public void InvalidArgument_ShouldOutputCommandArgumentNotFoundException()
        {
            var writer = TestCommandHelpers.CreateAndRunStandardCommandPipeline("test1", "test0");
            writer.Received(1).WriteHelpForException(Arg.Any<CommandArgumentNotFoundException>());
            Assert.AreEqual(1, writer.ReceivedCalls().Count());
        }

        [TestMethod]
        public void IncompatibleArguments_ShouldOutputCommandArgumentNotSupportedException()
        {
            var writer = TestCommandHelpers.CreateAndRunStandardCommandPipeline("test1", "test5");
            writer.Received(1).WriteHelpForException(Arg.Any<CommandArgumentNotSupportedException>());
            Assert.AreEqual(1, writer.ReceivedCalls().Count());
        }

        [TestMethod]
        public void DoubleArguments_ShouldOutputCommandArgumentNotSupportedException()
        {
            var writer = TestCommandHelpers.CreateAndRunStandardCommandPipeline("test2", "test2");
            writer.Received(1).WriteHelpForException(Arg.Any<CommandArgumentNotSupportedException>());
            Assert.AreEqual(1, writer.ReceivedCalls().Count());
        }

        [TestMethod]
        public void RequiredArguments_ShouldExecute()
        {
            var writer = TestCommandHelpers.CreateAndRunStandardCommandPipeline("test5", "test4", "value1");
            writer.DidNotReceiveWithAnyArgs().WriteHelpForException(Arg.Any<CommandException>());
            writer.Received(2).WriteLineInfo(Arg.Any<string>());
            Received.InOrder(() =>
            {
                writer.WriteLineInfo("TestCommand5 Execute: ");
                writer.WriteLineInfo("TestCommand4 Execute: value1");
            });
            Assert.AreEqual(2, writer.ReceivedCalls().Count());
        }

        [TestMethod]
        public void RequiredArgumentsInDescendants_ShouldExecute()
        {
            var writer = TestCommandHelpers.CreateAndRunStandardCommandPipeline("test5", "test1", "test4", "value1");
            writer.DidNotReceiveWithAnyArgs().WriteHelpForException(Arg.Any<CommandException>());
            writer.Received(3).WriteLineInfo(Arg.Any<string>());
            Received.InOrder(() =>
            {
                writer.WriteLineInfo("TestCommand5 Execute: ");
                writer.WriteLineInfo("TestCommand1 Execute: ");
                writer.WriteLineInfo("TestCommand4 Execute: value1");
            });
            Assert.AreEqual(3, writer.ReceivedCalls().Count());
        }

        [TestMethod]
        public void NoCommands_ShouldOutputCommandNotFoundException()
        {
            var writer = TestCommandHelpers.CreateAndRunStandardCommandPipeline();
            writer.Received(1).WriteHelpForException(Arg.Any<CommandNotFoundException>());
            Assert.AreEqual(1, writer.ReceivedCalls().Count());
        }

        [TestMethod]
        public void InvalidFirstCommand_ShouldOutputCommandNotFoundException()
        {
            var writer = TestCommandHelpers.CreateAndRunStandardCommandPipeline("test0");
            writer.Received(1).WriteHelpForException(Arg.Any<CommandNotFoundException>());
            Assert.AreEqual(1, writer.ReceivedCalls().Count());
        }

        [TestMethod]
        public void NoRequiredValue_ShouldOutputCommandValueNotFoundException()
        {
            var writer = TestCommandHelpers.CreateAndRunStandardCommandPipeline("test4");
            writer.Received(1).WriteHelpForException(Arg.Any<CommandValueNotFoundException>());
            Assert.AreEqual(1, writer.ReceivedCalls().Count());
        }

        [TestMethod]
        public void SkippedRequiredValue_ShouldOutputCommandValueRequiredException()
        {
            var writer = TestCommandHelpers.CreateAndRunStandardCommandPipeline("test4", "test1");
            writer.Received(1).WriteHelpForException(Arg.Any<CommandValueRequiredException>());
            Assert.AreEqual(1, writer.ReceivedCalls().Count());
        }
    }
}