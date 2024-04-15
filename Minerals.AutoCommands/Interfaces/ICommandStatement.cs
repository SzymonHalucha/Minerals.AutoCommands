namespace Minerals.AutoCommands.Interfaces
{
    public interface ICommandStatement : ICommand
    {
        public Type[] RequiredArguments { get; }
        public Type[] PossibleArguments { get; }

        public List<ICommand> Arguments { get; }

        public bool Execute();
    }
}