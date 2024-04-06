namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandNotFoundException : CommandExceptionBase
    {
        public CommandNotFoundException() : base(string.Empty) { }
        public CommandNotFoundException(string message) : base(message) { }
    }
}