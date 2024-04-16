namespace Minerals.AutoCommands.Interfaces
{
    public interface ICommandParser
    {
        public ICommandStatement Parse(string arg, StringComparison comparison);
        public bool IsAlias(string text, StringComparison comparison);
    }
}