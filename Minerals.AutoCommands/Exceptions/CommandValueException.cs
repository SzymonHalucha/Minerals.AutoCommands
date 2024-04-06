namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandValueException : CommandExceptionBase
    {
        public CommandValueException() : base(string.Empty) { }
        public CommandValueException(string message) : base(message) { }
    }
}