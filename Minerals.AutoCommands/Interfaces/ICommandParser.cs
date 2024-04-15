namespace Minerals.AutoCommands.Interfaces
{
    public interface ICommandParser
    {
        public ICommand Parse(string arg, StringComparison comparison);
    }
}