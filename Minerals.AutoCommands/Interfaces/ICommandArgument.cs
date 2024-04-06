namespace Minerals.AutoCommands.Interfaces
{
    public interface ICommandArgument : ICommand
    {
        public string[] PossibleValues { get; }

        public string Value { get; set; }
    }
}