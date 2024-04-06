namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandValueNotFoundException : CommandExceptionBase
    {
        public CommandValueNotFoundException() : base(string.Empty) { }
        public CommandValueNotFoundException(string message) : base(message) { }
    }
}