namespace Minerals.AutoCommands.Interfaces
{
    public interface ICommandValue : ICommand
    {
        public string[] PossibleValues { get; }
        public string Value { get; }
    }
}