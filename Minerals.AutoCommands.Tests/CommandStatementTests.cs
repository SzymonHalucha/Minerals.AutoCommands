namespace Minerals.AutoCommands.Tests
{
    [TestClass]
    public class CommandStatementTests : VerifyBase
    {
        public class TestCommand : CommandStatement
        {
            public override string[] Aliases { get; } = ["test"];
            public override string Description { get; } = "Lorem ipsum sit dolor amet.";
            public override Type[] PossibleArguments { get; } = [];

            public override void Execute()
            {
                Writer!.WriteInfo($"{GetType().Name}: Executed!");
                foreach (var arg in Arguments)
                {
                    arg.Execute();
                }
            }
        }

        [TestMethod]
        public void SingleCommandStatement_ShouldExecute()
        {
            var wirter = Substitute.For<ICommandWriter>();
            var command = new TestCommand { Writer = wirter };
            command.Execute();
            wirter.Received().WriteInfo("TestCommand: Executed!");
        }
    }
}